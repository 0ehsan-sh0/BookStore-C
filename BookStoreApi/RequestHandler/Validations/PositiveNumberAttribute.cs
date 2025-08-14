using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.RequestHandler.Validations
{
    public class PositiveNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return true; // Consider null as valid, use [Required] for null check

            return value switch
            {
                int intValue => intValue > 0,
                long longValue => longValue > 0,
                float floatValue => floatValue > 0,
                double doubleValue => doubleValue > 0,
                decimal decimalValue => decimalValue > 0,
                short shortValue => shortValue > 0,
                byte byteValue => byteValue > 0,
                _ => false,// Unsupported type
            };
        }
    }
}
