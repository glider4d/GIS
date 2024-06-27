using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Kts.EncryptorDecryptor
{
    /// <summary>
    /// Представляет основной класс приложения.
    /// </summary>
    internal sealed class Program
    {
        #region Закрытые константы

        /// <summary>
        /// IV.
        /// </summary>
        private const string IV = "h0wc@ny0us331nt0";

        /// <summary>
        /// Ключ.
        /// </summary>
        private const string key = "my3y3sl1k3";

        /// <summary>
        /// Соль.
        /// </summary>
        private const string salt = "0p3nd00rs";
        
        #endregion

        #region Закрытые статические методы

        /// <summary>
        /// Выполняет дешифрование заданной зашифрованной строки.
        /// </summary>
        /// <param name="input">Входная зашифрованная строка.</param>
        /// <returns>Расшифрованная строка.</returns>
        private static string Decrypt(string input)
        {
            var inputBytes = Convert.FromBase64String(input);

            var keyBytes = new Rfc2898DeriveBytes(key, Encoding.ASCII.GetBytes(salt)).GetBytes(256 / 8);

            var symmetricKey = new RijndaelManaged()
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.None
            };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(IV));

            byte[] resultBytes;
            int decryptedByteCount;

            MemoryStream memoryStream = null;

            try
            {
                memoryStream = new MemoryStream(inputBytes);

                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    memoryStream = null;

                    resultBytes = new byte[inputBytes.Length];

                    decryptedByteCount = cryptoStream.Read(resultBytes, 0, resultBytes.Length);
                }
            }
            finally
            {
                if (memoryStream != null)
                    memoryStream.Dispose();
            }

            return Encoding.UTF8.GetString(resultBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }

        /// <summary>
        /// Выполняет шифрование заданной строки.
        /// </summary>
        /// <param name="input">Входная строка.</param>
        /// <returns>Зашифрованная входная строка.</returns>
        private static string Encrypt(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);

            var keyBytes = new Rfc2898DeriveBytes(key, Encoding.ASCII.GetBytes(salt)).GetBytes(256 / 8);

            var symmetricKey = new RijndaelManaged()
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.Zeros
            };

            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(IV));

            byte[] resultBytes;

            MemoryStream memoryStream = null;

            try
            {
                memoryStream = new MemoryStream();

                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    var stream = memoryStream;

                    memoryStream = null;

                    cryptoStream.Write(inputBytes, 0, inputBytes.Length);

                    cryptoStream.FlushFinalBlock();

                    resultBytes = stream.ToArray();
                }
            }
            finally
            {
                if (memoryStream != null)
                    memoryStream.Dispose();
            }

            return Convert.ToBase64String(resultBytes);
        }

        #endregion

        #region Открытые статические методы

        /// <summary>
        /// Главный метод приложения.
        /// </summary>
        /// <param name="args">Аргументы.</param>
        public static void Main(string[] args)
        {
            var input = "";

            var output = Encrypt(input);
        }

        #endregion
    }
}