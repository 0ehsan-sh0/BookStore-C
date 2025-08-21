using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.RequestHandler.Validations
{
    public class MaxItemsAttribute : ValidationAttribute
    {
        private readonly int _maxCount;

        public MaxItemsAttribute(int maxCount)
        {
            _maxCount = maxCount;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            if (value is IEnumerable collection)
            {
                int count = 0;
                foreach (var _ in collection)
                    count++;

                if (count > _maxCount)
                    return new ValidationResult(ErrorMessage ?? $"The list cannot have more than {_maxCount} items.");
            }
            else
            {
                return new ValidationResult("The attribute can only be applied to IEnumerable types.");
            }

            return ValidationResult.Success;
        }
    }
}
