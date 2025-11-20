using BookStoreApi.Database.Models;

namespace BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel
{
    public interface IBLLUserPayment
    {
        Task<(string message, Payment? payment, int status)> PurchaseAsync(int invoiceId);
    }
}
