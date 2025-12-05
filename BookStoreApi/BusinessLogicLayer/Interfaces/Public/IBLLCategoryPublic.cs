using BookStoreApi.Database.Models;

namespace BookStoreApi.BusinessLogicLayer.Interfaces.Public
{
    public interface IBLLCategoryPublic
    {
        Task<List<Category>> GetCategoriesWithSubAsync();
    }
}
