using System;

namespace Kts.WpfUtilities
{
    /// <summary>
    /// Представляет модель представления переменной.
    /// </summary>
    public sealed class DebuggingVariableViewModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DebuggingVariableViewModel"/>.
        /// </summary>
        /// <param name="name">Название переменной.</param>
        /// <param name="dateTime">Дата и время изменения значения переменной.</param>
        /// <param name="value">Значение переменной.</param>
        public DebuggingVariableViewModel(string name, DateTime dateTime, string value)
        {
            this.Name = name;
            this.Time = dateTime.ToString("mm:ss.fff");
            this.Value = value;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает название переменной.
        /// </summary>
        public string Name
        {
            get;
        }

        /// <summary>
        /// Возвращает время изменения значения переменной.
        /// </summary>
        public string Time
        {
            get;
        }

        /// <summary>
        /// Возвращает значение переменной.
        /// </summary>
        public string Value
        {
            get;
        }

        #endregion
    }
}