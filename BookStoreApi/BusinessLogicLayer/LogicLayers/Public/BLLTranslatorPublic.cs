using BookStoreApi.BusinessLogicLayer.Interfaces.Public;
using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Public.QueryObjects.Book;
using BookStoreApi.RequestHandler.Public.Responses.Book;

namespace BookStoreApi.BusinessLogicLayer.LogicLayers.Public
{
    public class BLLTranslatorPublic(ITranslatorRepository repo) : IBLLTranslatorPublic
    {
        public async Task<(string message, Translator? translator, BPPaginationInfo? info, int status)> GetTranslatorAsync(int id, QTranslatorBooks query)
        {
            var (translator, pagination) = await repo.GetByIdAsync(id, query);
            if (translator is null) return ("مترجم پیدا نشد.", null, null, 404);

            return ("عملیات با موفقیت انجام شد.", translator, pagination, 200);
        }
    }
}
