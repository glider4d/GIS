namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет помощника по работе с моделями объектов карты.
    /// </summary>
    internal static class MapObjectModelHelper
    {
        #region Открытые статические методы

        /// <summary>
        /// Инициализирует свойства объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="isPlaced">Значение, указывающее на то, что размещен ли объект на карте.</param>
        public static void Init(IMapObjectModel obj, bool isPlaced)
        {
            obj.IsPlaced = isPlaced;
        }

        #endregion
    }
}