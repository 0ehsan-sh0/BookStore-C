using BookStoreApi.Enums;
using BookStoreApi.RequestHandler.User.Responses.Address;
using BookStoreApi.RequestHandler.User.Responses.Book;
using BookStoreApi.RequestHandler.User.Responses.Invoice;

namespace BookStoreApi.RequestHandler.User.Responses.User
{
    public class RUser
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LoggedInAt { get; set; }
        public List<RAddress>? Addresses { get; set; }
        public List<RInvoice>? Invoices { get; set; }
        public List<RBookAllData>? WishList { get; set; }
    }
}