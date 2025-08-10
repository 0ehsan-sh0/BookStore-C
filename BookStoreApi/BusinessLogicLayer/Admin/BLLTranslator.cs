using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.Mappers;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Translator;
using BookStoreApi.RequestHandler.Admin.Requests.Translator;
using BookStoreApi.RequestHandler.Admin.Responses.Translator;

namespace BookStoreApi.BusinessLogicLayer.Admin
{
    public class BLLTranslator(ITranslatorRepository repo)
    {
        public async Task<(List<Translator> translators, TPaginationInfo pagination)> GetAllAsync(QTranslatorGetAll query)
        {
            return await repo.GetAllAsync(query);
        }

        public async Task<Translator?> GetByIdAsync(int id)
        {
            return await repo.GetByIdAsync(id);
        }

        public async Task<(string message, Translator? translator, int status)> Create(CreateTranslatorRequest createTranslatorRequest)
        {
            var translator = createTranslatorRequest.ToTranslator();

            int id = await repo.CreateAsync(translator);
            translator = await repo.GetByIdAsync(id);

            return ("مترجم با موفقیت اضافه شد", translator, 201);
        }

        public async Task<(string message, Translator? translator, int status)> Update(int id, UpdateTranslatorRequest uTranslator)
        {
            var translator = await repo.GetByIdAsync(id);
            if (translator is null)
                return ("مترجم مورد نظر یافت نشد", null, 404);

            translator = await repo.UpdateAsync(uTranslator.ToTranslator(id));

            return ("مترجم با موفقیت بروزرسانی شد", translator, 200);
        }

        public async Task<(string message, int status)> Delete(int id)
        {
            var existingEntity = await repo.GetByIdAsync(id);
            if (existingEntity is null)
                return ("مترجم مورد نظر یافت نشد", 404);

            var entity = await repo.DeleteAsync(id);
            if (!entity) return ("مترجم مورد نظر یافت نشد", 404);

            return ("مترجم با موفقیت حذف شد", 204);
        }
    }
}
