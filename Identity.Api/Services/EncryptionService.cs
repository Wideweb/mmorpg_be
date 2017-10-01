using System;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Api.Services
{
    public class EncryptionService : IEncryptionService
    {
        public string CreateSaltKey(int size)
        {
            // Generate a cryptographic random number
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number
            return Convert.ToBase64String(buff);
        }

        public string CreatePasswordHash(string password, string saltkey)
        {
            var cryptoServiceProvider = new SHA1CryptoServiceProvider();

            var saltAndPassword = string.Concat(password, saltkey);
            var hash = cryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(saltAndPassword));
            return Encoding.UTF8.GetString(hash);
        }
    }
}
