using BookStoreApi.RequestHandler.Validations;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.RequestHandler.Admin.Requests.Image
{
    public class CreateImageRequest
    {
        [Required(ErrorMessage = "شناسه خارجی مورد نیاز است")]
        [PositiveNumber(ErrorMessage = "شناسه نباید صفر یا منفی باشد")]
        public int ForeignId { get; set; }
        [Required(ErrorMessage = "مشخص کنید تصویر متعلق به چه چیزی است")]
        [AcceptableValues(["Books"])]
        public string ForeignTable { get; set; } = string.Empty;
        [Required(ErrorMessage = "تصاویر الزامی است")]
        [AllowedExtensions([".jpg", ".jpeg", ".png", ".gif"])]
        public List<IFormFile> Images { get; set; }
    }
}
