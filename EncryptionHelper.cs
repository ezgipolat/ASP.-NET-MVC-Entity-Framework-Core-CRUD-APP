using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class EncryptionHelper
{
    private static readonly string Key = "Your_Secret_Key_That_Is_256_Bits_LoNg";
    private static readonly string IV = "Your_Init_Vector_123";

    private static byte[] GetKeyBytes()
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(Key);
        if (keyBytes.Length < 32)
        {
            Array.Resize(ref keyBytes, 32);
        }
        else if (keyBytes.Length > 32)
        {
            Array.Resize(ref keyBytes, 32);
        }
        return keyBytes;
    }

    private static byte[] GetIVBytes()
    {
        byte[] ivBytes = Encoding.UTF8.GetBytes(IV);
        if (ivBytes.Length < 16)
        {
            Array.Resize(ref ivBytes, 16);
        }
        else if (ivBytes.Length > 16)
        {
            Array.Resize(ref ivBytes, 16);
        }
        return ivBytes;
    }

    public static string Encrypt(string plainText)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = GetKeyBytes();
            aesAlg.IV = GetIVBytes();
            using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
            {
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }
    }

    public static string Decrypt(string cipherText)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = GetKeyBytes();
            aesAlg.IV = GetIVBytes();
            using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
            {
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
