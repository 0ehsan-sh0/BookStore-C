using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Author;
using BookStoreApi.RequestHandler.Admin.Responses.Author;
using BookStoreApi.RequestHandler.Public.QueryObjects.Book;
using BookStoreApi.RequestHandler.Public.Responses.Book;

namespace BookStoreApi.Database.Interfaces
{
    public interface IAuthorRepository
    {
        Task<(List<Author> authors, APaginationInfo info)> GetAllAsync(QAuthorGetAll query);
        Task<Author?> GetByIdAsync(int id);
        Task<(Author? author, BPPaginationInfo info)> GetByIdAsync(int id, QAuthorBooks query);
        Task<int> CreateAsync(Author author);
        Task<Author?> UpdateAsync(Author authorWithId);
        Task<bool> DeleteAsync(int id);
    }
}
