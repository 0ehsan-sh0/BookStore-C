using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Payment;
using BookStoreApi.RequestHandler.Admin.Responses.Payment;

namespace BookStoreApi.BusinessLogicLayer.Interfaces.Admin
{
    public interface IBLLPayment
    {
        Task<(string message, List<Payment> payments, PaymentPaginationInfo info, int status)> GetAllAsync(QPaymentGetAll query);
    }
}
