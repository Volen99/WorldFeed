﻿namespace Sharebook.Post.Domain.AggregatesModel.PostAggregate
{
    using Sharebook.Storage.Domain.AggregatesModel.PostAggregate.Entities;

    public class ExtendedPost : IExtendedPost
    {
        //[JsonProperty("text")]
        public string Text { get; set; }

        //[JsonProperty("full_text")]
        public string FullText { get; set; }

        //[JsonProperty("display_text_range")]
        public int[] DisplayTextRange { get; set; }

        //[JsonProperty("entities")]
        //[JsonConverter(typeof(JsonPropertyConverterRepository))]
        public ITweetEntities LegacyEntities { get; set; }

        //[JsonProperty("extended_entities")]
       // [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public ITweetEntities ExtendedEntities { get; set; }
    }
}
