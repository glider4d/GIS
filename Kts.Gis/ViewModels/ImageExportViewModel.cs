using Kts.Messaging;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.IO;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления экспорта изображения.
    /// </summary>
    internal sealed class ImageExportViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Путь к папке, в который будет(будут) сохранено(ы) изображение(я).
        /// </summary>
        private string folderName;

        /// <summary>
        /// Выбранный размер изображения.
        /// </summary>
        private ImageSize selectedSize;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Модель представления карты.
        /// </summary>
        private MapViewModel mapViewModel;

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        private IMessageService messageService;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса экспорта изображения.
        /// </summary>
        public event EventHandler ExportRequested;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ImageExportViewModel"/>.
        /// </summary>
        /// <param name="initialSize">Начальный размер изображения в пикселях.</param>
        /// <param name="fileNamePrefix">Префикс названия файла.</param>
        /// <param name="mapViewModel">Модель представления карты.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public ImageExportViewModel(Size initialSize, string fileNamePrefix, MapViewModel mapViewModel, IMessageService messageService)
        {
            this.InitialSize = initialSize;
            this.FileNamePrefix = fileNamePrefix;
            this.mapViewModel = mapViewModel;
            this.messageService = messageService;

            this.TargetSize = initialSize;

            this.ExportCommand = new RelayCommand(this.ExecuteExportCommand);

            // Заполняем размеры изображений.
            foreach (ImageSize size in Enum.GetValues(typeof(ImageSize)))
            {
                var temp = this.GetSizeFromImageSize(size);

                this.Sizes.Add(size, temp.Width + "x" + temp.Height);
            }

            this.Formats.Add(ImageFormat.Gif, "GIF");
            this.Formats.Add(ImageFormat.Jpeg, "JPEG");
            this.Formats.Add(ImageFormat.Png, "PNG");

            this.SelectedFormat = ImageFormat.Png;

            var maxSize = this.GetSizeFromImageSize(ImageSize.A4);

            this.IsTooBig = initialSize.Width > maxSize.Width || initialSize.Height > maxSize.Width;

            if (this.IsTooBig)
                this.SelectedSize = ImageSize.A4;

            this.InitStrings();
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает префикс названия файла.
        /// </summary>
        public string FileNamePrefix
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает заголовок выбора папки.
        /// </summary>
        public string FolderSelectTitle
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает команду экспорта изображения.
        /// </summary>
        public RelayCommand ExportCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает заголовок обзора папки.
        /// </summary>
        public string FolderBrowseTitle
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает путь к папке, в который будет(будут) сохранено(ы) изображение(я).
        /// </summary>
        public string FolderName
        {
            get
            {
                return this.folderName;
            }
            set
            {
                this.folderName = value;

                this.NotifyPropertyChanged(nameof(this.FolderName));
            }
        }

        /// <summary>
        /// Возвращает форматы изображений.
        /// </summary>
        public Dictionary<ImageFormat, string> Formats
        {
            get;
        } = new Dictionary<ImageFormat, string>();

        /// <summary>
        /// Возвращает или задает заголовок выбора формата изображения.
        /// </summary>
        public string FormatSelectTitle
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает начальный размер изображения в пикселях.
        /// </summary>
        public Size InitialSize
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли изображение слишком большим.
        /// </summary>
        public bool IsTooBig
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает выбранный формат изображения.
        /// </summary>
        public ImageFormat SelectedFormat
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает выбранный размер изображения.
        /// </summary>
        public ImageSize SelectedSize
        {
            get
            {
                return this.selectedSize;
            }
            set
            {
                this.selectedSize = value;

                this.TargetSize = this.GetSizeFromImageSize(value);
            }
        }

        /// <summary>
        /// Возвращает размеры изображений.
        /// </summary>
        public Dictionary<ImageSize, string> Sizes
        {
            get;
        } = new Dictionary<ImageSize, string>();

        /// <summary>
        /// Возвращает или задает заголовок выбора размера изображения.
        /// </summary>
        public string SizeSelectTitle
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает конечный размер изображения в пикселях.
        /// </summary>
        public Size TargetSize
        {
            get;
            private set;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выполняет экспорт изображения.
        /// </summary>
        private void ExecuteExportCommand()
        {
            if (!Directory.Exists(this.FolderName))
            {
                this.messageService.ShowMessage("Указанная Вами папка не существует. Пожалуйста, выберите другую папку.", "Экспорт изображения", MessageType.Error);

                return;
            }

            // Скрываем подложку карты, если она отображена.
            var isOpen = this.mapViewModel.IsSubstrateVisible;
            if (isOpen)
                this.mapViewModel.IsSubstrateVisible = false;

            this.ExportRequested?.Invoke(this, EventArgs.Empty);

            // Если подложка карты ранее была отображена, то снова показываем ее.
            if (isOpen)
                this.mapViewModel.IsSubstrateVisible = true;
        }

        /// <summary>
        /// Возвращает размер изображения в пикселях.
        /// </summary>
        /// <param name="size">Размер изображения.</param>
        /// <returns>Размер изображения в пикселях.</returns>
        private Size GetSizeFromImageSize(ImageSize size)
        {
            switch (size)
            {
                case ImageSize.A4:
                    return new Size(3508, 2480);

                case ImageSize.A5:
                    return new Size(2480, 1748);

                case ImageSize.A6:
                    return new Size(1748, 1240);

                case ImageSize.A7:
                    return new Size(1240, 874);

                default:
                    throw new NotImplementedException("Не реализована работа со следующим размером изображения: " + size.ToString());
            }
        }

        /// <summary>
        /// Инициализирует строковые переменные.
        /// </summary>
        private void InitStrings()
        {
            if (this.IsTooBig)
            {
                this.FolderSelectTitle = "Папка, куда будут сохранены изображения:";
                this.FormatSelectTitle = "Формат изображений:";
                this.SizeSelectTitle = "Конечный размер изображения слишком велик. Необходимо разделить его на несколько частей. Выберите размер одной части:";
                this.FolderBrowseTitle = "Выберите папку, куда будут сохранены изображения:";
            }
            else
            {
                this.FolderSelectTitle = "Папка, куда будет сохранено изображение:";
                this.FormatSelectTitle = "Формат изображения:";
                this.FolderBrowseTitle = "Выберите папку, куда будет сохранено изображение:";
            }
        }

        #endregion
    }
}