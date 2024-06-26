﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.PolicyService;

namespace SocialMedia.Api.Controllers
{
    
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyService _policyService;
        public PolicyController(IPolicyService _policyService)
        {
            this._policyService = _policyService;
        }

        [Authorize(Roles ="User")]
        [HttpGet("policies")]
        public async Task<IActionResult> GetPoliciesAsync()
        {
            try
            {
                var response = await _policyService.GetPoliciesAsync();
                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpPost("addPolicy")]
        public async Task<IActionResult> AddPolicyAsync([FromBody] AddPolicyDto addPolicyDto)
        {
            try
            {
                var response = await _policyService.AddPolicyAsync(addPolicyDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpPut("updatePolicy")]
        public async Task<IActionResult> UpdatePolicyAsync([FromBody] UpdatePolicyDto updatePolicyDto)
        {
            try
            {
                var response = await _policyService.UpdatePolicyAsync(updatePolicyDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("policyById/{policyId}")]
        public async Task<IActionResult> GetPolicyByIdAsync([FromRoute] string policyId)
        {
            try
            {
                var response = await _policyService.GetPolicyByIdAsync(policyId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("policyByName/{policyName}")]
        public async Task<IActionResult> GetPolicyByNameAsync([FromRoute]string policyName)
        {
            try
            {
                var response = await _policyService.GetPolicyByNameAsync(policyName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("policy/{policyIdOrName}")]
        public async Task<IActionResult> GetPolicyByIdOrNameAsync([FromRoute] string policyIdOrName)
        {
            try
            {
                var response = await _policyService.GetPolicyByIdOrNameAsync(policyIdOrName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpDelete("deletePolicyById/{policyId}")]
        public async Task<IActionResult> DeletePolicyByIdAsync([FromRoute] string policyId)
        {
            try
            {
                var response = await _policyService.DeletePolicyByIdAsync(policyId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpDelete("deletePolicyByName/{policyName}")]
        public async Task<IActionResult> DeletePolicyByNameAsync([FromRoute] string policyName)
        {
            try
            {
                var response = await _policyService.DeletePolicyByNameAsync(policyName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpDelete("deletePolicy/{policyIdOrName}")]
        public async Task<IActionResult> DeletePolicyByIdOrNameAsync([FromRoute] string policyIdOrName)
        {
            try
            {
                var response = await _policyService.DeletePolicyByIdOrNameAsync(policyIdOrName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }


    }
}
