using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.RequestHandler.User.Requests.Invoice
{
    public class CreateInvoiceRequest
    {
        [Required(ErrorMessage = "شناسه آدرس لازم است.")]
        [Range(1, int.MaxValue, ErrorMessage = "شناسه آدرس نامعتبر است.")]
        public int AddressId { get; set; }
        [Required(ErrorMessage = "لیست کتاب‌ها لازم است.")]
        public List<int> Books { get; set; } = new List<int>();
        [Required(ErrorMessage = "لیست تعداد کتاب‌ها لازم است.")]
        public List<int> Counts { get; set; } = new List<int>();
    }
}
