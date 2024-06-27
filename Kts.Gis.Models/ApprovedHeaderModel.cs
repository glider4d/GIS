using Kts.Utilities;
using System;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель заголовка "Утверждено"/"Согласовано".
    /// </summary>
    [Serializable]
    public sealed class ApprovedHeaderModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ApprovedHeaderModel"/>.
        /// </summary>
        /// <param name="id">Идентификатор заголовка.</param>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находится заголовок.</param>
        /// <param name="layerId">Идентификатор пользовательского слоя.</param>
        /// <param name="position">Положение заголовка на карте.</param>
        /// <param name="fontSize">Размер шрифта.</param>
        /// <param name="post">Пост утверждающего/согласующего.</param>
        /// <param name="name">Имя утверждающего/согласующего.</param>
        /// <param name="year">Год утверждения/согласования.</param>
        /// <param name="type">Тип заголовка.</param>
        public ApprovedHeaderModel(Guid id, int cityId, Guid layerId, Point position, int fontSize, string post, string name, int year, ApprovedHeaderType type)
        {
            this.Id = id;
            this.CityId = cityId;
            this.LayerId = layerId;
            this.Position = position;
            this.FontSize = fontSize;
            this.Post = post;
            this.Name = name;
            this.Year = year;
            this.Type = type;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает идентификатор населенного пункта, в котором находится заголовок.
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
        /// Возвращает или задает идентификатор заголовка.
        /// </summary>
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что сохранен ли заголовок.
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
        /// Возвращает или задает имя утверждающего/согласующего.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает положение заголовка на карте.
        /// </summary>
        public Point Position
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает пост утверждающего/согласующего.
        /// </summary>
        public string Post
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает тип заголовка.
        /// </summary>
        public ApprovedHeaderType Type
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает год утверждения/согласования.
        /// </summary>
        public int Year
        {
            get;
            set;
        }

        #endregion
    }
}