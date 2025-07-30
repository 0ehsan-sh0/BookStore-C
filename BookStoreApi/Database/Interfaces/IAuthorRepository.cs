using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Author;
using BookStoreApi.RequestHandler.Admin.Responses.Author;

namespace BookStoreApi.Database.Interfaces
{
    public interface IAuthorRepository
    {
        Task<(List<Author> authors, APaginationInfo info)> GetAllAsync(QAuthorGetAll query);
        Task<Author?> GetByIdAsync(int id);
        Task<int> CreateAsync(Author author);
        Task<Author?> UpdateAsync(Author authorWithId);
        Task<bool> DeleteAsync(int id);
    }
}
