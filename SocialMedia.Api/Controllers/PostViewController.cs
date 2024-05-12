﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Repository.PostRepository;
using SocialMedia.Repository.PostViewRepository;

namespace SocialMedia.Api.Controllers
{
    [ApiController]
    public class PostViewController : ControllerBase
    {
        private readonly IPostViewRepository _postViewRepository;
        private readonly IPostRepository _postRepository;
        public PostViewController(IPostViewRepository _postViewRepository, IPostRepository _postRepository)
        {
            this._postViewRepository = _postViewRepository;
            this._postRepository = _postRepository;
        }

        [HttpGet("postView/{postId}")]
        public async Task<IActionResult> GetPostViewsAsync([FromRoute] string postId)
        {
            try
            {
                var post = await _postRepository.GetPostByIdAsync(postId);
                if (post != null)
                {
                    var postView = await _postViewRepository.GetPostViewByPostIdAsync(postId);
                    return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>
                    {
                        StatusCode = 200,
                        IsSuccess = true,
                        Message = "Post views found successfully",
                        ResponseObject = $"Post views {postView.ViewNumber}"
                    });
                }
                return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>
                {
                    StatusCode = 404,
                    IsSuccess = false,
                    Message = "Post not found"
                });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }


    }
}