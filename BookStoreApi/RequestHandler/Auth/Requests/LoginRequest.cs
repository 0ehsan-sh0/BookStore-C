using BookStoreApi.RequestHandler.Validations;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.RequestHandler.Auth.Requests
{
    [RequireOneOf("Password", "Code", ErrorMessage = "کد تایید و یا رمز عبور باید ارسال شود")]
    public class LoginRequest
    {
        [Required(ErrorMessage = "فیلد موبایل الزامی است.")]
        [RegularExpression(@"^09\d{9}$", ErrorMessage = "شماره موبایل نامعتبر است.")]
        public string Mobile { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
