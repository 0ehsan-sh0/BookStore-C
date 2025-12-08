namespace BookStoreApi.RequestHandler.Admin.Responses.Payment
{
    public class PaymentListResponse
    {
        public List<RPayment>? Payments { get; set; }
        public PaymentPaginationInfo? Pagination { get; set; }
    }
}
