using Kts.Gis.ViewModels;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kts.Gis.Views
{
    internal sealed partial class ApprovedHeader : Control, IMapObject
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что выполняется ли перемещение заголовка.
        /// </summary>
        private bool isMoving;

        /// <summary>
        /// Значение, указывающее на то, что добавлен ли заголовок на холст.
        /// </summary>
        private bool isOnCanvas;

        /// <summary>
        /// Предыдущее положение мыши.
        /// </summary>
        private Point mousePrevPosition;

        /// <summary>
        /// Положение заголовка на карте.
        /// </summary>
        private Point position;

        /// <summary>
        /// Предыдущее положение заголовка на карте.
        /// </summary>
        private Point prevPosition;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Модель представления.
        /// </summary>
        private readonly ApprovedHeaderViewModel viewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LengthPerDiameterTable"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public ApprovedHeader(ApprovedHeaderViewModel viewModel)
        {
            this.viewModel = viewModel;

            this.DataContext = viewModel;

            this.Position = viewModel.Position;

            this.viewModel.OnEdit += this.ViewModel_OnEdit;
            this.viewModel.PropertyChanged += this.ViewModel_PropertyChanged;

            this.MouseLeave += this.ApprovedHeader_MouseLeave;
            this.PreviewMouseLeftButtonDown += this.ApprovedHeader_PreviewMouseLeftButtonDown;
            this.PreviewMouseLeftButtonUp += this.ApprovedHeader_PreviewMouseLeftButtonUp;
            this.PreviewMouseMove += this.ApprovedHeader_PreviewMouseMove;
            this.PreviewMouseRightButtonUp += this.ApprovedHeader_PreviewMouseRightButtonUp;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает центральную точку заголовка.
        /// </summary>
        public Point CenterPoint
        {
            get
            {
                return new Point(this.Position.X + this.ActualWidth / 2, this.Position.Y + this.ActualHeight / 2);
            }
        }

        /// <summary>
        /// Возвращает или задает положение заголовка на карте.
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

                System.Windows.Controls.Canvas.SetLeft(this, value.X);
                System.Windows.Controls.Canvas.SetTop(this, value.Y);
            }
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="ContextMenu.Closed"/> контекстного меню.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            if (this.ContextMenu != null)
            {
                this.ContextMenu.Closed -= this.ContextMenu_Closed;

                this.ContextMenu = null;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.MouseLeave"/> заголовка.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ApprovedHeader_MouseLeave(object sender, MouseEventArgs e)
        {
            this.OnLeaveOrLeftButtonUp();

            e.Handled = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseLeftButtonDown"/> заголовка.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ApprovedHeader_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

            this.prevPosition = this.Position;

            this.mousePrevPosition = e.GetPosition(this.Canvas);

            this.isMoving = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseLeftButtonUp"/> заголовка.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ApprovedHeader_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.OnLeaveOrLeftButtonUp();

            e.Handled = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseMove"/> заголовка.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ApprovedHeader_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!this.isMoving)
                return;

            var p = e.GetPosition(this.Canvas);

            var deltaX = p.X - this.mousePrevPosition.X;
            var deltaY = p.Y - this.mousePrevPosition.Y;

            // Сдвигаем заголовок.
            var point = this.Position;
            this.Position = new Point(point.X + deltaX, point.Y + deltaY);

            this.mousePrevPosition = p;

            e.Handled = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseRightButtonUp"/> заголовка.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ApprovedHeader_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!this.Canvas.Map.CanEdit)
                return;

            this.OnContextMenuRequested();

            e.Handled = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="ApprovedHeaderViewModel.OnEdit"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_OnEdit(object sender, EventArgs e)
        {
            var view = new ApprovedHeaderEditView(new ApprovedHeaderEditViewModel(this.viewModel))
            {
                Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive)
            };

            view.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="INotifyPropertyChanged.PropertyChanged"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ApprovedHeaderViewModel.Position):
                    this.Position = this.viewModel.Position;

                    break;
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выполняется при требовании контекстного меню.
        /// </summary>
        private void OnContextMenuRequested()
        {
            (this.viewModel as IEditableObjectViewModel).IsEditing = true;

            this.ContextMenu = Application.Current.Resources["ApprovedHeaderEditContextMenu"] as ContextMenu;

            // Задаем контекст данных, предварительно обнуляя его.
            this.ContextMenu.DataContext = null;
            this.ContextMenu.DataContext = this.viewModel;

            this.ContextMenu.Closed += this.ContextMenu_Closed;

            this.ContextMenu.IsOpen = true;
        }

        /// <summary>
        /// Выполняет действия, связанные с уходом мыши с таблицы или отжатии левой кнопки мыши.
        /// </summary>
        private void OnLeaveOrLeftButtonUp()
        {
            if (!this.isMoving)
                return;

            this.ReleaseMouseCapture();

            // Нужно запомнить изменение положения заголовка.
            this.OnPositionChanged(this.prevPosition);

            this.isMoving = false;
        }

        /// <summary>
        /// Выполняется при нажатии левой кнопки мыши по заголовку.
        /// </summary>
        private void OnMouseClick()
        {
            (this.viewModel as IEditableObjectViewModel).IsEditing = true;
        }

        /// <summary>
        /// Выполняется при двойном нажатии левой кнопки мыши по заголовку.
        /// </summary>
        private void OnMouseDoubleClick()
        {
            if (this.viewModel.EditCommand.CanExecute(null))
                this.viewModel.EditCommand.Execute(null);
        }

        /// <summary>
        /// Выполняет действия, связанные с изменением положения заголовка.
        /// </summary>
        /// <param name="prevPosition">Предыдущее положение заголовка.</param>
        private void OnPositionChanged(Point prevPosition)
        {
            this.viewModel.Position = this.position;
        }

        #endregion
    }

    // Реализация IMapObject.
    internal sealed partial class ApprovedHeader
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
            if (this.isOnCanvas || this.Canvas == null)
                return false;

            this.Canvas.Children.Add(this);

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