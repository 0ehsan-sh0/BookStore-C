using BookStoreApi.BusinessLogicLayer.Interfaces.Admin;
using BookStoreApi.RequestHandler.Admin.Mappers;
using BookStoreApi.RequestHandler.Admin.QueryObjects.User;
using BookStoreApi.RequestHandler.Admin.Requests.User;
using BookStoreApi.RequestHandler.Admin.Responses.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class UserController(IBLLUser bLL) : ApiResponseHelper
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] QUserGetAll query)
        {
            var (message, users, pagination, status) = await bLL.GetAllAsync(query);

            var rUsers = users.Select(c => c.ToRUser()).ToList();

            var response = new UserListResponse
            {
                Users = rUsers,
                Pagination = pagination,
            };

            return SuccessResponse("اطلاعات با موفقیت دریافت شد", response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var (message, user, status) = await bLL.GetByIdAsync(id);
            if (user is null)
                return NotFound("نویسنده یافت نشد");

            return SuccessResponse("اطلاعات با موفقیت دریافت شد", user.ToRUser());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] UpdateAdminUserRequest request)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid)
                return BadRequest(errors);

            var (message, user, status) = await bLL.UpdateAsync(request.ToUser(id));

            return status == 200
                ? SuccessResponse(message, user!.ToRUser(), status)
                : status == 404
                    ? NotFound(message)
                    : StatusCode(status, message);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest createUserRequest)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid)
                return BadRequest(errors);

            var (message, user, status) = await bLL.CreateAsync(createUserRequest.ToUser());
            if (user is null)
                return StatusCode(status, message);

            return SuccessResponse(message, user.ToRUser(), status);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid)
                return BadRequest(errors);

            var (message, status) = await bLL.DeleteAsync(id);

            return status == 204 ?
                NoContent() : StatusCode(status, message);
        }


    }
}
