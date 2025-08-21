using BookStoreApi.BusinessLogicLayer.Admin;
using BookStoreApi.RequestHandler.Admin.Mappers;
using BookStoreApi.RequestHandler.Admin.Requests.Image;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class ImageController(BLLImage bLL) : ApiResponseHelper
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateImageRequest createImageRequest)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid)
                return ErrorResponse("اطلاعات به درستی وارد نشده است", errors, 400);

            var (message, Images, status) = await bLL.Create(createImageRequest);

            return status == 201
                ? SuccessResponse(message, Images!.Select(i => i.ToRImage()).ToList(), status)
                : ErrorResponse(message, null, status);
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> ChangePrimary([FromRoute] int id)
        {
            var (message, status) = await bLL.ChangePrimary(id);
            return status == 201
                ? SuccessResponse(message, null, status)
                : ErrorResponse(message, null, status);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid) return ErrorResponse("اطلاعات به درستی وارد نشده است", errors, 400);

            var (message, status) = await bLL.Delete(id);

            if (status == 204)
                return NoContent(); // ✅ Correct way to return 204

            return ErrorResponse(message, null, status);
        }
    }
}
