using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;

namespace BookStoreApi.BusinessLogicLayer.Public
{
    public class BLLBook(IBookRepository repo)
    {
        public async Task<List<BookAllData>?> GetNewAsync(int pageSize = 20)
        {
            return await repo.GetNewAsync(pageSize);
        }
    }
}
