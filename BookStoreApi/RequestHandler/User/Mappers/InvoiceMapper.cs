using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.User.Responses.Invoice;

namespace BookStoreApi.RequestHandler.User.Mappers
{
    public static class InvoiceMapper
    {
        public static RInvoice ToRInvoice(this Invoice invoice)
        {
            return new RInvoice
            {
                Id = invoice.Id,
                TotalPrice = invoice.TotalPrice,
                Maliat = invoice.Maliat,
                FinalTotalPrice = invoice.FinalTotalPrice,
                PaymentStatus = invoice.PaymentStatus,
                InvoiceStatus = invoice.InvoiceStatus,
                UserId = invoice.UserId,
                AddressId = invoice.AddressId,
                CreatedAt = invoice.CreatedAt,
                UpdatedAt = invoice.UpdatedAt,
                ValidatedAt = invoice.ValidatedAt,

                Books = invoice.Books?.Select(b => b.ToRBook()).ToList(),
                Payments = invoice.Payments?.Select(p => p.ToRPayment()).ToList(),
                User = invoice.User?.ToRUser(),
                Address = invoice.Address?.ToRAddress()
            };
        }
    }
}
