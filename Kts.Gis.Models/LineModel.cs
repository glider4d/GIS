using Kts.Utilities;
using System;
using System.Collections.Generic;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель объекта, представляемого линией на карте.
    /// </summary>
    [Serializable]
    public sealed partial class LineModel : ObjectModel, IContainerObjectModel, IMapObjectModel, INamedObjectModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LineModel"/>.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        /// <param name="groupId">Идентификатор группы.</param>
        /// <param name="cityId">Идентификатор населенного пункта, в котором расположен объект.</param>
        /// <param name="type">Тип объекта.</param>
        /// <param name="isPlanning">Значение, указывающее на то, что является ли объект планируемым.</param>
        /// <param name="hasChildren">Значение, указывающее на то, что имеет ли объект дочерние объекты.</param>
        /// <param name="startPoint">Начальная точка линии, представляющей объект на карте.</param>
        /// <param name="endPoint">Конечная точка линии, представляющей объект на карте.</param>
        /// <param name="length">Длина линии, представляющей объект на карте.</param>
        /// <param name="points">Текстовое представление последовательности точек изгиба линии.</param>
        /// <param name="labelAngles">Углы поворотов надписей трубы. Ключом является индекс надписи, а значением - ее угол поворота.</param>
        /// <param name="labelPositions">Положения надписей трубы. Ключом является индекс надписи, а значением - ее положение.</param>
        /// <param name="labelSizes">Размеры надписей трубы. Ключом является индекс надписи, а значением - ее размер.</param>
        /// <param name="name">Название объекта.</param>
        /// <param name="diameter">Диаметр объекта, представляемого линией на карте.</param>
        /// <param name="labelOffset">Отступ надписи линии.</param>
        /// <param name="showLabels">Значение, указывающее на то, что следует ли отображать надписи линии.</param>
        /// <param name="forcedLengths">Строка, представляющая форсированные значения длин участков труб.</param>
        /// <param name="isPlanning">Значение, указывающее на то, что является ли объект активным.</param>
        public LineModel(Guid id, Guid groupId, int cityId, ObjectType type, bool isPlanning, bool hasChildren, Point startPoint, Point endPoint, double length, string points, Dictionary<int, double> labelAngles, Dictionary<int, Point> labelPositions, Dictionary<int, int> labelSizes, string name, int diameter, double labelOffset, bool showLabels, string forcedLengths, bool isActive) : base(id, null, cityId, type, isPlanning, isActive)
        {
            ContainerObjectModelHelper.Init(this, hasChildren);
            MapObjectModelHelper.Init(this, false);
            NamedObjectModelHelper.Init(this, name);

            this.GroupId = groupId;
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
#warning Теперь линии отдельно не отслеживают свое состояние, работают ли они или нет
            //this.IsWorking = isWorking;
            this.Length = length;
            this.Points = points;
            this.LabelAngles = labelAngles;
            this.LabelPositions = labelPositions;
            this.LabelSizes = labelSizes;
            this.Diameter = diameter;
            this.LabelOffset = labelOffset;
            this.ShowLabels = showLabels;
            this.ForcedLengths = forcedLengths;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает диаметр объекта, представляемого линией на карте.
        /// </summary>
        public int Diameter
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает конечную точку линии, представляющей объект на карте.
        /// </summary>
        public Point EndPoint
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает строку, представляющую форсированные значения длин участков труб.
        /// </summary>
        public string ForcedLengths
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает идентификатор группы.
        /// </summary>
        public Guid GroupId
        {
            get;
            set;
        }

#warning Теперь линии отдельно не отслеживают свое состояние, работают ли они или нет
        ///// <summary>
        ///// Возвращает или задает значение, указывающее на то, что работает ли линия.
        ///// </summary>
        //public bool IsWorking
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Возвращает или задает углы поворота надписей трубы. Ключом является индекс надписи, а значением - ее угол поворота.
        /// </summary>
        public Dictionary<int, double> LabelAngles
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает отступ надписи линии.
        /// </summary>
        public double LabelOffset
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает положения надписей трубы. Ключом является индекс надписи, а значением - ее положение.
        /// </summary>
        public Dictionary<int, Point> LabelPositions
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает размеры надписей трубы. Ключом является индекс надписи, а значением - ее размер.
        /// </summary>
        public Dictionary<int, int> LabelSizes
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает длину линии, представляющей объект на карте.
        /// </summary>
        public double Length
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает текстовое представление последовательности точек изгиба линии.
        /// </summary>
        public string Points
        {
            get;
            set;
        } = "";

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что следует ли отображать надписи линии.
        /// </summary>
        public bool ShowLabels
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает начальную точку линии, представляющей объект на карте.
        /// </summary>
        public Point StartPoint
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
        public LineModel Clone()
        {
            var startPoint = new Point(this.StartPoint.X, this.StartPoint.Y);
            var endPoint = new Point(this.EndPoint.X, this.EndPoint.Y);
                
            return new LineModel(this.Id, this.GroupId, this.CityId, this.Type, this.IsPlanning, false, startPoint, endPoint, this.Length, this.Points, new Dictionary<int, double>(this.LabelAngles), new Dictionary<int, Point>(this.LabelPositions), new Dictionary<int, int>(this.LabelSizes), this.Name, this.Diameter, this.LabelOffset, this.ShowLabels, this.ForcedLengths, this.IsActive);
        }

        #endregion
    }

    // Реализация IContainerObjectModel.
    public sealed partial class LineModel
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
    public sealed partial class LineModel
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
    public sealed partial class LineModel
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