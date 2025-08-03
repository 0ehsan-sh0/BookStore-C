using SixLabors.ImageSharp;

namespace BookStoreApi.Services
{
    public class ImageService
    {
        private static readonly string publicPath = "wwwroot/images";

        private static string GenerateGUID(string format = "jpeg")
        {
            string guid = Guid.NewGuid().ToString();

            string part1 = guid[..2];
            string part2 = guid.Substring(2, 2);
            string rest = guid[4..];

            string result = $"{part1}/{part2}/{rest}.{format}";

            return result;
        }

        private static readonly Dictionary<string, byte[]> MagicBytes = new()
    {
        { "image/jpeg", new byte[] { 0xFF, 0xD8, 0xFF } },
        { "image/png",  new byte[] { 0x89, 0x50, 0x4E, 0x47 } },
        { "image/gif",  new byte[] { 0x47, 0x49, 0x46, 0x38 } },
        // Add more as needed (e.g., WebP, BMP)
    };

        private static async Task<(bool IsValid, byte[]? CleanImage, string? MimeType)> InspectAndSanitizeAsync(Stream uploadedStream)
        {
            // Copy original stream to MemoryStream so we can rewind and inspect
            using var memoryStream = new MemoryStream();
            await uploadedStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            // 1. Magic Byte Check
            var headerBytes = new byte[8];
            memoryStream.Read(headerBytes, 0, headerBytes.Length);
            memoryStream.Position = 0;

            var matchedMime = MagicBytes.FirstOrDefault(kvp =>
                headerBytes.Take(kvp.Value.Length).SequenceEqual(kvp.Value));

            if (matchedMime.Key == null)
                return (false, null, null); // Invalid magic bytes

            // 2. Sanitize via re-encoding (ImageSharp)
            try
            {
                using var image = await Image.LoadAsync(memoryStream); // Load actual image
                using var sanitizedStream = new MemoryStream();
                if (matchedMime.Key == "image/jpeg" || matchedMime.Key == "image/gif")
                    await image.SaveAsJpegAsync(sanitizedStream); // Or .SaveAsPngAsync() based on input
                else await image.SaveAsPngAsync(sanitizedStream);

                return (true, sanitizedStream.ToArray(), matchedMime.Key);
            }
            catch
            {
                return (false, null, null); // Not a valid image even if magic bytes passed
            }
        }

        public static async Task<(string message, string? url, int status)> SaveImageAsync(IFormFile file, string subFolder = "")
        {
            if (file == null || file.Length == 0)
                return ("فایلی ارسال نشده", null, 400);

            using var stream = file.OpenReadStream();
            var (isValid, cleanImageBytes, mimeType) = await InspectAndSanitizeAsync(stream);

            if (!isValid)
                return ("فرمت عکس نامعتبر و یا عکس خراب است", null, 400);

            string imageId = GenerateGUID(mimeType![6..]);
            string path = Path.Combine(publicPath, subFolder, imageId);

            string? dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir!); // ✅ Ensure folders exist

            await File.WriteAllBytesAsync(path, cleanImageBytes!);

            string urlPath = $"/images/{(string.IsNullOrWhiteSpace(subFolder) ? "" : subFolder + "/")}{imageId}";
            return ("عکس با موفقیت ذخیره شد", urlPath, 201);
        }
    }
}
