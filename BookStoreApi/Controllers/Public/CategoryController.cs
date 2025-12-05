using BookStoreApi.BusinessLogicLayer.Interfaces.Public;
using BookStoreApi.RequestHandler.Public.Mappers;
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
    }
}
