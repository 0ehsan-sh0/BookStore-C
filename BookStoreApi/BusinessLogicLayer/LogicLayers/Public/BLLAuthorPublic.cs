using BookStoreApi.BusinessLogicLayer.Interfaces.Public;
using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Public.QueryObjects.Book;
using BookStoreApi.RequestHandler.Public.Responses.Book;

namespace BookStoreApi.BusinessLogicLayer.LogicLayers.Public
{
    public class BLLAuthorPublic(IAuthorRepository repo) : IBLLAuthorPublic
    {
        public async Task<(string message, Author? author, BPPaginationInfo? info, int status)> GetAuthorAsync(int id, QAuthorBooks query)
        {
            var (author, pagination) = await repo.GetByIdAsync(id, query);
            if (author is null) return ("نویسنده پیدا نشد.", null, null, 404);

            return ("عملیات با موفقیت انجام شد.", author, pagination, 200);
        }
    }
}
