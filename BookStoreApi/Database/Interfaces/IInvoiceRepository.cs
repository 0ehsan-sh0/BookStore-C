using BookStoreApi.Database.Models;
using BookStoreApi.Enums;
using BookStoreApi.RequestHandler.User.Responses.Invoice;

namespace BookStoreApi.Database.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<(List<Invoice> invoices, InvoicePaginationInfo info)> GetUserInvoicesAsync(int id);
        Task<(List<Invoice> invoices, InvoicePaginationInfo info)> GetUserInvoicesAsync(string mobile);
        Task<int> CreateAsync(int userId, List<int> books, List<int> counts);
        Task<bool> UpdatePaymentStatusAsync(int invoiceId, PaymentStatus status);
        Task<(List<InvoiceBooks> books, bool isValid)> GetBooksOfInvoiceAsync(int invoiceId);
        Task<Invoice?> GetByIdAsync(int id);
    }
}
