using Kts.Utilities;
using System;
using System.Runtime.Serialization;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель геометрии значка.
    /// </summary>
    [Serializable]
    public sealed class BadgeGeometryModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="BadgeGeometryModel"/>.
        /// </summary>
        /// <param name="geometry">Текстовое представление геометрии значка.</param>
        /// <param name="hotPoint">Главная точка значка.</param>
        /// <param name="originPoint">Точка поворота значка.</param>
        /// <param name="type">Тип объекта, представляемого значком.</param>
        public BadgeGeometryModel(string geometry, Point hotPoint, Point originPoint, ObjectType type)
        {
            this.Geometry = geometry;
            this.HotPoint = hotPoint;
            this.OriginPoint = originPoint;
            this.Type = type;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает текстовое представление геометрии значка.
        /// </summary>
        public string Geometry
        {
            get;
        }

        /// <summary>
        /// Возвращает главную точку значка.
        /// </summary>
        public Point HotPoint
        {
            get;
        }

        /// <summary>
        /// Возвращает точку поворота значка.
        /// </summary>
        public Point OriginPoint
        {
            get;
        }

        /// <summary>
        /// Возвращает тип объекта, представляемого значком на карте.
        /// </summary>
        public ObjectType Type
        {
            get;
        }

        #endregion
    }
}