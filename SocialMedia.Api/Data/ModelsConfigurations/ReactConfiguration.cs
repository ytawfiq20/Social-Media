﻿
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Data.ModelsConfigurations
{
    public class ReactConfiguration : IEntityTypeConfiguration<React>
    {
        public void Configure(EntityTypeBuilder<React> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.ReactValue).IsRequired().HasColumnName("React Value");
            builder.HasIndex(e => e.ReactValue).IsUnique();
        }
    }
}
