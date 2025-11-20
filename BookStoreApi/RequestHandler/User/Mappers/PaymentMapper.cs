using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.User.Responses.Payment;

namespace BookStoreApi.RequestHandler.User.Mappers
{
    public static class PaymentMapper
    {
        public static RPayment ToRPayment(this Payment payment)
        {
            return new RPayment
            {
                Id = payment.Id,
                InvoiceId = payment.InvoiceId,
                GatewayId = payment.GatewayId,
                PaymentGateway = payment.PaymentGateway,
                ResponseCode = payment.ResponseCode,
                Price = payment.Price,
                Message = payment.Message,
                Status = payment.Status,
                TransactionCode = payment.TransactionCode,
                CreatedAt = payment.CreatedAt,
                UpdatedAt = payment.UpdatedAt,
                Invoice = null // Prevent recursion (Invoice → Payments → Invoice → Payments...)
            };
        }
    }
}
