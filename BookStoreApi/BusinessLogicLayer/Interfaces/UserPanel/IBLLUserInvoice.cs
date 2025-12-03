using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.User.QueryObjects.Invoice;
using BookStoreApi.RequestHandler.User.Requests.Invoice;
using BookStoreApi.RequestHandler.User.Responses.Invoice;

namespace BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel
{
    public interface IBLLUserInvoice
    {
        Task<(string message, Invoice? invoice, int status)> CreateAsync(string userMobile, CreateInvoiceRequest request);
        Task<(string message, Invoice? invoice, int status)> GetByIdAsync(string mobile, int invoiceId);
        Task<(List<Invoice>? invoices, InvoicePaginationInfo pagination)> GetUserInvoicesAsync(string mobile, QUserInvoices query);
    }
}
