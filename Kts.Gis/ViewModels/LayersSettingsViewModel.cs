using Kts.Settings;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Windows.Media;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления настроек слоев.
    /// </summary>
    internal sealed class LayersSettingsViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что были ли загружены цвета при первоначальной загрузке.
        /// </summary>
        private bool areColorsLoaded;

        /// <summary>
        /// Значение, указывающее на то, что имеются ли изменения.
        /// </summary>
        private bool hasChanges;

        /// <summary>
        /// Кисть новых линий.
        /// </summary>
        private SolidColorBrush newLineBrush;

        /// <summary>
        /// Кисть старых линий.
        /// </summary>
        private SolidColorBrush oldLineBrush;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис настроек.
        /// </summary>
        private readonly ISettingService settingService;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса закрытия представления.
        /// </summary>
        public event EventHandler CloseRequested;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LayersSettingsViewModel"/>.
        /// </summary>
        /// <param name="settingService">Сервис настроек.</param>
        public LayersSettingsViewModel(ISettingService settingService)
        {
            this.settingService = settingService;

            this.SaveCommand = new RelayCommand(this.ExecuteSave, this.CanExecuteSave);

            this.SetNewLineBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(settingService.Settings["NewLineColor"].ToString()));
            this.SetOldLineBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(settingService.Settings["OldLineColor"].ToString()));

            this.areColorsLoaded = true;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что имеются ли изменения.
        /// </summary>
        public bool HasChanges
        {
            get
            {
                return this.hasChanges;
            }
            private set
            {
                this.hasChanges = value;

                this.SaveCommand.RaiseCanExecuteChanged();

                this.NotifyPropertyChanged(nameof(this.HasChanges));
            }
        }

        /// <summary>
        /// Возвращает или задает кисть новых линий.
        /// </summary>
        public SolidColorBrush NewLineBrush
        {
            get
            {
                return this.newLineBrush;
            }
            private set
            {
                this.newLineBrush = value;

                this.NotifyPropertyChanged(nameof(this.NewLineBrush));
            }
        }

        /// <summary>
        /// Возвращает или задает кисть старых линий.
        /// </summary>
        public SolidColorBrush OldLineBrush
        {
            get
            {
                return this.oldLineBrush;
            }
            private set
            {
                this.oldLineBrush = value;

                this.NotifyPropertyChanged(nameof(this.OldLineBrush));
            }
        }

        /// <summary>
        /// Возвращает команду сохранения изменений.
        /// </summary>
        public RelayCommand SaveCommand
        {
            get;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить сохранение.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteSave()
        {
            return this.HasChanges;
        }

        /// <summary>
        /// Выполняет сохранение изменений.
        /// </summary>
        private void ExecuteSave()
        {
            var color = this.NewLineBrush.Color;

            this.settingService.Settings["NewLineColor"] = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
            
            color = this.OldLineBrush.Color;

            this.settingService.Settings["OldLineColor"] = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");

            this.CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Задает кисть новых линий.
        /// </summary>
        /// <param name="color">Цвет.</param>
        public void SetNewLineBrush(System.Windows.Media.Color color)
        {
            var brush = new SolidColorBrush(color);

            if (brush.CanFreeze)
                brush.Freeze();

            this.NewLineBrush = brush;

            if (this.areColorsLoaded)
                this.HasChanges = true;
        }

        /// <summary>
        /// Задает кисть старых линий.
        /// </summary>
        /// <param name="color">Цвет.</param>
        public void SetOldLineBrush(System.Windows.Media.Color color)
        {
            var brush = new SolidColorBrush(color);

            if (brush.CanFreeze)
                brush.Freeze();

            this.OldLineBrush = brush;

            if (this.areColorsLoaded)
                this.HasChanges = true;
        }

        #endregion
    }
}