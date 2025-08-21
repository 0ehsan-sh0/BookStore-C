using BookStoreApi.RequestHandler.Validations;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.RequestHandler.Admin.Requests.Book
{
    public class CreateBookRequest
    {
        [Required(ErrorMessage = "نام کتاب الزامی است")]
        public string Name { get; set; } = string.Empty;
        [RegularExpression("^[a-zA-Z0-9_-]+$", ErrorMessage = "نام انگلیسی باید انگلیسی باشد")]
        public string? EnglishName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "قیمت کتاب الزامی است")]
        public long Price { get; set; }
        [Required(ErrorMessage = "سری چاپ را وارد کنید")]
        [PositiveNumber(ErrorMessage = "سری چاپ نمیتواند صفر یا منفی باشد")]
        public short PrintSeries { get; set; } // سری چاپ
        [Required(ErrorMessage = "شابک کتاب الزامی است")]
        public string ISBN { get; set; } = string.Empty; // شابک
        [Required(ErrorMessage = "نوع جلد کتاب الزامی است")]
        public string CoverType { get; set; } = string.Empty; // نوع جلد
        [Required(ErrorMessage = "فطع کتاب الزامی است")]
        public string Format { get; set; } = string.Empty; // قطع
        [Required(ErrorMessage = "تعداد صفحه کتاب الزامی است")]
        [PositiveNumber(ErrorMessage = "تعداد صفحه نمیتواند صفر یا منفی باشد")]
        public short Pages { get; set; }
        [Required(ErrorMessage = "سال انتشار الزامی است")]
        [PositiveNumber(ErrorMessage = "سال انتشار نمیتواند صفر یا منفی باشد")]
        public short PublishYear { get; set; }
        [Required(ErrorMessage = "وارد کردن نام انتشارات الزامی است")]
        public string Publisher { get; set; } = string.Empty;
        [Required(ErrorMessage = "نویسنده الزامی است")]
        [PositiveNumber(ErrorMessage = "شناسه نویسنده نمیتواند صفر یا منفی باشد")]
        public int AuthorId { get; set; }
        [Required(ErrorMessage = "تصاویر الزامی است")]
        [AllowedExtensions([".jpg", ".jpeg", ".png", ".gif"])]
        [MaxFileSize(1 * 1024 * 1024, ErrorMessage = "حجم فایل نباید بیش از 1 مگابایت باشد")]
        [MaxItems(3, ErrorMessage = "حداکثر 3 تصویر مجاز است")]
        public required List<IFormFile> Images { get; set; }
        [NoDuplicates(ErrorMessage = "لطفا مترجم تکراری وارد نکنید")]
        public List<int>? Translators { get; set; }
        [Required(ErrorMessage = "حداقل یک دسته بندی الزامی است")]
        [NoDuplicates(ErrorMessage = "لطفا دسته بندی تکراری وارد نکنید")]
        public required List<int> Categories { get; set; }
        [Required(ErrorMessage = "حداقل یک تگ الزامی است")]
        [NoDuplicates(ErrorMessage = "لطفا تگ تکراری وارد نکنید")]
        public required List<int> Tags { get; set; }

    }
}
