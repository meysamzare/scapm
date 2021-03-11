using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public static class Encrypt
{

    const string byteText = "127836asghdjasgjds";

    public static string DecryptStringAES(this string cipherText)
    {
        string password = byteText;
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            var salt = cipherBytes.Take(16).ToArray();
            var iv = cipherBytes.Skip(16).Take(16).ToArray();
            var encrypted = cipherBytes.Skip(32).ToArray();
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, salt, 100);
            encryptor.Key = pdb.GetBytes(32);
            encryptor.Padding = PaddingMode.PKCS7;
            encryptor.Mode = CipherMode.CBC;
            encryptor.IV = iv;
            using (MemoryStream ms = new MemoryStream(encrypted))
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (var reader = new StreamReader(cs, Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }

    public static string EncryptStringAES(this string plainText)
    {
        byte[] iv = new byte[16];
        byte[] array;

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(byteText);
            aes.IV = iv;
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }

                    array = memoryStream.ToArray();
                }
            }
        }

        return Convert.ToBase64String(array);
    }

}