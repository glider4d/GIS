using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет изменялку размеров области печати.
    /// </summary>
    internal sealed class PrintAreaResizeThumb : Thumb
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
        public static DependencyProperty ThicknessProperty = DependencyProperty.Register("Thickness", typeof(double), typeof(PrintAreaResizeThumb), new PropertyMetadata(0.0, new PropertyChangedCallback(ThicknessPropertyChanged)));

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PrintAreaResizeThumb"/>.
        /// </summary>
        public PrintAreaResizeThumb()
        {
            this.DragDelta += this.PrintAreaResizeThumb_DragDelta;
            this.DragStarted += this.PrintAreaResizeThumb_DragStarted;

            this.Loaded += this.PrintAreaResizeThumb_Loaded;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает толщину изменялки.
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
        /// Обрабатывает событие <see cref="Thumb.DragDelta"/> изменялки размеров прямоугольника.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void PrintAreaResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var printArea = (this.Parent as Grid).TemplatedParent as PrintArea;

            var coeff = printArea.Width / printArea.Height;

            var newWidth = 0.0;
            var newHeight = 0.0;
            
            switch (this.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    newWidth = printArea.Width - e.HorizontalChange;
                    newHeight = newWidth / coeff;

                    break;

                case HorizontalAlignment.Right:
                    newWidth = printArea.Width + e.HorizontalChange;
                    newHeight = newWidth / coeff;

                    break;
            }

            if (newWidth < 100 || newHeight < 100)
                return;

            var deltaHorizontal = printArea.Width - newWidth;
            var deltaVertical = printArea.Height - newHeight;

            var p = printArea.Position;

            switch (this.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    printArea.Position = new Point(p.X + deltaVertical * Math.Sin(-this.angleInRadians) - printArea.RenderTransformOrigin.Y * deltaVertical * Math.Sin(-this.angleInRadians), p.Y + deltaVertical * Math.Cos(-this.angleInRadians) + printArea.RenderTransformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angleInRadians)));

                    break;

                case VerticalAlignment.Bottom:
                    printArea.Position = new Point(p.X - deltaVertical * printArea.RenderTransformOrigin.Y * Math.Sin(-this.angleInRadians), p.Y + printArea.RenderTransformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angleInRadians)));

                    break;
            }

            p = printArea.Position;

            switch (this.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    printArea.Position = new Point(p.X + deltaHorizontal * Math.Cos(this.angleInRadians) + printArea.RenderTransformOrigin.X * deltaHorizontal * (1 - Math.Cos(this.angleInRadians)), p.Y + deltaHorizontal * Math.Sin(this.angleInRadians) - printArea.RenderTransformOrigin.X * deltaHorizontal * Math.Sin(this.angleInRadians));

                    break;

                case HorizontalAlignment.Right:
                    printArea.Position = new Point(p.X + deltaHorizontal * printArea.RenderTransformOrigin.X * (1 - Math.Cos(this.angleInRadians)), p.Y - printArea.RenderTransformOrigin.X * deltaHorizontal * Math.Sin(this.angleInRadians));

                    break;
            }

            printArea.Width = newWidth;
            printArea.Height = newHeight;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragStarted"/> изменялки размеров прямоугольника.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void PrintAreaResizeThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            var printArea = (this.Parent as Grid).TemplatedParent as PrintArea;
            
            this.angleInRadians = printArea.CustomAngle * Math.PI / 180.0;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="FrameworkElement.Loaded"/> изменялки размеров прямоугольника.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void PrintAreaResizeThumb_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= this.PrintAreaResizeThumb_Loaded;

            this.Thickness = ((this.Parent as Grid).TemplatedParent as PrintArea).Thickness.Right;
        }

        #endregion

        #region Статические обработчики событий

        /// <summary>
        /// Обрабатывает изменение свойства <see cref="ThicknessProperty"/> изменялки размеров прямоугольника.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="e">Аргументы.</param>
        private static void ThicknessPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var thumb = source as PrintAreaResizeThumb;

            var thickness = (double)e.NewValue;

            thumb.Width = thickness * 5;
            thumb.Height = thumb.Width;

            var margin = -thumb.Width / 2;

            switch (thumb.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    if (thumb.VerticalAlignment == VerticalAlignment.Top)
                        thumb.Margin = new Thickness(margin, margin, 0, 0);
                    
                    if (thumb.VerticalAlignment == VerticalAlignment.Bottom)
                        thumb.Margin = new Thickness(margin, 0, 0, margin);

                    break;

                case HorizontalAlignment.Right:
                    if (thumb.VerticalAlignment == VerticalAlignment.Top)
                        thumb.Margin = new Thickness(0, margin, margin, 0);

                    if (thumb.VerticalAlignment == VerticalAlignment.Bottom)
                        thumb.Margin = new Thickness(0, 0, margin, margin);

                    break;
            }
        }

        #endregion
    }
}