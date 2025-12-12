using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Public.QueryObjects.Book;
using BookStoreApi.RequestHandler.Public.Responses.Book;

namespace BookStoreApi.BusinessLogicLayer.Interfaces.Public
{
    public interface IBLLAuthorPublic
    {
        Task<(string message, Author? author, BPPaginationInfo? info, int status)> GetAuthorAsync(int id, QAuthorBooks query);
    }
}
