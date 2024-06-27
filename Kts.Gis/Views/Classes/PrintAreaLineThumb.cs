using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет линию-изменялку размеров области печати.
    /// </summary>
    internal sealed class PrintAreaLineThumb : Thumb
    {
        #region Закрытые поля

        /// <summary>
        /// Угол поворота области в радианах.
        /// </summary>
        private double angleInRadians;

        #endregion

        #region Открытые статические поля

        /// <summary>
        /// Толщина изменялки.
        /// </summary>
        public static DependencyProperty ThicknessProperty = DependencyProperty.Register("Thickness", typeof(double), typeof(PrintAreaLineThumb), new PropertyMetadata(0.0, new PropertyChangedCallback(ThicknessPropertyChanged)));

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PrintAreaLineThumb"/>.
        /// </summary>
        public PrintAreaLineThumb()
        {
            this.DragDelta += this.PrintAreaLineThumb_DragDelta;
            this.DragStarted += this.PrintAreaLineThumb_DragStarted;

            this.Loaded += this.PrintAreaLineThumb_Loaded;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает толщину линии-изменялки.
        /// </summary>
        public double Thickness
        {
            get
            {
                return (double)this.GetValue(ThicknessProperty);
            }
            set
            {
                this.SetValue(ThicknessProperty, value);
            }
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragDelta"/> линии-изменялки размеров прямоугольника.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void PrintAreaLineThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var printArea = (this.Parent as Grid).TemplatedParent as PrintArea;

            var coeff = printArea.Width / printArea.Height;

            var newWidth = 0.0;
            var newHeight = 0.0;

            var coeffX = 0.0;
            var coeffY = 0.0;

            if (this.HorizontalAlignment == HorizontalAlignment.Left)
            {
                newWidth = printArea.Width - e.HorizontalChange;
                newHeight = newWidth / coeff;

                coeffX = 1;
                coeffY = 0.5;
            }

            if (this.HorizontalAlignment == HorizontalAlignment.Right)
            {
                newWidth = printArea.Width + e.HorizontalChange;
                newHeight = newWidth / coeff;

                coeffX = 0;
                coeffY = 0.5;
            }

            if (this.VerticalAlignment == VerticalAlignment.Top)
            {
                newHeight = printArea.Height - e.VerticalChange;
                newWidth = newHeight * coeff;

                coeffX = 0.5;
                coeffY = 1;
            }

            if (this.VerticalAlignment == VerticalAlignment.Bottom)
            {
                newHeight = printArea.Height + e.VerticalChange;
                newWidth = newHeight * coeff;

                coeffX = 0.5;
                coeffY = 0;
            }

            if (newWidth < 100 || newHeight < 100)
                return;

            if (printArea.CustomAngle == 0)
            {
                printArea.Position = new Point(printArea.Position.X - (newWidth - printArea.Width) * coeffX, printArea.Position.Y - (newHeight - printArea.Height) * coeffY);

                printArea.Width = newWidth;
                printArea.Height = newHeight;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragStarted"/> линии-изменялки размеров прямоугольника.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void PrintAreaLineThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            var printArea = (this.Parent as Grid).TemplatedParent as PrintArea;

            this.angleInRadians = printArea.CustomAngle * Math.PI / 180.0;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="FrameworkElement.Loaded"/> линии-изменялки размеров прямоугольника.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void PrintAreaLineThumb_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= this.PrintAreaLineThumb_Loaded;

            this.Thickness = ((this.Parent as Grid).TemplatedParent as PrintArea).Thickness.Right;
        }

        #endregion

        #region Статические обработчики событий

        /// <summary>
        /// Обрабатывает изменение свойства <see cref="ThicknessProperty"/> линии-изменялки размеров прямоугольника.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="e">Аргументы.</param>
        private static void ThicknessPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var thumb = source as PrintAreaLineThumb;

            var thickness = (double)e.NewValue;

            var parent = thumb.Parent as FrameworkElement;

            if (thumb.HorizontalAlignment == HorizontalAlignment.Left || thumb.HorizontalAlignment == HorizontalAlignment.Right)
                thumb.Width = thickness;

            if (thumb.VerticalAlignment == VerticalAlignment.Top || thumb.VerticalAlignment == VerticalAlignment.Bottom)
                thumb.Height = thickness;
        }

        #endregion
    }
}