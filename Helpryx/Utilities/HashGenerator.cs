using System.Text;
using System.Security.Cryptography;

namespace Api.Utilities
{
    public static class HashGenerator
    {
        public static string SHA1(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString().ToLower();
            }
        }

        public static string UniqueHashGen()
        {
            return SHA1(SHA1(Guid.NewGuid().ToString() + " || " + DateTime.UtcNow.ToString("yyyy-MM-dd | HH:mm:ss.fffffff")));
        }
    }
}
