using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.RequestHandler.User.Requests.User
{
    public class UpdateUserRequest
    {
        [Required(ErrorMessage = "نام کاربر الزامی است")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "نام خانوادگی کاربر الزامی است")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "شماره موبایل الزامی است")]
        [RegularExpression(@"^09\d{9}$", ErrorMessage = "شماره موبایل نامعتبر است.")]
        public string Mobile { get; set; } = string.Empty;
    }
}