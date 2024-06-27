using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Kts.PublishHelper
{
    /// <summary>
    /// Представляет основной класс приложения.
    /// </summary>
    internal sealed class Program
    {
        #region Закрытые константы

        /// <summary>
        /// Конечная папка.
        /// </summary>
        private const string destination = @"C:\Output";

        /// <summary>
        /// Путь к папке публикации сервера-источника.
        /// </summary>
        private const string publishName = @"\\172.16.4.58\Gis\Publish";

        /// <summary>
        /// Название сервера-источника.
        /// </summary>
        private const string serverName = "172.16.4.58";

        #endregion

        #region Закрытые статические поля

        /// <summary>
        /// Список конечных серверов.
        /// </summary>
        private static List<string> servers = new List<string>()
        {
            "192.168.1.2",
            "192.168.0.9",
            "192.168.1.3",
            "127.0.0.1",
            "192.168.100.105",
            "192.168.1.200",
            "192.168.2.22"
        };

        #endregion

        #region Закрытые статические методы

        /// <summary>
        /// Задает вопрос "Все верно? (Д/Н)".
        /// </summary>
        /// <returns>true, если пользователь ответил "Д", иначе - false.</returns>
        private static bool AskYesNo()
        {
            Console.WriteLine();
            Console.WriteLine("Все верно? (Enter/Esc)");

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
            Console.Title = "Помощник публикации";

            Console.WriteLine("1. Сервер-источник: " + serverName);
            Console.WriteLine("2. Путь к папке публикации сервера-источника: " + publishName);

            if (!AskYesNo())
                return;

            // Получаем название файла с расширением .application и папки с последней версией приложения.
            var sourceFile = Directory.GetFiles(publishName, "*.application")[0];
            var sourceFolder = Directory.GetDirectories(publishName + @"\Application Files").OrderBy(x => Directory.GetCreationTime(x)).Last();

            Console.WriteLine("3. Копируемые файлы и папки:");
            Console.WriteLine("   " + sourceFile);
            Console.WriteLine("   " + sourceFolder);

            if (!AskYesNo())
                return;

            Console.WriteLine("4. Конечная папка: " + destination);

            if (!AskYesNo())
                return;

            Console.WriteLine("5. Список конечных серверов:");
            foreach (var server in servers)
                Console.WriteLine("   " + server);

            if (!AskYesNo())
                return;

            if (Directory.Exists(destination))
                // Удаляем предыдущую конечную папку.
                Directory.Delete(destination, true);

            // Создаем конечную папку.
            Directory.CreateDirectory(destination);

            // Создаем папки с названиями конечных серверов...
            var fileName = Path.GetFileName(sourceFile);
            foreach (var server in servers)
            {
                var directory = destination + @"\" + server;

                Directory.CreateDirectory(directory);

                // ...копируем необходимый файл...
                File.Copy(sourceFile, directory + @"\" + fileName);
                Console.WriteLine("Скопирован файл " + directory + @"\" + fileName);

                // ...и папку.
                Directory.CreateDirectory(directory + @"\Application Files");
                var targetDir = directory + @"\Application Files\" + Path.GetFileName(sourceFolder);
                CopyDirectory(sourceFolder, targetDir, true);
                Console.WriteLine("Скопирована папка " + targetDir);

                // Теперь заменяем адрес сервера во всех .application файлах.
                var apps = Directory.GetFiles(directory, "*.application", SearchOption.AllDirectories);
                foreach (var app in apps)
                {
                    var text = File.ReadAllText(app);

                    text = text.Replace(serverName, server);

                    File.WriteAllText(app, text, Encoding.UTF8);

                    Console.WriteLine("Заменен адрес сервера в файле " + app);
                }

                // Создаем архив.
                var zip = destination + @"\" + server + ".zip";
                ZipFile.CreateFromDirectory(directory, zip);
                Console.WriteLine("Создан архив " + zip);

                Console.WriteLine();
            }
            
            Console.WriteLine("Готово! Нажмите любую клавишу для закрытия приложения...");

            Console.ReadKey();
        }

        #endregion
    }
}