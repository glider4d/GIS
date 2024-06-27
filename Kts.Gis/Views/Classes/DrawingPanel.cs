using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет панель, содержащую в себе <see cref="Visual"/> элементы.
    /// </summary>
    internal sealed partial class DrawingPanel : Panel
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Визуальные элементы панели.
        /// </summary>
        private readonly List<Visual> visuals = new List<Visual>();

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает количество визуальных элементов панели.
        /// </summary>
        public int VisualCount
        {
            get
            {
                return this.VisualChildrenCount;
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Добавляет визуальный элемент на панель.
        /// </summary>
        /// <param name="visual">Добавляемый визуальный элемент.</param>
        public void AddVisual(Visual visual)
        {
            this.visuals.Add(visual);

            this.AddVisualChild(visual);
            this.AddLogicalChild(visual);
        }

        /// <summary>
        /// Удаляет заданный визуальный элемент с панели.
        /// </summary>
        /// <param name="visual">Удаляемый визуальный элемент.</param>
        public void DeleteVisual(Visual visual)
        {
            this.visuals.Remove(visual);

            this.RemoveVisualChild(visual);
            this.RemoveLogicalChild(visual);
        }

        /// <summary>
        /// Удаляет все визуальные элементы с панели.
        /// </summary>
        public void DeleteVisuals()
        {
            for (int i = this.VisualChildrenCount - 1; i >= 0; i--)
                this.DeleteVisual(this.GetVisualChild(i));
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеет ли панель заданный визуальный элемент.
        /// </summary>
        /// <param name="visual">Визуальный элемент.</param>
        /// <returns>true, если имеет, иначе - false.</returns>
        public bool HasVisual(Visual visual)
        {
            return this.visuals.Contains(visual);
        }

        #endregion
    }

    // Реализация Panel.
    internal sealed partial class DrawingPanel
    {
        #region Защищенные переопределенные свойства

        /// <summary>
        /// Возвращает количество визуальных элементов панели.
        /// </summary>
        protected override int VisualChildrenCount
        {
            get
            {
                return this.visuals.Count;
            }
        }

        #endregion

        #region Защищенные переопределенные методы

        /// <summary>
        /// Возвращает визуальный элемент панели по его индексу.
        /// </summary>
        /// <param name="index">Индекс.</param>
        /// <returns>Визуальный элемент.</returns>
        protected override Visual GetVisualChild(int index)
        {
            return this.visuals[index];
        }

        #endregion
    }
}