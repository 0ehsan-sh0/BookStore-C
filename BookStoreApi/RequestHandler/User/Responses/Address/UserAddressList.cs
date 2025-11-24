namespace BookStoreApi.RequestHandler.User.Responses.Address
{
    public class UserAddressList
    {
        public List<RAddress>? Addresses { get; set; }
        public AddressPaginationInfo? Pagination { get; set; }
    }
}
