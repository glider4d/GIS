using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет умную надпись, которая автоматически определяет свое положение относительно родителя-фигуры.
    /// </summary>
    internal abstract partial class SmartLabel : TextBlock, IMapObject
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что выполняется ли перемещение надписи.
        /// </summary>
        private bool isMoving;

        /// <summary>
        /// Значение, указывающее на то, что добавлена ли надпись на холст.
        /// </summary>
        private bool isOnCanvas;

        /// <summary>
        /// Положение левого верхнего угла надписи.
        /// </summary>
        private Point leftTopCorner;

        /// <summary>
        /// Предыдущее положение мыши.
        /// </summary>
        private Point mousePrevPosition;

        /// <summary>
        /// Предыдущее положение надписи.
        /// </summary>
        private Point prevPosition;

        /// <summary>
        /// Вращалка надписи.
        /// </summary>
        private LabelRotateThumb rotateThumb;

        #endregion

        #region Закрытые статические поля

        /// <summary>
        /// Кисть выделенного состояния.
        /// </summary>
        private static SolidColorBrush highlightedBrush = new SolidColorBrush(Colors.Blue);

        /// <summary>
        /// Кисть нормального состояния.
        /// </summary>
        private static SolidColorBrush normalBrush = new SolidColorBrush(Colors.Black);

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SmartLabel"/>.
        /// </summary>
        /// <param name="savedAngle">Сохраненный угол поворота надписи.</param>
        /// <param name="savedPosition">Сохраненное положение надписи.</param>
        /// <param name="savedSize">Сохраненный размер надписи.</param>
        /// <param name="defaultSize">Размер надписи по умолчанию.</param>
        /// <param name="isReadOnly">Значение, указывающее на то, что является ли запись нередактируемой.</param>
        public SmartLabel(double? savedAngle, Point? savedPosition, int? savedSize, int defaultSize, bool isReadOnly)
        {
            if (savedAngle.HasValue)
            {
                this.HasManualAngle = true;

                this.Angle = savedAngle.Value;
            }

            if (savedPosition.HasValue)
            {
                this.HasManualPosition = true;

                this.LeftTopCorner = savedPosition.Value;
            }

            if (savedSize.HasValue)
                this.FontSize = savedSize.Value;
            else
                this.FontSize = defaultSize;

            if (!isReadOnly)
            {
                this.MouseLeave += this.SmartLabel_MouseLeave;
                this.PreviewMouseLeftButtonDown += this.SmartLabel_PreviewMouseLeftButtonDown;
                this.PreviewMouseLeftButtonUp += this.SmartLabel_PreviewMouseLeftButtonUp;
                this.PreviewMouseMove += this.SmartLabel_PreviewMouseMove;
                this.PreviewMouseRightButtonUp += this.SmartLabel_PreviewMouseRightButtonUp;
            }
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает угол поворота надписи.
        /// </summary>
        public double Angle
        {
            get
            {
                var rt = this.RenderTransform as RotateTransform;

                if (rt != null)
                    return rt.Angle;
                else
                    return 0;
            }
            set
            {
                var rt = this.RenderTransform as RotateTransform;

                if (rt != null)
                    rt.Angle = value;
                else
                    this.RenderTransform = new RotateTransform(value);
            }
        }

        /// <summary>
        /// Возвращает центральную точку надписи.
        /// </summary>
        public Point CenterPoint
        {
            get
            {
                return new Point(this.LeftTopCorner.X + this.ActualWidth / 2, this.LeftTopCorner.Y + this.ActualHeight / 2);
            }
        }

        /// <summary>
        /// Возвращает или задает положение левого верхнего угла надписи.
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

                if (this.rotateThumb != null)
                    this.rotateThumb.Position = this.LeftTopCorner;
            }
        }

        /// <summary>
        /// Возвращает или задает кисть нормального состояния.
        /// </summary>
        public Brush NormalBrush
        {
            get;
            protected set;
        }

        /// <summary>
        /// Возвращает или задает размер надписи.
        /// </summary>
        public int Size
        {
            get
            {
                return Convert.ToInt32(this.FontSize);
            }
            private set
            {
                if (value < this.MinSize)
                    value = this.MinSize;

                if (value > this.MaxSize)
                    value = this.MaxSize;

                this.FontSize = value;

                this.UpdateLayout();

                if (this.rotateThumb != null)
                {
                    this.rotateThumb.Diameter = this.DesiredSize.Width * 2 + 4;

                    this.rotateThumb.Position = this.LeftTopCorner;
                }
            }
        }

        #endregion

        #region Открытые абстрактные свойства

        /// <summary>
        /// Возвращает размер надписи по умолчанию.
        /// </summary>
        public abstract int DefaultSize
        {
            get;
        }

        #endregion

        #region Открытые виртуальные свойства

        /// <summary>
        /// Возвращает максимальный размер надписи.
        /// </summary>
        public virtual int MaxSize
        {
            get
            {
#warning Это надо будет брать из настроек вида карты
                return this.DefaultSize * 25;
            }
        }

        /// <summary>
        /// Возвращает минимальный размер надписи.
        /// </summary>
        public virtual int MinSize
        {
            get
            {
#warning Это надо будет брать из настроек вида карты
                return Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(this.DefaultSize / 2)));
            }
        }

        /// <summary>
        /// Возвращает значение, которое будет отниматься/прибавляться к размеру надписи.
        /// </summary>
        public virtual int SizeDelta
        {
            get
            {
#warning Это надо будет брать из настроек вида карты
                return 1;
            }
        }

        #endregion

        #region Защищенные свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что имеется ли вручную заданное значение угла поворота надписи.
        /// </summary>
        protected bool HasManualAngle
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что имеется ли вручную заданное значение положения надписи.
        /// </summary>
        protected bool HasManualPosition
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает начальное значение угла поворота надписи.
        /// </summary>
        protected double InitialAngle
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает начальное значение положения надписи.
        /// </summary>
        protected Point InitialPosition
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что готова ли полностью надпись.
        /// </summary>
        protected bool IsReady
        {
            get;
            private set;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.MouseLeave"/> надписи.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void RotateThumb_MouseLeave(object sender, MouseEventArgs e)
        {
            this.OnMouseLeaved();

            this.rotateThumb.MouseLeave -= this.RotateThumb_MouseLeave;

            this.Canvas.Children.Remove(this.rotateThumb);
            
            this.rotateThumb = null;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.MouseLeave"/> надписи.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void SmartLabel_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.NormalBrush == null)
                this.Foreground = normalBrush;
            else
                this.Foreground = this.NormalBrush;

            this.OnLeaveOrLeftButtonUp();

            e.Handled = true;

            if (this.rotateThumb != null && !this.rotateThumb.IsMouseOver)
            {
                this.rotateThumb.MouseLeave -= this.RotateThumb_MouseLeave;

                this.Canvas.Children.Remove(this.rotateThumb);

                this.rotateThumb = null;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseLeftButtonDown"/> надписи.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void SmartLabel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            if (!this.Canvas.Map.CanEdit)
                return;

            if (e.ClickCount == 2)
            {
                this.OnMouseDoubleClick();

                return;
            }

            this.OnMouseClick();
            
            this.CaptureMouse();

            this.prevPosition = this.LeftTopCorner;

            this.mousePrevPosition = e.GetPosition(this.Canvas);

            this.isMoving = true;

            if (this.rotateThumb == null)
            {
                // Отображаем вращалку.
                this.rotateThumb = new LabelRotateThumb(this, this.LeftTopCorner, this.ActualWidth * 2 + 4);
                this.Canvas.Children.Add(this.rotateThumb);

                // Отодвигаем надпись на передний план, чтобы была возможность ее передвинуть.
                Panel.SetZIndex(this, int.MaxValue);

                this.rotateThumb.MouseLeave += this.RotateThumb_MouseLeave;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseLeftButtonUp"/> надписи.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void SmartLabel_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.OnLeaveOrLeftButtonUp();

            e.Handled = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseMove"/> надписи.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void SmartLabel_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (this.Canvas.Map.CanEdit)
                this.Foreground = highlightedBrush;

            if (!this.isMoving)
                return;

            var p = e.GetPosition(this.Canvas);

            var deltaX = p.X - this.mousePrevPosition.X;
            var deltaY = p.Y - this.mousePrevPosition.Y;

            // Сдвигаем надпись.
            var point = this.LeftTopCorner;
            this.LeftTopCorner = new Point(point.X + deltaX, point.Y + deltaY);

            this.mousePrevPosition = p;

            e.Handled = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseRightButtonUp"/> надписи.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void SmartLabel_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!this.Canvas.Map.CanEdit)
                return;

            this.OnContextMenuRequested();

            e.Handled = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="FrameworkElement.SizeChanged"/> надписи.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void SmartLabel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.SizeChanged -= this.SmartLabel_SizeChanged;

            this.Relocate(new Point(0, 0));

            this.IsReady = true;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выполняет действия, связанные с уходом мыши с надписи или отжатии левой кнопки мыши.
        /// </summary>
        private void OnLeaveOrLeftButtonUp()
        {
            if (!this.isMoving)
                return;

            this.ReleaseMouseCapture();

            // Нужно запомнить изменение положения надписи.
            this.OnPositionChanged(this.prevPosition);

            this.isMoving = false;
        }

        #endregion

        #region Защищенные методы

        /// <summary>
        /// Возвращает допустимые значения размера надписи.
        /// </summary>
        /// <returns>Список значений размеров надписи.</returns>
        protected List<int> GetSizes()
        {
            var result = new List<int>();

            for (int i = this.MinSize; i <= MaxSize; i += this.SizeDelta)
                result.Add(i);

            return result;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли уменьшить размер шрифта надписи.
        /// </summary>
        /// <returns>true, если можно уменьшить, иначе - false.</returns>
        public bool CanDecreaseFontSize()
        {
            return this.Size > this.MinSize;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли увеличить размер шрифта надписи.
        /// </summary>
        /// <returns>true, если можно увеличить, иначе - false.</returns>
        public bool CanIncreaseFontSize()
        {
            return this.Size < this.MaxSize;
        }

        /// <summary>
        /// Уменьшает размер шрифта надписи.
        /// </summary>
        public void DecreaseFontSize()
        {
            if (!this.CanDecreaseFontSize())
                return;

            this.Size = this.GetDecreasedFontSize();

            this.OnSizeChanged();
        }

        /// <summary>
        /// Возвращает размер шрифта надписи, получаемый при его уменьшении.
        /// </summary>
        /// <returns>Размер шрифта.</returns>
        public int GetDecreasedFontSize()
        {
            return this.Size - this.SizeDelta;
        }

        /// <summary>
        /// Возвращает размер шрифта надписи, получаемый при его увеличении.
        /// </summary>
        /// <returns>Размер шрифта.</returns>
        public int GetIncreasedFontSize()
        {
            return this.Size + this.SizeDelta;
        }

        /// <summary>
        /// Увеличивает размер шрифта надписи.
        /// </summary>
        public void IncreaseFontSize()
        {
            if (!this.CanIncreaseFontSize())
                return;

            this.Size = this.GetIncreasedFontSize();

            this.OnSizeChanged();
        }

        /// <summary>
        /// Задает угол поворота надписи.
        /// </summary>
        /// <param name="angle">Угол.</param>
        public void SetAngle(double? angle)
        {
            if (angle.HasValue)
            {
                this.Angle = angle.Value;

                this.HasManualAngle = true;
            }
            else
            {
                this.HasManualAngle = false;

                this.Angle = this.InitialAngle;

                this.Relocate(new Point(0, 0));
            }
        }

        /// <summary>
        /// Задает положение надписи.
        /// </summary>
        /// <param name="position">Положение.</param>
        public void SetPosition(Point? position)
        {
            if (position.HasValue)
            {
                this.LeftTopCorner = position.Value;

                this.HasManualPosition = true;
            }
            else
            {
                this.HasManualPosition = false;

                this.LeftTopCorner = this.InitialPosition;

                this.Relocate(new Point(0, 0));
            }
        }

        /// <summary>
        /// Задает размер надписи.
        /// </summary>
        /// <param name="size">Размер.</param>
        public void SetSize(int? size)
        {
            if (size.HasValue)
                this.Size = size.Value;
            else
                this.Size = this.DefaultSize;
        }

        /// <summary>
        /// Задает текст надписи.
        /// </summary>
        /// <param name="text">Текст.</param>
        public void SetText(string text)
        {
            this.Text = text;

            if (!string.IsNullOrEmpty(text))
                if (!this.isOnCanvas)
                    this.AddToCanvas(this.Canvas);
				else
					this.SizeChanged += this.SmartLabel_SizeChanged;
            else
                this.RemoveFromCanvas();
        }

        #endregion

        #region Защищенные абстрактные методы

        /// <summary>
        /// Выполняет действия, связанные с изменением положения надписи.
        /// </summary>
        /// <param name="prevPosition">Предыдущее положение надписи.</param>
        protected abstract void OnPositionChanged(Point prevPosition);

        /// <summary>
        /// Выполняет действия, связанные с изменением размера надписи.
        /// </summary>
        protected abstract void OnSizeChanged();

        #endregion

        #region Защищенные виртуальные методы

        /// <summary>
        /// Выполняется при требовании контекстного меню.
        /// </summary>
        protected virtual void OnContextMenuRequested()
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Выполняется при нажатии левой кнопки мыши по надписи.
        /// </summary>
        protected virtual void OnMouseClick()
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Выполняется при двойном нажатии левой кнопки мыши по надписи.
        /// </summary>
        protected virtual void OnMouseDoubleClick()
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Выполняет действия, связанные с выходом курсора мыши с надписи.
        /// </summary>
        protected virtual void OnMouseLeaved()
        {
            // Ничего не делаем.
        }

        #endregion

        #region Открытые абстрактные методы

        /// <summary>
        /// Выполняет действия, связанные с изменением угла поворота надписи.
        /// </summary>
        /// <param name="prevAngle">Предыдущий угол поворота надписи.</param>
        public abstract void OnAngleChanged(double prevAngle);

        /// <summary>
        /// Переопределяет положение надписи.
        /// </summary>
        /// <param name="positionDelta">Разница между предыдущим и текущим положением фигуры.</param>
        public abstract void Relocate(Point positionDelta);

        #endregion
    }

    // Реализация IMapObject.
    internal abstract partial class SmartLabel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает холст, на котором расположен объект.
        /// </summary>
        public IndentableCanvas Canvas
        {
            get;
            set;
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
            if (this.isOnCanvas || this.Canvas == null || string.IsNullOrEmpty(this.Text))
                return false;

            this.Canvas.Children.Add(this);

            this.SizeChanged += this.SmartLabel_SizeChanged;

            this.isOnCanvas = true;

            return true;
		}

        /// <summary>
        /// Удаляет объект с холста.
        /// </summary>
        /// <returns>true, если объект был удален, иначе - false.</returns>
        public bool RemoveFromCanvas()
		{
            if (!this.isOnCanvas)
                return false;

            this.Canvas.Children.Remove(this);

            this.isOnCanvas = false;

            return true;
		}
		
		#endregion
	}
}