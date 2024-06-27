using System;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель объекта, представляемого значком на карте.
    /// </summary>
    [Serializable]
    public sealed partial class BadgeModel : ObjectModel, IMapObjectModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="BadgeModel"/>.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        /// <param name="parentId">Идентификатор родителя объекта.</param>
        /// <param name="cityId">Идентификатор населенного пункта, в котором расположен объект.</param>
        /// <param name="type">Тип объекта.</param>
        /// <param name="isPlanning">Значение, указывающее на то, что является ли объект планируемым.</param>
        /// <param name="distance">Расстояние, на которое объект отдален от объекта-родителя.</param>
        /// <param name="isActive">Значение, указывающее на то, что является ли объект активным.</param>
        public BadgeModel(Guid id, Guid parentId, int cityId, ObjectType type, bool isPlanning, double distance, bool isActive) : base(id, parentId, cityId, type, isPlanning, isActive)
        {
            this.Distance = distance;

            MapObjectModelHelper.Init(this, false);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает расстояние, на которое объект отдален от объекта-родителя.
        /// </summary>
        public double Distance
        {
            get;
            set;
        }

        #endregion
    }

    // Реализация IMapObjectModel.
    public sealed partial class BadgeModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что размещен ли объект на карте.
        /// </summary>
        public bool IsPlaced
        {
            get;
            set;
        }

        #endregion
    }
}