using System;

namespace Kts.Utilities
{
    /// <summary>
    /// Представляет помощника по работе с типами.
    /// </summary>
    public static class TypeHelper
    {
        #region Открытые статические методы

        /// <summary>
        /// Возвращает значение типа по умолчанию.
        /// </summary>
        /// <param name="type">Тип.</param>
        /// <returns>Значение типа по умолчанию.</returns>
        public static object GetDefaultValue(Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);

            if (type == typeof(string))
                // Если тип является строковым, то возвращаем пустую строку.
                return "";

            // Иначе тип является ссылочным, поэтому возвращаем null.
            return null;
        }

        #endregion
    }
}