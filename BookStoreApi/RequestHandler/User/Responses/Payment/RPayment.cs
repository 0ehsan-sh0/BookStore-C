using BookStoreApi.Enums;
using BookStoreApi.RequestHandler.User.Responses.Invoice;

namespace BookStoreApi.RequestHandler.User.Responses.Payment
{
    public class RPayment
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string GatewayId { get; set; } = string.Empty;
        public string PaymentGateway { get; set; } = string.Empty;
        public string ResponseCode { get; set; } = string.Empty;
        public long Price { get; set; }
        public string? Message { get; set; }
        public PaymentStatus Status { get; set; }
        public string? TransactionCode { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public RInvoice? Invoice { get; set; }
    }
}
