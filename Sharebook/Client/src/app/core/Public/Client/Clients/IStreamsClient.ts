import {InjectionToken} from "@angular/core";

import {ISampleStream} from "../../Streaming/ISampleStream";
import {ICreateSampleStreamParameters} from "../../Parameters/StreamsClient/CreateSampleStreamParameters";
import {ICreateFilteredTweetStreamParameters} from "../../Parameters/StreamsClient/CreateFilteredStreamParameters";
import {ICreateTweetStreamParameters} from "../../Parameters/StreamsClient/CreateTweetStreamParameters";
import {ICreateTrackedTweetStreamParameters} from "../../Parameters/StreamsClient/CreateTrackedStreamParameters";

export interface IStreamsClient {
  createSampleStream(): ISampleStream;

  // Create a stream notifying that a random tweets has been created.
  // https://dev.twitter.com/streaming/reference/get/statuses/sample
  createSampleStream(parameters: ICreateSampleStreamParameters): ISampleStream;

  createFilteredStream(): null; // IFilteredStream;

  // Create a stream notifying the client when a tweet matching the specified criteria is created.
  // https://dev.twitter.com/streaming/reference/post/statuses/filter
  createFilteredStream(parameters: ICreateFilteredTweetStreamParameters): null; // IFilteredStream;

  createTweetStream(): null; // ITweetStream;

  // Create a stream that receive tweets
  createTweetStream(parameters: ICreateTweetStreamParameters): null; // ITweetStream;

  createTrackedTweetStream(): null; // ITrackedStream;

  // Create a stream that receive tweets. In addition this stream allow you to filter the results received.
  createTrackedTweetStream(parameters: ICreateTrackedTweetStreamParameters): null; // ITrackedStream;
}


export const IStreamsClientToken = new InjectionToken<IStreamsClient>('IStreamsClient', {
  providedIn: 'root',
  factory: () => null,
});
