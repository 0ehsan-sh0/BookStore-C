using BookStoreApi.BusinessLogicLayer.Interfaces.Admin;
using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Payment;
using BookStoreApi.RequestHandler.Admin.Responses.Payment;

namespace BookStoreApi.BusinessLogicLayer.LogicLayers.Admin
{
    public class BLLPayment(IPaymentRepository repo) : IBLLPayment
    {
        public async Task<(string message, List<Payment> payments, PaymentPaginationInfo info, int status)> GetAllAsync(QPaymentGetAll query)
        {
            var (payments, info) = await repo.GetAllAsync(query);
            return ("پرداخت ها با موفقیت بارگزاری شد.", payments, info, 200);
        }
    }
}
