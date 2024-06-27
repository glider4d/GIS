namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет помощника по работе с моделями именованных объектов.
    /// </summary>
    internal static class NamedObjectModelHelper
    {
        #region Открытые статические методы

        /// <summary>
        /// Инициализирует свойства объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="name">Название объекта.</param>
        public static void Init(INamedObjectModel obj, string name)
        {
            obj.Name = name;
        }

        #endregion
    }
}