using Kts.Gis.ViewModels;
using Kts.Messaging;
using System;
using System.Windows;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление поиска объекта.
    /// </summary>
    internal sealed partial class FindObjectView : Window
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        private readonly IMessageService messageService;

        /// <summary>
        /// Модель представления.
        /// </summary>
        private readonly FindObjectViewModel viewModel;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса открытия объекта.
        /// </summary>
        public event EventHandler<OpenObjectRequestedEventArgs> OpenObjectRequested;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="FindObjectView"/>.
        /// </summary>
        /// <param name="messageService">Сервис сообщений.</param>
        public FindObjectView(IMessageService messageService)
        {
            this.InitializeComponent();

            this.messageService = messageService;

            this.viewModel = new FindObjectViewModel(messageService);

            this.viewModel.FindRequested += this.ViewModel_FindRequested;

            this.DataContext = this.viewModel;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="System.Windows.Controls.Primitives.ButtonBase.Click"/> кнопки "Отмена".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.GotFocus"/> поля ввода значения.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() => this.textBox.SelectAll()), null);
        }
        
        /// <summary>
        /// Обрабатывает событие <see cref="FindObjectViewModel.FindRequested"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_FindRequested(object sender, EventArgs e)
        {
            var eventArgs = new OpenObjectRequestedEventArgs(this.viewModel.ObjectId);

            this.OpenObjectRequested?.Invoke(this, eventArgs);

            if (eventArgs.IsFound)
                this.Close();
            else
                this.messageService.ShowMessage("Объект с заданным идентификатором не найден", "Поиск", MessageType.Information);
        }

        #endregion
    }
}