using Kts.Messaging;
using Kts.WpfUtilities;
using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления поиска объекта.
    /// </summary>
    internal sealed class FindObjectViewModel
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        private readonly IMessageService messageService;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса поиска объекта.
        /// </summary>
        public event EventHandler FindRequested;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="FindObjectViewModel"/>.
        /// </summary>
        /// <param name="messageService">Сервис сообщений.</param>
        public FindObjectViewModel(IMessageService messageService)
        {
            this.messageService = messageService;

            this.FindCommand = new RelayCommand(this.ExecuteFind);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает команду поиска объекта.
        /// </summary>
        public RelayCommand FindCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает идентификатор объекта.
        /// </summary>
        public Guid ObjectId
        {
            get;
            set;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выполняет поиск объекта.
        /// </summary>
        public void ExecuteFind()
        {
            if (this.ObjectId == Guid.Empty)
            {
                this.messageService.ShowMessage("Введен неверный идентификатор объекта", "Поиск", MessageType.Error);

                return;
            }

            this.FindRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}