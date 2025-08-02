namespace BookStoreApi.RequestHandler.Admin.Responses.Translator
{
    public class TranslatorListResponse
    {
        public List<RTranslator>? Translators { get; set; }
        public TPaginationInfo? Pagination { get; set; }
    }
}
