using BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel;
using BookStoreApi.RequestHandler.User.Requests.Invoice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.User
{
    [Route("api/user/invoice")]
    [ApiController]
    [Authorize(Roles = "User,Admin")]
    public class UserInvoiceController(IBLLUserInvoice bLL) : ApiResponseHelper
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInvoiceRequest request)
        {
            var userMobile = User.Identity?.Name!;
            var (isValid, errors) = ModelStateValidation();
            if (!isValid)
                return BadRequest(errors);

            var (message, invocieId, status) = await bLL.CreateAsync(userMobile, request);

            return status == 201
                ? SuccessResponse(message, invocieId, status)
                : StatusCode(status, message);
        }
    }
}
