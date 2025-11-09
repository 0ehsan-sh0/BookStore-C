using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.RequestHandler.Auth.Requests
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "فیلد موبایل الزامی است.")]
        [RegularExpression(@"^09\d{9}$", ErrorMessage = "شماره موبایل نامعتبر است.")]
        public string Mobile { get; set; } = string.Empty;
        [Required(ErrorMessage = "فیلد رمز عبور الزامی است.")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "فیلد کد تایید الزامی است.")]
        public string Code { get; set; } = string.Empty;
    }
}
