﻿

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Repository.PostRepository
{
    public interface IPostRepository
    {
        Task<PostDto> AddPostAsync(SiteUser user, Post post, List<PostImages> postImages);
        Task<PostDto> UpdatePostAsync(SiteUser user, Post post, List<PostImages> postImages);
        Task<bool> UpdateUserPostsPolicyToFriendsOnlyAsync
            (SiteUser user);
        Task<Post> UpdatePostPolicyAsync(SiteUser user, Post post);
        Task<Post> UpdatePostReactPolicyAsync(SiteUser user, Post post);
        Task<Post> UpdatePostCommentPolicyAsync(SiteUser user, Post post);
        Task<PostDto> DeletePostAsync(SiteUser user, string postId);
        Task<PostDto> GetPostByIdAsync(SiteUser user, string postId);
        Task<Post> GetPostByIdAsync(string postId);
        Task<Post> IsPostExistsAsync(string postId);
        Task<IEnumerable<PostDto>> GetUserPostsAsync(SiteUser user);
        Task<IEnumerable<PostDto>> GetUserPostsByPolicyAsync(SiteUser user, Policy policy);
        Task SaveChangesAsync();
    }
}