using BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel;
using BookStoreApi.RequestHandler.Public.Mappers;
using BookStoreApi.RequestHandler.User.Mappers;
using BookStoreApi.RequestHandler.User.QueryObjects.Comment;
using BookStoreApi.RequestHandler.User.Requests.Comment;
using BookStoreApi.RequestHandler.User.Responses.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.User
{
    [Route("api/user/comment")]
    [ApiController]
    [Authorize(Roles = "User,Admin")]
    public class UserCommentController(IBLLUserComment bLL) : ApiResponseHelper
    {
        [HttpGet]
        public async Task<IActionResult> GetUserCommentsAsync([FromQuery] QUserComments query)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid)
                return BadRequest(errors);

            var userMobile = User.Identity?.Name!;
            var (message, comments, pagination, status) =
                await bLL.GetUserCommentsAsync(userMobile, query);

            return status == 200 ?
                SuccessResponse(message, new CommentListResponse
                {
                    Comments = comments!.Select(c => c.ToRComment()).ToList(),
                    Pagination = pagination
                }, status) :
                StatusCode(status, message);
        }

        [HttpPost("{bookId:int}")]
        public async Task<IActionResult> CreateCommentAsync(
            [FromRoute] int bookId,
            [FromBody] CreateCommentRequest request)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid)
                return BadRequest(errors);

            var userMobile = User.Identity?.Name!;
            var (message, comment, status) =
                await bLL.CreateAsync(userMobile, bookId, request);

            return status == 201 ?
                SuccessResponse(message, comment!.ToPublicComment(), status) :
                StatusCode(status, message);
        }
    }
}
