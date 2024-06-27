using Kts.Utilities;
using System;
using System.Collections.Generic;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель объекта, представляемого фигурой на карте.
    /// </summary>
    [Serializable]
    public sealed partial class FigureModel : ObjectModel, IContainerObjectModel, IMapObjectModel, INamedObjectModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="FigureModel"/>.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        /// <param name="cityId">Идентификатор населенного пункта, в котором расположен объект.</param>
        /// <param name="type">Тип объекта.</param>
        /// <param name="isPlanning">Значение, указывающее на то, что является ли объект планируемым.</param>
        /// <param name="hasChildren">Значение, указывающее на то, что имеет ли объект дочерние объекты.</param>
        /// <param name="name">Название объекта.</param>
        /// <param name="labelPosition">Положение надписи фигуры.</param>
        /// <param name="isActive">Значение, указывающее на то, что является ли объект активным.</param>
        /// <param name="childrenTypes">Типы дочерних объектов.</param>
        public FigureModel(Guid id, int cityId, ObjectType type, bool isPlanning, bool hasChildren, string name, bool isActive, List<ObjectType> childrenTypes) : base(id, null, cityId, type, isPlanning, isActive)
        {
            ContainerObjectModelHelper.Init(this, hasChildren);
            MapObjectModelHelper.Init(this, false);
            NamedObjectModelHelper.Init(this, name);

            this.ChildrenTypes = childrenTypes;

            this.FigureType = FigureType.None;
            this.Size = new Size(0, 0);
            this.Position = new Point(0, 0);
            this.Angle = 0;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="FigureModel"/>.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        /// <param name="cityId">Идентификатор населенного пункта, в котором расположен объект.</param>
        /// <param name="type">Тип объекта.</param>
        /// <param name="isPlanning">Значение, указывающее на то, что является ли объект планируемым.</param>
        /// <param name="hasChildren">Значение, указывающее на то, что имеет ли объект дочерние объекты.</param>
        /// <param name="name">Название объекта.</param>
        /// <param name="figureType">Тип фигуры, представляющей объект на карте.</param>
        /// <param name="size">Размер фигуры, представляющей объект на карте.</param>
        /// <param name="position">Положение фигуры, представляющей объект на карте.</param>
        /// <param name="angle">Угол поворота фигуры, представляющей объект на карте.</param>
        /// <param name="points">Текстовое представление последовательности точек, из которых состоит фигура, представляющая объект на карте.</param>
        /// <param name="labelAngle">Угол поворота надписи фигуры.</param>
        /// <param name="labelPosition">Положение надписи фигуры.</param>
        /// <param name="labelSize">Размер надписи фигуры.</param>
        /// <param name="isActive">Значение, указывающее на то, что является ли объект активным.</param>
        /// <param name="childrenTypes">Типы дочерних объектов.</param>
        public FigureModel(Guid id, int cityId, ObjectType type, bool isPlanning, bool hasChildren, string name, FigureType figureType, Size size, Point position, double angle, string points, double? labelAngle, Point labelPosition, int? labelSize, bool isActive, List<ObjectType> childrenTypes) : base(id, null, cityId, type, isPlanning, isActive)
        {
            ContainerObjectModelHelper.Init(this, hasChildren);
            MapObjectModelHelper.Init(this, false);
            NamedObjectModelHelper.Init(this, name);
            
            this.FigureType = figureType;
            this.Size = size;
            this.Position = position;
            this.Angle = angle;
            this.Points = points;
            this.LabelAngle = labelAngle;
            this.LabelPosition = labelPosition;
            this.LabelSize = labelSize;
            this.ChildrenTypes = childrenTypes;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает угол поворота фигуры, представляющей объект на карте.
        /// </summary>
        public double Angle
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает тип фигуры, представляющей объект на карте.
        /// </summary>
        public FigureType FigureType
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает угол поворота надписи фигуры.
        /// </summary>
        public double? LabelAngle
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает положение надписи фигуры.
        /// </summary>
        public Point LabelPosition
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает размер надписи фигуры.
        /// </summary>
        public int? LabelSize
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает текстовое представление последовательности точек, из которых состоит фигура, представляющая объект на карте.
        /// </summary>
        public string Points
        {
            get;
            set;
        } = "";

        /// <summary>
        /// Возвращает или задает положение фигуры, представляющей объект на карте.
        /// </summary>
        public Point Position
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает размер фигуры, представляющей объект на карте.
        /// </summary>
        public Size Size
        {
            get;
            set;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Возвращает копию модели объекта.
        /// </summary>
        /// <returns>Копия модели объекта.</returns>
        public FigureModel Clone()
        {
            Size size = null;
            Point position = null;
            Point labelPosition = null;

            if (this.Size != null)
                size = new Size(this.Size.Width, this.Size.Height);
            if (this.Position != null)
                position = new Point(this.Position.X, this.Position.Y);
            if (this.LabelPosition != null)
                labelPosition = new Point(this.LabelPosition.X, this.LabelPosition.Y);

            return new FigureModel(this.Id, this.CityId, this.Type, this.IsPlanning, false, this.Name, this.FigureType, size, position, this.Angle, this.Points, this.LabelAngle, labelPosition, this.LabelSize, this.IsActive, new List<ObjectType>());
        }

        #endregion
    }

    // Реализация IContainerObjectModel.
    public sealed partial class FigureModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает список типов дочерних объектов.
        /// </summary>
        public List<ObjectType> ChildrenTypes
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что имеет ли объект дочерние объекты.
        /// </summary>
        public bool HasChildren
        {
            get;
            set;
        }

        #endregion
    }

    // Реализация IMapObjectModel.
    public sealed partial class FigureModel
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

    // Реализация INamedObjectModel.
    public sealed partial class FigureModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает название объекта.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        #endregion
    }
}