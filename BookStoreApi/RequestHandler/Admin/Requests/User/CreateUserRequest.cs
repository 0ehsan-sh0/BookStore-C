using System.ComponentModel.DataAnnotations;
using BookStoreApi.Enums;

namespace BookStoreApi.RequestHandler.Admin.Requests.User
{
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "نام کاربر الزامی است")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "نام خانوادگی کاربر الزامی است")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "شماره موبایل الزامی است")]
        [RegularExpression(@"^09\d{9}$", ErrorMessage = "شماره موبایل نامعتبر است.")]
        public string Mobile { get; set; } = string.Empty;

        [Required(ErrorMessage = "رمز عبور الزامی است")]
        [MinLength(6, ErrorMessage = "رمز عبور باید حداقل 6 کاراکتر باشد")]
        public string Password { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.User;
    }
}