using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers
{
    public class ApiResponseHelper : ControllerBase
    {
        protected (bool isValid, object? errors) ModelStateValidation()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value!.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return (false, errors);
            }
            return (true, null);
        }

        protected IActionResult SuccessResponse(string message, object? data, int status = 200)
        {
            var result = new
            {
                message,
                data
            };

            return StatusCode(status, result);
        }

        protected IActionResult ErrorResponse(string message, object? data, int status = 404)
        {
            var result = new
            {
                message,
                errors = data
            };

            return StatusCode(status, result);
        }
    }
}
