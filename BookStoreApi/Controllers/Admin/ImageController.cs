using BookStoreApi.BusinessLogicLayer.Interfaces.Admin;
using BookStoreApi.RequestHandler.Admin.Mappers;
using BookStoreApi.RequestHandler.Admin.Requests.Image;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class ImageController(IBLLImage bLL) : ApiResponseHelper
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateImageRequest createImageRequest)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid)
                return BadRequest(errors);

            var (message, images, status) = await bLL.Create(createImageRequest);

            return status switch
            {
                201 => SuccessResponse(message, images!.Select(i => i.ToRImage()).ToList(), status),
                404 => NotFound(message),
                500 => StatusCode(500, message),
                _ => StatusCode(status, message)
            };
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> ChangePrimary([FromRoute] int id)
        {
            var (message, status) = await bLL.ChangePrimary(id);

            return status switch
            {
                201 => SuccessResponse(message, null, status),
                404 => NotFound(message),
                403 => StatusCode(403, message),
                500 => StatusCode(500, message),
                _ => StatusCode(status, message)
            };
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (message, status) = await bLL.Delete(id);

            return status switch
            {
                204 => NoContent(),
                403 => StatusCode(403, message),
                404 => NotFound(message),
                500 => StatusCode(500, message),
                _ => StatusCode(status, message)
            };
        }
    }
}
