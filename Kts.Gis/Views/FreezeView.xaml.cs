using Kts.Gis.ViewModels;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление ожидания разморозки UI-потока из-за длительного действия.
    /// </summary>
    internal partial class FreezeView : Window
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что может ли представление быть закрытым.
        /// </summary>
        private bool canClose;

        /// <summary>
        /// Значение, указывающее на то, что было ли отображено представление.
        /// </summary>
        private bool isShown;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Модель представления ожидания разморозки UI-потока из-за длительного действия.
        /// </summary>
        private readonly FreezeViewModel viewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="FreezeView"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления ожидания разморозки UI-потока из-за длительного действия.</param>
        public FreezeView(FreezeViewModel viewModel)
        {
            this.InitializeComponent();

            this.viewModel = viewModel;

            this.DataContext = viewModel;
        }

        #endregion

        #region Обработчики событий
        
        /// <summary>
        /// Обрабатывает событие <see cref="Window.Closing"/> представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = !this.canClose;
        }

        #endregion

        #region Защищенные переопределенные методы

        /// <summary>
        /// Выполняет действия, связанные с завершением отрисовки содержимого представления.
        /// </summary>
        /// <param name="e">Аргументы.</param>
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            if (this.isShown)
                return;

            this.isShown = true;

            var startTime = DateTime.Now;
            
            this.viewModel.StartAction();

            if ((DateTime.Now - startTime).TotalSeconds <= 500)
                Thread.Sleep(500);

            this.canClose = true;

            this.Close();
        }

        #endregion
    }
}