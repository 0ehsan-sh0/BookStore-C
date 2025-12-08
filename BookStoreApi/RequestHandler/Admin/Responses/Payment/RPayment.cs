using BookStoreApi.Enums;
using BookStoreApi.RequestHandler.Admin.Responses.Invoice;

namespace BookStoreApi.RequestHandler.Admin.Responses.Payment
{
    public class RPayment
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string GatewayId { get; set; } = string.Empty;
        public long Price { get; set; }
        public string PaymentGateway { get; set; } = string.Empty;
        public string ResponseCode { get; set; } = string.Empty;
        public string? Message { get; set; }
        public PaymentStatus Status { get; set; }
        public string? TransactionCode { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public RInvoice? Invoice { get; set; }
    }
}
