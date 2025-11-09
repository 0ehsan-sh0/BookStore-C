using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.RequestHandler.Auth.Requests
{
    public class SendCodeRequest
    {
        [Required(ErrorMessage = "فیلد موبایل الزامی است.")]
        [RegularExpression(@"^09\d{9}$", ErrorMessage = "شماره موبایل نامعتبر است.")]
        public string Mobile { get; set; } = string.Empty;
    }
}
