using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Core_App.BL
{
    public class QueryStringEncryption
    {
        public static string Encrypt(string inputText)
        {
            try
            {
                byte[] plainText = Encoding.UTF8.GetBytes(inputText);

                using (RijndaelManaged rijndaelCipher = new RijndaelManaged())
                {
                    PasswordDeriveBytes secretKey = new PasswordDeriveBytes(
                        Encoding.ASCII.GetBytes(Constants.QUERY_STRING_KEY),
                        Encoding.ASCII.GetBytes(Constants.QUERY_STRING_SALT));
                    using (ICryptoTransform encryptor = rijndaelCipher.CreateEncryptor(secretKey.GetBytes(32), secretKey.GetBytes(16)))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainText, 0, plainText.Length);
                                cryptoStream.FlushFinalBlock();
                                return HttpUtility.UrlEncode(Convert.ToBase64String(memoryStream.ToArray()));
                            }
                        }
                    }
                }
            }
            catch
            {
                return "error";
            }
        }

        public static string Decrypt(string inputText)
        {
            try
            {
                byte[] encryptedData = Convert.FromBase64String(inputText);
                PasswordDeriveBytes secretKey = new PasswordDeriveBytes(
                    Encoding.ASCII.GetBytes(Constants.QUERY_STRING_KEY),
                    Encoding.ASCII.GetBytes(Constants.QUERY_STRING_SALT));

                using (RijndaelManaged rijndaelCipher = new RijndaelManaged())
                {
                    using (ICryptoTransform decryptor = rijndaelCipher.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16)))
                    {
                        using (MemoryStream memoryStream = new MemoryStream(encryptedData))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                byte[] plainText = new byte[encryptedData.Length];
                                return Encoding.UTF8.GetString(plainText, 0, cryptoStream.Read(plainText, 0, plainText.Length));
                            }
                        }
                    }
                }
            }
            catch
            {
                return "error";
            }
        }
    }
}