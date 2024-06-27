using Kts.Messaging;
using Kts.WpfUtilities;
using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления добавления объекта, представляемого значком на карте.
    /// </summary>
    internal sealed class AddBadgeViewModel
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        //[NonSerialized]
        private readonly IMessageService messageService;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса закрытия представления.
        /// </summary>
        public event EventHandler CloseRequested;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AddBadgeViewModel"/>.
        /// </summary>
        /// <param name="line">Линия, которой добавляется значок.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public AddBadgeViewModel(LineViewModel line, IMessageService messageService)
        {
            this.Line = line;
            this.messageService = messageService;

            this.CheckDistanceCommand = new RelayCommand(this.ExecuteCheckDistance);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает команду проверки правильности ввода расстояния.
        /// </summary>
        public RelayCommand CheckDistanceCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает расстояние.
        /// </summary>
        public string Distance
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает линию, которой нужно добавить значок.
        /// </summary>
        public LineViewModel Line
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает введенное расстояние.
        /// </summary>
        public double? Result
        {
            get;
            private set;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выполняет проверку правильности ввода расстояния.
        /// </summary>
        private void ExecuteCheckDistance()
        {
            try
            {
                this.Result = Convert.ToDouble(this.Distance);

                // Уведомляем о том, что нужно закрыть представление.
                this.CloseRequested?.Invoke(this, EventArgs.Empty);
            }
            catch
            {
                this.Result = null;

                this.messageService.ShowMessage("Вы ввели неверное значение", "Добавление значка", MessageType.Error);
            }
        }

        #endregion
    }
}