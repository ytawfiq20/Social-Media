﻿
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Repository.CommentPolicyRepository;
using SocialMedia.Repository.PolicyRepository;
using SocialMedia.Service.GenericReturn;
using SocialMedia.Service.PolicyService;

namespace SocialMedia.Service.CommentPolicyService
{
    public class CommentPolicyService : ICommentPolicyService
    {
        private readonly ICommentPolicyRepository _commentPolicyRepository;
        private readonly IPolicyRepository _policyRepository;
        private readonly IPolicyService _policyService;
        public CommentPolicyService(ICommentPolicyRepository _commentPolicyRepository,
            IPolicyRepository _policyRepository, IPolicyService _policyService)
        {
            this._commentPolicyRepository = _commentPolicyRepository;
            this._policyRepository = _policyRepository;
            this._policyService = _policyService;
        }
        public async Task<ApiResponse<CommentPolicy>> AddCommentPolicyAsync(
            AddCommentPolicyDto addCommentPolicyDto)
        {
            var policy = await GetPolicyByIdOrNameAsync(addCommentPolicyDto.PolicyIdOrName);
            if (policy == null)
            {
                return StatusCodeReturn<CommentPolicy>
                    ._404_NotFound("Policy not found");
            }
            var commentPolicy = await _commentPolicyRepository.GetCommentPolicyByPolicyIdAsync
                (policy.Id);
            if (commentPolicy != null)
            {
                return StatusCodeReturn<CommentPolicy>
                    ._403_Forbidden("Comment policy already exists");
            }
            addCommentPolicyDto.PolicyIdOrName = policy.Id;
            var newComentPolicy = await _commentPolicyRepository.AddCommentPolicyAsync(
                ConvertFromDto.ConvertFromCommentPolicyDto_Add(addCommentPolicyDto));
            return StatusCodeReturn<CommentPolicy>
                ._200_Success("Comment policy added successfully", newComentPolicy);
        }

        public async Task<ApiResponse<CommentPolicy>> DeleteCommentPolicyByIdAsync(string commentPolicyId)
        {
            var commentPolicy = await _commentPolicyRepository.GetCommentPolicyByIdAsync(commentPolicyId);
            if (commentPolicy == null)
            {
                return StatusCodeReturn<CommentPolicy>
                    ._404_NotFound("Comment policy not found");
            }
            var deletedCommentPolicy = await _commentPolicyRepository.DeleteCommentPolicyByIdAsync(commentPolicyId);
            return StatusCodeReturn<CommentPolicy>
                ._200_Success("Comment policy deleted successfully", deletedCommentPolicy);
        }

        public async Task<ApiResponse<CommentPolicy>> GetCommentPolicyByIdAsync(string commentPolicyId)
        {
            var commentPolicy = await _commentPolicyRepository.GetCommentPolicyByIdAsync(commentPolicyId);
            if (commentPolicy == null)
            {
                return StatusCodeReturn<CommentPolicy>
                    ._404_NotFound("Comment policy not found");
            }
            return StatusCodeReturn<CommentPolicy>
                ._200_Success("Comment policy found successfully", commentPolicy);
        }

        public async Task<ApiResponse<IEnumerable<CommentPolicy>>> GetCommentPoliciesAsync()
        {
            var commentPolicies = await _commentPolicyRepository.GetCommentPoliciesAsync();
            if (commentPolicies.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<CommentPolicy>>
                    ._200_Success("No comment policies found", commentPolicies);
                    
            }
            return StatusCodeReturn<IEnumerable<CommentPolicy>>
                    ._200_Success("Comment policies found successfully", commentPolicies);
        }

        public async Task<ApiResponse<CommentPolicy>> UpdateCommentPolicyAsync
            (UpdateCommentPolicyDto updateCommentPolicyDto)
        {
            var policy = await GetPolicyByIdOrNameAsync(updateCommentPolicyDto.PolicyIdOrName);
            if (policy == null)
            {
                return StatusCodeReturn<CommentPolicy>
                    ._404_NotFound("Policy not found");
            }
            var commentPolicy = await _commentPolicyRepository
                .GetCommentPolicyByPolicyIdAsync(policy.Id);
            if (commentPolicy == null)
            {
                return StatusCodeReturn<CommentPolicy>
                    ._404_NotFound("Comment policy not found");
            }
            var existCommentPolicy = await _commentPolicyRepository.GetCommentPolicyByPolicyIdAsync(
                policy.Id);
            if (existCommentPolicy != null)
            {
                return StatusCodeReturn<CommentPolicy>._403_Forbidden("Comment policy already exists");
            }
            var updatedComentPolicy = await _commentPolicyRepository.UpdateCommentPolicyAsync(
                ConvertFromDto.ConvertFromCommentPolicyDto_Update(updateCommentPolicyDto));
            return StatusCodeReturn<CommentPolicy>
                ._200_Success("Comment policy updated successfully", updatedComentPolicy);
        }

        public async Task<ApiResponse<CommentPolicy>> GetCommentPolicyAsync(string commentPolicyIdOrPolicyName)
        {
            var policy = await _policyRepository.GetPolicyByNameAsync(commentPolicyIdOrPolicyName);
            CommentPolicy commentPolicy = null!;
            if (policy != null)
            {
                commentPolicy = await _commentPolicyRepository
                .GetCommentPolicyByPolicyIdAsync(policy.Id);
                if (commentPolicy != null)
                {
                    return StatusCodeReturn<CommentPolicy>._200_Success(
                        "Comment policy found successfully", commentPolicy);
                }
                return StatusCodeReturn<CommentPolicy>._404_NotFound("Comment policy not found");
            }
            commentPolicy = await _commentPolicyRepository
                .GetCommentPolicyByIdAsync(commentPolicyIdOrPolicyName);
            if (commentPolicy != null)
            {
                return StatusCodeReturn<CommentPolicy>._200_Success(
                        "Comment policy found successfully", commentPolicy);
            }
            Guid _;
            bool isValid = Guid.TryParse(commentPolicyIdOrPolicyName, out _);
            if (isValid)
            {
                return StatusCodeReturn<CommentPolicy>._404_NotFound(
                        "Comment policy not found");
            }
            return StatusCodeReturn<CommentPolicy>._404_NotFound(
                        "Policy not found");
        }


        private async Task<Policy> GetPolicyByIdOrNameAsync(string policyIdOrName)
        {
            var policyById = await _policyRepository.GetPolicyByIdAsync(policyIdOrName);
            var policyByName = await _policyRepository.GetPolicyByNameAsync(policyIdOrName);
            return policyById == null ? policyByName! : policyById;
        }


    }
}
