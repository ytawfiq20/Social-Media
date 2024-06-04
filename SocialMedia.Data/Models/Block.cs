﻿

using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class Block
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string BlockedUserId { get; set; } = null!;
        public SiteUser? User { get; set; }
    }
}
