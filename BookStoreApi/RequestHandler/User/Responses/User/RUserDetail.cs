using BookStoreApi.RequestHandler.User.Responses.Address;

namespace BookStoreApi.RequestHandler.User.Responses.User
{
    public class RUserDetail : RUser
    {
        public List<RAddress>? Addresses { get; set; }
    }
}
