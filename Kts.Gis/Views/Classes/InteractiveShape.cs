using Kts.Gis.Services;
using Kts.Gis.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет интерактивную фигуру.
    /// </summary>
    [Serializable]
    internal abstract partial class InteractiveShape : IEditableObject, IInteractiveShape
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что редактируется ли фигура.
        /// </summary>
        private bool isEditing;

        /// <summary>
        /// Значение, указывающее на то, что инициализирована ли фигура.
        /// </summary>
        private bool isInitialized;

        /// <summary>
        /// Значение, указывающее на то, что перемещается ли фигура.
        /// </summary>
        private bool isMoving;

        /// <summary>
        /// Значение, указывающее на то, что добавлена ли фигура на холст.
        /// </summary>
        private bool isOnCanvas;

        /// <summary>
        /// Значение, указывающее на то, что выбрана ли фигура.
        /// </summary>
        private bool isSelected;

        /// <summary>
        /// Предыдущее положение мыши.
        /// </summary>
        private Point mousePrevPosition;

        /// <summary>
        /// Предыдущий индекс фигуры по Z.
        /// </summary>
        private int? prevZIndex = null;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="InteractiveShape"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        /// <param name="shape">Фигура.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        public InteractiveShape(IObjectViewModel viewModel, Shape shape, IMapBindingService mapBindingService)
        {
            this.ViewModel = viewModel;
            this.Shape = shape;
            this.MapBindingService = mapBindingService;
        }

        #endregion

        #region Защищенные свойства

        /// <summary>
        /// Возвращает сервис привязки представлений карты с моделями представлений.
        /// </summary>
        protected IMapBindingService MapBindingService
        {
            get;
        }

        /// <summary>
        /// Возвращает фигуру.
        /// </summary>
        protected Shape Shape
        {
            get;
        }

        #endregion

        #region Защищенные виртуальные свойства

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли редактировать фигуру.
        /// </summary>
        protected virtual bool CanBeEdited
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выбрать фигуру.
        /// </summary>
        protected virtual bool CanBeSelected
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Открытые свойства
        
        /// <summary>
        /// Возвращает модель представления фигуры.
        /// </summary>
        public IObjectViewModel ViewModel
        {
            get;
        }

        #endregion

        #region Открытые виртуальные свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что может ли фигура быть выделенной.
        /// </summary>
        public virtual bool CanBeHighlighted
        {
            get;
            set;
        } = true;

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.MouseLeave"/> фигуры.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Shape_MouseLeave(object sender, MouseEventArgs e)
        {
            this.OnLeaveOrLeftButtonUp();

            e.Handled = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseLeftButtonDown"/> фигуры.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Shape_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift) || Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                return;

            this.IsEditing = this.Canvas.Map.CanEdit;
            this.IsSelected = this.Canvas.Map.CanSelect;

            if (this.IsEditing)
            {
                this.Shape.CaptureMouse();

                this.mousePrevPosition = e.GetPosition(this.Canvas);

                this.OnMovingStarted();

                this.isMoving = true;
            }
            else
                if (this.IsSelected && e.ClickCount == 2)
                    this.OnMouseDoubleClick(this.Canvas.Map.MousePosition);

            e.Handled = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseLeftButtonUp"/> фигуры.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Shape_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.OnLeaveOrLeftButtonUp();

            e.Handled = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseMove"/> фигуры.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Shape_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!this.isMoving)
                return;

            var p = e.GetPosition(this.Canvas);

            var deltaX = p.X - this.mousePrevPosition.X;
            var deltaY = p.Y - this.mousePrevPosition.Y;

            this.OnMoving(deltaX, deltaY);

            this.mousePrevPosition = p;

            e.Handled = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseRightButtonUp"/> фигуры.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Shape_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!this.Canvas.Map.EnableContextMenus)
            {
                e.Handled = true;

                return;
            }

            if (this.Canvas.Map.CanEdit && this.CanBeEdited)
            {
                var editable = this.ViewModel as IEditableObjectViewModel;

                if (editable != null)
                {
                    editable.IsEditing = true;

                    this.OnEditContextMenuRequested(this.Canvas.Map.MousePosition);
                }
            }
            else
                if (this.Canvas.Map.CanSelect && this.CanBeSelected)
                {
                    var selectable = this.ViewModel as ISelectableObjectViewModel;

                    if (selectable != null)
                    {
                        selectable.IsSelected = true;

                        this.OnContextMenuRequested(this.Canvas.Map.MousePosition);
                    }
                }

            e.Handled = true;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Отводит фигуру на задний план.
        /// </summary>
        private void BringToBack()
        {
            if (this.prevZIndex.HasValue)
            {
                this.Canvas.Map.BringToBack(this.Canvas);

                // Если предыдущий индекс фигуры по Z не пуст, то возвращаем ей обратно его значение.
                Panel.SetZIndex(this.Shape, this.prevZIndex.Value);

                this.prevZIndex = null;
            }
        }

        /// <summary>
        /// Выводит фигуру на передний план.
        /// </summary>
        private void BringToFront()
        {
            if (!this.prevZIndex.HasValue)
            {
                // Запоминаем индекс фигуры по Z.
                this.prevZIndex = Panel.GetZIndex(this.Shape);

                this.Canvas.Map.BringToFront(this.Canvas);

                // Задаем ей почти самый наибольший индекс, чтобы она была впереди всех.
                Panel.SetZIndex(this.Shape, int.MaxValue - 1);
            }
        }

        /// <summary>
        /// Выполняет действия, связанные с уходом мыши с фигуры или отжатии левой кнопки мыши.
        /// </summary>
        private void OnLeaveOrLeftButtonUp()
        {
            this.Shape.ReleaseMouseCapture();

            if (this.isMoving)
            {
                this.isMoving = false;

                this.OnMovingCompleted();
            }
        }

        #endregion

        #region Защищенные абстрактные методы

        /// <summary>
        /// Скрывает элементы управления свойствами фигуры.
        /// </summary>
        protected abstract void HideUI();

        /// <summary>
        /// Выполняет действия, связанные с запросом контекстного меню.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        protected abstract void OnContextMenuRequested(Point mousePosition);

        /// <summary>
        /// Выполняет действия, связанные с запросом контекстного меню редактирования.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        protected abstract void OnEditContextMenuRequested(Point mousePosition);

        /// <summary>
        /// Выполняет действия, связанные с двойным нажатием мыши по фигуре.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        protected abstract void OnMouseDoubleClick(Point mousePosition);

        /// <summary>
        /// Выполняет действия, связанные с перемещением фигуры.
        /// </summary>
        /// <param name="deltaX">Изменение положения фигуры по X.</param>
        /// <param name="deltaY">Изменение положения фигуры по Y.</param>
        protected abstract void OnMoving(double deltaX, double deltaY);

        /// <summary>
        /// Выполняет действия, связанные с завершением перемещения фигуры.
        /// </summary>
        protected abstract void OnMovingCompleted();

        /// <summary>
        /// Выполняет действия, связанные с началом перемещения фигуры.
        /// </summary>
        protected abstract void OnMovingStarted();

        /// <summary>
        /// Показывает элементы управления свойствами фигуры.
        /// </summary>
        protected abstract void ShowUI();

        #endregion

        #region Защищенные виртуальные методы

        /// <summary>
        /// Выполняет действия, связанные с инициализацией фигуры.
        /// </summary>
        protected virtual void OnInitialized()
        {
            // Сохраняем в теге фигуры ссылку на данный экземпляр класса.
            this.Shape.Tag = this;

            this.Shape.PreviewMouseLeftButtonDown += this.Shape_PreviewMouseLeftButtonDown;
            this.Shape.PreviewMouseRightButtonUp += this.Shape_PreviewMouseRightButtonUp;
        }

        #endregion

        #region Открытые абстрактные свойства
        
        /// <summary>
        /// Возвращает геометрию фигуры.
        /// </summary>
        /// <returns>Геометрия.</returns>
        public abstract Geometry GetGeometry();

        /// <summary>
        /// Возвращает кисть обводки фигуры.
        /// </summary>
        /// <returns>Кисть обводки.</returns>
        public abstract Pen GetStrokePen();

        #endregion
    }

    // Реализация IEditableObject.
    internal abstract partial class InteractiveShape
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что редактируется ли объект.
        /// </summary>
        public bool IsEditing
        {
            get
            {
                return this.isEditing;
            }
            set
            {
                if (!this.CanBeEdited)
                    return;

                var editable = this.ViewModel as IEditableObjectViewModel;

                if (editable == null || !editable.CanBeEdited)
                    return;

                if (this.IsEditing != value)
                {
                    this.isEditing = value;

                    if (value)
                    {
                        this.Shape.MouseLeave += this.Shape_MouseLeave;
                        this.Shape.PreviewMouseMove += this.Shape_PreviewMouseMove;
                        this.Shape.PreviewMouseLeftButtonUp += this.Shape_PreviewMouseLeftButtonUp;

                        // Выводим фигуру на передний план.
                        this.BringToFront();

                        // Показываем элементы управления свойствами фигуры.
                        this.ShowUI();

                        this.Shape.Cursor = Cursors.SizeAll;
                    }
                    else
                    {
                        if (this.isMoving)
                            this.OnLeaveOrLeftButtonUp();

                        this.Shape.MouseLeave -= this.Shape_MouseLeave;
                        this.Shape.PreviewMouseMove -= this.Shape_PreviewMouseMove;
                        this.Shape.PreviewMouseLeftButtonUp -= this.Shape_PreviewMouseLeftButtonUp;

                        // Отводим фигуру на задний план.
                        this.BringToBack();

                        // Скрываем элементы управления свойствами фигуры.
                        this.HideUI();

                        this.Shape.Cursor = this.Canvas.Cursor;
                    }

                    this.MapBindingService.SetMapObjectViewModelValue(this, nameof(this.IsEditing), value);
                }
            }
        }

        #endregion
    }

    // Реализация IInteractiveShape.
    internal abstract partial class InteractiveShape
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

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что инициализирована ли фигура.
        /// </summary>
        public bool IsInitialized
        {
            get
            {
                return this.isInitialized;
            }
            set
            {
                if (this.IsInitialized != value)
                {
                    this.isInitialized = value;

                    if (value)
                        this.OnInitialized();
                }
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что выбрана ли фигура.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                if (!this.CanBeSelected)
                    return;

                if (this.IsSelected != value)
                {
                    this.isSelected = value;

                    if (this.isOnCanvas)
                        if (value)
                            // Выводим фигуру на передний план.
                            this.BringToFront();
                        else
                            // Отводим фигуру на задний план.
                            this.BringToBack();

                    this.MapBindingService.SetMapObjectViewModelValue(this, nameof(this.IsSelected), value);
                }
            }
        }

        #endregion

        #region Открытые абстрактные свойства

        /// <summary>
        /// Возвращает центральную точку фигуры.
        /// </summary>
        public abstract Point CenterPoint
        {
            get;
        }

        /// <summary>
        /// Возвращает важную точку фигуры.
        /// </summary>
        public abstract Point MajorPoint
        {
            get;
        }

        #endregion

        #region Открытые виртуальные свойства

        /// <summary>
        /// Возвращает значение, указывающее на то, что видна ли фигура.
        /// </summary>
        public virtual bool IsVisible
        {
            get
            {
                return this.Canvas.Map.IsLayerVisible(this.ViewModel.Type);
            }
        }

        #endregion

        #region Открытые абстрактные методы

        /// <summary>
        /// Завершает перемещение фигуры. Используется для закрепления результата работы метода <see cref="MoveTo(Point)"/>.
        /// </summary>
        public abstract void EndMoveTo();

        /// <summary>
        /// Перемещает фигуру в заданную точку.
        /// </summary>
        /// <param name="point">Точка.</param>
        public abstract void MoveTo(Point point);

        /// <summary>
        /// Начинает перемещение фигуры.
        /// </summary>
        public abstract void StartMoveTo();

        #endregion

        #region Открытые виртуальные методы

        /// <summary>
        /// Добавляет объект на холст.
        /// </summary>
        /// <param name="canvas">Холст.</param>
        /// <returns>true, если объект был добавлен, иначе - false.</returns>
        public virtual bool AddToCanvas(IndentableCanvas canvas)
        {
            if (this.isOnCanvas)
                return false;

            this.Canvas = canvas;

            canvas.Children.Add(this.Shape);

            this.isOnCanvas = true;

            return true;
        }

        /// <summary>
        /// Удаляет объект с холста.
        /// </summary>
        /// <returns>true, если объект был удален, иначе - false.</returns>
        public virtual bool RemoveFromCanvas()
        {
            if (!this.isOnCanvas)
                return false;

            this.Canvas.Children.Remove(this.Shape);

            this.Canvas = null;

            this.isOnCanvas = false;

            return true;
        }

        #endregion
    }
}