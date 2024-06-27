namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет интерфейс модели объекта карты.
    /// </summary>
    public interface IMapObjectModel
    {
        #region Свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что размещен ли объект на карте.
        /// </summary>
        bool IsPlaced
        {
            get;
            set;
        }

        #endregion
    }
}