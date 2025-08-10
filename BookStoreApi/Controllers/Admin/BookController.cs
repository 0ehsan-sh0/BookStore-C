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

            var (message, author, status) = await bLL.Create(createBookRequest);

            return status == 201
                ? SuccessResponse(message, author, status)
                : ErrorResponse(message, author, status);
        }
    }
}
