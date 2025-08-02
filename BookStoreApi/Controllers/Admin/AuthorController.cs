using BookStoreApi.BusinessLogicLayer.Admin;
using BookStoreApi.RequestHandler.Admin.Mappers;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Author;
using BookStoreApi.RequestHandler.Admin.Requests.Author;
using BookStoreApi.RequestHandler.Admin.Responses.Author;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController(BLLAuthor bLL) : ApiResponseHelper
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] QAuthorGetAll query)
        {
            var (Authors, pagination) = await bLL.GetAllAsync(query);

            var rAuthors = Authors.Select(c => c.ToRAuthor()).ToList();

            var response = new AuthorListResponse
            {
                Authors = rAuthors,
                Pagination = pagination
            };

            return SuccessResponse("اطلاعات با موفقیت دریافت شد", response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var author = await bLL.GetByIdAsync(id);
            if (author is null) return ErrorResponse("نویسنده یافت نشد", null);
            return SuccessResponse("اطلاعات با موفقیت دریافت شد", author);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAuthorRequest createAuthorRequest)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid)
                return ErrorResponse("اطلاعات به درستی وارد نشده است", errors, 400);

            var (message, author, status) = await bLL.Create(createAuthorRequest);

            return status == 201
                ? SuccessResponse(message, author, status)
                : ErrorResponse(message, author, status);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateAuthorRequest UAuthor)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid) return ErrorResponse("اطلاعات به درستی وارد نشده است", errors, 400);

            var (message, author, status) = await bLL.Update(id, UAuthor);
            return status == 200
                ? SuccessResponse(message, author, status)
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
