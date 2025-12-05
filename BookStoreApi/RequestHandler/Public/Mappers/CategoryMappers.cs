using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Public.Responses.Category;

namespace BookStoreApi.RequestHandler.Public.Mappers
{
    public static class CategoryMappers
    {
        public static RCategory ToPublicCategory(this Category category)
        {
            return new RCategory
            {
                Id = category.Id,
                Name = category.Name,
                Url = category.Url,
                MainCategoryId = category.MainCategoryId,
                SubCategories = category.SubCategories?.Select(c => c.ToPublicCategory()).ToList(),
            };
        }

    }
}
