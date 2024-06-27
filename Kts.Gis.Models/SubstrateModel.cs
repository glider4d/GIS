using Kts.Utilities;
using System;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель подложки.
    /// </summary>
    [Serializable()]
    public sealed class SubstrateModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SubstrateModel"/>.
        /// </summary>
        /// <param name="city">Населенный пункт, для которого используется данная подложка.</param>
        /// <param name="lastUpdate">Дата последнего обновления подложки.</param>
        /// <param name="width">Ширина подложки.</param>
        /// <param name="height">Высота подложки.</param>
        public SubstrateModel(TerritorialEntityModel city, DateTime lastUpdate, int width, int height)
        {
            this.City = city;
            this.LastUpdate = lastUpdate;
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SubstrateModel"/>.
        /// </summary>
        /// <param name="city">Населенный пункт, для которого используется данная подложка.</param>
        /// <param name="lastUpdate">Дата последнего обновления подложки.</param>
        /// <param name="width">Ширина подложки.</param>
        /// <param name="height">Высота подложки.</param>
        /// <param name="columnCount">Количество столбцов источников изображения подложки.</param>
        /// <param name="rowCount">Количество строк источников изображения подложки.</param>
        public SubstrateModel(TerritorialEntityModel city, DateTime lastUpdate, int width, int height, int columnCount, int rowCount) : this(city, lastUpdate, width, height)
        {
            this.ColumnCount = columnCount;
            this.RowCount = rowCount;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает населенный пункт, для которого используется данная подложка.
        /// </summary>
        public TerritorialEntityModel City
        {
            get;
        }

        /// <summary>
        /// Возвращает количество столбцов источников изображения подложки.
        /// </summary>
        public int ColumnCount
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеет ли подложка изображение.
        /// </summary>
        public bool HasImageSource
        {
            get
            {
                return this.ColumnCount > 0 && this.RowCount > 0;
            }
        }

        /// <summary>
        /// Возвращает высоту подложки.
        /// </summary>
        public int Height
        {
            get;
        }

        /// <summary>
        /// Возвращает дату последнего обновления подложки.
        /// </summary>
        public DateTime LastUpdate
        {
            get;
        }

        /// <summary>
        /// Возвращает количество строк источников изображения подложки.
        /// </summary>
        public int RowCount
        {
            get;
        }

        /// <summary>
        /// Возвращает ширину подложки.
        /// </summary>
        public int Width
        {
            get;
        }

        #endregion
    }
}