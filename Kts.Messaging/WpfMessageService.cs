using System;
using System.Linq;
using System.Windows;

namespace Kts.Messaging
{
    /// <summary>
    /// Представляет сервис сообщений, использующий для вывода <see cref="MessageBox"/>.
    /// </summary>
    [Serializable]
    public sealed partial class WpfMessageService : IMessageService
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Приложение, использующее данный сервис.
        /// </summary>
        private readonly Application application;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="WpfMessageService"/>.
        /// </summary>
        /// <param name="application">Приложение, использующее данный сервис.</param>
        public WpfMessageService(Application application)
        {
            this.application = application;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает активное окно приложения, использующего данный сервис.
        /// </summary>
        /// <returns>Окно.</returns>
        public Window GetActiveWindow()
        {
            var window = application.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

            // Если такого окна нет, то пробуем активировать главное окно приложения и выбрать его.
            if (window == null && this.application.MainWindow != null)
            {
                this.application.MainWindow.Activate();

                window = application.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            }

            return window;
        }

        #endregion
    }

    // Реализация IMessageService.
    public sealed partial class WpfMessageService
    {
        #region Открытые методы

        /// <summary>
        /// Показывает сообщение.
        /// </summary>
        /// <param name="content">Содержимое сообщения.</param>
        /// <param name="caption">Заголовок сообщения.</param>
        /// <param name="messageType">Тип сообщения.</param>
        public void ShowMessage(string content, string caption, MessageType messageType)
        {
            // Выбираем иконку сообщения в зависимости от его типа.
            MessageBoxImage image;
            switch (messageType)
            {
                case MessageType.Error:
                    image = MessageBoxImage.Error;

                    break;

                case MessageType.Information:
                    image = MessageBoxImage.Information;

                    break;

                default:
                    throw new NotImplementedException("Не реализовано отображение сообщения следующего типа: " + messageType.ToString());
            }

            WpfCustomMessageBox.WpfCustomMessageBox.Show(content, caption, MessageBoxButton.OK, image, this.GetActiveWindow());
        }

        /// <summary>
        /// Показывает сообщение с выбором "Да", "Нет" или "Отмена".
        /// </summary>
        /// <param name="content">Содержимое сообщения.</param>
        /// <param name="caption">Заголовок сообщения.</param>
        /// <returns>true - если выбрано "Да", false - если "Нет" и null - если "Отмена".</returns>
        public bool? ShowYesNoCancelMessage(string content, string caption)
        {
            var result = WpfCustomMessageBox.WpfCustomMessageBox.Show(content, caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Question, this.GetActiveWindow());

            switch (result)
            {
                case MessageBoxResult.Yes:
                    return true;

                case MessageBoxResult.No:
                    return false;
            }

            return null;
        }

        /// <summary>
        /// Показывает сообщение с выбором "Да" или "Нет".
        /// </summary>
        /// <param name="content">Содержимое сообщения.</param>
        /// <param name="caption">Заголовок сообщения.</param>
        /// <returns>Значение, указывающее на то, что выбран ли вариант "Да".</returns>
        public bool ShowYesNoMessage(string content, string caption)
        {
            if (WpfCustomMessageBox.WpfCustomMessageBox.Show(content, caption, MessageBoxButton.YesNo, MessageBoxImage.Question, this.GetActiveWindow()) == MessageBoxResult.Yes)
                return true;

            return false;
        }

        #endregion
    }
}