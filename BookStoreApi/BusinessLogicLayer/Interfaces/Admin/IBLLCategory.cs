using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Category;
using BookStoreApi.RequestHandler.Admin.Requests.Category;
using BookStoreApi.RequestHandler.Admin.Responses.Category;

namespace BookStoreApi.BusinessLogicLayer.Interfaces.Admin
{
    public interface IBLLCategory
    {
        Task<(List<Category> categories, CPaginationInfo pagination)> GetAllAsync(QCategoryGetAll query);
        Task<Category?> GetByIdAsync(int id);
        Task<(string message, Category? category, int status)> Create(CreateCategoryRequest createCategoryRequest);
        Task<(string message, Category? category, int status)> Update(int id, UpdateCategoryRequest UCategory);
        Task<(string message, int status)> Delete(int id);
    }
}