using BookStoreApi.BusinessLogicLayer.Interfaces.Public;
using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Public.QueryObjects.Book;
using BookStoreApi.RequestHandler.Public.Responses.Book;

namespace BookStoreApi.BusinessLogicLayer.LogicLayers.Public
{
    public class BLLTagPublic(ITagRepository repo) : IBLLTagPublic
    {
        public async Task<(string message, Tag? tag, BPPaginationInfo? info, int status)> GetTagAsync(string url, QTagBooks query)
        {
            var (tag, pagination) = await repo.GetByUrlAsync(url, query);
            if (tag is null) return ("تگ پیدا نشد.", null, null, 404);

            return ("عملیات با موفقیت انجام شد.", tag, pagination, 200);
        }
    }
}
