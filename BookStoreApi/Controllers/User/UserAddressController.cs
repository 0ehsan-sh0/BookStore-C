using BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel;
using BookStoreApi.RequestHandler.User.Mappers;
using BookStoreApi.RequestHandler.User.QueryObjects.Address;
using BookStoreApi.RequestHandler.User.Requests.Address;
using BookStoreApi.RequestHandler.User.Responses.Address;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.User
{
    [Route("api/user/address")]
    [ApiController]
    [Authorize(Roles = "User,Admin")]
    public class UserAddressController(IBLLUserAddress bLL) : ApiResponseHelper
    {
        [HttpGet]
        public async Task<IActionResult> GetUserAddressesAsync([FromQuery] QUserAddress query)
        {
            var mobile = User.Identity?.Name;

            var (addresses, info) = await bLL.GetUserAddressesAsync(mobile!, query);

            if (addresses == null || info == null)
                return NotFound("کاربر پیدا نشد.");

            var response = new UserAddressList
            {
                Addresses = addresses.Select(a => a.ToRAddress()).ToList(),
                Pagination = info
            };

            return SuccessResponse("آدرس‌های کاربر با موفقیت بازیابی شد.", response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var mobile = User.Identity?.Name;
            var (message, address, status) = await bLL.GetById(mobile!, id);

            return status == 200
                ? SuccessResponse(message, address!.ToRAddress(), status)
                : NotFound(message);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAddressRequest createaddressRequest)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid)
                return BadRequest(errors);

            var mobile = User.Identity?.Name;

            var (message, address, status) = await bLL.CreateAsync(mobile!, createaddressRequest.ToAddressInfo());

            return status == 201
                ? SuccessResponse(message, address!.ToRAddress(), status)
                : StatusCode(status, message);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateAddressRequest request)
        {
            var (isValid, errors) = ModelStateValidation();
            if (!isValid)
                return BadRequest(errors);

            var mobile = User.Identity?.Name;

            var (message, address, status) = await bLL.UpdateAsync(mobile!, request.ToAddressInfo(id));

            return status == 200
                ? SuccessResponse(message, address!.ToRAddress(), status)
                : status == 404
                    ? NotFound(message)
                    : StatusCode(status, message);
        }
    }
}
