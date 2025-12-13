using BookStoreApi.RequestHandler.Public.Responses.Book;

namespace BookStoreApi.RequestHandler.Public.Responses.Translator
{
    public class TranslatorDetails
    {
        public RTranslator? Translator { get; set; }
        public BookAllDataListResponse? Books { get; set; }
    }
}
