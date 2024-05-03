﻿

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    public class FriendRequestsConfiguration : IEntityTypeConfiguration<FriendRequest>
    {
        public void Configure(EntityTypeBuilder<FriendRequest> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.User).WithMany(e => e.FriendRequests).HasForeignKey(e => e.UserId);
            builder.Property(e => e.PersonId).IsRequired().HasColumnName("Friend Request Person Id");
            builder.HasIndex(e => e.PersonId).IsUnique();
            builder.Property(e => e.UserId).IsRequired().HasColumnName("User sended friend request Id");
            builder.Property(e => e.IsAccepted).IsRequired();
        }
    }
}