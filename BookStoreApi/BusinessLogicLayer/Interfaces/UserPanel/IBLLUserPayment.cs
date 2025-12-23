using BookStoreApi.Database.Models;

namespace BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel
{
    public interface IBLLUserPayment
    {
        Task<(string message, string? paymentUrl, int status)> InitiatePurchaseAsync(int invoiceId, string callbackUrl);
        Task<(string message, Payment? payment, int status)> VerifyPurchaseAsync(int invoiceId, string authority);
    }
}
