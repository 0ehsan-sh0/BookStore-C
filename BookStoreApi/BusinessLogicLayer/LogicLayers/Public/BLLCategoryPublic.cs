using BookStoreApi.BusinessLogicLayer.Interfaces.Public;
using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;

namespace BookStoreApi.BusinessLogicLayer.LogicLayers.Public
{
    public class BLLCategoryPublic(ICategoryRepository repo) : IBLLCategoryPublic
    {
        public async Task<List<Category>> GetCategoriesWithSubAsync()
        {
            return await repo.GetCategoriesWithSubAsync();
        }
    }
}
