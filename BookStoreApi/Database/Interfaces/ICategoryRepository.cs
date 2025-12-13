using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Category;
using BookStoreApi.RequestHandler.Admin.Responses.Category;
using BookStoreApi.RequestHandler.Public.QueryObjects.Book;
using BookStoreApi.RequestHandler.Public.Responses.Book;

namespace BookStoreApi.Database.Interfaces
{
    public interface ICategoryRepository
    {
        Task<(List<Category> categories, CPaginationInfo info)> GetAllAsync(QCategoryGetAll query);
        Task<List<Category>> GetCategoriesWithSubAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<(Category? category, BPPaginationInfo info)> GetByUrlAsync(string url, QCategoryBooks query);
        Task<Category?> GetByUrlAsync(string Url);
        Task<int> CreateAsync(Category category);
        Task<Category?> UpdateAsync(Category categoryWithId);
        Task<bool> DeleteAsync(int id);
    }
}
