using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.Mappers;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Category;
using BookStoreApi.RequestHandler.Admin.Requests.Category;
using BookStoreApi.RequestHandler.Admin.Responses.Category;

namespace BookStoreApi.BusinessLogicLayer.Admin
{
    public class BLLCategory(ICategoryRepository repo)
    {
        public async Task<(List<Category> categories, CPaginationInfo pagination)> GetAllAsync(QCategoryGetAll query)
        {
            return await repo.GetAllAsync(query);
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await repo.GetByIdAsync(id);
        }

        public async Task<(string message, Category? category, int status)> Create(CreateCategoryRequest createCategoryRequest)
        {
            var category = createCategoryRequest.ToCategory();

            if (category.MainCategoryId is int parentId)
            {
                var existingCategory = await repo.GetByIdAsync(parentId);
                if (existingCategory is null)
                    return ("دسته بندی اصلی یافت نشد", null, 404);
            }

            var urlCategory = await repo.GetByUrlAsync(category.Url);
            if (urlCategory is not null) return ("لینک وارد شده تکراری است", null, 400);

            int id = await repo.CreateAsync(category);
            category = await repo.GetByIdAsync(id);
            return ("دسته بندی با موفقیت اضافه شد", category, 201);
        }

        public async Task<(string message, Category? category, int status)> Update(int id, UpdateCategoryRequest UCategory)
        {
            var category = await repo.GetByIdAsync(id);
            if (category is null)
                return ("دسته بندی مورد نظر یافت نشد", null, 404);

            if (UCategory.MainCategoryId is int parentId)
            {
                var existingCategory = await repo.GetByIdAsync(parentId);
                if (existingCategory is null)
                    return ("دسته بندی اصلی یافت نشد", null, 404);
            }

            var urlCategory = await repo.GetByUrlAsync(category.Url);
            if (urlCategory is not null)
            {
                if (!(urlCategory.Url == UCategory.Url))
                    return ("لینک وارد شده تکراری است", null, 400);
            }
            category = await repo.UpdateAsync(UCategory.ToCategory(id));
            return ("دسته بندی با موفقیت بروزرسانی شد", category, 200);
        }

        public async Task<(string message, int status)> Delete(int id)
        {
            var existingCategory = await repo.GetByIdAsync(id);
            if (existingCategory is null)
                return ("دسته بندی مورد نظر یافت نشد", 404);

            var category = await repo.DeleteAsync(id);
            if (!category) return ("دسته بندی مورد نظر یافت نشد", 404);

            return ("دسته بندی با موفقیت حذف شد", 204);
        }


    }
}
