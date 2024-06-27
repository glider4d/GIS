using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Kts.SubstrateHelper
{
    /// <summary>
    /// Представляет основной класс приложения.
    /// </summary>
    internal sealed class Program
    {
        #region Закрытые константы

        /// <summary>
        /// Путь к утилите convert.exe.
        /// </summary>
        private const string convert = @"C:\Program Files\ImageMagick-6.9.3-Q16\convert.exe";

        /// <summary>
        /// Входная папка.
        /// </summary>
        private const string input = @"C:\Input";

        /// <summary>
        /// Выходная папка.
        /// </summary>
        private const string output = @"C:\Output";

        /// <summary>
        /// Папка на сервере, где хранятся подложки.
        /// </summary>
        private const string server = @"\\172.16.4.58\Gis\Images";//@"C:\Output\Images";//@"\\172.16.4.58\Gis\Images";

        /// <summary>
        /// Папка на сервере, где хранятся оригинальные изображения подложек.
        /// </summary>
        private const string serverOriginal = @"\\172.16.4.58\Gis\Original Images";//@"C:\Output\Original Images";//@"\\172.16.4.58\Gis\Original Images";

        /// <summary>
        /// Папка на сервере, где хранятся миниатюры подложек.
        /// </summary>
        private const string thumbnails = @"\\172.16.4.58\Gis\Thumbnails";//@"C:\Output\Thumbnails";//@"\\172.16.4.58\Gis\Thumbnails";

        #endregion

        #region Закрытые статические методы

        /// <summary>
        /// Задает вопрос, на который следует ответить "Да" или "Нет".
        /// </summary>
        /// <param name="question">Вопрос.</param>
        /// <returns>true, если пользователь ответил "Д", иначе - false.</returns>
        private static bool AskYesNo(string question)
        {
            Console.WriteLine();
            Console.WriteLine(question + " (Enter/Esc)");

            while (true)
            {
                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.Enter)
                    break;
                else
                    if (key == ConsoleKey.Escape)
                        return false;
            }

            Console.WriteLine();

            return true;
        }

        /// <summary>
        /// Копирует директорию.
        /// </summary>
        /// <param name="sourceDirName">Путь к директории-источнику.</param>
        /// <param name="destDirName">Путь к конечной директории.</param>
        /// <param name="copySubDirs">Значение, указывающее на то, что нужно ли копировать поддиректории.</param>
        private static void CopyDirectory(string sourceDirName, string destDirName, bool copySubDirs)
        {
            var dir = new DirectoryInfo(sourceDirName);

            var dirs = dir.GetDirectories();

            if (!Directory.Exists(destDirName))
                Directory.CreateDirectory(destDirName);

            var files = dir.GetFiles();

            string tempPath;

            foreach (var file in files)
            {
                tempPath = Path.Combine(destDirName, file.Name);

                file.CopyTo(tempPath, false);
            }

            if (copySubDirs)
                foreach (var subDir in dirs)
                {
                    tempPath = Path.Combine(destDirName, subDir.Name);

                    CopyDirectory(subDir.FullName, tempPath, copySubDirs);
                }
        }

        #endregion

        #region Открытые статические методы

        /// <summary>
        /// Главный метод приложения.
        /// </summary>
        /// <param name="args">Аргументы.</param>
        public static void Main(string[] args)
        {
            Console.Title = "Помощник создания подложек";
            
            // Выполняем поиск утилиты convert.exe.
            Console.WriteLine("Поиск утилиты конвертирования изображений...");
            if (File.Exists(convert))
                Console.WriteLine("Утилита найдена!");
            else
            {
                Console.WriteLine("Утилита не найдена!");
                Console.WriteLine();
                Console.WriteLine("Потрачено! Нажмите любую клавишу для закрытия приложения...");

                Console.ReadKey();
                
                return;
            }

            Console.WriteLine();
            Console.WriteLine("1. Папка с входными изображениями: " + input);
            Console.WriteLine("2. Папка с выходными изображениями: " + output);
            Console.WriteLine("3. Минимальный размер тайлов: 512x512");
            Console.WriteLine("4. Папка на сервере, где хранятся подложки: " + server);
            Console.WriteLine("5. Папка на сервере, где хранятся миниатюры подложек: " + thumbnails);

            if (!AskYesNo("Все верно?"))
                return;

            var supportedExtensions = new List<string>()
            {
                ".gif",
                ".jpeg",
                ".jpg",
                ".png"
            };

            // Находим все изображения во входной папке и выводим пути к ним.
            Console.WriteLine("Поиск изображений во входной папке...");
            Console.WriteLine("Во входной папке найдены следующие изображения:");
            var allImages = Directory.GetFiles(input).Where(x => supportedExtensions.Contains(Path.GetExtension(Path.GetFileName(x))));
            foreach (var image in allImages)
                Console.WriteLine(image);

            if (!AskYesNo("Все верно?"))
                return;

            var targetDirectories = new List<string>();

            // Выполняем разбиение на тайлы.
            Console.WriteLine("Пожалуйста подождите, идет разбиение изображений на тайлы...");
            foreach (var image in allImages)
            {
                Console.WriteLine("Разбиваем " + image + "...");

                var tileSize = 512;

                while (true)
                {
                    double resizedWidth = 0;
                    double resizedHeight = 0;

                    // Проверяем наличие необходимости создать тайлы худшего качества.
                    if (tileSize != 512)
                    {
                        var bitmapDecoder = BitmapDecoder.Create(new Uri(image), BitmapCreateOptions.None, BitmapCacheOption.None);

                        var frame = bitmapDecoder.Frames[0];

                        resizedWidth = frame.PixelWidth / (tileSize / 512);
                        resizedHeight = frame.PixelHeight / (tileSize / 512);

                        if (resizedWidth / tileSize < 1 || resizedHeight / tileSize < 1)
                            break;
                    }

                    // Удаляем и пересоздаем папку, где будут храниться тайлы изображения, если такая существует. Добавляем в конец размер тайлов.
                    var targetDirectory = output + @"\" + Path.GetFileNameWithoutExtension(image) + (tileSize != 512 ? "." + tileSize : "");
                    if (Directory.Exists(targetDirectory))
                        Directory.Delete(targetDirectory, true);
                    Directory.CreateDirectory(targetDirectory);

                    // Разбиваем на тайлы, используя утилиту convert.exe.
                    var originalExt = Path.GetExtension(Path.GetFileName(image));
                    var processStartInfo = new ProcessStartInfo()
                    {
                        Arguments = string.Format("/c \"{0}\" {1} {2} -crop {3}x{3} {4}\\-{5}", convert, image, tileSize != 512 ? "-resize " + resizedWidth + "x" + resizedHeight : "", tileSize, targetDirectory, originalExt),
                        FileName = "cmd.exe",
                        WindowStyle = ProcessWindowStyle.Hidden,
                        WorkingDirectory = Path.GetDirectoryName(convert)
                    };
                    var process = new Process()
                    {
                        StartInfo = processStartInfo
                    };
                    process.Start();
                    process.WaitForExit();

                    // Переименовываем полученные тайлы.
                    var tiles = Directory.GetFiles(targetDirectory).Where(x => supportedExtensions.Contains(Path.GetExtension(Path.GetFileName(x))));
                    string temp;
                    var zeros = "{0:";
                    for (int i = 0; i < tiles.Count().ToString().Length; i++)
                        zeros += "0";
                    zeros += "}";
                    foreach (var tile in tiles)
                    {
                        temp = Path.GetFileNameWithoutExtension(tile).Remove(0, 2);

                        File.Move(tile, targetDirectory + @"\" + string.Format(zeros, Convert.ToInt32(temp) + 1) + originalExt);
                    }

                    targetDirectories.Add(targetDirectory);

                    tileSize *= 2;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Разбиение завершено!");

            // Создаем миниатюры.
            Console.WriteLine();
            Console.WriteLine("Пожалуйста подождите, идет создание миниатюр...");
            int targetWidth;
            int targetHeight;
            foreach (var image in allImages)
            {
                Console.WriteLine("Создаем миниатюру " + image + "...");

                // Определяем размеры миниатюры.
                var bitmapDecoder = BitmapDecoder.Create(new Uri(image), BitmapCreateOptions.None, BitmapCacheOption.None);
                var frame = bitmapDecoder.Frames[0];
                if (frame.Width > frame.Height)
                {
                    var coeff = frame.Width / frame.Height;

                    targetWidth = 280;
                    targetHeight = Convert.ToInt32(280 / coeff);
                }
                else
                {
                    var coeff = frame.Height / frame.Width;

                    targetWidth = Convert.ToInt32(280 / coeff);
                    targetHeight = 280;
                }

                // Получаем миниатюру и сохраняем ее.
                using (var img = Image.FromFile(image))
                    using (var thumbnail = img.GetThumbnailImage(targetWidth, targetHeight, () => false, IntPtr.Zero))
                        thumbnail.Save(output + @"\" + Path.GetFileName(image));
            }

            Console.WriteLine();
            Console.WriteLine("Создание миниатюр завершено!");

            if (!AskYesNo("Перейти к следующему шагу?"))
                return;

            // Копируем все папки на сервер.
            Console.WriteLine("Пожалуйста подождите, идет копирование тайлов на сервер...");
            foreach (var directory in targetDirectories)
            {
                Console.WriteLine("Копируем " + directory + "...");

                var targetDir = server + @"\" + Path.GetFileName(directory);

                if (Directory.Exists(targetDir))
                    Directory.Delete(targetDir, true);

                CopyDirectory(directory, server + @"\" + Path.GetFileName(directory), true);
            }

            Console.WriteLine();
            Console.WriteLine("Копирование завершено!");

            if (!AskYesNo("Перейти к следующему шагу?"))
                return;

            // Копируем все миниатюры на сервер.
            Console.WriteLine("Пожалуйста подождите, идет копирование миниатюр на сервер...");
            foreach (var image in allImages)
            {
                var fileName = Path.GetFileName(image);
                var thumbnail = output + @"\" + fileName;

                Console.WriteLine("Копируем " + thumbnail + "...");

                File.Copy(thumbnail, thumbnails + @"\" + fileName, true);
            }

            Console.WriteLine();
            Console.WriteLine("Копирование завершено!");

            if (!AskYesNo("Перейти к следующему шагу?"))
                return;

            // Копируем все оригинальные изображения на сервер.
            Console.WriteLine("Пожалуйста подождите, идет копирование оригинальных изображений на сервер...");
            foreach (var image in allImages)
            {
                Console.WriteLine("Копируем " + image + "...");

                File.Copy(image, serverOriginal + @"\" + Path.GetFileName(image), true);
            }

            Console.WriteLine();
            Console.WriteLine("Копирование завершено!");

            if (!AskYesNo("Перейти к следующему шагу?"))
                return;

            // Вносим изменения в базу данных.
            Console.WriteLine("Пожалуйста подождите, идет внесение изменений в базу данных...");
            foreach (var image in allImages)
            {
                // Получаем размер изображения.
                var bitmapDecoder = BitmapDecoder.Create(new Uri(image), BitmapCreateOptions.None, BitmapCacheOption.None);
                var frame = bitmapDecoder.Frames[0];

                double colCount = frame.PixelWidth / (double)512;
                double rowCount = frame.PixelHeight / (double)512;
                //10.180.199.2
                //using (var connection = new SqlConnection(@"user id=sa;password=159357;server=10.180.199.2;database=Gis;connection timeout=0;language=English"))
                //якутск 
                using (var connection = new SqlConnection(@"user id=sa;password=gjghj,eqgjl,thb;server=172.16.3.85;database=Gis;connection timeout=5;language=English"))
                //using (var connection = new SqlConnection(@"user id=sa;password=159357;server=10.180.118.2;database=Gis;connection timeout=0;language=English"))
                //using (var connection = new SqlConnection("user id=sa;password=159357;server=vvs;database=Gis;connection timeout=5;language=English"))
                //using (var connection = new SqlConnection(@"user id=sa;password=159357;server=DESKTOP-1LONAHF\SQLEXPRESS;database=Gis;connection timeout=5;language=English"))
                using (var command = new SqlCommand("update_substrate", connection))
                    {

                        connection.Open();

                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@city_id", Convert.ToInt32(Path.GetFileNameWithoutExtension(image))));
                        command.Parameters.Add(new SqlParameter("@width", frame.PixelWidth));
                        command.Parameters.Add(new SqlParameter("@height", frame.PixelHeight));
                        command.Parameters.Add(new SqlParameter("@column_count", Math.Ceiling(colCount)));
                        command.Parameters.Add(new SqlParameter("@row_count", Math.Ceiling(rowCount)));

                        command.ExecuteNonQuery();
                        Console.WriteLine("Написали в базу всякого");
                    }
            }

            Console.WriteLine();
            Console.WriteLine("Готово! Нажмите любую клавишу для закрытия приложения...");

            Console.ReadKey();
        }

        #endregion
    }
}