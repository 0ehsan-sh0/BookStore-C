using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Public.QueryObjects.Book;
using BookStoreApi.RequestHandler.Public.Responses.Book;

namespace BookStoreApi.BusinessLogicLayer.Interfaces.Public
{
    public interface IBLLTranslatorPublic
    {
        Task<(string message, Translator? translator, BPPaginationInfo? info, int status)> GetTranslatorAsync(int id, QTranslatorBooks query);
    }
}
