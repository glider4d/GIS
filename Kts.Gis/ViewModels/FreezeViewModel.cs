using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления ожидания разморозки UI-потока из-за длительного действия.
    /// </summary>
    internal sealed class FreezeViewModel
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Длительное действие.
        /// </summary>
        private readonly Action action;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="FreezeViewModel"/>.
        /// </summary>
        /// <param name="title">Заголовок представления.</param>
        /// <param name="content">Содержимое представления.</param>
        /// <param name="action">Длительное действие.</param>
        public FreezeViewModel(string title, string content, Action action)
        {
            this.Title = title;
            this.Content = content;
            this.action = action;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает содержимое представления.
        /// </summary>
        public string Content
        {
            get;
        }

        /// <summary>
        /// Возвращает заголовок представления.
        /// </summary>
        public string Title
        {
            get;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Начинает выполнение длительного действия.
        /// </summary>
        public void StartAction()
        {
            if (this.action == null)
                throw new NullReferenceException(nameof(this.action));

            this.action();
        }

        #endregion
    }
}