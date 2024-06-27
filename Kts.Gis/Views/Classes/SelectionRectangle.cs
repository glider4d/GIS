using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет прямоугольную выделялку объектов.
    /// </summary>
    internal sealed partial class SelectionRectangle : Control, IDrawableObject, IMapObject
    {
        #region Закрытые поля

        /// <summary>
        /// Список фигур, которые попали в зону выделялки.
        /// </summary>
        private List<IInteractiveShape> hittedShapes;

        /// <summary>
        /// Значение, указывающее на то, что следует ли игнорировать значки.
        /// </summary>
        private bool ignoreBadges;

        /// <summary>
        /// Значение, указывающее на то, что следует ли игнорировать узлы.
        /// </summary>
        private bool ignoreNodes;

        /// <summary>
        /// Положение левого верхнего угла выделялки.
        /// </summary>
        private Point leftTopCorner;

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает положение левого верхнего угла выделялки.
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

        #region Закрытые методы

        /// <summary>
        /// Проверяет коллизию выделялки с объектом.
        /// </summary>
        /// <param name="result">Результат проверки.</param>
        /// <returns>Значение, указывающее на то, что следует ли продолжать поиск объектов.</returns>
        private HitTestResultBehavior HitTestCallback(HitTestResult result)
        {
            if (!(result.VisualHit is FrameworkElement))
                return HitTestResultBehavior.Continue;

            var tag = ((FrameworkElement)result.VisualHit).Tag;

            if (tag != null)
            {
                var obj = tag as IInteractiveShape;

                if (obj != null)
                {
                    var type = obj.GetType();

                    if (this.ignoreBadges && type == typeof(InteractiveBadge))
                        return HitTestResultBehavior.Continue;

                    if (this.ignoreNodes && type == typeof(InteractiveNode))
                        return HitTestResultBehavior.Continue;

                    if (obj.IsVisible)
                        // Не включаем в список фигур те фигуры, которые не видны.
                        this.hittedShapes.Add(obj);
                }
            }

            return HitTestResultBehavior.Continue;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Возвращает список фигур, которые попали в зону выделялки.
        /// </summary>
        /// <param name="ignoreNodes">Значение, указывающее на то, что следует ли игнорировать узлы.</param>
        /// <param name="ignoreBadges">Значение, указывающее на то, что следует ли игнорировать значки.</param>
        /// <returns>Список фигур.</returns>
        public List<IInteractiveShape> GetHittedShapes(bool ignoreNodes = true, bool ignoreBadges = true)
        {
            this.ignoreNodes = ignoreNodes;
            this.ignoreBadges = ignoreBadges;

            var rg = new RectangleGeometry(new Rect(this.LeftTopCorner, new Size(this.ActualWidth != 0 ? this.ActualWidth : 1, this.ActualHeight != 0 ? this.ActualHeight : 1)));

            this.hittedShapes = new List<IInteractiveShape>();

            VisualTreeHelper.HitTest(this.Canvas.Parent as Visual, null, new HitTestResultCallback(this.HitTestCallback), new GeometryHitTestParameters(rg));

            return this.hittedShapes;
        }

        #endregion
    }

    // Реализация IDrawableObject.
    internal sealed partial class SelectionRectangle
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
    internal sealed partial class SelectionRectangle
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