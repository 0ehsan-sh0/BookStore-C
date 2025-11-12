using BookStoreApi.BusinessLogicLayer.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Auth.Requests;
using BookStoreApi.RequestHandler.User.Mappers;
using BookStoreApi.Services;
using BookStoreApi.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        ISmsIrService smsService,
        JWTService jWTService,
        IBLLAuth bLLAuth,
        IWebHostEnvironment env
    ) : ApiResponseHelper
    {
        private readonly bool _isDev = env.IsDevelopment();

        private void SetJwtCookie(string token, int expiresInSeconds)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // secure in prod, false in dev
                SameSite = _isDev ? SameSiteMode.None : SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddSeconds(expiresInSeconds),
                Path = "/"
            };
            Response.Cookies.Append("access_token", token, cookieOptions);
        }

        [HttpPost("send-code")]
        public async Task<IActionResult> SendCode([FromBody] SendCodeRequest request)
        {
            if (request.IsRegister && await bLLAuth.UserExist(request.Mobile))
                return BadRequest("درخواست نامعتبر");

            var response = await smsService.SendVerificationAsync(request.Mobile);

            if (response == null || response.Status != 1)
                return StatusCode(500, "خطا در ارسال کد");

            var code = response.Data?.MessageId.ToString() ?? "000000";
            VerificationStore.Codes[request.Mobile] = code;

            Console.WriteLine($"[DEBUG] Verification code for {request.Mobile}: {code}");

            return Created(string.Empty, new { message = "کد تایید ارسال شد" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!VerificationStore.Codes.TryGetValue(request.Mobile, out var storedCode) || storedCode != request.Code)
                return Unauthorized("کد نامعتبر");

            VerificationStore.Codes.Remove(request.Mobile);

            var user = await bLLAuth.RegisterAsync(new User { Mobile = request.Mobile, Password = request.Password });
            if (user == null)
                return StatusCode(500, "خطا در ثبت نام");

            var loginResponse = jWTService.Authenticate(user.Mobile, user.Role.ToString());
            SetJwtCookie(loginResponse.AccessToken, loginResponse.ExpiresIn);

            return Ok(new
            {
                message = "ثبت نام و ورود با موفقیت انجام شد",
                username = loginResponse.Username,
                expiresIn = loginResponse.ExpiresIn
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            User? user;

            if (!string.IsNullOrWhiteSpace(request.Code))
            {
                user = await bLLAuth.LoginAsync(request.Mobile, request.Code, true);
                if (user == null) return Unauthorized("کد نامعتبر");
            }
            else if (!string.IsNullOrWhiteSpace(request.Password))
            {
                user = await bLLAuth.LoginAsync(request.Mobile, request.Password);
                if (user == null) return Unauthorized("شماره تلفن یا رمز عبور معتبر نیست");
            }
            else
            {
                return BadRequest("کد یا رمز عبور ارسال نشده است");
            }

            var loginResponse = jWTService.Authenticate(user.Mobile, user.Role.ToString());
            SetJwtCookie(loginResponse.AccessToken, loginResponse.ExpiresIn);

            return Ok(new
            {
                message = "ورود با موفقیت انجام شد",
                username = loginResponse.Username,
                expiresIn = loginResponse.ExpiresIn
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token", new CookieOptions
            {
                HttpOnly = true,
                Secure = !_isDev,
                SameSite = _isDev ? SameSiteMode.Lax : SameSiteMode.None,
                Path = "/"
            });

            return Ok(new { message = "خروج با موفقیت انجام شد." });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var mobile = User?.Identity?.Name;
            if (string.IsNullOrEmpty(mobile))
                return Unauthorized("کاربر وارد نشده است");

            var user = await bLLAuth.GetUserByMobileAsync(mobile);
            if (user == null)
                return NotFound("کاربر یافت نشد");

            return Ok(new
            {
                message = "اطلاعات کاربر",
                data = user.ToRUser()
            });
        }
    }
}
