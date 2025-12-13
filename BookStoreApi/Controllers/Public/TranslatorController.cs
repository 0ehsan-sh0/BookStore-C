using BookStoreApi.BusinessLogicLayer.Interfaces.Public;
using BookStoreApi.RequestHandler.Public.Mappers;
using BookStoreApi.RequestHandler.Public.QueryObjects.Book;
using BookStoreApi.RequestHandler.Public.Responses.Book;
using BookStoreApi.RequestHandler.Public.Responses.Translator;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.Public
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslatorController(IBLLTranslatorPublic bLL) : ApiResponseHelper
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTranslatorAsync(
            [FromRoute] int id,
            [FromQuery] QTranslatorBooks query)
        {
            var (message, translator, pagination, status) = await bLL.GetTranslatorAsync(id, query);

            var translatorDetails = new TranslatorDetails
            {
                Translator = translator?.ToPublicTranslator(),
                Books =
                    new BookAllDataListResponse
                    {
                        Books = translator?.Books?.Select(b => b.ToPublicBookAllData()).ToList(),
                        Pagination = pagination
                    }
            };
            return status == 200 ?
                SuccessResponse(message, translatorDetails) : StatusCode(status, message);
        }

    }
}
