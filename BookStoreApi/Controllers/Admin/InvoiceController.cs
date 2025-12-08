using BookStoreApi.BusinessLogicLayer.Interfaces.Admin;
using BookStoreApi.RequestHandler.Admin.Mappers;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Invoice;
using BookStoreApi.RequestHandler.Admin.Responses.Invoice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class InvoiceController(IBLLInvoice bLL) : ApiResponseHelper
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] QInvoiceGetAll query)
        {
            var (message, invoices, pagination, status) = await bLL.GetAllAsync(query);

            var rInvoices = invoices.Select(c => c.ToRInvoice()).ToList();

            var response = new InvoiceListResponse
            {
                Invoices = rInvoices,
                Pagination = pagination
            };

            return status == 200 ?
                SuccessResponse("اطلاعات با موفقیت دریافت شد", response)
                : StatusCode(status, message);
        }
    }
}
