using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.User.Responses.Invoice;

namespace BookStoreApi.Database.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<(List<Invoice> invoices, InvoicePaginationInfo info)> GetUserInvoicesAsync(int id);
        Task<(List<Invoice> invoices, InvoicePaginationInfo info)> GetUserInvoicesAsync(string mobile);
        Task<int> CreateAsync(int userId, List<int> books, List<int> counts);
    }
}
