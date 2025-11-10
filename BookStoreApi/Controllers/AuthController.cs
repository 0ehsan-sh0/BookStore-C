using BookStoreApi.BusinessLogicLayer;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Auth.Requests;
using BookStoreApi.RequestHandler.Auth.Responses;
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
        BLLAuth bLLAuth,
        IWebHostEnvironment env
        ) : ApiResponseHelper
    {
        private readonly bool _isDev = env.IsDevelopment();

        private void SetJwtCookie(string token, int expiresInSeconds)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = !_isDev, // secure in prod, false in dev
                SameSite = _isDev ? SameSiteMode.Lax : SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddSeconds(expiresInSeconds),
                Path = "/"
            };
            Response.Cookies.Append("access_token", token, cookieOptions);
        }

        [HttpPost("send-code")]
        public async Task<IActionResult> SendCode([FromBody] SendCodeRequest request)
        {
            if (request.IsRegister && await bLLAuth.UserExist(request.Mobile))
                return ErrorResponse("درخواست نامعتبر", null, 400);

            var response = await smsService.SendVerificationAsync(request.Mobile);

            if (response == null || response.Status != 1)
                return ErrorResponse("خطا در ارسال کد", null, 400);

            var code = response.Data?.MessageId.ToString() ?? "000000";
            VerificationStore.Codes[request.Mobile] = code;

            Console.WriteLine($"[DEBUG] Verification code for {request.Mobile}: {code}");

            return SuccessResponse("کد تایید ارسال شد", null, 201);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!VerificationStore.Codes.TryGetValue(request.Mobile, out var storedCode) || storedCode != request.Code)
                return ErrorResponse("کد نامعتبر", null, 401);

            VerificationStore.Codes.Remove(request.Mobile);

            var user = await bLLAuth.RegisterAsync(new User { Mobile = request.Mobile, Password = request.Password });
            if (user == null)
                return ErrorResponse("خطا در ثبت نام", null, 500);

            var loginResponse = jWTService.Authenticate(user.Mobile, user.Role.ToString());
            SetJwtCookie(loginResponse.AccessToken, loginResponse.ExpiresIn);

            return SuccessResponse("ثبت نام و ورود با موفقیت انجام شد", new
            {
                username = loginResponse.Username,
                expiresIn = loginResponse.ExpiresIn
            }, 200);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            User? user;

            if (!string.IsNullOrWhiteSpace(request.Code))
            {
                user = await bLLAuth.LoginAsync(request.Mobile, request.Code, true);
                if (user == null) return ErrorResponse("کد نامعتبر", null, 401);
            }
            else if (!string.IsNullOrWhiteSpace(request.Password))
            {
                user = await bLLAuth.LoginAsync(request.Mobile, request.Password);
                if (user == null) return ErrorResponse("شماره تلفن یا رمز عبور معتبر نیست", null, 401);
            }
            else
            {
                return ErrorResponse("کد یا رمز عبور ارسال نشده است", null, 400);
            }

            var loginResponse = jWTService.Authenticate(user.Mobile, user.Role.ToString());
            SetJwtCookie(loginResponse.AccessToken, loginResponse.ExpiresIn);

            return SuccessResponse("ورود با موفقیت انجام شد", new
            {
                username = loginResponse.Username,
                expiresIn = loginResponse.ExpiresIn
            }, 200);
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

            return SuccessResponse("خروج با موفقیت انجام شد.", null, 200);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var mobile = User?.Identity?.Name;
            if (string.IsNullOrEmpty(mobile))
                return ErrorResponse("کاربر وارد نشده است", null, 401);

            var user = await bLLAuth.GetUserByMobileAsync(mobile);
            if (user == null)
                return ErrorResponse("کاربر یافت نشد", null, 404);

            var response = MeResponse.FromEntity(user);
            return SuccessResponse("اطلاعات کاربر", response, 200);
        }
    }
}
