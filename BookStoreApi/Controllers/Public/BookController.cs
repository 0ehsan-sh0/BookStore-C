using BookStoreApi.BusinessLogicLayer.Public;
using BookStoreApi.RequestHandler.Public.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.Public
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController(BLLBook bLL) : ApiResponseHelper
    {
        [HttpGet("new")]
        public async Task<IActionResult> GetAllAsync()
        {
            var books = await bLL.GetNewAsync();
            if (books is null)
                return ErrorResponse("خطا در بارگذاری اطلاعات", null, 500);

            var booksResponse = books.Select(b => b.ToPublicBookAllData()).ToList();

            return SuccessResponse("اطلاعات با موفقیت دریافت شد", booksResponse);
        }
    }
}
