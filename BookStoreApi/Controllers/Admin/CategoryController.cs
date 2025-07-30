using BookStoreApi.BusinessLogicLayer.Admin;
using BookStoreApi.RequestHandler.Admin.Mappers;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Category;
using BookStoreApi.RequestHandler.Admin.Requests.Category;
using BookStoreApi.RequestHandler.Admin.Responses.Category;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class CategoryController(BLLCategory bLLCategory) : ApiResponseHelper
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] QCategoryGetAll query)
        {
            var (categories, pagination) = await bLLCategory.GetAllAsync(query);

            var rCategories = categories.Select(c => c.ToRCategory()).ToList();

            var response = new CategoryListResponse
            {
                Categories = rCategories,
                Pagination = pagination
            };

            return SuccessResponse("اطلاعات با موفقیت دریافت شد", response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var category = await bLLCategory.GetByIdAsync(id);
            if (category is null) return ErrorResponse("دسته بندی یافت نشد", null);
            return SuccessResponse("اطلاعات با موفقیت دریافت شد", category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest createCategoryRequest)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid)
                return ErrorResponse("اطلاعات به درستی وارد نشده است", errors, 400);

            var (message, category, status) = await bLLCategory.Create(createCategoryRequest);

            return status == 201
                ? SuccessResponse(message, category, status)
                : ErrorResponse(message, category, status);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCategoryRequest UCategory)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid) return ErrorResponse("اطلاعات به درستی وارد نشده است", errors, 400);

            var (message, category, status) = await bLLCategory.Update(id, UCategory);

            return status == 200
                ? SuccessResponse(message, category, status)
                : ErrorResponse(message, null, status);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid) return ErrorResponse("اطلاعات به درستی وارد نشده است", errors, 400);

            var (message, status) = await bLLCategory.Delete(id);

            return status == 204
                ? SuccessResponse(message, null, status)
                : ErrorResponse(message, null, status);
        }
    }
}
