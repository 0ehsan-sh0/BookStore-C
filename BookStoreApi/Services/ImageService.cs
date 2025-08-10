using SixLabors.ImageSharp;

namespace BookStoreApi.Services
{
    public static class ImageService
    {
        public static string publicPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

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

        private static async Task<(bool IsValid, byte[]? CleanImage, string? MimeType, int Height, int Width)> InspectAndSanitizeAsync(Stream uploadedStream)
        {
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
                return (false, null, null, 0, 0);

            // 2. Sanitize + get dimensions
            try
            {
                using var image = await Image.LoadAsync(memoryStream);
                int height = image.Height;
                int width = image.Width;

                using var sanitizedStream = new MemoryStream();
                if (matchedMime.Key == "image/jpeg" || matchedMime.Key == "image/gif")
                    await image.SaveAsJpegAsync(sanitizedStream);
                else
                    await image.SaveAsPngAsync(sanitizedStream);

                return (true, sanitizedStream.ToArray(), matchedMime.Key, height, width);
            }
            catch
            {
                return (false, null, null, 0, 0);
            }
        }

        public static async Task<(string message, Models.ImageInfo? information, int status)> SaveImageAsync(IFormFile file, string subFolder = "")
        {
            if (file == null || file.Length == 0)
                return ("فایلی ارسال نشده", null, 400);

            using var stream = file.OpenReadStream();
            var (isValid, cleanImageBytes, mimeType, height, width) = await InspectAndSanitizeAsync(stream);

            if (!isValid)
                return ("فرمت عکس نامعتبر و یا عکس خراب است", null, 400);

            string imageId = GenerateGUID(mimeType![6..]);
            string path = Path.Combine(publicPath, subFolder, imageId);

            string? dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir!); // ✅ Ensure folders exist

            await File.WriteAllBytesAsync(path, cleanImageBytes!);

            string urlPath = $"/images/{(string.IsNullOrWhiteSpace(subFolder) ? "" : subFolder + "/")}{imageId}";
            int relativePathLength = subFolder.Length + 6;
            return ("عکس با موفقیت ذخیره شد", new Models.ImageInfo
            {
                Height = height.ToString(), // Image dimensions can be set later if needed
                Width = width.ToString(),
                StoredFileName = imageId[6..],
                RelativePath = urlPath.Substring(8, relativePathLength),
                FileSize = file.Length.ToString(),
                MimeType = mimeType
            }, 201);
        }

        public static async Task<(string message, int deletedCount)> DeleteImagesAsync(List<string> relativePaths)
        {
            if (relativePaths == null || relativePaths.Count == 0)
                return ("لیست تصاویر برای حذف خالی است", 0);

            int deletedCount = 0;

            foreach (var relativePath in relativePaths)
            {
                try
                {
                    // Compose the full physical path based on relativePath and subFolder
                    // If relativePath is something like "d8/c2/xxxx.jpeg" or includes subfolder already:
                    // Adjust logic as needed for your input format.
                    string fullPath = Path.Combine(publicPath, relativePath);

                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                        deletedCount++;
                    }
                    // Optionally, log if file not found
                }
                catch (Exception ex)
                {
                    // Log the error (ex) if you have a logging framework
                    // For now just skip and continue
                }
            }

            string msg = deletedCount == relativePaths.Count
                ? "تمام تصاویر با موفقیت حذف شدند."
                : $"تعداد {deletedCount} از {relativePaths.Count} تصویر حذف شدند.";

            return (msg, deletedCount);
        }
    }
}
