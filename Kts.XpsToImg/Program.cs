using System;
using System.IO;
using System.Linq;
using xps2img;

namespace Kts.XpsToImg
{
    /// <summary>
    /// Представляет основной класс приложения.
    /// </summary>
    public sealed class Program
    {
        /// <summary>
        /// Представляет точку входа в приложение
        /// </summary>
        /// <param name="args">Аргументы.</param>
        private static int Main(string[] args)
        {
            Console.Title = "Конвертация XPS в PNG";

            string inputXps = "";
            string outputFolder = "";
            int dpi = 300;

            // Разбираем аргументы.
            try
            {
                inputXps = args[0];
                outputFolder = args[1];
                dpi = Convert.ToInt32(args[2]);
            }
            catch
            {
                Console.WriteLine("Неверно заданы аргументы");
                Console.WriteLine("Операция не выполнена. Нажмите любую клавишу для продолжения...");

                Console.ReadLine();

                return 1;
            }

            Console.WriteLine("Исходный файл: " + inputXps);
            Console.WriteLine("Выходная папка: " + outputFolder);
            Console.WriteLine("DPI: " + dpi);

            // Конвертируем.
            try
            {
                using (var converter = new Xps2Image(inputXps))
                {
                    var pages = converter.ToBitmap(new Parameters()
                    {
                        ImageType = ImageType.Png,
                        Dpi = dpi
                    }).ToList();

                    string path;

                    if (pages.Count > 1)
                        for (int i = 0; i < pages.Count; i++)
                            pages[i].Save(Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(inputXps) + "-" + i.ToString() + ".png"));
                    else
                        if (pages.Count == 1)
                        {
                            path = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(inputXps) + ".png");

                            Console.WriteLine("Сохраняем изображение: " + path);

                            pages[0].Save(path);
                        }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Операция не выполнена. Нажмите любую клавишу для продолжения...");

                Console.ReadLine();

                return 1;
            }

            Console.WriteLine("Операция успешно выполнена. Нажмите любую клавишу для продолжения...");

            Console.ReadLine();

            return 0;
        }
    }
}