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
            var files = new List<IFormFile>();

            if (value is IFormFile singleFile)
            {
                files.Add(singleFile);
            }
            else if (value is IEnumerable<IFormFile> multipleFiles)
            {
                files.AddRange(multipleFiles);
            }

            foreach (var file in files)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_extensions.Contains(extension))
                {
                    return new ValidationResult(
                        $"فرمت فایل مجاز نیست. فرمت‌های مجاز: {string.Join(", ", _extensions)}"
                    );
                }
            }

            return ValidationResult.Success;
        }
    }
}
