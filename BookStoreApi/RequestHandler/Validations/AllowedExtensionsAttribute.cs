using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.RequestHandler.Validations
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file == null) return ValidationResult.Success;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!_extensions.Contains(extension))
            {
                return new ValidationResult($"فرمت فایل مجاز نیست. فرمت‌های مجاز: {string.Join(", ", _extensions)}");
            }

            return ValidationResult.Success;
        }
    }
}
