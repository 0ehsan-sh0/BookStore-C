using BookStoreApi.BusinessLogicLayer.Admin;
using BookStoreApi.RequestHandler.Admin.Requests.Book;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController(BLLBook bLL) : ApiResponseHelper
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateBookRequest createBookRequest)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid)
                return ErrorResponse("اطلاعات به درستی وارد نشده است", errors, 400);

            var (message, book, status) = await bLL.Create(createBookRequest);

            return status == 201
                ? SuccessResponse(message, book, status)
                : ErrorResponse(message, book, status);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var book = await bLL.GetByIdAsync(id);
            if (book is null) return ErrorResponse("کتاب یافت نشد", null);
            return SuccessResponse("اطلاعات با موفقیت دریافت شد", book);
        }
    }
}
