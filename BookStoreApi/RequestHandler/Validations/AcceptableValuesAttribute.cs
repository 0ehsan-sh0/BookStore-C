using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.RequestHandler.Validations
{
    public class AcceptableValuesAttribute : ValidationAttribute
    {
        private readonly string[] _allowedValues;

        public AcceptableValuesAttribute(string[] allowedValues)
        {
            _allowedValues = allowedValues;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success; // Let [Required] handle null checks

            // Normalize allowed values for case-insensitive comparison
            var allowedSet = _allowedValues.Select(v => v.ToLowerInvariant()).ToHashSet();

            if (value is string strValue)
            {
                if (!allowedSet.Contains(strValue.ToLowerInvariant()))
                {
                    return new ValidationResult(
                        $"مقدار '{strValue}' مجاز نیست. مقادیر مجاز: {string.Join(", ", _allowedValues)}"
                    );
                }
            }
            else if (value is IEnumerable enumerable) // Covers List<string>, arrays, etc.
            {
                foreach (var item in enumerable)
                {
                    if (item is string s && !allowedSet.Contains(s.ToLowerInvariant()))
                    {
                        return new ValidationResult(
                            $"مقدار '{s}' مجاز نیست. مقادیر مجاز: {string.Join(", ", _allowedValues)}"
                        );
                    }
                }
            }
            else
            {
                return new ValidationResult("نوع داده پشتیبانی نمی‌شود.");
            }

            return ValidationResult.Success;
        }
    }
}
