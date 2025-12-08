using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Invoice;
using BookStoreApi.RequestHandler.Admin.Responses.Invoice;

namespace BookStoreApi.BusinessLogicLayer.Interfaces.Admin
{
    public interface IBLLInvoice
    {
        Task<(string message, List<Invoice> invoices, InvoicePaginationInfo info, int status)> GetAllAsync(QInvoiceGetAll query);
    }
}
