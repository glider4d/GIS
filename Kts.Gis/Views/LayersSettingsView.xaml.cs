using Kts.Gis.ViewModels;
using Kts.Settings;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление настройки слоев.
    /// </summary>
    internal sealed partial class LayersSettingsView : Window
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Модель представления.
        /// </summary>
        private readonly LayersSettingsViewModel viewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LayersSettingsView"/>.
        /// </summary>
        /// <param name="settingService">Сервис настроек.</param>
        public LayersSettingsView(ISettingService settingService)
        {
            this.InitializeComponent();

            this.viewModel = new LayersSettingsViewModel(settingService);

            this.DataContext = this.viewModel;

            this.viewModel.CloseRequested += this.ViewModel_CloseRequested;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.MouseLeftButtonDown"/> прямоугольника со цветом новых линий.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void RectangleNew_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var color = this.viewModel.NewLineBrush.Color;

                var dialog = new System.Windows.Forms.ColorDialog()
                {
                    Color = System.Drawing.Color.FromArgb(color.R, color.G, color.B),
                    FullOpen = true
                };

                dialog.ShowDialog();

                this.viewModel.SetNewLineBrush(Color.FromRgb(dialog.Color.R, dialog.Color.G, dialog.Color.B));
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.MouseLeftButtonDown"/> прямоугольника со цветом старых линий.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void RectangleOld_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var color = this.viewModel.OldLineBrush.Color;

                var dialog = new System.Windows.Forms.ColorDialog()
                {
                    Color = System.Drawing.Color.FromArgb(color.R, color.G, color.B),
                    FullOpen = true
                };

                dialog.ShowDialog();

                this.viewModel.SetOldLineBrush(Color.FromRgb(dialog.Color.R, dialog.Color.G, dialog.Color.B));
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="LayersSettingsViewModel.CloseRequested"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_CloseRequested(object sender, System.EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}