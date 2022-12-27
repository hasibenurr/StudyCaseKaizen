using System;
using System.Linq;
using System.Security.Cryptography;

namespace EncryptionCode
{
    class Program
    {
        static void Main(string[] args)
        {
            int counter = 0;
            //This string data will be our original key
            Guid g = Guid.NewGuid();
            string guidString = Convert.ToBase64String(g.ToByteArray());
            Console.WriteLine("Original Key: " + guidString);
            Console.WriteLine("--------------------------------------");

            // How many key do you need?
            while (counter < 10)
            {
                // Now, I will encrypt my original key
                string cipher = CreateAESCode(guidString);
                Console.WriteLine("Encyrpt Key: " + cipher);

                // Now, I will edit as requested
                string validKey = ValidValues(cipher.Substring(0, 8).ToUpper());
                Console.WriteLine("Encyrpt Valid Key: " + validKey);
                counter++;
            }
        }

        private static string CreateAESCode(string originalKey)
        {
            using (Aes aesAlgorithm = Aes.Create())
            {
                string keyBase64 = Convert.ToBase64String(aesAlgorithm.Key);
                aesAlgorithm.Key = Convert.FromBase64String(keyBase64);
                //Calling the GenerateIV method to create a new initialization vector for every original key that we want to encrypt.
                aesAlgorithm.GenerateIV();

                //set the parameters with out keyword
                //string vectorBase64 = Convert.ToBase64String(aesAlgorithm.IV);

                // Create encryptor object
                ICryptoTransform encryptor = aesAlgorithm.CreateEncryptor();

                byte[] encryptedData;

                //Encryption will be done in a memory stream through a CryptoStream object
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(originalKey);
                        }
                        encryptedData = ms.ToArray();
                    }
                }

                return Convert.ToBase64String(encryptedData);
            }
        }

        private static string ValidValues(string encryptedData)
        {
            var validValuesArray = new string[] { "A", "C", "D", "E", "F", "G", "H", "K", "L", "M", "N", "P", "R", "T", "X", "Y", "Z", "2", "3", "4", "5", "7", "9" };
            string newEncrypted;

            foreach (var data in encryptedData)
            {
                if (!validValuesArray.Contains(data.ToString()))
                {
                    var randomIndex = new Random().Next(0, validValuesArray.Length);
                    encryptedData = encryptedData.Replace(data, validValuesArray[randomIndex].ToCharArray()[0]);
                }
            }

            return encryptedData;
        }
    }
}
