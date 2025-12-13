using BookStoreApi.BusinessLogicLayer.Interfaces.Public;
using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Public.QueryObjects.Book;
using BookStoreApi.RequestHandler.Public.Responses.Book;

namespace BookStoreApi.BusinessLogicLayer.LogicLayers.Public
{
    public class BLLCategoryPublic(ICategoryRepository repo) : IBLLCategoryPublic
    {
        public async Task<List<Category>> GetCategoriesWithSubAsync()
        {
            return await repo.GetCategoriesWithSubAsync();
        }

        public async Task<(string message, Category? category, BPPaginationInfo? info, int status)> GetTagAsync(string url, QCategoryBooks query)
        {
            var (category, pagination) = await repo.GetByUrlAsync(url, query);
            if (category is null) return ("دسته بندی پیدا نشد.", null, null, 404);

            return ("عملیات با موفقیت انجام شد.", category, pagination, 200);
        }
    }
}
