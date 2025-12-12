using BookStoreApi.BusinessLogicLayer.Interfaces.Public;
using BookStoreApi.RequestHandler.Public.Mappers;
using BookStoreApi.RequestHandler.Public.QueryObjects.Book;
using BookStoreApi.RequestHandler.Public.Responses.Author;
using BookStoreApi.RequestHandler.Public.Responses.Book;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.Public
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController(IBLLAuthorPublic bLL) : ApiResponseHelper
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAuthorAsync(
            [FromRoute] int id,
            [FromQuery] QAuthorBooks query)
        {
            var (message, author, pagination, status) = await bLL.GetAuthorAsync(id, query);

            var authorDetails = new AuthorDetails
            {
                Author = author?.ToPublicAuthor(),
                Books =
                    new BookAllDataListResponse
                    {
                        Books = author?.Books?.Select(b => b.ToPublicBookAllData()).ToList(),
                        Pagination = pagination
                    }
            };
            return status == 200 ?
                SuccessResponse(message, authorDetails) : StatusCode(status, message);
        }
    }
}
