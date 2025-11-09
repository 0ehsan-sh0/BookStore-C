namespace BookStoreApi.Services.Models
{
    public static class VerificationStore
    {
        // key = mobile number, value = code
        public static Dictionary<string, string> Codes { get; } = [];
    }
}
