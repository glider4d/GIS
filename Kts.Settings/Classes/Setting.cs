using System;

namespace Kts.Settings
{
    /// <summary>
    /// Представляет настройку.
    /// </summary>
    public sealed class Setting
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Setting"/>.
        /// </summary>
        /// <param name="name">Название настройки.</param>
        /// <param name="type">Тип значения настройки.</param>
        /// <param name="defaultValue">Значение настройки по умолчанию.</param>
        public Setting(string name, Type type, object defaultValue)
        {
            this.Name = name;
            this.Type = type;
            this.DefaultValue = defaultValue;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает значение настройки по умолчанию.
        /// </summary>
        public object DefaultValue
        {
            get;
        }

        /// <summary>
        /// Возвращает название настройки.
        /// </summary>
        public string Name
        {
            get;
        }

        /// <summary>
        /// Возвращает тип значения настройки.
        /// </summary>
        public Type Type
        {
            get;
        }

        #endregion
    }
}