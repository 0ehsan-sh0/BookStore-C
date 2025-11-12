using BookStoreApi.BusinessLogicLayer.Interfaces.Admin;
using BookStoreApi.RequestHandler.Admin.Mappers;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Author;
using BookStoreApi.RequestHandler.Admin.Requests.Author;
using BookStoreApi.RequestHandler.Admin.Responses.Author;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class AuthorController(IBLLAuthor bLL) : ApiResponseHelper
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
            if (author is null)
                return NotFound("نویسنده یافت نشد");

            return SuccessResponse("اطلاعات با موفقیت دریافت شد", author.ToRAuthor());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAuthorRequest createAuthorRequest)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid)
                return BadRequest(errors);

            var (message, author, status) = await bLL.Create(createAuthorRequest);

            return status == 201
                ? SuccessResponse(message, author!.ToRAuthor(), status)
                : StatusCode(status, message);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateAuthorRequest UAuthor)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid)
                return BadRequest(errors);

            var (message, author, status) = await bLL.Update(id, UAuthor);

            return status == 200
                ? SuccessResponse(message, author!.ToRAuthor(), status)
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
