using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.RequestHandler.User.Requests.User
{
    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "رمز عبور فعلی الزامی است")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "رمز عبور جدید الزامی است")]
        [MinLength(6, ErrorMessage = "رمز عبور باید حداقل 6 کاراکتر باشد")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "تکرار رمز عبور جدید الزامی است")]
        [Compare("NewPassword", ErrorMessage = "رمز عبور و تکرار آن مطابقت ندارند")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}