using BookStoreApi.BusinessLogicLayer.Admin;
using BookStoreApi.RequestHandler.Admin.Mappers;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Comment;
using BookStoreApi.RequestHandler.Admin.Responses.Comment;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class CommentController(BLLComment bLL) : ApiResponseHelper
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
            if (comment is null) return ErrorResponse("نویسنده یافت نشد", null);
            return SuccessResponse("اطلاعات با موفقیت دریافت شد", comment.ToRComment());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid) return ErrorResponse("اطلاعات به درستی وارد نشده است", errors, 400);

            var (message, status) = await bLL.Delete(id);

            return status == 204
                ? SuccessResponse(message, null, status)
                : ErrorResponse(message, null, status);
        }

        [HttpPost("status/{id:int}")]
        public async Task<IActionResult> ChangeStatus([FromRoute] int id)
        {
            var (message, comment, status) = await bLL.SwitchIsConfirmed(id);

            return status == 201
                ? SuccessResponse(message, comment!.ToRComment(), status)
                : ErrorResponse(message, null, status);
        }
    }
}
