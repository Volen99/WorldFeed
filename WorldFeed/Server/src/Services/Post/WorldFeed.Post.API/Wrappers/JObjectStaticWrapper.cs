﻿namespace WorldFeed.Post.API.Wrappers
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using WorldFeed.Common.Extensions;
    using WorldFeed.Common.Wrappers;
    using WorldFeed.Post.API.JsonConverters;

    // Wrapper classes "cannot" be tested
    public class JObjectStaticWrapper : IJObjectStaticWrapper
    {
        private readonly JsonSerializer serializer;

        public JObjectStaticWrapper()
        {
            this.serializer = new JsonSerializer();

            foreach (var converter in JsonPropertiesConverterRepository.Converters)
            {
                this.serializer.Converters.Add(converter);
            }
        }

        public JObject GetJobjectFromJson(string json)
        {
            if (StringExtensions.IsMatchingJsonFormat(json) == false)
            {
                return null;
            }

            return JObject.Parse(json);
        }

        public T ToObject<T>(JObject jObject)
        {
            return jObject.ToObject<T>(this.serializer);
        }

        public T ToObject<T>(JToken jToken) where T : class
        {
            if (jToken == null)
            {
                return null;
            }

            return jToken.ToObject<T>(this.serializer);
        }

        public string GetNodeRootName(JToken jToken)
        {
            var jProperty = jToken as JProperty;
            return jProperty != null ? jProperty.Name : null;
        }
    }
}
