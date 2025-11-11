using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.RequestHandler.User.QueryObjects
{
    public class QueryObject
    {
        public int PageNumber { get; set; } = 1;
        [Range(1, 100, ErrorMessage = "حداکثر تعداد در هر صفحه 100 عدد است.")]
        public int PageSize { get; set; } = 20;
    }
}