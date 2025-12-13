using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Translator;
using BookStoreApi.RequestHandler.Admin.Responses.Translator;
using BookStoreApi.RequestHandler.Public.QueryObjects.Book;
using BookStoreApi.RequestHandler.Public.Responses.Book;

namespace BookStoreApi.Database.Interfaces
{
    public interface ITranslatorRepository
    {
        Task<(List<Translator> translators, TPaginationInfo info)> GetAllAsync(QTranslatorGetAll query);
        Task<Translator?> GetByIdAsync(int id);
        Task<(Translator? translator, BPPaginationInfo info)> GetByIdAsync(int id, QTranslatorBooks query);
        Task<int> CreateAsync(Translator translator);
        Task<Translator?> UpdateAsync(Translator translatorWithId);
        Task<bool> DeleteAsync(int id);

    }
}
