using BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel;
using BookStoreApi.RequestHandler.User.Mappers;
using BookStoreApi.RequestHandler.User.Requests.Invoice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.User
{
    [Route("api/user/purchase")]
    [ApiController]
    [Authorize(Roles = "User,Admin")]
    public class UserPurchaseController(
        IBLLUserInvoice invoiceBLL,
        IBLLUserPayment paymentBLL) : ApiResponseHelper
    {
        [HttpPost]
        public async Task<IActionResult> Purchase([FromBody] CreateInvoiceRequest request)
        {
            // 1. Validate model
            var (isValid, errors) = ModelStateValidation();
            if (!isValid)
                return BadRequest(errors);

            var userMobile = User.Identity?.Name!;

            // 2. Create invoice
            var (invMessage, invoice, invStatus) = await invoiceBLL.CreateAsync(userMobile, request);
            if (invStatus != 201 || invoice == null)
                return StatusCode(invStatus, invMessage);

            // 3. Create fake payment
            var (payMessage, _, payStatus) = await paymentBLL.PurchaseAsync(invoice.Id);
            if (payStatus != 201)
                return StatusCode(payStatus, payMessage);

            // 4. Reload invoice with payments
            var (_, fullInvoice, _) = await invoiceBLL.GetByIdAsync(invoice.Id);
            if (fullInvoice == null)
                return StatusCode(500, "خطا در بارگذاری اطلاعات فاکتور.");

            return SuccessResponse("خرید با موفقیت انجام شد.", fullInvoice.ToRInvoice(), 201);
        }
    }
}
