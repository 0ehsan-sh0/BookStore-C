using BookStoreApi.BusinessLogicLayer.Interfaces.Admin;
using BookStoreApi.RequestHandler.Admin.Mappers;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Payment;
using BookStoreApi.RequestHandler.Admin.Responses.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class PaymentController(IBLLPayment bLL) : ApiResponseHelper
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] QPaymentGetAll query)
        {
            var (message, payments, pagination, status) = await bLL.GetAllAsync(query);

            var rPayments = payments.Select(c => c.ToRPayment()).ToList();

            var response = new PaymentListResponse
            {
                Payments = rPayments,
                Pagination = pagination
            };

            return status == 200 ?
                SuccessResponse("اطلاعات با موفقیت دریافت شد", response)
                : StatusCode(status, message);
        }
    }
}
