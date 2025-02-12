﻿import {IMediaEntity} from "./interfaces/IMediaEntity";
import {IMediaEntitySize} from "./interfaces/IMediaEntitySize";
import {IVideoInformationEntity} from "./interfaces/ExtendedEntities/IVideoInformationEntity";

// Object storing information related with a Media on Twitter
export class MediaEntity implements IMediaEntity { // TODO: you use it in code, but you should not. Delete it.
  fullSizeImageUrl: string;
  imageUrl: string;
  thumbImageUrl: string;
  blurhash: string;
  // [JsonProperty("id")]
  public id: number; // long?

  // [JsonProperty("id_str")]
  public idStr: string;

  // [JsonProperty("url")]
  public URL: string;

  // [JsonProperty("display_url")]
  public displayURL: string;

  // [JsonProperty("expanded_url")]
  public expandedURL: string;

  // [JsonProperty("media_url")]
  public mediaURL: string;

  // [JsonProperty("media_url_https")]
  public mediaURLHttps: string;

  // [JsonProperty("type")]
  public mediaType: string;

  // [JsonProperty("indices")]
  public indices: number[];

  // [JsonProperty("sizes")]
  public meta: Map<string, IMediaEntitySize>;

  // [JsonProperty("video_info")]
  // [JsonConverter(typeof(JsonPropertyConverterRepository))]
  public videoDetails: IVideoInformationEntity;

  public equals(other: IMediaEntity): boolean {
    if (this.id == null || other == null || this.id !== other.id) {
      return false;
    }

    if (this.indices == null || other.indices == null) {
      return this.indices === other.indices;
    }

    // @ts-ignore
    return this.indices.containsSameObjectsAs<number>(other.indices, true);
  }
}
