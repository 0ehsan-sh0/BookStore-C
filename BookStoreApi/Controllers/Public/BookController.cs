using BookStoreApi.BusinessLogicLayer.Interfaces.Public;
using BookStoreApi.RequestHandler.Public.Mappers;
using BookStoreApi.RequestHandler.Public.QueryObjects.Book;
using BookStoreApi.RequestHandler.Public.QueryObjects.Comment;
using BookStoreApi.RequestHandler.Public.Responses.Book;
using BookStoreApi.RequestHandler.Public.Responses.Comment;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.Public
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController(IBLLBookPublic bLL) : ApiResponseHelper
    {
        [HttpGet]
        public async Task<IActionResult> GetNewAsync([FromQuery] QPBookGetAll query)
        {
            var (books, info) = await bLL.GetNewAsync(query.PageSize, query.PageNumber, query.IsRecommended);
            if (books is null)
                return StatusCode(500, "خطا در بارگزاری اطلاعات");

            BookAllDataListResponse response = new BookAllDataListResponse
            {
                Books = books.Select(b => b.ToPublicBookAllData()).ToList(),
                Pagination = info
            };

            return SuccessResponse("اطلاعات با موفقیت دریافت شد", response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var book = await bLL.GetByIdAsync(id);
            if (book is null) return NotFound("کتاب یافت نشد");
            return SuccessResponse("اطلاعات با موفقیت دریافت شد", book.ToPublicBookAllData());
        }

        [HttpGet("{bookId:int}/comments")]
        public async Task<IActionResult> GetBookComments(
    [FromRoute] int bookId,
    [FromQuery] QBookComments query)
        {
            var (message, comments, info, status) =
                await bLL.GetBookComments(bookId, query.PageNumber, query.PageSize);

            var commentList = new CommentListResponse
            {
                Comments = comments?.Select(c => c.ToPublicComment()).ToList(),
                Pagination = info
            };

            return status == 200 ?
                SuccessResponse(message, commentList) : StatusCode(status, message);
        }
    }


}
