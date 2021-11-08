using System.Security.Cryptography;
using System;
using System.IO;
using System.Text;

namespace pmp_client_cli
{
    public static class Crypto
    {
        //TODO: Replace this with a hash
        private static string HashKey(string key)
        {
             if (key.Length > 32)
             {
                 key = key.Substring(0, 31);
             }
             else if (key.Length < 32)
             {
                 for (int i = key.Length; i < 32; i++)
                 {
                     key += "0";
                 }
             }

             return key;
        }
        
        public static string Encrypt(string str, string key)
        {
            byte[] iv = new byte[16];
            byte[] array;
            string hashedKey = HashKey(key);

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(hashedKey);
                    aes.IV = iv;
                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, 
                               encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                            {
                                streamWriter.Write(str);
                            }
                            array = memoryStream.ToArray();
                        }
                    }
                }
            
                return Convert.ToBase64String(array);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error encountered while encrypting string: " + e.Message);
                return "";
            }
        }

        public static string Decrypt(string str, string key)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(str);
            string hashedKey = HashKey(key);

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(hashedKey);
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, 
                               decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error encountered while decrypting string: " + e.Message);
                return "";
            }
        }
    }
}