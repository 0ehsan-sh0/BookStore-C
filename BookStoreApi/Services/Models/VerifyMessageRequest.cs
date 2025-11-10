using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.Services.Models
{
    public class VerifyMessageRequest
    {
        [Required(ErrorMessage = "شماره موبایل الزامی است.")]
        public string Mobile { get; set; } = string.Empty;

        public int TemplateId { get; set; } = 123456;

        public List<VerifyMessageParameter> Parameters { get; set; } =
        [
            new VerifyMessageParameter()
        ];
    }

    public class VerifyMessageParameter
    {
        public string Name { get; set; } = "Code";
        public string Value { get; set; }

        public VerifyMessageParameter()
        {
            // Generate random 6-digit code
            Value = "12345";
        }
    }

    public class VerifyMessageResponse
    {
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public VerifyMessageResponseData? Data { get; set; }
    }

    public class VerifyMessageResponseData
    {
        public int MessageId { get; set; }
        public float Cost { get; set; }
    }
}
