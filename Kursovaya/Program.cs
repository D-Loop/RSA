using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Kursovaya
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создаем объект RSACryptoServiceProvider
            using (var rsa = new RSACryptoServiceProvider())
            {
                // Генерируем пару ключей
                var publicKey = rsa.ExportParameters(false);
                var privateKey = rsa.ExportParameters(true);

                // Сообщение для шифрования
                string message = String.Empty;

                //сохраняем в файл 
                if (!File.Exists("Input.txt"))
                {
                    using (StreamWriter sr = new StreamWriter("Input.txt"))
                    {
                        sr.WriteLineAsync("Нет данных");
                    }
                }

                //считываем из файла 
                using (StreamReader sr = new StreamReader("Input.txt"))
                {
                    message = sr.ReadToEnd().Replace("\r\n","");
                        Console.WriteLine(message);
                }

                // Шифруем сообщение с помощью открытого ключа
                string encryptedMessage = Encrypt(message, publicKey);

                // Выводим зашифрованное сообщение в консоль
                Console.WriteLine("Encrypted message: {0}", encryptedMessage);
                //сохраняем в файл 
                using (StreamWriter sr = new StreamWriter("Output.txt"))
                {
                    sr.WriteLineAsync(encryptedMessage);
                }
                
                // Расшифровываем сообщение с помощью закрытого ключа
                string decryptedMessage = Decrypt(encryptedMessage, privateKey);

                // Выводим расшифрованное сообщение в консоль
                Console.WriteLine("Decrypted message: {0}", decryptedMessage);

                Console.ReadKey();
            }
        }

        // Метод для шифрования строки с
        public static string Encrypt(string dataToEncrypt, RSAParameters publicKeyInfo)
        {
            // Преобразуем строку в массив байтов
            byte[] dataBytes = Encoding.UTF8.GetBytes(dataToEncrypt);

            // Создаем новый объект RSACryptoServiceProvider с открытым ключом
            using (var rsa = new RSACryptoServiceProvider(4096))
            {
                rsa.ImportParameters(publicKeyInfo);
                var dd = new byte[117];
                dd = dataBytes;
                // Шифруем данные с помощью метода Encrypt и возвращаем результат в виде строки base64 
                byte[] encryptedBytes = rsa.Encrypt(dataBytes, false);
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        // Метод для расшифровки строки с помощью RSA
        public static string Decrypt(string dataToDecrypt, RSAParameters privateKeyInfo)
        {
            // Преобразуем строку base64 в массив байтов 
            byte[] dataBytes = Convert.FromBase64String(dataToDecrypt);

            // Создаем новый объект RSACryptoServiceProvider с закрытым ключом 
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(privateKeyInfo);

                // Расшифровываем данные с помощью метода Decrypt и возвращаем результат в виде строки UTF8 
                byte[] decryptedBytes = rsa.Decrypt(dataBytes, false);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}