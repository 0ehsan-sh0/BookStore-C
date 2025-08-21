using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.RequestHandler.Validations
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly long _maxBytes;

        public MaxFileSizeAttribute(long maxBytes)
        {
            _maxBytes = maxBytes;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            // Single file
            if (value is IFormFile file)
            {
                if (file.Length > _maxBytes)
                    return new ValidationResult(ErrorMessage ?? $"File size cannot exceed {_maxBytes / 1024 / 1024} MB.");
            }
            // Multiple files
            else if (value is IEnumerable<IFormFile> files)
            {
                foreach (var f in files)
                {
                    if (f.Length > _maxBytes)
                        return new ValidationResult(ErrorMessage ?? $"Each file cannot exceed {_maxBytes / 1024 / 1024} MB.");
                }
            }
            else
            {
                return new ValidationResult("Invalid file type.");
            }

            return ValidationResult.Success;
        }
    }
}
