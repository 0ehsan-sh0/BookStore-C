using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.RequestHandler.User.Requests.WishList
{
    public class ToggleWishList
    {
        [Required(ErrorMessage = "شناسه کتاب الزامی است.")]
        [Range(0, int.MaxValue)]
        public int BookId { get; set; }
    }
}
