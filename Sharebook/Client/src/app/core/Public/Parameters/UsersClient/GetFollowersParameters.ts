﻿import {GetFollowerIdsParameters, IGetFollowerIdsParameters} from "./GetFollowerIdsParameters";
import {IGetUsersOptionalParameters} from "../Optionals/GetUsersOptionalParameters";
import {SharebookLimits} from "../../Settings/SharebookLimits";
import {UserIdentifier} from "../../Models/UserIdentifier";
import {IUserIdentifier} from "../../Models/Interfaces/IUserIdentifier";
import Type from "typescript-dotnet-commonjs/System/Types";

// For more information visit : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-followers-ids
export interface IGetFollowersParameters extends IGetFollowerIdsParameters, IGetUsersOptionalParameters {
  // Page size when retrieving the users objects from Twitter
  GetUsersPageSize: number;
}

export class GetFollowersParameters extends GetFollowerIdsParameters implements IGetFollowersParameters {
  constructor(usernameOrUserIdOrUserParameters: | string | number | IUserIdentifier | IGetFollowersParameters) {
    if (GetFollowersParameters.isIGetFollowersParameters(usernameOrUserIdOrUserParameters)) {
      super(usernameOrUserIdOrUserParameters);
      this.skipStatus = usernameOrUserIdOrUserParameters.skipStatus;
      this.includeEntities = usernameOrUserIdOrUserParameters.includeEntities;
      this.GetUsersPageSize = usernameOrUserIdOrUserParameters.GetUsersPageSize;
    } else {
      let userCurrent: IUserIdentifier;
      if (Type.isString(usernameOrUserIdOrUserParameters) || Type.isNumber(usernameOrUserIdOrUserParameters)) {
        userCurrent = new UserIdentifier(usernameOrUserIdOrUserParameters);
      } else {
        userCurrent = usernameOrUserIdOrUserParameters;
      }
      super(userCurrent);

      this.GetUsersPageSize = SharebookLimits.DEFAULTS.USERS_GET_USERS_MAX_SIZE;
      this.user = userCurrent;
    }
  }

  public skipStatus?: boolean;
  public includeEntities?: boolean;
  public GetUsersPageSize: number;

  private static isIGetFollowersParameters(usernameOrUserIdOrUserParameters: | string | number | IUserIdentifier | IGetFollowersParameters):
    usernameOrUserIdOrUserParameters is IGetFollowersParameters {
    return (usernameOrUserIdOrUserParameters as IGetFollowersParameters).user !== undefined;
  }
}


// public GetFollowersParameters(IUserIdentifier user) : base(user)
// {
//   GetUsersPageSize = TwitterLimits.DEFAULTS.USERS_GET_USERS_MAX_SIZE;
// }
//
// public GetFollowersParameters(string username) : this(new UserIdentifier(username))
// {
// }
//
// public GetFollowersParameters(long userId) : this(new UserIdentifier(userId))
// {
// }
//
// public GetFollowersParameters(IGetFollowersParameters parameters) : base(parameters)
// {
//   GetUsersPageSize = TwitterLimits.DEFAULTS.USERS_GET_USERS_MAX_SIZE;
//
//   if (parameters == null)
//   {
//     return;
//   }
//
//   SkipStatus = parameters.SkipStatus;
//   IncludeEntities = parameters.IncludeEntities;
//   GetUsersPageSize = parameters.GetUsersPageSize;
// }
