using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет изменялку размеров фигуры.
    /// </summary>
    internal abstract class FigureResizeThumb : Thumb
    {
        #region Закрытые поля

        /// <summary>
        /// Положение изменялки размеров фигуры.
        /// </summary>
        private Point position;

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает угол поворота изменялки размеров фигуры.
        /// </summary>
        public double Angle
        {
            get
            {
                var rt = this.RenderTransform as RotateTransform;

                if (rt == null)
                {
                    this.RenderTransform = new RotateTransform(0);

                    return 0;
                }

                return rt.Angle;
            }
            set
            {
                var rt = this.RenderTransform as RotateTransform;

                if (rt == null)
                    this.RenderTransform = new RotateTransform(value);
                else
                    rt.Angle = value;
            }
        }

        /// <summary>
        /// Задает положение изменялки размеров фигуры.
        /// </summary>
        public Point Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
                
                Canvas.SetLeft(this, value.X - this.Width / 2);
                Canvas.SetTop(this, value.Y - this.Height / 2);
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Выводит изменялку размеров фигуры на передний план.
        /// </summary>
        public void BringToFront()
        {
            // Задаем ей самый наибольший индекс, чтобы она был впереди всех.
            Panel.SetZIndex(this, int.MaxValue);
        }

        #endregion
    }
}