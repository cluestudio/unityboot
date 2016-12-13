using System;
using System.Text;
using System.Security.Cryptography;

public class EncryptionServiceImpl : Singleton<EncryptionServiceImpl>, EncryptionService {
    public string Encrypt(string key, string ToEncrypt) {
        byte[] keyArray;
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(ToEncrypt);
        

        MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
        keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
        hashmd5.Clear();  

        TripleDESCryptoServiceProvider tDes = new TripleDESCryptoServiceProvider();
        tDes.Key = keyArray;
        tDes.Mode = CipherMode.ECB;
        tDes.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = tDes.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        tDes.Clear();
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    public string Decrypt(string key, string cypherString) {
        byte[] keyArray;
        
        MD5CryptoServiceProvider hashmd = new MD5CryptoServiceProvider();
        keyArray = hashmd.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
        hashmd.Clear();

        TripleDESCryptoServiceProvider tDes = new TripleDESCryptoServiceProvider();
        tDes.Key = keyArray;
        tDes.Mode = CipherMode.ECB;
        tDes.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = tDes.CreateDecryptor();
        try {
            byte[] toDecryptArray = Convert.FromBase64String(cypherString);
            byte[] resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
            tDes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray,0,resultArray.Length);
        }
        catch (Exception) {
        }

        return "";
    }
}