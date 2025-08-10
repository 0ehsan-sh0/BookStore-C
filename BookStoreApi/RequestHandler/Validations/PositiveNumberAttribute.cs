using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.RequestHandler.Validations
{
    public class PositiveNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return true; // Consider null as valid, use [Required] for null check

            switch (value)
            {
                case int intValue:
                    return intValue > 0;
                case long longValue:
                    return longValue > 0;
                case float floatValue:
                    return floatValue > 0;
                case double doubleValue:
                    return doubleValue > 0;
                case decimal decimalValue:
                    return decimalValue > 0;
                case short shortValue:
                    return shortValue > 0;
                case byte byteValue:
                    return byteValue > 0;
                default:
                    return false; // Unsupported type
            }
        }
    }
}
