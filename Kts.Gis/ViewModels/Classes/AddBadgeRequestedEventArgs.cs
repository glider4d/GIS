using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет аргумент события запроса добавления объекта, представляемого значком на карте.
    /// </summary>
    internal sealed class AddBadgeRequestedEventArgs : EventArgs
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AddBadgeRequestedEventArgs"/>.
        /// </summary>
        /// <param name="line">Линия, к которой нужно добавить объект, представляемый значком на карте.</param>
        public AddBadgeRequestedEventArgs(LineViewModel line)
        {
            this.Line = line;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает линию, к которой нужно добавить объект, представляемый значком на карте.
        /// </summary>
        public LineViewModel Line
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает результат запроса, являющийся расстоянием, на которое значок отдален от конца линии.
        /// </summary>
        public double? Result
        {
            get;
            set;
        }

        #endregion
    }
}