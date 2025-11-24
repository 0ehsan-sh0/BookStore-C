using BookStoreApi.Enums;

namespace BookStoreApi.Database.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public long TotalPrice { get; set; }
        public long Maliat { get; set; }
        public long FinalTotalPrice { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public InvoiceStatus InvoiceStatus { get; set; }
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ValidatedAt { get; set; }
        public List<Book>? Books { get; set; }
        public List<Payment>? Payments { get; set; }
        public User? User { get; set; }
        public AddressInfo? Address { get; set; }
    }
}
