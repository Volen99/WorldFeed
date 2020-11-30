﻿namespace Sharebook.Post.Domain.AggregatesModel.PostAggregate.TwitterEntities.ExtendedEntities
{
    using Newtonsoft.Json;

    using Sharebook.Storage.Domain.AggregatesModel.PostAggregate.Entities.ExtendedEntities;

    public class VideoInformationEntity : IVideoInformationEntity
    {
        [JsonProperty("aspect_ratio")]
        public int[] AspectRatio { get; set; }

        [JsonProperty("duration_millis")]
        public int DurationInMilliseconds { get; set; }

        [JsonProperty("variants")]
        public IVideoEntityVariant[] Variants { get; set; }
    }
}
