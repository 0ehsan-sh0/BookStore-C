using BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel;
using BookStoreApi.RequestHandler.User.Mappers;
using BookStoreApi.RequestHandler.User.Requests.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.User
{
    [Route("api/user")]
    [ApiController]
    [Authorize(Roles = "User,Admin")]
    public class UserPanelController(IBLLUserPanel bllUser) : ApiResponseHelper
    {
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateUserRequest user)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid)
                return BadRequest(errors);

            var updatedUser = await bllUser.UpdateAsync(user.ToUser(User.Identity?.Name!));

            if (updatedUser is null)
                return StatusCode(500, "خطایی در بروزرسانی اطلاعات رخ داد");


            return SuccessResponse("اطلاعات با موفقیت بروزرسانی شد", updatedUser.ToRUser());
        }
    }
}
