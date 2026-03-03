using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DLMS_DAL
{


    public class Cryptage
    {
        public static String sCleCryptage = "ASCDLMS";

        //public static byte[] Key;
        //public static byte[] IV;

        //public Cryptage()
        //{
        //    GenerateKey();
        //}

        //public Cryptage(string Key)
        //{
        //    GenerateKey(Key);
        //}  


        //private void GenerateKey()
        //{
        //    string Password = "C25B0E08-ED2A-4911-9686-79FFD0867CA4";
        //    GenerateKey(Password);
        //}

        public static string CryptageMD5(string mdp)
        {
            return GetMd5Hash(MD5.Create(), mdp);
        }

        public static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string. 
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }

        private static void GenerateKey(string SecretPhrase, out byte[] pKey, out byte[] pIV)
        {
            //// Initialize internal values
            //Key = new byte[24];
            //IV = new byte[16];
            pKey = new byte[24];
            pIV = new byte[16];


            byte[] bytePhrase = Encoding.ASCII.GetBytes(SecretPhrase);

            HashAlgorithm sha = new SHA1CryptoServiceProvider();
            sha.ComputeHash(bytePhrase);
            byte[] resultat = sha.Hash;

            byte[] result = new byte[48];

            for (int index = 0; index < 48; index++)
            {
                for (int i = 0; i < 20; i++)
                {
                    result[index] = resultat[i];
                }
            }

            for (int loop = 0;
                loop < 24; loop++) pKey[loop] = result[loop];
            for (int loop = 24;
                loop < 40; loop++) pIV[loop - 24] = result[loop];
        }
        // private static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV, String sCleCrypTage)
        private static byte[] Encrypt(byte[] clearData, String sCleCrypTage)
        {
            // Create a MemoryStream to accept the encrypted bytes 

            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();

            byte[] Key;
            byte[] IV;
            GenerateKey(sCleCrypTage, out Key, out IV);

            alg.Key = Key;
            alg.IV = IV;

            CryptoStream cs = new CryptoStream(ms,
               alg.CreateEncryptor(), CryptoStreamMode.Write);

            // Write the data and make it do the encryption 

            cs.Write(clearData, 0, clearData.Length);
            cs.Close();

            byte[] encryptedData = ms.ToArray();

            return encryptedData;
        }

        //private static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV, String sCleCrypTage)
        private static byte[] Decrypt(byte[] cipherData, String sCleCrypTage)
        {
            MemoryStream ms = new MemoryStream();

            Rijndael alg = Rijndael.Create();

            byte[] Key;
            byte[] IV;
            GenerateKey(sCleCrypTage, out Key, out IV);
            alg.Key = Key;
            alg.IV = IV;

            CryptoStream cs = new CryptoStream(ms,
                alg.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(cipherData, 0, cipherData.Length);
            cs.Close();


            byte[] decryptedData = ms.ToArray();

            return decryptedData;
        }

        ////public static string Encrypt(string TexteToEncrypt, byte[] Key, byte[] IV, String sCleCrypTage)
        public static string Encrypt(string TexteToEncrypt, String sCleCrypTage)
        {
            byte[] textBytes = Encoding.Unicode.GetBytes(TexteToEncrypt);

            string res = Convert.ToBase64String(Encrypt(textBytes, sCleCrypTage));

            return res;
        }

        public static string Decrypt(string TexteToDecrypt, String sCleCrypTage)
        {
            byte[] textBytes = Convert.FromBase64String(TexteToDecrypt);
            //byte[] DecryptData = Decrypt(textBytes, Key, IV,sCleCrypTage);
            byte[] DecryptData = Decrypt(textBytes, sCleCrypTage);
            return Encoding.Unicode.GetString(DecryptData, 0, DecryptData.Length);
        }

        //public static string Encrypt(string TexteToEncrypt, String sCleCrypTage)
        //{
        //    return Encrypt(TexteToEncrypt,sCleCrypTage);
        //}

        //public static string Decrypt(string TexteToDecrypt, String sCleCrypTage)
        //{
        //    return Decrypt(TexteToDecrypt, Key, IV, sCleCrypTage);
        //}


        //Pour la gestion des licences
        public static byte[] EncryptToByte(string TexteToEncrypt, String sCleCrypTage)
        {
            //byte[] textBytes = Encoding.UTF8.GetBytes(TexteToEncrypt);
            byte[] textBytes = Encoding.Unicode.GetBytes(TexteToEncrypt);
            //return Encrypt(textBytes, Key, IV, sCleCrypTage);
            return Encrypt(textBytes, sCleCrypTage);
        }

        public static String DecryptToByte(byte[] textBytes, String sCleCrypTage)
        {
            //byte[] textBytes = Convert.FromBase64String(TexteToDecrypt);
            //byte[] textBytes = Encoding.Unicode.GetBytes(TexteToDecrypt);
            // byte[] DecryptData = Decrypt(textBytes, Key, IV,sCleCrypTage);
            byte[] DecryptData = Decrypt(textBytes, sCleCrypTage);
            return Encoding.Unicode.GetString(DecryptData, 0, DecryptData.Length - 1);
        }
        //Fin gestion des licences
    }
}