using Kts.Gis.Models;
using System;
using System.Collections.Generic;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет аргумент события запроса ввода значения.
    /// </summary>
    internal sealed class ValueInputRequestedEventArgs : EventArgs
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ValueInputRequestedEventArgs"/>.
        /// </summary>
        /// <param name="content">Содержимое диалога.</param>
        /// <param name="caption">Заголовок диалога.</param>
        /// <param name="valueType">Тип вводимого значения.</param>
        /// <param name="initialValue">Начальное значение.</param>
        /// <param name="lineTypes">Типы линий.</param>
        public ValueInputRequestedEventArgs(string content, string caption, Type valueType, object initialValue, List<ObjectType> lineTypes)
        {
            this.Content = content;
            this.Caption = caption;
            this.ValueType = valueType;
            this.InitialValue = initialValue;
            this.LineTypes = lineTypes;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает заголовок диалога.
        /// </summary>
        public string Caption
        {
            get;
        }

        /// <summary>
        /// Возвращает содержимое диалога.
        /// </summary>
        public string Content
        {
            get;
        }

        /// <summary>
        /// Возвращает начальное значение.
        /// </summary>
        public object InitialValue
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает тип линий.
        /// </summary>
        public ObjectType LineType
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает список типов линий.
        /// </summary>
        public List<ObjectType> LineTypes
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает выбранную опцию обновления длин линий.
        /// </summary>
        public LengthUpdateOption Option
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает результат ввода.
        /// </summary>
        public object Result
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает тип вводимого значения.
        /// </summary>
        public Type ValueType
        {
            get;
        }

        #endregion
    }
}