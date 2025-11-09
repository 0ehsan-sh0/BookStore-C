using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.RequestHandler.Validations
{
    public class RequireOneOfAttribute : ValidationAttribute
    {
        private readonly string[] _properties;

        public RequireOneOfAttribute(params string[] properties)
        {
            _properties = properties;
            ErrorMessage = $"حداقل یکی از فیلدهای {string.Join(", ", _properties)} باید مقدار داشته باشد.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var type = validationContext.ObjectType;

            bool anyHasValue = false;
            foreach (var propName in _properties)
            {
                var prop = type.GetProperty(propName);
                if (prop == null) continue;
                var val = prop.GetValue(validationContext.ObjectInstance) as string;
                if (!string.IsNullOrWhiteSpace(val))
                {
                    anyHasValue = true;
                    break;
                }
            }

            if (!anyHasValue)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
