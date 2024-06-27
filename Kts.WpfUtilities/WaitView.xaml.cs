using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Kts.WpfUtilities
{
    /// <summary>
    /// Представляет представление ожидания окончания выполнения какой-либо задачи.
    /// </summary>
    public sealed partial class WaitView : Window
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что может ли представление быть закрытым.
        /// </summary>
        private bool canClose;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Модель представления ожидания окончания выполнения какой-либо задачи.
        /// </summary>
        private readonly WaitViewModel waitViewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="WaitView"/>.
        /// </summary>
        /// <param name="waitViewModel">Модель представления ожидания окончания выполнения какой-либо задачи.</param>
        /// <param name="icon">Иконка представления.</param>
        public WaitView(WaitViewModel waitViewModel, ImageSource icon)
        {
            this.InitializeComponent();

            this.waitViewModel = waitViewModel;
            this.Icon = icon;

            this.DataContext = waitViewModel;

            waitViewModel.CloseRequested += this.WaitViewModel_CloseRequested;
        }

        public WaitView(WaitViewModel waitViewModel)
        {
            this.InitializeComponent();

            this.waitViewModel = waitViewModel;
         

            this.DataContext = waitViewModel;

            waitViewModel.CloseRequested += this.WaitViewModel_CloseRequested;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="WaitViewModel.CloseRequested"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void WaitViewModel_CloseRequested(object sender, EventArgs e)
        {
            this.canClose = true;

            Application.Current.Dispatcher.BeginInvoke(new Action(() => this.Close()));
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Window.Closing"/> представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = !this.canClose;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="FrameworkElement.Loaded"/> представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.waitViewModel.Action != null)
                await this.waitViewModel.StartActionAsync();
            else
                await this.waitViewModel.StartFuncAsync();
        }

        #endregion
    }
}