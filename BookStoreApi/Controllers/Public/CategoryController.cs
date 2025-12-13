using BookStoreApi.BusinessLogicLayer.Interfaces.Public;
using BookStoreApi.RequestHandler.Public.Mappers;
using BookStoreApi.RequestHandler.Public.QueryObjects.Book;
using BookStoreApi.RequestHandler.Public.Responses.Book;
using BookStoreApi.RequestHandler.Public.Responses.Category;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.Public
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(IBLLCategoryPublic bLL) : ApiResponseHelper
    {
        [HttpGet]
        public async Task<IActionResult> GetCategoriesList()
        {
            var categories = await bLL.GetCategoriesWithSubAsync();
            if (categories is null)
                return StatusCode(500, "خطا در بارگزاری اطلاعات");

            return SuccessResponse(
                "اطلاعات با موفقیت دریافت شد",
                categories.Select(c => c.ToPublicCategory()).ToList()
                );
        }

        [HttpGet("{url}")]
        public async Task<IActionResult> GetCategoryAsync(
           [FromRoute] string url,
           [FromQuery] QCategoryBooks query)
        {
            var (message, category, pagination, status) = await bLL.GetTagAsync(url, query);

            var categoryDetails = new CategoryDetails
            {
                Category = category?.ToPublicCategory(),
                Books =
                    new BookAllDataListResponse
                    {
                        Books = category?.Books?.Select(b => b.ToPublicBookAllData()).ToList(),
                        Pagination = pagination
                    }
            };
            return status == 200 ?
                SuccessResponse(message, categoryDetails) : StatusCode(status, message);
        }
    }
}
