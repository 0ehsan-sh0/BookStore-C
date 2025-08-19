using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.RequestHandler.Admin.Requests.Author
{
    public class CreateAuthorRequest
    {
        [Required(ErrorMessage = "نام دسته بندی الزامی است")]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
