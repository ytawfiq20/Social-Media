﻿

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Repository.FriendsRepository;
using SocialMedia.Service.FriendListPolicyService;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Service.FriendsService
{
    public class FriendService : IFriendService
    {
        private readonly IFriendsRepository _friendsRepository;
        private readonly IFriendListPolicyService _friendListPolicyService;
        public FriendService(IFriendsRepository _friendsRepository,
            IFriendListPolicyService _friendListPolicyService)
        {
            this._friendsRepository = _friendsRepository;
            this._friendListPolicyService = _friendListPolicyService;
        }
        public async Task<ApiResponse<Friend>> AddFriendAsync(
            AddFriendDto addFriendDto)
        {
            var existFriend = await _friendsRepository.GetFriendByUserAndFriendIdAsync(addFriendDto.UserId,
                addFriendDto.FriendId);
            if (existFriend != null)
            {
                return StatusCodeReturn<Friend>
                    ._400_BadRequest("You are already friends");
            }
            var userFriendList = await _friendsRepository.GetAllUserFriendsAsync(addFriendDto.UserId);
            var newFriend = await _friendsRepository.AddFriendAsync(
                ConvertFromDto.ConvertFromFriendtDto_Add(addFriendDto));
            newFriend.User = null;
            return StatusCodeReturn<Friend>
                ._201_Created("Friend added successfully to your friend list", newFriend);
        }

        public async Task<ApiResponse<Friend>> DeleteFriendAsync(string userId, string friendId)
        {
            var isYourFriend = await _friendsRepository.GetFriendByUserAndFriendIdAsync(userId, friendId);
            if (isYourFriend == null)
            {
                return StatusCodeReturn<Friend>
                    ._404_NotFound("Friend not in your friend list");
            }
            var deletedFriend = await _friendsRepository.DeleteFriendAsync(userId, friendId);
            if (deletedFriend == null)
            {
                return StatusCodeReturn<Friend>
                    ._500_ServerError("Can't delete friend");
            }
            deletedFriend.User = null;
            return StatusCodeReturn<Friend>
                ._200_Success("Friend deleted successfully", deletedFriend);
        }

        public async Task<ApiResponse<IEnumerable<Friend>>> GetAllUserFriendsAsync(string userId)
        {
            var friends = await _friendsRepository.GetAllUserFriendsAsync(userId);
            foreach(var friend in friends)
            {
                friend.User = null;
            }
            if (friends.ToList().Count==0)
            {
                return StatusCodeReturn<IEnumerable<Friend>>
                    ._200_Success("No friends found");
            }
            return StatusCodeReturn<IEnumerable<Friend>>
                    ._200_Success("Friends found successfully", friends);
        }

        public async Task<ApiResponse<bool>> IsUserFriendAsync(string userId, string friendId)
        {
            var check = await _friendsRepository.GetFriendByUserAndFriendIdAsync(userId, friendId);
            if (check == null)
            {
                return StatusCodeReturn<bool>
                    ._404_NotFound("Not friend", false);
            }
            return StatusCodeReturn<bool>
                    ._200_Success("Friend", true);
        }

        public async Task<ApiResponse<bool>> IsUserFriendOfFriendAsync(string userId, string friendId)
        {
            var friendsOfFriends = (await _friendsRepository.GetUserFriendsOfFriendsAsync(userId)).ToList();
            var friend = await _friendsRepository.GetFriendByUserAndFriendIdAsync(userId, friendId);
            if (friend != null)
            {
                foreach (var f in friendsOfFriends)
                {
                    if (f.Contains(friend))
                    {
                        return StatusCodeReturn<bool>
                            ._200_Success("Friend of friend", true);
                    }
                }
            }

            return StatusCodeReturn<bool>
                    ._404_NotFound("Not friend of friend", false);
        }
    }
}
