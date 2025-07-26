using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.Requests.Category;
using BookStoreApi.RequestHandler.Admin.Responses.Category;

namespace BookStoreApi.RequestHandler.Admin.Mappers
{
    public static class CategoryMappers
    {
        public static RCategory ToRCategory(this Category category)
        {
            return new RCategory
            {
                Id = category.Id,
                Name = category.Name,
                Url = category.Url,
                MainCategoryId = category.MainCategoryId,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt,
            };
        }

        public static Category ToCategory(this CreateCategoryRequest category)
        {
            return new Category
            {
                Name = category.Name,
                Url = category.Url,
                MainCategoryId = category.MainCategoryId,
            };
        }

        public static Category ToCategory(this UpdateCategoryRequest category, int id)
        {
            return new Category
            {
                Id = id,
                Name = category.Name,
                Url = category.Url,
                MainCategoryId = category.MainCategoryId,
            };
        }

    }
}
