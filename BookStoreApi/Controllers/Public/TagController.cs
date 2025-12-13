using BookStoreApi.BusinessLogicLayer.Interfaces.Public;
using BookStoreApi.RequestHandler.Public.Mappers;
using BookStoreApi.RequestHandler.Public.QueryObjects.Book;
using BookStoreApi.RequestHandler.Public.Responses.Book;
using BookStoreApi.RequestHandler.Public.Responses.Tag;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.Public
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController(IBLLTagPublic bLL) : ApiResponseHelper
    {
        [HttpGet("{url}")]
        public async Task<IActionResult> GetTagAsync(
           [FromRoute] string url,
           [FromQuery] QTagBooks query)
        {
            var (message, tag, pagination, status) = await bLL.GetTagAsync(url, query);

            var tagDetails = new TagDetails
            {
                Tag = tag?.ToPublicTag(),
                Books =
                    new BookAllDataListResponse
                    {
                        Books = tag?.Books?.Select(b => b.ToPublicBookAllData()).ToList(),
                        Pagination = pagination
                    }
            };
            return status == 200 ?
                SuccessResponse(message, tagDetails) : StatusCode(status, message);
        }
    }
}
