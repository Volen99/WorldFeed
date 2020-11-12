﻿// Information used to generate an OAuth query
import {InjectionToken} from "@angular/core";
import {OAuthQueryParameter} from "../../../webLogic/OAuthQueryParameter";

export interface IOAuthQueryParameter {
  // Parameter name
  key: string;

  // Parameter value
  value: string;

  // Is this parameter required to generate the signature
  requiredForSignature: boolean;

  // Is this parameter required to generate the headers
  requiredForHeader: boolean;

  // Is this parameter required to generate the secret key
  isPartOfOAuthSecretKey: boolean;
}

export const IOAuthQueryParameterToken = new InjectionToken<IOAuthQueryParameter>('IOAuthQueryParameter', {
  providedIn: 'root',
  factory: () => new OAuthQueryParameter(null, null, false, false, false),
});
