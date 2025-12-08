using BookStoreApi.Enums;
using BookStoreApi.RequestHandler.Admin.Responses.Address;
using BookStoreApi.RequestHandler.Admin.Responses.Book;
using BookStoreApi.RequestHandler.Admin.Responses.Payment;
using BookStoreApi.RequestHandler.Admin.Responses.User;

namespace BookStoreApi.RequestHandler.Admin.Responses.Invoice
{
    public class RInvoice
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
        public List<RBookAllData>? Books { get; set; }
        public List<RPayment>? Payments { get; set; }
        public RUser? User { get; set; }
        public RAddress? Address { get; set; }
    }
}
