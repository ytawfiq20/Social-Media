﻿

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;

namespace SocialMedia.Service.FriendsService
{
    public interface IFriendService
    {
        Task<ApiResponse<Friend>> AddFriendAsync(AddFriendDto addFriendDto);
        Task<ApiResponse<Friend>> DeleteFriendAsync(string userId, string friendId);
        Task<ApiResponse<bool>> IsUserFriendAsync(string userId, string friendId);
        Task<ApiResponse<bool>> IsUserFriendOfFriendAsync(string userId, string friendId);
        Task<ApiResponse<IEnumerable<Friend>>> GetAllUserFriendsAsync(string userId);
    }
}
