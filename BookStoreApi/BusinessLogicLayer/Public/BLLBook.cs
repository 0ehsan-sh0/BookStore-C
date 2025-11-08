using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Public.Responses.Book;

namespace BookStoreApi.BusinessLogicLayer.Public
{
    public class BLLBook(IBookRepository repo)
    {
        public async Task<(List<BookAllData>? books, BPPaginationInfo info)> GetNewAsync(int pageSize = 20, int pageNumber = 1, bool isRecommended = false)
        {
            return await repo.GetNewAsync(pageSize, pageNumber, isRecommended);
        }

        public async Task<BookAllData?> GetByIdAsync(int id)
        {
            return await repo.GetByIdAsync(id);
        }
    }
}
