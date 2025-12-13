using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Public.QueryObjects.Book;
using BookStoreApi.RequestHandler.Public.Responses.Book;

namespace BookStoreApi.BusinessLogicLayer.Interfaces.Public
{
    public interface IBLLTagPublic
    {
        Task<(string message, Tag? tag, BPPaginationInfo? info, int status)> GetTagAsync(string url, QTagBooks query);

    }
}
