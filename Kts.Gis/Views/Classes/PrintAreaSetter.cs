using System;
using System.Windows;
using System.Windows.Controls;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет установщик области печати.
    /// </summary>
    internal sealed partial class PrintAreaSetter : Control, IDrawableObject, IMapObject
    {
        #region Закрытые поля
    
        /// <summary>
        /// Положение левого верхнего угла установщика области печати.
        /// </summary>
        private Point leftTopCorner;

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает положение левого верхнего угла установщика области печати.
        /// </summary>
        public Point LeftTopCorner
        {
            get
            {
                return this.leftTopCorner;
            }
            set
            {
                this.leftTopCorner = value;

                System.Windows.Controls.Canvas.SetLeft(this, value.X);
                System.Windows.Controls.Canvas.SetTop(this, value.Y);
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Задает область области печати.
        /// </summary>
        /// <param name="printArea">Область печати.</param>
        public void SetArea(PrintArea printArea)
        {
            printArea.SetArea(this.LeftTopCorner, new Size(this.ActualWidth, this.ActualHeight));
        }

        #endregion
    }

    // Реализация IDrawableObject.
    internal sealed partial class PrintAreaSetter
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет рисование объекта.
        /// </summary>
        /// <param name="mousePrevPosition">Предыдущее положение мыши.</param>
        /// <param name="mousePosition">Положение мыши.</param>
        public void Draw(Point mousePrevPosition, Point mousePosition)
        {
            if (mousePrevPosition.X > mousePosition.X && mousePrevPosition.Y > mousePosition.Y)
                this.LeftTopCorner = new Point(mousePosition.X, mousePosition.Y);
            else
                if (mousePrevPosition.X > mousePosition.X)
                    this.LeftTopCorner = new Point(mousePosition.X, this.LeftTopCorner.Y);
                else
                    if (mousePrevPosition.Y > mousePosition.Y)
                        this.LeftTopCorner = new Point(this.LeftTopCorner.X, mousePosition.Y);

            this.Width = Math.Abs(mousePosition.X - mousePrevPosition.X);
            this.Height = Math.Abs(mousePosition.Y - mousePrevPosition.Y);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что завершено ли рисование объекта.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        /// <returns>Значение, указывающее на то, что завершено ли рисование объекта.</returns>
        public bool IsDrawCompleted(Point mousePosition)
        {
            return true;
        }

        #endregion
    }

    // Реализация IMapObject.
    internal sealed partial class PrintAreaSetter
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает холст, на котором расположен объект.
        /// </summary>
        public IndentableCanvas Canvas
        {
            get;
            private set;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Добавляет объект на холст.
        /// </summary>
        /// <param name="canvas">Холст.</param>
        /// <returns>true, если объект был добавлен, иначе - false.</returns>
        public bool AddToCanvas(IndentableCanvas canvas)
        {
            if (this.Canvas != null)
                return false;

            this.Canvas = canvas;

            this.Canvas.Children.Add(this);

            return true;
        }

        /// <summary>
        /// Удаляет объект с холста.
        /// </summary>
        /// <returns>true, если объект был удален, иначе - false.</returns>
        public bool RemoveFromCanvas()
        {
            if (this.Canvas == null)
                return false;

            this.Canvas.Children.Remove(this);

            this.Canvas = null;

            return true;
        }

        #endregion
    }
}