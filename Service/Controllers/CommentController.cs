﻿using System;
using System.Text.Json;
using System.Threading.Tasks;
using Management.DomainModels;
using Management.Ports;
using Microsoft.AspNetCore.Mvc;
using ApiComment = Management.ApiModels.Comment;
using ApiUserId = Management.ApiModels.UserId;
using DomainUserId = Management.DomainModels.UserId;

namespace Service.Controllers
{
    /// <summary>
    /// Controller for the user commenting system for this safe-travel service.
    /// </summary>
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly CommentPort _commentPort;

        public CommentController(CommentPort commentPort)
        {
            _commentPort = commentPort ?? throw new ArgumentNullException(nameof(commentPort));
        }

        /// <summary>
        /// Intended to get the comments for the specified country and state for the user. 
        /// </summary>
        // TODO: @mli: Get apiUserId from auth token instead of from body latter.
        [HttpGet]
        [Route("country/{countryCode}/state/{stateCode}")]
        public async Task<IActionResult> GetCommentByLocation([FromBody] ApiUserId apiUserId, [FromRoute] string countryCode, [FromRoute] string stateCode)
        {
            Console.WriteLine($"GetCommentByLocation: userId: {apiUserId.UserIdStr}. countryCode: {countryCode}, stateCode: {stateCode}");
            try
            {
                return Ok(await _commentPort.GetCommentAsync(DomainUserId.Wrap(apiUserId.UserIdStr), new Location(Country.Wrap(countryCode), State.Wrap(stateCode))));
            }
            catch (Exception e)
            {
                Console.WriteLine($"GetComment caught exception: {e.Message}");
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Intended to post the user's comments for this specific country and state.
        /// </summary>
        [HttpPost]
        [Route("country/{countryCode}/state/{stateCode}")]
        public async Task<IActionResult> CreateNewComment([FromBody] ApiComment apiComment, [FromRoute] string countryCode, [FromRoute] string stateCode)
        {
            Console.WriteLine($"CreateNewComment: countryCode: {countryCode}, stateCode: {stateCode}, body: {JsonSerializer.Serialize(apiComment)}");
            try
            {
                await _commentPort.AddCommentAsync(new Location(Country.Wrap(countryCode), State.Wrap(stateCode)), apiComment);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine($"CreateNewComment caught exception: {e.Message}");
                return NotFound(e.Message);
            }
        }
    }
}
