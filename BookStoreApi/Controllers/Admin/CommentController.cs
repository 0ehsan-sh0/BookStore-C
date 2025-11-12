using BookStoreApi.BusinessLogicLayer.Interfaces.Admin;
using BookStoreApi.RequestHandler.Admin.Mappers;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Comment;
using BookStoreApi.RequestHandler.Admin.Responses.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class CommentController(IBLLComment bLL) : ApiResponseHelper
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] QCommentGetAll query)
        {
            var (comments, pagination) = await bLL.GetAllAsync(query);
            var rComments = comments.Select(c => c.ToRComment()).ToList();

            var response = new CommentListResponse
            {
                Comments = rComments,
                Pagination = pagination
            };

            return SuccessResponse("اطلاعات با موفقیت دریافت شد", response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var comment = await bLL.GetByIdAsync(id);
            if (comment is null)
                return NotFound("نظر مورد نظر یافت نشد");

            return SuccessResponse("اطلاعات با موفقیت دریافت شد", comment.ToRComment());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (message, status) = await bLL.Delete(id);

            return status switch
            {
                204 => NoContent(),
                404 => NotFound(message),
                _ => StatusCode(status, message)
            };
        }

        [HttpPost("status/{id:int}")]
        public async Task<IActionResult> ChangeStatus([FromRoute] int id)
        {
            var (message, comment, status) = await bLL.SwitchIsConfirmed(id);

            return status switch
            {
                201 => SuccessResponse(message, comment!.ToRComment(), status),
                404 => NotFound(message),
                500 => StatusCode(500, message),
                _ => StatusCode(status, message)
            };
        }
    }
}
