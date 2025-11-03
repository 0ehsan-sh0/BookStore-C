using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Public.Responses.Translator;

namespace BookStoreApi.RequestHandler.Public.Mappers
{
    public static class TranslatorMapper
    {
        public static RTranslator ToPublicTranslator(this Translator translator)
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
    }
}
