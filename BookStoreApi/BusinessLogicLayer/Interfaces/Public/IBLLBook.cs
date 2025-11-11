using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Public.Responses.Book;

namespace BookStoreApi.BusinessLogicLayer.Interfaces.Public
{
    public interface IBLLBook
    {
        Task<(List<BookAllData>? books, BPPaginationInfo info)> GetNewAsync(int pageSize = 20, int pageNumber = 1, bool isRecommended = false);
        Task<BookAllData?> GetByIdAsync(int id);
    }
}