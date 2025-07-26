using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Category;
using BookStoreApi.RequestHandler.Admin.Responses.Category;

namespace BookStoreApi.Database.Interfaces
{
    public interface ICategoryRepository
    {
        Task<(List<Category> categories, CPaginationInfo info)> GetAllAsync(QCategoryGetAll query);
        Task<Category?> GetByIdAsync(int id);
        Task<Category?> GetByUrlAsync(string Url);
        Task<int> CreateAsync(Category category);
        Task<Category?> UpdateAsync(Category categoryWithId);
        Task<bool> DeleteAsync(int id);
    }
}
