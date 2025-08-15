using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.RequestHandler.Validations
{
    public class NoDuplicatesAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IEnumerable list)
            {
                var seen = new HashSet<object?>();
                foreach (var item in list)
                {
                    if (!seen.Add(item))
                    {
                        return new ValidationResult(ErrorMessage ?? "Duplicate values are not allowed.");
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
