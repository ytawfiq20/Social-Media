﻿

namespace SocialMedia.Data.Models
{
    public class React
    {
        public string Id { get; set; } = null!;
        public string ReactValue { get; set; } = null!;
        public List<PostReacts>? PostReacts { get; set; }
    }
}
