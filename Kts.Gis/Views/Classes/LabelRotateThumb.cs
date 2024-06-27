using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет вращалку надписи.
    /// </summary>
    internal sealed partial class LabelRotateThumb : Thumb
    {
        #region Закрытые поля

        /// <summary>
        /// Начальный угол поворота надписи.
        /// </summary>
        private double initialAngle;

        /// <summary>
        /// Начальный вектор, проведенный из центральной точки надписи до положения курсора.
        /// </summary>
        private Vector startVector;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Надпись.
        /// </summary>
        private readonly SmartLabel label;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LabelRotateThumb"/>.
        /// </summary>
        /// <param name="label">Надпись.</param>
        /// <param name="position">Положение вращалки на холсте.</param>
        /// <param name="diameter">Диаметр вращалки.</param>
        public LabelRotateThumb(SmartLabel label, Point position, double diameter)
        {
            this.label = label;

            // Задаем центральную точку трансформации.
            this.RenderTransformOrigin = this.label.RenderTransformOrigin;

            this.Diameter = diameter;

            this.Position = position;

            this.DragStarted += this.LabelRotateThumb_DragStarted;
            this.DragDelta += this.LabelRotateThumb_DragDelta;
            this.DragCompleted += this.LabelRotateThumb_DragCompleted;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Задает диаметр вращалки надписи.
        /// </summary>
        public double Diameter
        {
            set
            {
                this.Height = value;
                this.Width = value;
            }
        }

        /// <summary>
        /// Задает положение вращалки надписи.
        /// </summary>
        public Point Position
        {
            set
            {
                Canvas.SetLeft(this, value.X - this.Width / 2);
                Canvas.SetTop(this, value.Y - this.Height / 2);
            }
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragCompleted"/> вращалки надписи.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void LabelRotateThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            this.label.OnAngleChanged(this.initialAngle);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragDelta"/> вращалки надписи.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void LabelRotateThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var currentPoint = Mouse.GetPosition(this.label.Canvas.Parent as IInputElement);
            var deltaVector = Point.Subtract(currentPoint, this.label.CenterPoint);
            var angle = Vector.AngleBetween(this.startVector, deltaVector);

            this.label.Angle = this.initialAngle + angle;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragStarted"/> вращалки надписи.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void LabelRotateThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            // Получаем положение мыши относительно родителя холста.
            var startPoint = Mouse.GetPosition(this.label.Canvas.Parent as IInputElement);

            this.startVector = Point.Subtract(startPoint, this.label.CenterPoint);

            this.initialAngle = this.label.Angle;
        }

        #endregion
    }

#warning Заблокировано отображение кнопок уменьшения/увеличения размера надписи
    // Реализация Thumb.
    //internal sealed partial class LabelRotateThumb
    //{
    //    #region Открытые переопределенные методы

    //    /// <summary>
    //    /// Выполняется при применении шаблона.
    //    /// </summary>
    //    public override void OnApplyTemplate()
    //    {
    //        base.OnApplyTemplate();

    //        var grid = this.GetTemplateChild("grid") as Grid;

    //        // Добавляем кнопки увеличения/уменьшения размера шрифта.
    //        var upButton = new LabelResizeButton(this.label, true);
    //        var downButton = new LabelResizeButton(this.label, false);
    //        grid.Children.Add(upButton);
    //        grid.Children.Add(downButton);
    //    }

    //    #endregion
    //}
}