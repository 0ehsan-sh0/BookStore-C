using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.Responses.Payment;

namespace BookStoreApi.RequestHandler.Admin.Mappers
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
                Price = payment.Price,
                PaymentGateway = payment.PaymentGateway,
                ResponseCode = payment.ResponseCode,
                Message = payment.Message,
                Status = payment.Status,
                TransactionCode = payment.TransactionCode,
                CreatedAt = payment.CreatedAt,
                UpdatedAt = payment.UpdatedAt,
                Invoice = payment.Invoice?.ToRInvoice()
            };
        }
    }
}
