﻿namespace WorldFeed.History.API.Models.Media
{
    public class ImageResponseModel : MediaInitResponseModel
    {
        public long Size { get; set; }

        public Image Image { get; set; }
    }
}
