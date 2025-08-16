using BookStoreApi.BusinessLogicLayer.Admin;
using BookStoreApi.RequestHandler.Admin.Mappers;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Book;
using BookStoreApi.RequestHandler.Admin.Requests.Book;
using BookStoreApi.RequestHandler.Admin.Responses.Book;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController(BLLBook bLL) : ApiResponseHelper
    {
        [HttpPost]
        [RequestSizeLimit(2_000_000)]
        public async Task<IActionResult> Create([FromForm] CreateBookRequest createBookRequest)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid)
                return ErrorResponse("اطلاعات به درستی وارد نشده است", errors, 400);

            var (message, book, status) = await bLL.Create(createBookRequest);

            return status == 201
                ? SuccessResponse(message, book!.ToRBookAllData(), status)
                : ErrorResponse(message, null, status);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var book = await bLL.GetByIdAsync(id);
            if (book is null) return ErrorResponse("کتاب یافت نشد", null);


            return SuccessResponse("اطلاعات با موفقیت دریافت شد", book.ToRBookAllData());
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] QBookGetAll query)
        {
            var (books, pagination) = await bLL.GetAllAsync(query);
            if (books is null)
                return ErrorResponse("خطا در بارگذاری اطلاعات", null, 500);


            BookAllDataListResponse response = new BookAllDataListResponse
            {
                Books = books.Select(b => b.ToRBookAllData()).ToList(),
                Pagination = pagination
            };

            return SuccessResponse("اطلاعات با موفقیت دریافت شد", response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromBody] UpdateBookRequest updateBookRequest, [FromRoute] int id)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid)
                return ErrorResponse("اطلاعات به درستی وارد نشده است", errors, 400);

            var (message, book, status) = await bLL.Update(updateBookRequest, id);

            return status == 201
                ? SuccessResponse(message, book!.ToRBookAllData(), status)
                : ErrorResponse(message, null, status);
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
    }
}
