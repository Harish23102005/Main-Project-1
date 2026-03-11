using System.Security.Cryptography;
using System.Text;

namespace MainProject1.Helpers
{
    public static class PasswordHelper
    {
        public static string Hash(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes).ToLower();
        }

        public static bool Verify(string password, string hash)
        {
            return Hash(password) == hash;
        }

        // A plain-text password will never be 64 hex chars
        public static bool IsHashed(string value) => value.Length == 64;
    }
}
