using BookStoreApi.BusinessLogicLayer.Admin;
using BookStoreApi.RequestHandler.Admin.Mappers;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Tag;
using BookStoreApi.RequestHandler.Admin.Requests.Tag;
using BookStoreApi.RequestHandler.Admin.Responses.Tag;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController(BLLTag bLL) : ApiResponseHelper
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] QTagGetAll query)
        {
            var (tags, pagination) = await bLL.GetAllAsync(query);

            var rTags = tags.Select(c => c.ToRTag()).ToList();

            var response = new TagListResponse
            {
                Tags = rTags,
                Pagination = pagination
            };

            return SuccessResponse("اطلاعات با موفقیت دریافت شد", response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var tag = await bLL.GetByIdAsync(id);
            if (tag is null) return ErrorResponse("دسته بندی یافت نشد", null);
            return SuccessResponse("اطلاعات با موفقیت دریافت شد", tag.ToRTag());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTagRequest createTagRequest)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid)
                return ErrorResponse("اطلاعات به درستی وارد نشده است", errors, 400);

            var (message, tag, status) = await bLL.Create(createTagRequest);

            return status == 201
                ? SuccessResponse(message, tag!.ToRTag(), status)
                : ErrorResponse(message, tag, status);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTagRequest UTag)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid) return ErrorResponse("اطلاعات به درستی وارد نشده است", errors, 400);

            var (message, tag, status) = await bLL.Update(id, UTag);

            return status == 200
                ? SuccessResponse(message, tag!.ToRTag(), status)
                : ErrorResponse(message, null, status);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid) return ErrorResponse("اطلاعات به درستی وارد نشده است", errors, 400);

            var (message, status) = await bLL.Delete(id);

            return status == 204
                ? SuccessResponse(message, null, status)
                : ErrorResponse(message, null, status);
        }
    }
}
