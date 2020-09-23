﻿namespace WorldFeed.Post.API.Json
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using WorldFeed.Common.Helpers;
    using WorldFeed.Common.Json;
    using WorldFeed.Post.API.Infrastructure.Inject.Contracts;
    using WorldFeed.Post.Domain.AggregatesModel;
    using WorldFeed.Post.DTO;

    public interface IPostJsonConverter
    {
        string ToJson<TFrom>(TFrom obj) where TFrom : class;
        string ToJson<TFrom, TTo>(TFrom obj) where TFrom : class where TTo : class;

        TTo ConvertJsonTo<TTo>(string json) where TTo : class;
    }

    public class PostJsonConverter : IPostJsonConverter
    {
        private readonly IJsonObjectConverter _jsonObjectConverter;
        private readonly Dictionary<Type, IJsonConverter> _defaultSerializers;
        private readonly Dictionary<Type, Dictionary<Type, IJsonConverter>> _serializers;

        public PostJsonConverter(ITwitterClient client, IJsonObjectConverter jsonObjectConverter)
        {
            _jsonObjectConverter = jsonObjectConverter;
            var factories = client.Factories;

            _defaultSerializers = new Dictionary<Type, IJsonConverter>();
            _serializers = new Dictionary<Type, Dictionary<Type, IJsonConverter>>();

            // ReSharper disable RedundantTypeArgumentsOfMethod
            Map<IPost, IPostDTO>(tweet => tweet.TweetDTO, factories.CreateTweet);
            Map<IUser, IUserDTO>(user => user.UserDTO, factories.CreateUser);
            
            Map<IOEmbedTweet, IOEmbedTweetDTO>(oEmbedTweet => oEmbedTweet.OembedTweetDTO, factories.CreateOEmbedTweet);

            // ReSharper restore RedundantTypeArgumentsOfMethod
        }

        // TO JSON
        public string ToJson<T>(T obj) where T : class
        {
            return ToJson(obj, null);
        }

        public string ToJson<T1, T2>(T1 obj) where T1 : class where T2 : class
        {
            if (_serializers.TryGetValue(typeof(T1), out var serializersByType))
            {
                if (serializersByType.TryGetValue(typeof(T2), out var serializer))
                {
                    return ToJson(obj, serializer);
                }
            }

            return ToJson(obj, null);
        }

        private string ToJson<T>(T obj, IJsonConverter converter)
        {
            var type = typeof(T);
            object toSerialize = obj;

            if (obj is IEnumerable enumerable)
            {
                var genericType = type.GetElementType() ?? type.GetGenericArguments()[0];

                converter ??= GetSerializerFromNonCollectionType(genericType);

                if (converter != null)
                {
                    var list = new List<object>();

                    foreach (var o in enumerable)
                    {
                        list.Add(converter.GetObjectToSerialize(o));
                    }

                    toSerialize = list;
                }
            }
            else
            {
                converter ??= GetSerializerFromNonCollectionType(type);
                if (converter != null)
                {
                    toSerialize = converter.GetObjectToSerialize(obj);
                }
            }

            try
            {
                return _jsonObjectConverter.Serialize(toSerialize);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "The type provided is probably not compatible with Tweetinvi Json serializer." +
                    "If you think class should be serializable by default please report on github.com/linvi/tweetinvi.",
                    ex
                );
            }
        }

        public T ConvertJsonTo<T>(string json) where T : class
        {
            return ConvertJsonTo<T>(json, null);
        }

        public TTo ConvertJsonTo<TFrom, TTo>(string json) where TFrom : class where TTo : class
        {
            if (_serializers.TryGetValue(typeof(TFrom), out var serializersByType))
            {
                if (serializersByType.TryGetValue(typeof(TTo), out var serializer))
                {
                    return ConvertJsonTo<TTo>(json, serializer);
                }
            }

            return ConvertJsonTo<TTo>(json, null);
        }

        private T ConvertJsonTo<T>(string json, IJsonConverter converter) where T : class
        {
            var type = typeof(T);

            try
            {
                if (typeof(IEnumerable).IsAssignableFrom(type))
                {
                    Type genericType = null;

                    if (type.GetTypeInfo().IsGenericType)
                    {
                        genericType = type.GetGenericArguments()[0];
                    }
                    else if (typeof(Array).IsAssignableFrom(type))
                    {
                        genericType = type.GetElementType();
                    }

                    converter ??= GetSerializerFromNonCollectionType(genericType);
                    if (genericType != null && converter != null)
                    {
                        var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(genericType));

                        JArray jsonArray = JArray.Parse(json);

                        foreach (var elt in jsonArray)
                        {
                            var eltJson = elt.ToString();
                            list.Add(converter.Deserialize(eltJson));
                        }

                        if (typeof(Array).IsAssignableFrom(type))
                        {
                            var array = Array.CreateInstance(genericType, list.Count);
                            list.CopyTo(array, 0);
                            return array as T;
                        }

                        return list as T;
                    }
                }

                converter ??= GetSerializerFromNonCollectionType(type);

                if (converter != null)
                {
                    return converter.Deserialize(json) as T;
                }

                return _jsonObjectConverter.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "The type provided is probably not compatible with Tweetinvi Json serializer." +
                    "If you think class should be deserializable by default please report on github.com/linvi/tweetinvi.",
                    ex
                );
            }
        }

        private IJsonConverter GetSerializerFromNonCollectionType(Type type)
        {
            // Test interfaces
            if (_defaultSerializers.ContainsKey(type))
            {
                return _defaultSerializers[type];
            }

            // Test concrete classes from mapped interfaces
            if (_defaultSerializers.Keys.Any(x => x.IsAssignableFrom(type)))
            {
                return _defaultSerializers.FirstOrDefault(x => x.Key.IsAssignableFrom(type)).Value;
            }

            return null;
        }

        private void Map<T1, T2>(Func<T1, T2> getDto, Func<string, T1> deserialize) where T1 : class where T2 : class
        {
            var jsonTypeConverter = new JsonTypeConverter<T1, T2>(getDto, deserialize);

            if (!_defaultSerializers.ContainsKey(typeof(T1)))
            {
                _defaultSerializers.Add(typeof(T1), jsonTypeConverter);
            }

            if (!_serializers.ContainsKey(typeof(T1)))
            {
                _serializers.Add(typeof(T1), new Dictionary<Type, IJsonConverter>());
            }

            _serializers[typeof(T1)].Add(typeof(T2), jsonTypeConverter);
        }
    }
}
