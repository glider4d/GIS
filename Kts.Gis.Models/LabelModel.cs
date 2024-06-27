using Kts.Utilities;
using System;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель надписи.
    /// </summary>
    [Serializable]
    public sealed class LabelModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LabelModel"/>.
        /// </summary>
        /// <param name="content">Содержимое надписи.</param>
        /// <param name="position">Положение надписи.</param>
        /// <param name="size">Размер надписи.</param>
        /// <param name="angle">Угол поворота надписи.</param>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находится надпись.</param>
        /// <param name="layerId">Идентификатор кастомного слоя, к которому принадлежит надпись.</param>
        public LabelModel(string content, Point position, int size, double angle, int cityId, Guid layerId) : this(DefaultId, content, position, size, angle, true, false, false, cityId, layerId)
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LabelModel"/>.
        /// </summary>
        /// <param name="content">Содержимое надписи.</param>
        /// <param name="position">Положение надписи.</param>
        /// <param name="size">Размер надписи.</param>
        /// <param name="angle">Угол поворота надписи.</param>
        /// <param name="isBold">Значение, указывающее на то, что является ли шрифт надписи полужирным.</param>
        /// <param name="isItalic">Значение, указывающее на то, что является ли шрифт надписи курсивным.</param>
        /// <param name="isUnderline">Значение, указывающее на то, что является ли шрифт надписи подчеркнутым.</param>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находится надпись.</param>
        /// <param name="layerId">Идентификатор кастомного слоя, к которому принадлежит надпись.</param>
        public LabelModel(string content, Point position, int size, double angle, bool isBold, bool isItalic, bool isUnderline, int cityId, Guid layerId) : this(DefaultId, content, position, size, angle, isBold, isItalic, isUnderline, cityId, layerId)
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LabelModel"/>.
        /// </summary>
        /// <param name="id">Идентификатор надписи.</param>
        /// <param name="content">Содержимое надписи.</param>
        /// <param name="position">Положение надписи.</param>
        /// <param name="size">Размер надписи.</param>
        /// <param name="angle">Угол поворота надписи.</param>
        /// <param name="isBold">Значение, указывающее на то, что является ли шрифт надписи полужирным.</param>
        /// <param name="isItalic">Значение, указывающее на то, что является ли шрифт надписи курсивным.</param>
        /// <param name="isUnderline">Значение, указывающее на то, что является ли шрифт надписи подчеркнутым.</param>
        /// <param name="cityId">Идентификатор населенного пункта, в котором находится надпись.</param>
        /// <param name="layerId">Идентификатор кастомного слоя, к которому принадлежит надпись.</param>
        public LabelModel(Guid id, string content, Point position, int size, double angle, bool isBold, bool isItalic, bool isUnderline, int cityId, Guid layerId)
        {
            this.Id = id;
            this.Content = content;
            this.Position = position;
            this.Size = size;
            this.Angle = angle;
            this.IsBold = isBold;
            this.IsItalic = isItalic;
            this.IsUnderline = isUnderline;
            this.CityId = cityId;
            this.LayerId = layerId;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает угол поворота надписи.
        /// </summary>
        public double Angle
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает идентификатор населенного пункта, в котором находится надпись.
        /// </summary>
        public int CityId
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает содержимое надписи.
        /// </summary>
        public string Content
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает идентификатор надписи.
        /// </summary>
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли шрифт надписи полужирным.
        /// </summary>
        public bool IsBold
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли шрифт надписи курсивным.
        /// </summary>
        public bool IsItalic
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что сохранена ли надпись.
        /// </summary>
        public bool IsSaved
        {
            get
            {
                return this.Id != DefaultId;
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли шрифт надписи подчеркнутым.
        /// </summary>
        public bool IsUnderline
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает идентификатор кастомного слоя, к которому принадлежит надпись.
        /// </summary>
        public Guid LayerId
        {
            get;
            set;
        }
        
        /// <summary>
        /// Возвращает или задает положение надписи.
        /// </summary>
        public Point Position
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает размер надписи.
        /// </summary>
        public int Size
        {
            get;
            set;
        }

        #endregion

        #region Открытые статические свойства

        /// <summary>
        /// Возвращает идентификатор надписей по умолчанию.
        /// </summary>
        public static Guid DefaultId
        {
            get
            {
                return Guid.Empty;
            }
        }

        #endregion
    }
}