using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BookStoreApi.RequestHandler.Validations
{
    public class IsbnAttribute : ValidationAttribute
    {
        private static readonly Regex isbn10Regex = new(@"^(?:\d[\- ]?){9}[\dX]$", RegexOptions.Compiled);
        private static readonly Regex isbn13Regex = new(@"^(?:\d[\- ]?){13}$", RegexOptions.Compiled);

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null)
                return ValidationResult.Success; // Handle [Required] separately if needed

            string isbn = value.ToString()?.Replace("-", "").Replace(" ", "") ?? "";

            if (isbn.Length == 10 && isbn10Regex.IsMatch(isbn))
            {
                if (IsValidIsbn10(isbn))
                    return ValidationResult.Success;
            }
            else if (isbn.Length == 13 && isbn13Regex.IsMatch(isbn))
            {
                if (IsValidIsbn13(isbn))
                    return ValidationResult.Success;
            }

            return new ValidationResult("Invalid ISBN number.");
        }

        private bool IsValidIsbn10(string isbn)
        {
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                if (!char.IsDigit(isbn[i])) return false;
                sum += (isbn[i] - '0') * (10 - i);
            }

            char lastChar = isbn[9];
            sum += (lastChar == 'X') ? 10 : (char.IsDigit(lastChar) ? (lastChar - '0') : -100);

            return sum % 11 == 0;
        }

        private bool IsValidIsbn13(string isbn)
        {
            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                if (!char.IsDigit(isbn[i])) return false;
                int digit = isbn[i] - '0';
                sum += (i % 2 == 0) ? digit : digit * 3;
            }

            int checksum = (10 - (sum % 10)) % 10;
            return checksum == (isbn[12] - '0');
        }
    }
}
