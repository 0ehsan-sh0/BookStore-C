using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.Requests.Translator;
using BookStoreApi.RequestHandler.Admin.Responses.Translator;

namespace BookStoreApi.RequestHandler.Admin.Mappers
{
    public static class TranslatorMapper
    {
        public static RTranslator ToRTranslator(this Translator translator)
        {
            return new RTranslator
            {
                Id = translator.Id,
                Name = translator.Name,
                Description = translator.Description,
                CreatedAt = translator.CreatedAt,
                UpdatedAt = translator.UpdatedAt,
            };
        }

        public static Translator ToTranslator(this CreateTranslatorRequest translator)
        {
            return new Translator
            {
                Name = translator.Name,
                Description = translator.Description,
            };
        }

        public static Translator ToTranslator(this UpdateTranslatorRequest translator, int id)
        {
            return new Translator
            {
                Id = id,
                Name = translator.Name,
                Description = translator.Description,
            };
        }
    }
}
