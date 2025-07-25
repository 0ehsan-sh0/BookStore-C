using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok();
        }
    }
}
