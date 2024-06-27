using Kts.Utilities;
using System;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель таблицы с данными о протяженностях труб, разбитых по диаметрам.
    /// </summary>
    [Serializable]
    public class LengthPerDiameterTableModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LengthPerDiameterTableModel"/>.
        /// </summary>
        /// <param name="id">Идентификатор таблицы.</param>
        /// <param name="cityId">Идентификатор населенного пункта, которому принадлежит таблица.</param>
        /// <param name="boilerId">Идентификатор котельной, чьи данные таблица отображает.</param>
        /// <param name="layerId">Идентификатор пользовательского слоя.</param>
        /// <param name="position">Положение таблицы на карте.</param>
        /// <param name="fontSize">Размер шрифта.</param>
        public LengthPerDiameterTableModel(Guid id, int cityId, Guid boilerId, Guid layerId, Point position, int fontSize)
        {
            this.Id = id;
            this.CityId = cityId;
            this.BoilerId = boilerId;
            this.LayerId = layerId;
            this.Position = position;
            this.FontSize = fontSize;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LengthPerDiameterTableModel"/>.
        /// </summary>
        /// <param name="id">Идентификатор таблицы.</param>
        /// <param name="cityId">Идентификатор населенного пункта, которому принадлежит таблица.</param>
        /// <param name="boilerId">Идентификатор котельной, чьи данные таблица отображает.</param>
        /// <param name="layerId">Идентификатор пользовательского слоя.</param>
        /// <param name="position">Положение таблицы на карте.</param>
        /// <param name="fontSize">Размер шрифта.</param>
        /// <param name="title">Заголовок таблицы.</param>
        public LengthPerDiameterTableModel(Guid id, int cityId, Guid boilerId, Guid layerId, Point position, int fontSize, string title) : this(id, cityId, boilerId, layerId, position, fontSize)
        {
            this.Title = title;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает идентификатор котельной, чьи данные таблица отображает.
        /// </summary>
        public Guid BoilerId
        {
            get;
        }

        /// <summary>
        /// Возвращает идентификатор населенного пункта, которому принадлежит таблица.
        /// </summary>
        public int CityId
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает размер шрифта.
        /// </summary>
        public int FontSize
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает идентификатор таблицы.
        /// </summary>
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что сохранена ли таблица.
        /// </summary>
        public bool IsSaved
        {
            get
            {
                return this.Id != Guid.Empty;
            }
        }

        /// <summary>
        /// Возвращает или задает идентификатор пользовательского слоя.
        /// </summary>
        public Guid LayerId
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает положение таблицы на карте.
        /// </summary>
        public Point Position
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает заголовок таблицы.
        /// </summary>
        public string Title
        {
            get;
            set;
        } = "Свод участков по диаметрам:";

        #endregion
    }
}