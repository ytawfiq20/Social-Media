﻿

using Microsoft.AspNetCore.Identity;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.FriendRequestRepository;

namespace SocialMedia.Service.FriendRequestService
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly UserManager<SiteUser> _userManager;
        public FriendRequestService(IFriendRequestRepository _friendRequestRepository,
            UserManager<SiteUser> _userManager)
        {
            this._friendRequestRepository = _friendRequestRepository;
            this._userManager = _userManager;
        }

        public async Task<ApiResponse<FriendRequest>> AddFriendRequestAsync(FriendRequestDto friendRequestDto)
        {
            var user = await _userManager.FindByIdAsync(friendRequestDto.UserId);
            var person = await _userManager.FindByIdAsync(friendRequestDto.PersonId);
            if (user == null)
            {
                return new ApiResponse<FriendRequest>
                {
                    IsSuccess = false,
                    Message = "User not found",
                    StatusCode = 404
                };
            }
            else if (person == null)
            {
                return new ApiResponse<FriendRequest>
                {
                    IsSuccess = false,
                    Message = "Person not found",
                    StatusCode = 404
                };
            }

            var friendRequest = await _friendRequestRepository.GetFriendRequestByUserAndPersonIdAsync(
                friendRequestDto.UserId, friendRequestDto.PersonId);
            if (friendRequest != null)
            {
                return new ApiResponse<FriendRequest>
                {
                    IsSuccess = false,
                    Message = "Friend request already sent before please wait till user accept your friend request",
                    StatusCode = 400,
                };
            }
            var newFriendRequest = await _friendRequestRepository.AddFriendRequestAsync(
                ConvertFromDto.ConvertFromFriendRequestDto_Add(friendRequestDto));
            newFriendRequest.User = null;
            return new ApiResponse<FriendRequest>
            {
                IsSuccess = true,
                Message = "Friend request send successfully",
                StatusCode = 201,
                ResponseObject = newFriendRequest
            };

        }

        public async Task<ApiResponse<FriendRequest>> DeleteFriendRequestByAsync
            (SiteUser user, Guid friendRequestId)
        {
            var friendRequests = await _friendRequestRepository.GetAllFriendRequestsByUserIdAsync(user.Id);
            var friendRequest = await _friendRequestRepository.GetFriendRequestByIdAsync(friendRequestId);
            if (friendRequest == null)
            {
                return new ApiResponse<FriendRequest>
                {
                    IsSuccess = false,
                    Message = "Friend request not found",
                    StatusCode = 404
                };
            }
            if (!friendRequests.ToList().Contains(friendRequest)
                && !await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return new ApiResponse<FriendRequest>
                {
                    IsSuccess = true,
                    Message = "You haven't sent friend request to this user",
                    StatusCode = 400,
                };
            }
            await _friendRequestRepository.DeleteFriendRequestByAsync(friendRequestId);
            return new ApiResponse<FriendRequest>
            {
                IsSuccess = true,
                Message = "Friend request deleted successfully",
                StatusCode = 200,
            };
        }

        public async Task<ApiResponse<IEnumerable<FriendRequest>>> GetAllFriendRequestsAsync()
        {
            var friendRequsts = await _friendRequestRepository.GetAllFriendRequestsAsync();
            if (friendRequsts == null)
            {
                return new ApiResponse<IEnumerable<FriendRequest>>
                {
                    IsSuccess = true,
                    Message = "No friend requests found",
                    StatusCode = 200
                };
            }

            return new ApiResponse<IEnumerable<FriendRequest>>
            {
                IsSuccess = true,
                Message = "Friend requests found successfully",
                StatusCode = 200,
                ResponseObject = friendRequsts
            };
        }

        public async Task<ApiResponse<IEnumerable<FriendRequest>>> 
            GetAllFriendRequestsByUserIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ApiResponse<IEnumerable<FriendRequest>>
                {
                    IsSuccess = false,
                    Message = "User not found",
                    StatusCode = 404
                };
            }
            var userFriendRequsts = await _friendRequestRepository.GetAllFriendRequestsByUserIdAsync(userId);
            if (userFriendRequsts == null)
            {
                return new ApiResponse<IEnumerable<FriendRequest>>
                {
                    IsSuccess = true,
                    Message = "No friend requests found",
                    StatusCode = 200
                };
            }

            return new ApiResponse<IEnumerable<FriendRequest>>
            {
                IsSuccess = true,
                Message = "Friend requests found successfully",
                StatusCode = 200,
                ResponseObject = userFriendRequsts
            };
        }

        public async Task<ApiResponse<IEnumerable<FriendRequest>>> 
            GetAllFriendRequestsByUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new ApiResponse<IEnumerable<FriendRequest>>
                {
                    IsSuccess = false,
                    Message = "User not found",
                    StatusCode = 404
                };
            }
            var userFriendRequsts = await _friendRequestRepository.GetAllFriendRequestsByUserIdAsync(user.Id);
            if (userFriendRequsts == null)
            {
                return new ApiResponse<IEnumerable<FriendRequest>>
                {
                    IsSuccess = true,
                    Message = "No friend requests found",
                    StatusCode = 200
                };
            }

            return new ApiResponse<IEnumerable<FriendRequest>>
            {
                IsSuccess = true,
                Message = "Friend requests found successfully",
                StatusCode = 200,
                ResponseObject = userFriendRequsts
            };
        }

        public async Task<ApiResponse<FriendRequest>> GetFriendRequestByIdAsync(Guid friendRequestId)
        {
            var friendRequest = await _friendRequestRepository.GetFriendRequestByIdAsync(friendRequestId);
            if (friendRequest == null)
            {
                return new ApiResponse<FriendRequest>
                {
                    IsSuccess = false,
                    Message = "Friend request not found",
                    StatusCode = 404
                };
            }
            return new ApiResponse<FriendRequest>
            {
                IsSuccess = true,
                Message = "Friend request found successfully",
                StatusCode = 200,
                ResponseObject = friendRequest
            };
        }

        public async Task<ApiResponse<FriendRequest>> UpdateFriendRequestAsync(FriendRequestDto friendRequestDto)
        {
            var user = await _userManager.FindByIdAsync(friendRequestDto.UserId);
            var person = await _userManager.FindByIdAsync(friendRequestDto.PersonId);
            
            if (friendRequestDto.Id == null)
            {
                return new ApiResponse<FriendRequest>
                {
                    IsSuccess = false,
                    Message = "Friend request id must not be null",
                    StatusCode = 400
                };
            }
            var friendRequest = await _friendRequestRepository.GetFriendRequestByIdAsync(
                new Guid(friendRequestDto.Id));
            if (friendRequest == null)
            {
                return new ApiResponse<FriendRequest>
                {
                    IsSuccess = false,
                    Message = "Friend request not found",
                    StatusCode = 404
                };
            }
            if (user == null)
            {
                return new ApiResponse<FriendRequest>
                {
                    IsSuccess = false,
                    Message = "User not found",
                    StatusCode = 404
                };
            }
            if (person == null)
            {
                return new ApiResponse<FriendRequest>
                {
                    IsSuccess = false,
                    Message = "Person not found",
                    StatusCode = 404
                };
            }
            var updatedFriendRequest = await _friendRequestRepository.UpdateFriendRequestAsync(
                ConvertFromDto.ConvertFromFriendRequestDto_Update(friendRequestDto));
            updatedFriendRequest.User = null;
            return new ApiResponse<FriendRequest>
            {
                IsSuccess = true,
                Message = "Friend request updated successfully",
                StatusCode = 200,
                ResponseObject = updatedFriendRequest
            };
        }
    }
}
