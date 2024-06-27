using Kts.Gis.ViewModels;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление экспорта изображения.
    /// </summary>
    internal sealed partial class ImageExportView : Window
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Карта.
        /// </summary>
        private readonly Map map;

        /// <summary>
        /// Модель представления.
        /// </summary>
        private readonly ImageExportViewModel viewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ImageExportView"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        /// <param name="map">Карта.</param>
        public ImageExportView(ImageExportViewModel viewModel, Map map)
        {
            this.InitializeComponent();

            this.viewModel = viewModel;
            this.map = map;

            this.DataContext = viewModel;

            viewModel.FolderName = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            viewModel.ExportRequested += this.ViewModel_ExportRequested;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="System.Windows.Controls.Primitives.ButtonBase.Click"/> кнопки выбора папки.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog()
            {
                Description = this.viewModel.FolderBrowseTitle,
                SelectedPath = this.viewModel.FolderName
            };

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.viewModel.FolderName = dialog.SelectedPath;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="ImageExportViewModel.ExportRequested"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_ExportRequested(object sender, EventArgs e)
        {
            // Составляем префикс и постфикс названия папки/файлов.
            var prefix = this.viewModel.FolderName + "\\" + this.viewModel.FileNamePrefix + "." + DateTime.Now.ToString("dd.MM.yy.HH.mm");
            var postfix = "";
            switch (this.viewModel.SelectedFormat)
            {
                case ImageFormat.Gif:
                    postfix = ".gif";

                    break;

                case ImageFormat.Jpeg:
                    postfix = ".jpg";

                    break;

                case ImageFormat.Png:
                    postfix = ".png";

                    break;

                default:
                    throw new NotImplementedException("Не реализована работа со следующим форматом изображения: " + this.viewModel.SelectedFormat.ToString());
            }

            if (!this.viewModel.IsTooBig)
                this.map.SaveAsImage(prefix + postfix, new Point(0, 0), new Size(this.viewModel.TargetSize.Width, this.viewModel.TargetSize.Height));
            else
            {
                // Создаем папку, в которой будут храниться файлы.
                Directory.CreateDirectory(prefix);

                int columnCount = Convert.ToInt32(Math.Ceiling(this.viewModel.InitialSize.Width / this.viewModel.TargetSize.Width));
                int rowCount = Convert.ToInt32(Math.Ceiling(this.viewModel.InitialSize.Height / this.viewModel.TargetSize.Height));

                int counter = 1;

                // Составляем маску названий файлов.
                var mask = "";
                for (int i = 0; i < (columnCount * rowCount).ToString().Length; i++)
                    mask += "0";

                for (int j = 0; j < rowCount; j++)
                    for (int i = 0; i < columnCount; i++)
                    {
                        this.map.SaveAsImage(prefix + "\\" + counter.ToString(mask) + postfix, new Point(i * this.viewModel.TargetSize.Width, j * this.viewModel.TargetSize.Height), new Size(this.viewModel.TargetSize.Width, this.viewModel.TargetSize.Height));

                        counter++;
                    }
            }

            this.Close();
        }

        #endregion
    }
}