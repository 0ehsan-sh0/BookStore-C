using BookStoreApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.RequestHandler.Admin.Requests.User
{
    public class UpdateAdminUserRequest
    {
        [Required(ErrorMessage = "نام کاربر الزامی است")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "نام خانوادگی کاربر الزامی است")]
        public string LastName { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.User;
    }
}
