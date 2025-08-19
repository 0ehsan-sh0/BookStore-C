using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.RequestHandler.Admin.Requests.Category
{
    public class UpdateCategoryRequest
    {
        [Required(ErrorMessage = "نام دسته بندی الزامی است")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "لینک دسته بندی الزامی است")]
        [MinLength(4, ErrorMessage = "حداقل کاراکتر مجاز 4 کاراکتر است")]
        [MaxLength(255, ErrorMessage = "حداکثر کاراکتر مجاز 255 کاراکتراست")]
        [RegularExpression("^[a-zA-Z0-9_-]+$", ErrorMessage = "لینک باید انگلیسی باشد")]
        public string Url { get; set; } = string.Empty;
        public int? MainCategoryId { get; set; }
    }
}
