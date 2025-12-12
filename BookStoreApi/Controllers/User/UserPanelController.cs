using BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel;
using BookStoreApi.RequestHandler.User.Mappers;
using BookStoreApi.RequestHandler.User.QueryObjects.Book;
using BookStoreApi.RequestHandler.User.QueryObjects.Invoice;
using BookStoreApi.RequestHandler.User.Requests.User;
using BookStoreApi.RequestHandler.User.Requests.WishList;
using BookStoreApi.RequestHandler.User.Responses.Book;
using BookStoreApi.RequestHandler.User.Responses.Invoice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers.User
{
    [Route("api/user")]
    [ApiController]
    [Authorize(Roles = "User,Admin")]
    public class UserPanelController(IBLLUserPanel bllUser, IBLLUserInvoice bLLUserInvoice) : ApiResponseHelper
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

        [HttpGet("invoice")]
        public async Task<IActionResult> GetInvoicesAsync([FromQuery] QUserInvoices query)
        {
            var userMobile = User.Identity?.Name!;
            var (invoices, pagination) = await bLLUserInvoice.GetUserInvoicesAsync(userMobile, query);

            UserInvoicesList response = new UserInvoicesList
            {
                Invoices = invoices?.Select(b => b.ToRInvoice()).ToList(),
                Pagination = pagination
            };

            return SuccessResponse("اطلاعات با موفقیت دریافت شد", response);
        }

        [HttpGet("invoice/{id:int}")]
        public async Task<IActionResult> GetInvoiceAsync([FromRoute] int id)
        {
            var userMobile = User.Identity?.Name!;
            var (message, invoice, status) = await bLLUserInvoice.GetByIdAsync(userMobile, id);

            return status == 404 ?
                NotFound(message) :
                SuccessResponse(message, invoice, status);
        }

        [HttpPost]
        [Route("wishlist")]
        public async Task<IActionResult> ToggleWishListAsync([FromBody] ToggleWishList request)
        {
            var userMobile = User.Identity?.Name!;
            var (message, wishlistStatus, status) = await bllUser.ToggleWishListAsync(userMobile, request.BookId);

            return status == 404 ?
                NotFound(message) :
                SuccessResponse(message, wishlistStatus, status);
        }

        [HttpGet("wishlist")]
        public async Task<IActionResult> GetUserWishListAsync([FromQuery] QUserWishList query)
        {
            var userMobile = User.Identity?.Name!;

            var (message, books, info, status) = await bllUser.GetUserWithList(userMobile, query);
            if (status != 200)
                return StatusCode(status, message);

            var response = new UserBooksListResponse
            {
                Books = books!.Select(b => b.ToRBookAllData()).ToList(),
                Pagination = info!
            };
            return SuccessResponse(message, response, status);
        }
    }
}
