using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;

namespace BookStoreApi.Services
{
    public static class ImageService
    {
        // Keep configurable but static for backward compatibility. Consider moving to an injected option later.
        public static string publicPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        private static string GenerateGUID(string extension = "jpeg")
        {
            // Use GUID without dashes and produce nested folders: ab/cd/<rest>.<ext>
            string guid = Guid.NewGuid().ToString("N"); // no dashes

            string part1 = guid.Substring(0, 2);
            string part2 = guid.Substring(2, 2);
            string rest = guid.Substring(4);

            // return a path-like string (will be split by Path.Combine later)
            return Path.Combine(part1, part2, rest + "." + extension.TrimStart('.'));
        }

        // Inspect and sanitize the uploaded image. Returns sanitized bytes, mime, dims and extension.
        private static async Task<(bool IsValid, byte[]? CleanImage, string? MimeType, int Height, int Width, string? Extension)> InspectAndSanitizeAsync(Stream uploadedStream)
        {
            using var memoryStream = new MemoryStream();
            await uploadedStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            try
            {
                // Detect format first (returns null for unknown/corrupted data)
                IImageFormat? detectedFormat = Image.DetectFormat(memoryStream);
                if (detectedFormat == null)
                    return (false, null, null, 0, 0, null);

                // Reset position before loading
                memoryStream.Position = 0;

                // Load the image (use the parameterless overload that accepts a Stream)
                using var image = Image.Load(memoryStream);
                int height = image.Height;
                int width = image.Width;

                using var sanitizedStream = new MemoryStream();

                var mime = detectedFormat.DefaultMimeType ?? "application/octet-stream";
                var ext = detectedFormat.FileExtensions?.FirstOrDefault() ?? "jpg";

                if (mime.Equals("image/gif", StringComparison.OrdinalIgnoreCase))
                {
                    // preserve gif (animation if present)
                    await image.SaveAsGifAsync(sanitizedStream);
                }
                else if (mime.Equals("image/jpeg", StringComparison.OrdinalIgnoreCase) ||
                         ext.Equals("jpg", StringComparison.OrdinalIgnoreCase) ||
                         ext.Equals("jpeg", StringComparison.OrdinalIgnoreCase))
                {
                    await image.SaveAsJpegAsync(sanitizedStream);
                }
                else if (mime.Equals("image/png", StringComparison.OrdinalIgnoreCase) ||
                         ext.Equals("png", StringComparison.OrdinalIgnoreCase))
                {
                    await image.SaveAsPngAsync(sanitizedStream);
                }
                else
                {
                    // fallback: save as PNG
                    await image.SaveAsPngAsync(sanitizedStream);
                    mime = "image/png";
                    ext = "png";
                }

                var bytes = sanitizedStream.ToArray();
                return (true, bytes, mime, height, width, ext);
            }
            catch
            {
                return (false, null, null, 0, 0, null);
            }
        }
        // Keep original tuple signature so calling code expects the same shape.
        public static async Task<(string message, Models.ImageInfo? information, int status)> SaveImageAsync(IFormFile file, string subFolder = "")
        {
            if (file == null || file.Length == 0)
                return ("فایلی ارسال نشده", null, 400);

            using var stream = file.OpenReadStream();
            var (isValid, cleanImageBytes, mimeType, height, width, extension) = await InspectAndSanitizeAsync(stream);

            if (!isValid || cleanImageBytes == null || mimeType == null)
                return ("فرمت عکس نامعتبر و یا عکس خراب است", null, 400);

            string ext = string.IsNullOrWhiteSpace(extension) ? "jpg" : extension;
            string imageId = GenerateGUID(ext);

            // Build target path safely using path segments
            string safeSub = string.IsNullOrWhiteSpace(subFolder) ? string.Empty : subFolder;

            var segments = new List<string> { publicPath };
            if (!string.IsNullOrWhiteSpace(safeSub))
                segments.Add(safeSub);

            // imageId may contain directory separators (from GenerateGUID)
            segments.AddRange(imageId.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries));

            string path = Path.Combine(segments.ToArray());

            string? dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir!); // Ensure folders exist

            await File.WriteAllBytesAsync(path, cleanImageBytes);

            // Compute relative path (web-facing) and stored filename
            var relative = Path.GetRelativePath(publicPath, path).Replace('\\', '/'); // e.g., "ab/cd/<file.ext>"
            var storedFileName = Path.GetFileName(path);

            // Directory portion (with trailing slash) so callers can safely concat StoredFileName
            var relativeDir = Path.GetDirectoryName(relative)?.Replace('\\', '/') ?? string.Empty;
            if (!string.IsNullOrEmpty(relativeDir) && !relativeDir.EndsWith('/'))
                relativeDir += "/";

            var imageInfo = new Models.ImageInfo
            {
                Height = height.ToString(),
                Width = width.ToString(),
                StoredFileName = storedFileName,
                RelativePath = relativeDir,
                FileSize = cleanImageBytes.Length.ToString(),
                MimeType = mimeType
            };

            return ("عکس با موفقیت ذخیره شد", imageInfo, 201);
        }

        // NOTE: These deletion functions expect relative paths like "ab/cd/xxxx.ext" or "subfolder/ab/cd/xxxx.ext".
        public static (string message, int deletedCount) DeleteImages(List<string> relativePaths)
        {
            if (relativePaths == null || relativePaths.Count == 0)
                return ("لیست تصاویر برای حذف خالی است", 0);

            int deletedCount = 0;

            foreach (var relativePath in relativePaths)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(relativePath))
                        continue;

                    // Normalize and build full path under publicPath (defense-in-depth against traversal)
                    var parts = relativePath.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                    var candidate = Path.Combine(new[] { publicPath }.Concat(parts).ToArray());
                    var fullPath = Path.GetFullPath(candidate);
                    var fullRoot = Path.GetFullPath(publicPath).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;

                    if (!fullPath.StartsWith(fullRoot, StringComparison.OrdinalIgnoreCase))
                    {
                        // outside of public path - skip
                        continue;
                    }

                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                        deletedCount++;
                    }
                }
                catch
                {
                    // Skip failures for now; consider logging
                }
            }

            string msg = deletedCount == relativePaths.Count
                ? "تمام تصاویر با موفقیت حذف شدند."
                : $"تعداد {deletedCount} از {relativePaths.Count} تصویر حذف شدند.";

            return (msg, deletedCount);
        }

        public static (string message, bool status) DeleteImage(string relativePath)
        {
            if (relativePath == null || string.IsNullOrWhiteSpace(relativePath))
                return ("لیست تصاویر برای حذف خالی است", false);

            bool status = false;
            try
            {
                var parts = relativePath.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                var candidate = Path.Combine(new[] { publicPath }.Concat(parts).ToArray());
                var fullPath = Path.GetFullPath(candidate);
                var fullRoot = Path.GetFullPath(publicPath).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;

                if (fullPath.StartsWith(fullRoot, StringComparison.OrdinalIgnoreCase) && File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    status = true;
                }
            }
            catch
            {
                // consider logging
            }

            string msg = status
               ? "تصویر با موفقیت حذف شد"
               : $"در حذف تصویر مشکلی پیش آمد";

            return (msg, status);
        }
    }
}