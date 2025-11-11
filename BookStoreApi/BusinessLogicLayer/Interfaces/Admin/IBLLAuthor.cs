using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Author;
using BookStoreApi.RequestHandler.Admin.Requests.Author;
using BookStoreApi.RequestHandler.Admin.Responses.Author;

namespace BookStoreApi.BusinessLogicLayer.Interfaces.Admin
{
    public interface IBLLAuthor
    {
        Task<(List<Author> authors, APaginationInfo pagination)> GetAllAsync(QAuthorGetAll query);
        Task<Author?> GetByIdAsync(int id);
        Task<(string message, Author? author, int status)> Create(CreateAuthorRequest createAuthorRequest);
        Task<(string message, Author? author, int status)> Update(int id, UpdateAuthorRequest UAuthor);
        Task<(string message, int status)> Delete(int id);
    }
}