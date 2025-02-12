﻿namespace Chessbook.Web.Models
{
    using System;
    using Chessbook.Web.Models.Users;

    // TODO: delete
    public class UserDTO : UserIdentifierDTO
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

        public bool FollowedBy { get; set; }

        public override string ToString()
        {
            return base.ScreenName;
        }
    }
}
