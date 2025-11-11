using BookStoreApi.RequestHandler.Admin.Responses.Address;

namespace BookStoreApi.RequestHandler.Admin.Responses.User
{
    public class RUserDetail : RUser
    {
        public List<RAddress>? Addresses { get; set; }
    }
}
