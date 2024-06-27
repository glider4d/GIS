using System;
using System.Collections.Generic;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель данных о регионе.
    /// </summary>
    [Serializable]
    public sealed class VisualRegionInfoModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="VisualRegionInfoModel"/>.
        /// </summary>
        /// <param name="objectCount">Количество объектов в регионе, разбитые по планируемости и типу.</param>
        /// <param name="lengths">Протяженности труб в регионе, разбитые по планируемости и типу.</param>
        public VisualRegionInfoModel(List<Tuple<bool, string, int>> objectCount, List<Tuple<bool, string, double>> lengths)
        {
            this.ObjectCount = objectCount;
            this.Lengths = lengths;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает протяженности труб в регионе, разбитые по планируемости и типу.
        /// </summary>
        public List<Tuple<bool, string, double>> Lengths
        {
            get;
        }
        
        /// <summary>
        /// Возвращает количество объектов в регионе, разбитые по планируемости и типу.
        /// </summary>
        public List<Tuple<bool, string, int>> ObjectCount
        {
            get;
        }

        #endregion
    }
}