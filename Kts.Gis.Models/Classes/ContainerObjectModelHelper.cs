namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет помощника по работе с моделями объектов, способных иметь дочерние объекты.
    /// </summary>
    internal static class ContainerObjectModelHelper
    {
        #region Открытые статические методы

        /// <summary>
        /// Инициализирует свойства объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="hasChildren">Значение, указывающее на то, что имеет ли объект дочерние объекты.</param>
        public static void Init(IContainerObjectModel obj, bool hasChildren)
        {
            obj.HasChildren = hasChildren;
        }

        #endregion
    }
}