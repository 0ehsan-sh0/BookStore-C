using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Translator;
using BookStoreApi.RequestHandler.Admin.Responses.Translator;

namespace BookStoreApi.Database.Interfaces
{
    public interface ITranslatorRepository
    {
        Task<(List<Translator> translators, TPaginationInfo info)> GetAllAsync(QTranslatorGetAll query);
        Task<Translator?> GetByIdAsync(int id);
        Task<int> CreateAsync(Translator translator);
        Task<Translator?> UpdateAsync(Translator translatorWithId);
        Task<bool> DeleteAsync(int id);

    }
}
