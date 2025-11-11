using BookStoreApi.BusinessLogicLayer.Public;
using BookStoreApi.RequestHandler.Public.Mappers;
using BookStoreApi.RequestHandler.Public.QueryObjects.Book;
using BookStoreApi.RequestHandler.Public.Responses.Book;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.Public
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController(BLLBook bLL) : ApiResponseHelper
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
    }


}
