using BookStoreApi.BusinessLogicLayer.Interfaces.Admin;
using BookStoreApi.Controllers;
using BookStoreApi.RequestHandler.Admin.Mappers;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Tag;
using BookStoreApi.RequestHandler.Admin.Requests.Tag;
using BookStoreApi.RequestHandler.Admin.Responses.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/admin/[controller]")]
[Authorize(Roles = "Admin")]
[ApiController]
public class TagController(IBLLTag bLL) : ApiResponseHelper
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
        return tag is null
            ? NotFound()
            : SuccessResponse("اطلاعات با موفقیت دریافت شد", tag.ToRTag());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTagRequest createTagRequest)
    {
        var (isValid, errors) = ModelStateValidation();
        if (!isValid)
            return BadRequest(errors);

        var (message, tag, status) = await bLL.Create(createTagRequest);
        return SuccessResponse(message, tag?.ToRTag(), status);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTagRequest UTag)
    {
        var (isValid, errors) = ModelStateValidation();
        if (!isValid)
            return BadRequest(errors);

        var (message, tag, status) = await bLL.Update(id, UTag);
        return SuccessResponse(message, tag?.ToRTag(), status);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var (message, status) = await bLL.Delete(id);

        // Still good to handle 204 properly (no content response)
        if (status == 204)
            return NoContent();

        return SuccessResponse(message, null, status);
    }
}
