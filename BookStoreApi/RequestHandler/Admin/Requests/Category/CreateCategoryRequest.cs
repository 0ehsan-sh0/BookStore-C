using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.RequestHandler.Admin.Requests.Category
{
    public class CreateCategoryRequest
    {
        [Required(ErrorMessage = "نام دسته بندی الزامی است")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "لینک دسته بندی الزامی است")]
        [MinLength(4, ErrorMessage = "حداقل کاراکتر مجاز 4 کاراکتر است")]
        [MaxLength(255, ErrorMessage = "حداکثر کاراکتر مجاز 255 کاراکتراست")]
        public string Url { get; set; } = string.Empty;
        public int? MainCategoryId { get; set; } = null;
    }
}
