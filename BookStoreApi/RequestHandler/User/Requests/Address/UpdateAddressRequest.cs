using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.RequestHandler.User.Requests.Address
{
    public class UpdateAddressRequest
    {
        [Required(ErrorMessage = "نام گیرنده الزامی است")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "نام خانوادگی گیرنده الزامی است")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "شماره موبایل الزامی است")]
        [RegularExpression(@"^09\d{9}$", ErrorMessage = "شماره موبایل نامعتبر است.")]
        public string Phone { get; set; } = string.Empty;
        [Required(ErrorMessage = "کد پستی الزامی است")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "کد پستی باید 10 رقم باشد")]
        public string PostCode { get; set; } = string.Empty;
        [Required(ErrorMessage = "نام استان الزامی است")]
        public string State { get; set; } = string.Empty;
        [Required(ErrorMessage = "نام شهر الزامی است")]
        public string City { get; set; } = string.Empty;
        [Required(ErrorMessage = "آدرس الزامی است")]
        public string Address { get; set; } = string.Empty;
    }
}
