namespace Kts.Messaging
{
    /// <summary>
    /// Представляет интерфейс сервиса сообщений.
    /// </summary>
    public interface IMessageService
    {
        #region Методы

        /// <summary>
        /// Показывает сообщение.
        /// </summary>
        /// <param name="content">Содержимое сообщения.</param>
        /// <param name="caption">Заголовок сообщения.</param>
        /// <param name="messageType">Тип сообщения.</param>
        void ShowMessage(string content, string caption, MessageType messageType);

        /// <summary>
        /// Показывает сообщение с выбором "Да", "Нет" или "Отмена".
        /// </summary>
        /// <param name="content">Содержимое сообщения.</param>
        /// <param name="caption">Заголовок сообщения.</param>
        /// <returns>true - если выбрано "Да", false - если "Нет" и null - если "Отмена".</returns>
        bool? ShowYesNoCancelMessage(string content, string caption);

        /// <summary>
        /// Показывает сообщение с выбором "Да" или "Нет".
        /// </summary>
        /// <param name="content">Содержимое сообщения.</param>
        /// <param name="caption">Заголовок сообщения.</param>
        /// <returns>Значение, указывающее на то, что выбран ли вариант "Да".</returns>
        bool ShowYesNoMessage(string content, string caption);

        #endregion
    }
}