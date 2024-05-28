﻿

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class SavePostDto
    {
        [Required]
        public string PostId { get; set; } = null!;

        [Required]
        public string FolderId { get; set; } = null!;
    }
}
