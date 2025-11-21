namespace BookStoreApi.RequestHandler.User.Responses.Invoice
{
    public class UserInvoicesList
    {
        public List<RInvoice>? Invoices { get; set; }
        public InvoicePaginationInfo? Pagination { get; set; }
    }
}
