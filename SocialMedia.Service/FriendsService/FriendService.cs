﻿

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.BlockRepository;
using SocialMedia.Repository.FriendsRepository;
using SocialMedia.Repository.PolicyRepository;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Service.FriendsService
{
    public class FriendService : IFriendService
    {
        private readonly IFriendsRepository _friendsRepository;
        private readonly IBlockRepository _blockRepository;
        private readonly IPolicyRepository _policyRepository;
        public FriendService(IFriendsRepository _friendsRepository, IBlockRepository _blockRepository,
            IPolicyRepository _policyRepository)
        {
            this._friendsRepository = _friendsRepository;
            this._blockRepository = _blockRepository;
            this._policyRepository = _policyRepository;
        }
        public async Task<ApiResponse<Friend>> AddFriendAsync(AddFriendDto addFriendDto)
        {
            var existFriend = await _friendsRepository.GetFriendByUserAndFriendIdAsync(addFriendDto.UserId,
                addFriendDto.FriendId);
            if (existFriend == null)
            {
                var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                    addFriendDto.FriendId, addFriendDto.UserId);
                if (isBlocked == null)
                {
                    var newFriend = await _friendsRepository.AddFriendAsync(
                    ConvertFromDto.ConvertFromFriendtDto_Add(addFriendDto));
                    return StatusCodeReturn<Friend>
                        ._201_Created("Friend added successfully to your friend list", newFriend);
                }
                return StatusCodeReturn<Friend>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<Friend>
                    ._403_Forbidden("You are already friends");
        }

        public async Task<ApiResponse<Friend>> DeleteFriendAsync(string userId, string friendId)
        {
            if (userId != friendId)
            {
                var isYourFriend = await _friendsRepository.GetFriendByUserAndFriendIdAsync(userId, friendId);
                if (isYourFriend != null)
                {
                    await _friendsRepository.DeleteFriendAsync(userId, friendId);
                    return StatusCodeReturn<Friend>
                        ._200_Success("Friend deleted successfully", isYourFriend);
                }
                return StatusCodeReturn<Friend>
                        ._404_NotFound("Friend not in your friend list");
            }
            return StatusCodeReturn<Friend>
                ._403_Forbidden();
        }


        private async Task<ApiResponse<T>> CheckGetFriendPolicyAsync<T>(SiteUser user, SiteUser user1)
        {
            var policy = await _policyRepository.GetPolicyByIdAsync(user.FriendListPolicyId);
            if (policy != null)
            {
                if(user.Id != user1.Id)
                {
                    if (policy.PolicyType == "FRIENDS ONLY")
                    {
                        var isFriend = await IsUserFriendAsync(user.Id, user1.Id);
                        if (!isFriend.ResponseObject)
                        {
                            return StatusCodeReturn<T>
                                ._403_Forbidden();
                        }
                    }
                    else if (policy.PolicyType == "FRIENDS OF FRIENDS")
                    {
                        var isFriendOfFriend = await IsUserFriendOfFriendAsync(user.Id, user1.Id);
                        if (!isFriendOfFriend.ResponseObject)
                        {
                            return StatusCodeReturn<T>
                                ._403_Forbidden();
                        }
                    }

                    else if (policy.PolicyType == "PRIVATE")
                    {
                        return StatusCodeReturn<T>
                            ._403_Forbidden();
                    }
                }
                return StatusCodeReturn<T>
                    ._200_Success("Success");
            }
            return StatusCodeReturn<T>
                            ._404_NotFound("Policy not found");
        }


        public async Task<ApiResponse<IEnumerable<Friend>>> GetAllUserFriendsAsync(SiteUser user, 
            SiteUser user1)
        {
            var friends = await _friendsRepository.GetAllUserFriendsAsync(user.Id);
            if((await CheckGetFriendPolicyAsync<IEnumerable<Friend>>(user, user1)).IsSuccess)
            {
                if (friends.ToList().Count == 0)
                {
                    return StatusCodeReturn<IEnumerable<Friend>>
                        ._200_Success("No friends found");
                }
                return StatusCodeReturn<IEnumerable<Friend>>
                        ._200_Success("Friends found successfully", friends);
            }
            return await CheckGetFriendPolicyAsync<IEnumerable<Friend>>(user, user1);
        }

        public async Task<ApiResponse<IEnumerable<Friend>>> GetAllUserFriendsAsync(SiteUser user)
        {
            var friends = await _friendsRepository.GetAllUserFriendsAsync(user.Id);
            if (friends.ToList().Count == 0)
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
            foreach(var userFriends in friendsOfFriends)
            {
                foreach(var friend in userFriends)
                {
                    if(friend.FriendId == friendId || friend.UserId == friendId)
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
