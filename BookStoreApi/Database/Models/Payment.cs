using BookStoreApi.Enums;

namespace BookStoreApi.Database.Models
{
    public class Payment
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
        public Invoice? Invoice { get; set; }
    }
}
