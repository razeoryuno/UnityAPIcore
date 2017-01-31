using System;
using System.Security.Cryptography;
using System.Text;

public static class SHA_Encryptor {

    public static string EncryptSHA1 (string plainText)
    {
        HashAlgorithm hash = new SHA1CryptoServiceProvider();
        byte[] plainData = Encoding.UTF8.GetBytes(plainText);
        byte[] cipherData = hash.ComputeHash(plainData);
        hash.Clear();
        
        string cipherText = Convert.ToBase64String(cipherData);
        return cipherText;
    }
    
    public static string EncryptSHA256 (string plainText, string password)
    {
        HMACSHA256 hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(password));
              byte[] cipherData = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(plainText));
              string cipherText = Convert.ToBase64String(cipherData);
        
        hmacsha256.Clear ();
        
        return cipherText;
    }
    
    public static string DecryptSHA256 (string cipherText, string password)
    {
        HMACSHA256 hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(password));
        byte[] plainData = hmacsha256.ComputeHash(Convert.FromBase64String(cipherText));
        
        string plainText = Encoding.UTF8.GetString(plainData, 0, plainData.Length);
        
        hmacsha256.Clear ();
        
        return plainText;
    }

    public static string CreateKey(int numBytes) 
    {
        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        byte[] buff = new byte[numBytes];
        
        rng.GetBytes(buff);
        return BytesToHexString(buff);
    }

    public static string BytesToHexString(byte[] bytes) 
    {
        StringBuilder hexString = new StringBuilder(64);
        
        for (int counter = 0; counter < bytes.Length; counter++) 
        {
            hexString.Append(string.Format("{0:X2}", bytes[counter]));
        }
        return hexString.ToString();
    }
}
