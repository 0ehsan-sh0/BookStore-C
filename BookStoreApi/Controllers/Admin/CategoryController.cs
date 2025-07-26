using BookStoreApi.Database.Interfaces;
using BookStoreApi.RequestHandler.Admin.Mappers;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Category;
using BookStoreApi.RequestHandler.Admin.Requests.Category;
using BookStoreApi.RequestHandler.Admin.Responses.Category;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryRepository repo) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] QCategoryGetAll query)
        {
            var (categories, pagination) = await repo.GetAllAsync(query);

            var rCategories = categories.Select(c => c.ToRCategory()).ToList();

            var response = new CategoryListResponse
            {
                Categories = rCategories,
                Pagination = pagination
            };

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var category = await repo.GetByIdAsync(id);
            if (category is null) return NotFound();
            return Ok(category.ToRCategory());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest createCategoryRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var category = createCategoryRequest.ToCategory();

            if (category.MainCategoryId is int parentId)
            {
                var existingCategory = await repo.GetByIdAsync(parentId);
                if (existingCategory is null)
                    return NotFound();
            }

            var urlCategory = await repo.GetByUrlAsync(category.Url);
            if (urlCategory is not null) return BadRequest(new
            {
                errors = new
                {
                    Url = new[] { "لینک وارد شده تکراری است" }
                }
            });

            int id = await repo.CreateAsync(category);
            category = await repo.GetByIdAsync(id);

            return Created($"api/admin/category/{id}", category!.ToRCategory());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCategoryRequest UCategory)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var category = await repo.GetByIdAsync(id);
            if (category is null)
                return NotFound();

            if (UCategory.MainCategoryId is int parentId)
            {
                var existingCategory = await repo.GetByIdAsync(parentId);
                if (existingCategory is null)
                    return NotFound();
            }

            var urlCategory = await repo.GetByUrlAsync(category.Url);
            if (urlCategory is not null)
            {
                if (!(urlCategory.Url == UCategory.Url))
                    return BadRequest(new
                    {
                        errors = new
                        {
                            Url = new[] { "لینک وارد شده تکراری است" }
                        }
                    });
            }

            category = await repo.UpdateAsync(UCategory.ToCategory(id));

            return Ok(category!.ToRCategory());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingCategory = await repo.GetByIdAsync(id);
            if (existingCategory is null)
                return NotFound();

            var category = await repo.DeleteAsync(id);
            if (!category) return NotFound();

            return NoContent();
        }
    }
}
