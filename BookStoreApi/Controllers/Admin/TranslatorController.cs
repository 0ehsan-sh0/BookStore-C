using BookStoreApi.BusinessLogicLayer.Interfaces.Admin;
using BookStoreApi.RequestHandler.Admin.Mappers;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Translator;
using BookStoreApi.RequestHandler.Admin.Requests.Translator;
using BookStoreApi.RequestHandler.Admin.Responses.Translator;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class TranslatorController(IBLLTranslator bLL) : ApiResponseHelper
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] QTranslatorGetAll query)
        {
            var (Translators, pagination) = await bLL.GetAllAsync(query);
            var rTranslators = Translators.Select(c => c.ToRTranslator()).ToList();

            var response = new TranslatorListResponse
            {
                Translators = rTranslators,
                Pagination = pagination
            };

            return SuccessResponse("اطلاعات با موفقیت دریافت شد", response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var translator = await bLL.GetByIdAsync(id);
            if (translator is null)
                return NotFound("مترجم مورد نظر یافت نشد");

            return SuccessResponse("اطلاعات با موفقیت دریافت شد", translator);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTranslatorRequest createTranslatorRequest)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid)
                return BadRequest(errors);

            var (message, translator, status) = await bLL.Create(createTranslatorRequest);

            return status == 201
                ? SuccessResponse(message, translator, status)
                : StatusCode(status, message);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTranslatorRequest UTranslator)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid)
                return BadRequest(errors);

            var (message, translator, status) = await bLL.Update(id, UTranslator);

            return status == 200
                ? SuccessResponse(message, translator, status)
                : status == 404
                    ? NotFound(message)
                    : StatusCode(status, message);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var (message, status) = await bLL.Delete(id);

            return status switch
            {
                204 => NoContent(),
                404 => NotFound(message),
                _ => StatusCode(status, message)
            };
        }
    }
}
