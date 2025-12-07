using BookStoreApi.BusinessLogicLayer.Interfaces.Admin;
using BookStoreApi.RequestHandler.Admin.Mappers;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Book;
using BookStoreApi.RequestHandler.Admin.Requests.Book;
using BookStoreApi.RequestHandler.Admin.Responses.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class BookController(IBLLBook bLL) : ApiResponseHelper
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateBookRequest createBookRequest)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid)
                return BadRequest(errors);

            var (message, book, status) = await bLL.Create(createBookRequest);

            return status == 201
                ? SuccessResponse(message, book!.ToRBookAllData(), status)
                : StatusCode(status, message);
        }

        [HttpPost("Recommended/{id:int}")]
        public async Task<IActionResult> ToggleIsRecommended([FromRoute] int id)
        {
            var book = await bLL.ToggleIsRecommended(id);
            if (book is null) return NotFound();

            return SuccessResponse("با موفقیت تغییر داده شد", book.ToRBookAllData(), 201);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var book = await bLL.GetByIdAsync(id);
            if (book is null)
                return NotFound("کتاب یافت نشد");

            return SuccessResponse("اطلاعات با موفقیت دریافت شد", book.ToRBookAllData());
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] QBookGetAll query)
        {
            var (books, pagination) = await bLL.GetAllAsync(query);

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
                return BadRequest(errors);

            var (message, book, status) = await bLL.Update(updateBookRequest, id);

            return status == 201
                ? SuccessResponse(message, book!.ToRBookAllData(), status)
                : status == 404
                    ? NotFound(message)
                    : StatusCode(status, message);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var (message, status) = await bLL.Delete(id);

            return status switch
            {
                204 => NoContent(),
                404 => NotFound(message),
                _ => StatusCode(status, message)
            };
        }
    }
}
