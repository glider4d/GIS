using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Kts.GisUpdater
{
    /// <summary>
    /// Основной класс приложения.
    /// </summary>
    internal static class Program
    {
        #region Закрытые константы

        /// <summary>
        /// Название папки Application Files.
        /// </summary>
        private const string appFiles = "Application Files";

        /// <summary>
        /// Название установочника.
        /// </summary>
        private const string appName = "Kts.Gis.Application";

        #endregion

        #region Закрытые статические методы

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
        /// Главная точка входа в приложение.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Environment.CurrentDirectory = Application.StartupPath;

            // Проверяем наличие уже запущенной версии приложения.
            bool createdNew;
            var mutex = new Mutex(true, "Kts.GisUpdater", out createdNew);
            if (!createdNew)
                // Закрываем приложение.
                return;

            try
            {
                var targetPath = File.ReadAllText("Settings.txt");
                var localPath = Path.Combine(Environment.CurrentDirectory, "Publish");

                var localDirs = Directory.GetDirectories(Path.Combine(localPath, appFiles));
                string[] targetDirs = null;

                var hasChanges = false;

                using (var unc = new UNCAccessWithCredentials())
                    if (unc.NetUseWithCredentials(targetPath, "programmist_kts", "jkhrs", "159357люк"))
                    {
                        targetDirs = Directory.GetDirectories(Path.Combine(targetPath, appFiles));

                        bool contains;

                        foreach (var dir in targetDirs)
                        {
                            contains = false;

                            foreach (var localDir in localDirs)
                                if (Path.GetFileName(localDir) == Path.GetFileName(dir))
                                {
                                    contains = true;

                                    break;
                                }

                            if (!contains)
                            {
                                var newDir = Path.Combine(Path.Combine(localPath, appFiles), Path.GetFileName(dir));

                                try
                                {
                                    CopyDirectory(dir, newDir, true);

                                    hasChanges = true;
                                }
                                catch
                                {
                                    if (Directory.Exists(newDir))
                                        Directory.Delete(newDir, true);

                                    hasChanges = false;

                                    Console.Write("Cannot copy folder " + Path.GetFileName(dir));

                                    break;
                                }
                            }
                        }

                        if (hasChanges)
                        {
                            File.Copy(Path.Combine(targetPath, appName), Path.Combine(localPath, appName), true);

                            Console.Write("Successfully updated");
                        }
                    }
                    else
                        Console.Write("Cannot authorize");
            }
            catch
            {
                Console.Write("Unhandled exception");
            }
            
            mutex.Close();
        }

        #endregion
    }
}