using Kts.Messaging;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления изменения отображения длины.
    /// </summary>
    internal sealed class ChangeLengthViewModel
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        private readonly IMessageService messageService;

        /// <summary>
        /// Общая длина.
        /// </summary>
        private readonly double totalLength;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса закрытия представления.
        /// </summary>
        public event EventHandler CloseRequested;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ChangeLengthViewModel"/>.
        /// </summary>
        /// <param name="segments">Сегменты.</param>
        /// <param name="totalLength">Общая длина.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public ChangeLengthViewModel(List<LineSegment> segments, double totalLength, IMessageService messageService)
        {
            this.Segments = new AdvancedObservableCollection<LineSegment>(segments);
            this.totalLength = totalLength;
            this.messageService = messageService;

            this.ChangeCommand = new RelayCommand(this.ExecuteChange);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает команду изменения отображения длины.
        /// </summary>
        public RelayCommand ChangeCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает результат изменения отображения длины.
        /// </summary>
        public bool Result
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает сегменты линии.
        /// </summary>
        public AdvancedObservableCollection<LineSegment> Segments
        {
            get;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выполняет изменение отображения длины.
        /// </summary>
        private void ExecuteChange()
        {
            // Проверяем сумму длин сегментов. Если они не совпадают, то запрещаем смену отображения длины.
            var length = 0.0;
            foreach (var segment in this.Segments)
                length += segment.Length;
#warning Оказалось что тип double может выдавать странные результаты при работе с рациональными числами, так что лучше в будущем перейти на decimal
            length = Math.Round(length, 2);
            if (this.totalLength != length)
                this.messageService.ShowMessage(string.Format("Сумма заданных длин ({0}) не совпадает с длиной из параметров ({1})", length, this.totalLength), "Изменение отображения длины", MessageType.Error);
            else
            {
                this.Result = true;

                this.CloseRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}