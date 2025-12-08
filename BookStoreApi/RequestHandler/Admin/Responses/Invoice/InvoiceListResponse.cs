namespace BookStoreApi.RequestHandler.Admin.Responses.Invoice
{
    public class InvoiceListResponse
    {
        public List<RInvoice>? Invoices { get; set; }
        public InvoicePaginationInfo? Pagination { get; set; }
    }
}
