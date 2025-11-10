using BookStoreApi.BusinessLogicLayer;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Auth.Requests;
using BookStoreApi.Services;
using BookStoreApi.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        ISmsIrService smsService,
        JWTService jWTService,
        BLLAuth bLLAuth
        ) : ApiResponseHelper
    {
        [HttpPost("send-code")]
        public async Task<IActionResult> SendCode([FromBody] SendCodeRequest request)
        {
            // Send code via SmsIrService
            var response = await smsService.SendVerificationAsync(request.Mobile);

            if (response == null || response.Status != 1)
                return ErrorResponse("خطا در ارسال کد", null, 400);

            // Save the code in memory (for demo purposes)
            var code = response.Data?.MessageId.ToString() ?? "000000"; // In sandbox we use MessageId to simulate code
            VerificationStore.Codes[request.Mobile] = code;

            Console.WriteLine(code); // For testing purposes only

            return SuccessResponse("کد تایید ارسال شد", null, 201); // in real, do not return code
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (VerificationStore.Codes.TryGetValue(request.Mobile, out var storedCode))
            {
                if (storedCode == request.Code)
                {
                    // Verification successful
                    VerificationStore.Codes.Remove(request.Mobile); // remove after success

                    var user = await bLLAuth.RegisterAsync(new User { Mobile = request.Mobile, Password = request.Password });
                    if (user is null) return ErrorResponse("خطا در ثبت نام", null, 401);

                    var loginResponce = jWTService.Authenticate(user.Mobile, user.Role.ToString());

                    Response.Cookies.Append("access_token", loginResponce.AccessToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true, // Always true in production (requires HTTPS)
                        SameSite = SameSiteMode.Strict, // or Lax if cross-site requests needed
                        Expires = DateTime.UtcNow.AddSeconds(loginResponce.ExpiresIn)
                    });

                    return SuccessResponse("شما با موفقیت وارد شدید.", new
                    {
                        username = loginResponce.Username,
                        expiresIn = loginResponce.ExpiresIn
                    }, 200);

                }
            }

            return ErrorResponse("کد نامعتبر", null, 401);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Code))
            {
                // Login with code
                var user = await bLLAuth.LoginAsync(request.Mobile, request.Code, true);

                if (user is null)
                    return ErrorResponse("ورود ناموفق", null, 401);

                var loginResponce = jWTService.Authenticate(user.Mobile, user.Role.ToString());

                return SuccessResponse("شما با موفقیت وارد شدید.", loginResponce, 200);
            }
            else if (!string.IsNullOrWhiteSpace(request.Password))
            {
                // Login with password
                var user = await bLLAuth.LoginAsync(request.Mobile, request.Password);
                if (user is null)
                    return ErrorResponse("شماره تلفن یا رمز عبور معتبر نیست", null, 401);

                var loginResponce = jWTService.Authenticate(user.Mobile, user.Role.ToString());
                return SuccessResponse("شما با موفقیت وارد شدید.", loginResponce, 200);
            }
            else
            {
                return ErrorResponse("کد یا رمز عبور ارسال نشده است", null, 400);
            }
        }
    }
}
