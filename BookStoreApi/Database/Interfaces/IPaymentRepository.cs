using BookStoreApi.Database.Models;

namespace BookStoreApi.Database.Interfaces
{
    public interface IPaymentRepository
    {
        Task<int> CreateAsync(Payment payment);
        Task<Payment?> GetByIdAsync(int id);
    }
}
