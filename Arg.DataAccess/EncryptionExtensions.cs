using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Arg.DataAccess
{
    public static class EncryptionExtensions
    {
        public static string Decrypt(this string data, string key, Encoding encodingUsing = null, SymmetricAlgorithm algorithmUsing = null, string salt = "Kosher", string hashAlgorithm = "SHA1", int passwordIterations = 2, string initialVector = "OFRna73m*aze01xY", int keySize = 256)
        {
            if (string.IsNullOrEmpty(data))
            {
                return "";
            }

            byte[] cipherBytes = Convert.FromBase64String(data);
            byte[] keyBytes = new Rfc2898DeriveBytes(key, Encoding.UTF8.GetBytes(salt), passwordIterations).GetBytes(keySize / 8);

            using (SymmetricAlgorithm algorithm = algorithmUsing ?? Aes.Create())
            {
                algorithm.Mode = CipherMode.CBC;
                using (ICryptoTransform decryptor = algorithm.CreateDecryptor(keyBytes, Encoding.UTF8.GetBytes(initialVector)))
                {
                    using (MemoryStream memoryStream = new MemoryStream(cipherBytes))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader reader = new StreamReader(cryptoStream, encodingUsing ?? Encoding.UTF8))
                            {
                                return reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }

        public static string Encrypt(this string data, string key, Encoding encodingUsing = null, SymmetricAlgorithm algorithmUsing = null, string salt = "Kosher", string hashAlgorithm = "SHA1", int passwordIterations = 2, string initialVector = "OFRna73m*aze01xY", int keySize = 256)
        {
            if (string.IsNullOrEmpty(data))
            {
                return "";
            }

            byte[] clearBytes = (encodingUsing ?? Encoding.UTF8).GetBytes(data);
            byte[] keyBytes = new Rfc2898DeriveBytes(key, Encoding.UTF8.GetBytes(salt), passwordIterations).GetBytes(keySize / 8);

            using (SymmetricAlgorithm algorithm = algorithmUsing ?? Aes.Create())
            {
                algorithm.Mode = CipherMode.CBC;
                using (ICryptoTransform encryptor = algorithm.CreateEncryptor(keyBytes, Encoding.UTF8.GetBytes(initialVector)))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(clearBytes, 0, clearBytes.Length);
                            cryptoStream.FlushFinalBlock();
                            return Convert.ToBase64String(memoryStream.ToArray());
                        }
                    }
                }
            }
        }
    }
}
