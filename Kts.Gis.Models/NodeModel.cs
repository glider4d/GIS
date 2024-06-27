using Kts.Utilities;
using System;
using System.Collections.Generic;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель объекта, представляемого узлом на карте.
    /// </summary>
    [Serializable]
    public sealed partial class NodeModel : ObjectModel, IMapObjectModel
    {
        #region Закрытые константы

        private const bool isActive = true;

        /// <summary>
        /// Значение, указывающее на то, что является ли объект планируемым.
        /// </summary>
        private const bool isPlanning = false;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NodeModel"/>.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        /// <param name="cityId">Идентификатор населенного пункта, в котором расположен объект.</param>
        /// <param name="type">Тип объекта.</param>
        /// <param name="position">Положение узла, представляющего объект на карте.</param>
        /// <param name="connectedObjectData">Данные о подключенном к узлу объекте (его идентификатор, тип и значение, указывающее на то, что является ли объект планируемым).</param>
        /// <param name="connectionData">Данные соединений линий с узлом.</param>
        /// <param name="ignoreStick">Значение, указывающее на то, что стоит ли игнорировать прикрепление узла к границе родительской фигуры.</param>
        public NodeModel(Guid id, int cityId, ObjectType type, Point position, Tuple<Guid, ObjectType, bool> connectedObjectData, List<NodeConnectionData> connectionData, bool ignoreStick) : base(id, null, cityId, type, isPlanning, isActive)
        {
            MapObjectModelHelper.Init(this, false);

            this.Position = position;
            this.ConnectedObjectData = connectedObjectData;

            if (connectionData != null)
                this.ConnectionData.AddRange(connectionData);

            this.IgnoreStick = ignoreStick;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает данные о подключенном к узлу объекте (его идентификатор, тип и значение, указывающее на то, что является ли объект планируемым).
        /// </summary>
        public Tuple<Guid, ObjectType, bool> ConnectedObjectData
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает данные соединений линий с узлом.
        /// </summary>
        public List<NodeConnectionData> ConnectionData
        {
            get;
        } = new List<NodeConnectionData>();

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что стоит ли игнорировать прикрепление узла к границе родительской фигуры.
        /// </summary>
        public bool IgnoreStick
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает положение узла, представляющего объект на карте.
        /// </summary>
        public Point Position
        {
            get;
            set;
        }

        #endregion
    }

    // Реализация IMapObjectModel.
    public sealed partial class NodeModel
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