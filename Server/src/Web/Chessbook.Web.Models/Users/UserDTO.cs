﻿namespace Chessbook.Web.Models
{
    using Newtonsoft.Json;
    using Chessbook.Data.Models;
    using Chessbook.Services.Mapping;
    using Chessbook.Web.Models.Outputs.Posts;
    using Chessbook.Web.Models.Users;
    using System;
    using System.Collections.Generic;
    using AutoMapper;

    public class UserDTO : UserIdentifierDTO, IMapFrom<User>
    {
        public string DisplayName { get; set; }

        public string Email { get; set; }

        public int? Age { get; set; }

        public SettingsDTO Settings { get; set; }

        // Verify : ProfileImageTile

        public string Name { get; set; }

        //[JsonProperty("status")]
        //public PostDTO Status { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; } // DateTimeOffset

        public string Location { get; set; }

        public bool? GeoEnabled { get; set; }

        public string Url { get; set; }

        public int StatusesCount { get; set; }

        public int FollowersCount { get; set; }

        public int FriendsCount { get; set; }

        public bool? Following { get; set; }

        public bool Protected { get; set; }

        public bool Verified { get; set; }

        //[JsonProperty("entities")]
        //public IUserEntities Entities { get; set; }

        public bool? Notifications { get; set; }

        public string ProfileImageUrlHttps { get; set; }

        public bool? FollowRequestSent { get; set; }

        public bool DefaultProfile { get; set; }

        public bool DefaultProfileImage { get; set; }

        public int FavoritesCount { get; set; }

        public int ListedCount { get; set; }

         public string ProfileSidebarFillColor { get; set; }

        public string ProfileSidebarBorderColor { get; set; }

        public bool ProfileBackgroundTile { get; set; }

        public string ProfileBackgroundColor { get; set; }

        public string ProfileBackgroundImageUrl { get; set; }

        public string ProfileBackgroundImageUrlHttps { get; set; }

        public string ProfileBannerURL { get; set; }

        public string ProfileTextColor { get; set; }

        public string ProfileLinkColor { get; set; }

        public bool ProfileUseBackgroundImage { get; set; }

        public bool IsTranslator { get; set; }

        public bool ContributorsEnabled { get; set; }

        public int? UtcOffset { get; set; }

        public string TimeZone { get; set; }

        //[JsonProperty("withheld_in_countries")]
        //public IEnumerable<string> WithheldInCountries { get; set; }

        //[JsonProperty("withheld_scope")]
        //public string WithheldScope { get; set; }

        [JsonIgnore]
        public int ProfilePictureId { get; set; }

        public bool FollowedBy { get; set; }

        public override string ToString()
        {
            return base.ScreenName;
        }
    }
}
