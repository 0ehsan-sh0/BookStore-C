using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.User.Requests.Invoice;

namespace BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel
{
    public interface IBLLUserInvoice
    {
        Task<(string message, Invoice? invoice, int status)> CreateAsync(string userMobile, CreateInvoiceRequest request);
        Task<(string message, Invoice? invoice, int status)> GetByIdAsync(int invoiceId);
    }
}
