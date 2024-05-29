
using System.Security.Cryptography;
using System.Text;

namespace QuickLinker.API.Services
{
    public class ShortLinkService : IShortLinkService
    {
        private const int ShortCodeLength = 10;

        public string GenerateShortLink(string originalUrl)
        {
            if (string.IsNullOrEmpty(originalUrl))
            {
                return string.Empty;
            }

            using var sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(originalUrl));
            string base64Hash = Convert.ToBase64String(hashBytes);
            // Remove padding characters '='
            base64Hash = base64Hash.Replace("=", "");
            // Replace non-alphanumeric characters with '-'
            base64Hash = base64Hash.Replace("+", "-").Replace("/", "_");

            // Take the first ShortCodeLength characters
            return base64Hash.Substring(0, ShortCodeLength);
        }
    }
}
