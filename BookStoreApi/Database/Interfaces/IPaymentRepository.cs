using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Payment;
using BookStoreApi.RequestHandler.Admin.Responses.Payment;

namespace BookStoreApi.Database.Interfaces
{
    public interface IPaymentRepository
    {
        Task<(List<Payment> payments, PaymentPaginationInfo info)> GetAllAsync(QPaymentGetAll query);
        Task<int> CreateAsync(Payment payment);
        Task<Payment?> GetByIdAsync(int id);
    }
}
