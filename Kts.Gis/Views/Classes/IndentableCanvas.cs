using System.Windows;
using System.Windows.Controls;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет холст, который поддерживает отступы и отложенное распределение дочерних элементов.
    /// </summary>
    internal sealed partial class IndentableCanvas : Canvas
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что готов ли холст к распределению своих дочерних элементов.
        /// </summary>
        private bool isReady;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="IndentableCanvas"/>.
        /// </summary>
        /// <param name="map">Карта, содержащая данный холст.</param>
        /// <param name="zOrder">Уровень холста по Z.</param>
        public IndentableCanvas(Map map, int zOrder)
        {
            this.Map = map;
            this.ZOrder = zOrder;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает отступ.
        /// </summary>
        public Thickness Indent
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что готов ли холст к распределению своих дочерних элементов.
        /// </summary>
        public bool IsReady
        {
            get
            {
                return this.isReady;
            }
            set
            {
                this.isReady = value;

                this.InvalidateMeasure();
                this.InvalidateArrange();
            }
        }

        /// <summary>
        /// Возвращает карту, содержающую данный холст.
        /// </summary>
        public Map Map
        {
            get;
        }
        
        /// <summary>
        /// Возвращает уровень холста по Z.
        /// </summary>
        public int ZOrder
        {
            get;
        }

        #endregion
    }

    // Реализация Canvas.
    internal sealed partial class IndentableCanvas
    {
        #region Защищенные переопределенные методы

        /// <summary>
        /// Размещает дочерние элементы холста.
        /// </summary>
        /// <param name="finalSize">Конечный размер холста.</param>
        /// <returns>Конечный размер холста.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (this.IsReady)
                foreach (UIElement element in this.InternalChildren)
                {
                    double x = 0;
                    double y = 0;

                    // Получаем координаты дочернего элемента.
                    double left = GetLeft(element);
                    if (!double.IsNaN(left))
                        x = left;
                    double top = GetTop(element);
                    if (!double.IsNaN(top))
                        y = top;

                    // Добавляем отступы.
                    x += this.Indent.Left;
                    y += this.Indent.Top;

                    element.Arrange(new Rect(new Point(x, y), element.DesiredSize));
                }

            return finalSize;
        }

        /// <summary>
        /// Определяет размеры для дочерних элементов холста.
        /// </summary>
        /// <param name="availableSize">Доступный размер холста.</param>
        /// <returns>Конечный размер холста.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            if (this.IsReady)
            {
                var size = new Size(double.PositiveInfinity, double.PositiveInfinity);

                foreach (UIElement element in this.InternalChildren)
                    element.Measure(size);
            }

            return new Size();
        }

        #endregion
    }
}