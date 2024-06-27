using Kts.Gis.Models;
using System;
using System.ComponentModel;
using System.Windows.Data;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления данных о визуальном регионе.
    /// </summary>
    internal sealed class VisualRegionInfoViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что планируемые ли объекты должны быть отображены.
        /// </summary>
        private bool isPlanning;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="VisualRegionInfoViewModel"/>.
        /// </summary>
        /// <param name="model">Модель.</param>
        public VisualRegionInfoViewModel(VisualRegionInfoModel model)
        {
            this.Lengths = CollectionViewSource.GetDefaultView(model.Lengths);
            this.ObjectCount = CollectionViewSource.GetDefaultView(model.ObjectCount);

            this.Lengths.Filter = this.FilterLengths;
            this.ObjectCount.Filter = this.FilterObjectCount;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что планируемые ли объекты должны быть отображены.
        /// </summary>
        public bool IsPlanning
        {
            get
            {
                return this.isPlanning;
            }
            set
            {
                this.isPlanning = value;

                this.ObjectCount.Refresh();
                this.Lengths.Refresh();
            }
        }

        /// <summary>
        /// Возвращает коллекцию, представлящую протяженность.
        /// </summary>
        public ICollectionView Lengths
        {
            get;
        }

        /// <summary>
        /// Возвращает коллекцию, представляющую количество объектов.
        /// </summary>
        public ICollectionView ObjectCount
        {
            get;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что должен ли отображаться заданный элемент в коллекции.
        /// </summary>
        /// <param name="item">Элемент коллекции.</param>
        /// <returns>true, если должен отображаться, иначе - false.</returns>
        private bool FilterLengths(object item)
        {
            var tuple = item as Tuple<bool, string, double>;

            return tuple.Item1 == this.IsPlanning;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что должен ли отображаться заданный элемент в коллекции.
        /// </summary>
        /// <param name="item">Элемент коллекции.</param>
        /// <returns>true, если должен отображаться, иначе - false.</returns>
        private bool FilterObjectCount(object item)
        {
            var tuple = item as Tuple<bool, string, int>;

            return tuple.Item1 == this.IsPlanning;
        }

        #endregion
    }
}