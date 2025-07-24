using System.Security.Cryptography;

namespace BookStoreApi.Services
{
    public static class PasswordHasher
    {
        // Configuration constants
        private const int SaltSize = 16; // 128-bit
        private const int HashSize = 32; // 256-bit
        private const int Iterations = 100_000;

        public static string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

            // Derive the hash
            var hash = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256)
                            .GetBytes(HashSize);

            // Combine salt + hash + iteration count (for storage)
            var hashBytes = new byte[1 + 4 + SaltSize + HashSize];
            hashBytes[0] = 1; // version marker
            BitConverter.GetBytes(Iterations).CopyTo(hashBytes, 1);
            salt.CopyTo(hashBytes, 5);
            hash.CopyTo(hashBytes, 5 + SaltSize);

            // Return as base64 string
            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            var hashBytes = Convert.FromBase64String(hashedPassword);

            if (hashBytes[0] != 1)
                throw new NotSupportedException("Unknown hash version.");

            int iterations = BitConverter.ToInt32(hashBytes, 1);
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 5, salt, 0, SaltSize);

            byte[] storedHash = new byte[HashSize];
            Array.Copy(hashBytes, 5 + SaltSize, storedHash, 0, HashSize);

            // Hash input password using the same salt and iterations
            var hash = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256)
                            .GetBytes(HashSize);

            // Compare securely
            return CryptographicOperations.FixedTimeEquals(hash, storedHash);
        }
    }
}
