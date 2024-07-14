using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;

namespace Builder.Common
{
    public static class Extensions
    {
        public static string Encrypt(this string text, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            if (keyBytes.Length != 16 && keyBytes.Length != 24 && keyBytes.Length != 32)
            {
                throw new ArgumentException("Key must be 16, 24, or 32 bytes long.");
            }

            byte[] iv = Encoding.UTF8.GetBytes("0000000000000000"); // 16-byte zero IV

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = iv;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(text);
                        }
                    }
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }

        public static string Decrypt(this string encodedText, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            if (keyBytes.Length != 16 && keyBytes.Length != 24 && keyBytes.Length != 32)
            {
                throw new ArgumentException("Key must be 16, 24, or 32 bytes long.");
            }

            byte[] iv = Encoding.UTF8.GetBytes("0000000000000000"); // 16-byte zero IV
            byte[] cipherTextBytes = Convert.FromBase64String(encodedText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = iv;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static object ToJsonObject(this string data)
        {
            try
            {
                return JsonObject.Parse(data);
            }
            catch
            {
                return data;
            }
        }

        public static object ToJToken(this string data)
        {
            try
            {
                if (data is string)
                {
                    return JToken.Parse((string)data);
                }
                else
                    return JToken.FromObject((dynamic)data);
            }
            catch
            {
                return data;
            }
        }
    }
}
