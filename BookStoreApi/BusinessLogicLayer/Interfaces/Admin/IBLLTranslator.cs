using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Translator;
using BookStoreApi.RequestHandler.Admin.Requests.Translator;
using BookStoreApi.RequestHandler.Admin.Responses.Translator;

namespace BookStoreApi.BusinessLogicLayer.Interfaces.Admin
{
    public interface IBLLTranslator
    {
        Task<(List<Translator> translators, TPaginationInfo pagination)> GetAllAsync(QTranslatorGetAll query);
        Task<Translator?> GetByIdAsync(int id);
        Task<(string message, Translator? translator, int status)> Create(CreateTranslatorRequest createTranslatorRequest);
        Task<(string message, Translator? translator, int status)> Update(int id, UpdateTranslatorRequest uTranslator);
        Task<(string message, int status)> Delete(int id);
    }
}