namespace BookStoreApi.RequestHandler.User.Requests.Invoice
{
    public class CreateInvoiceRequest
    {
        public List<int> Books { get; set; } = new List<int>();
        public List<int> Counts { get; set; } = new List<int>();
    }
}
