using BookStoreApi.BusinessLogicLayer.Interfaces.Admin;
using BookStoreApi.RequestHandler.Admin.Mappers;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Category;
using BookStoreApi.RequestHandler.Admin.Requests.Category;
using BookStoreApi.RequestHandler.Admin.Responses.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class CategoryController(IBLLCategory bLL) : ApiResponseHelper
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] QCategoryGetAll query)
        {
            var (categories, pagination) = await bLL.GetAllAsync(query);

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
            var category = await bLL.GetByIdAsync(id);
            if (category is null)
                return NotFound("دسته بندی یافت نشد");

            return SuccessResponse("اطلاعات با موفقیت دریافت شد", category.ToRCategory());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest createCategoryRequest)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid)
                return BadRequest(errors);

            var (message, category, status) = await bLL.Create(createCategoryRequest);

            return status == 201
                ? SuccessResponse(message, category!.ToRCategory(), status)
                : StatusCode(status, message);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCategoryRequest UCategory)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid)
                return BadRequest(errors);

            var (message, category, status) = await bLL.Update(id, UCategory);

            return status == 200
                ? SuccessResponse(message, category!.ToRCategory(), status)
                : status == 404
                    ? NotFound(message)
                    : StatusCode(status, message);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid) return BadRequest(errors);

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
