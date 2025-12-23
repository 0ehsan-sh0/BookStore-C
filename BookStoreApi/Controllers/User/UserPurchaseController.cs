using BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel;
using BookStoreApi.RequestHandler.User.Mappers;
using BookStoreApi.RequestHandler.User.Requests.Invoice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.User
{
    [Route("api/user/purchase")]
    [ApiController]
    public class UserPurchaseController(
        IBLLUserInvoice invoiceBLL,
        IBLLUserPayment paymentBLL,
        IConfiguration configuration) : ApiResponseHelper
    {
        [HttpPost]
    [Authorize(Roles = "User,Admin")]
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

        // 3. Initiate ZarinPal payment
        var callbackUrl = $"{Request.Scheme}://{Request.Host}/api/user/purchase/verify?invoiceId={invoice.Id}";
        var (payMessage, paymentUrl, payStatus) = await paymentBLL.InitiatePurchaseAsync(invoice.Id, callbackUrl);

        if (payStatus != 201 || paymentUrl == null)
            return StatusCode(payStatus, payMessage);

        // 4. Reload invoice (optional, but keep for consistency)
        var (_, fullInvoice, _) = await invoiceBLL.GetByIdAsync(userMobile, invoice.Id);
        if (fullInvoice == null)
            return StatusCode(500, "خطا در بارگذاری اطلاعات فاکتور.");

        // Return both invoice data and payment URL
        return SuccessResponse("درخواست خرید ثبت شد. در حال انتقال به درگاه پرداخت...", new
        {
            invoice = fullInvoice.ToRInvoice(),
            paymentUrl
        }, 201);
    }

    [HttpGet("verify")]
    public async Task<IActionResult> VerifyPayment([FromQuery] int invoiceId, [FromQuery] string Authority, [FromQuery] string Status)
    {
        if (Status != "OK")
        {
            return Redirect(configuration["Frontend:URL"] + "/purchase/invoice/" + invoiceId + "?status=failed");
        }

        var (message, payment, status) = await paymentBLL.VerifyPurchaseAsync(invoiceId, Authority);

        if (status == 200)
        {
            return Redirect(configuration["Frontend:URL"] + "/purchase/invoice/" + invoiceId + "?status=success");
        }

        return Redirect(configuration["Frontend:URL"] + "/purchase/invoice/" + invoiceId + "?status=failed&message=" + Uri.EscapeDataString(message));
    }
}
}
