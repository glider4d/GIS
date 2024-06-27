using Kts.Gis.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление глобальной карты.
    /// </summary>
    internal sealed partial class GlobalMapView : UserControl
    {
        #region Закрытые константы

        /// <summary>
        /// Фактор масштабирования.
        /// </summary>
        private const double scaleFactor = 0.15;

        #endregion

        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что загружено ли представление.
        /// </summary>
        private bool isLoaded;

        /// <summary>
        /// Значение, указывающее на то, что перемещается ли карта.
        /// </summary>
        private bool isMoving;

        /// <summary>
        /// Предыдущее положение мыши.
        /// </summary>
        private Point mousePrevPosition;

        /// <summary>
        /// Текущий масштаб.
        /// </summary>
        private double scale = 1;

        /// <summary>
        /// Модель представления.
        /// </summary>
        private GlobalMapViewModel viewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="GlobalMapView"/>.
        /// </summary>
        public GlobalMapView()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает модель представления.
        /// </summary>
        public GlobalMapViewModel ViewModel
        {
            get
            {
                return this.viewModel;
            }
            set
            {
                this.viewModel = value;

                this.DataContext = value;
            }
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="FrameworkElement.Loaded"/> холста.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void canvas_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.isLoaded)
                return;

            this.isLoaded = true;

            Path path;

            double maxHeight = 0;
            double maxWidth = 0;

            bool isMinSetted = false;

            double minTop = 0;
            double minLeft = 0;

            double curHeight;
            double curWidth;
            /*
              for (int i = (ViewModel.VisualRegions.Count - 1); i >= 0; i--)
              {
                  var visualRegion = ViewModel.VisualRegions[i];

              }
              */
        
            foreach (var visualRegion in this.ViewModel.VisualRegions)
            {
                
                path = new Path();

                path.DataContext = visualRegion;

                this.canvas.Children.Add(path);
                
                var rect = path.RenderTransform.TransformBounds(path.Data.Bounds);
                
                

                curHeight = rect.Top + rect.Height;
                curWidth = rect.Left + rect.Width;

               

                if (!isMinSetted)
                {
                    minTop = curHeight;
                    minLeft = curWidth;

                    isMinSetted = true;
                }

                if (curHeight > maxHeight)
                    maxHeight = curHeight;
                if (curWidth > maxWidth)
                    maxWidth = curWidth;

                if (rect.Top < minTop)
                    minTop = rect.Top;
                if (rect.Left < minLeft)
                    minLeft = rect.Left;

                path.PreviewMouseLeftButtonDown += this.Path_PreviewMouseLeftButtonDown;
            }

            this.canvas.Height = maxHeight + minTop;
            this.canvas.Width = maxWidth + minLeft;

            this.canvas.UpdateLayout();
            this.scrollViewer.UpdateLayout();

            var deltaX = this.canvas.Height - this.scrollViewer.ViewportHeight;
            var deltaY = this.canvas.Width - this.scrollViewer.ViewportWidth;
            
            if (deltaX > 0 || deltaY > 0)
            {
                double delta;

                if (deltaX > deltaY)
                    delta = this.canvas.Height / this.scrollViewer.ViewportHeight;
                else
                    delta = this.canvas.Width / this.scrollViewer.ViewportWidth;

                this.scale = 0;

                while (this.scale < 1 / delta)
                    this.scale += scaleFactor;

                this.scale -= scaleFactor;

                this.scaleTransform.ScaleX = this.scale;
                this.scaleTransform.ScaleY = this.scale;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.MouseLeave"/> холста.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            this.isMoving = false;

            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseMove"/> холста.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!this.isMoving)
                return;

            this.viewModel.SelectedVisualRegion = null;

            var mousePosition = e.GetPosition(this.canvas);

            // Приходится уменьшать дельты, так как если не делать этого, то при перемещении вида карты при большом зум ауте, все подергивается. Вообще хер его знает, что это за магия такая.
            double deltaX = (this.mousePrevPosition.X - mousePosition.X) / 4;
            double deltaY = (this.mousePrevPosition.Y - mousePosition.Y) / 4;

            this.scrollViewer.ScrollToHorizontalOffset(this.scrollViewer.HorizontalOffset + deltaX);
            this.scrollViewer.ScrollToVerticalOffset(this.scrollViewer.VerticalOffset + deltaY);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseRightButtonDown"/> холста.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void canvas_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.mousePrevPosition = e.GetPosition(this.canvas);

            this.isMoving = true;

            this.Cursor = Cursors.SizeAll;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseRightButtonUp"/> холста.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void canvas_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.isMoving = false;

            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseLeftButtonDown"/> визуального региона.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Path_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.viewModel.SelectedVisualRegion = (sender as Path).DataContext as VisualRegionViewModel;

            e.Handled = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseLeftButtonDown"/> скроллвьювера.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void scrollViewer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.viewModel.SelectedVisualRegion = null;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseWheel"/> скроллвьювера.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void scrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Отменяем скроллинг, чтобы выполнять только зуминг.
            e.Handled = true;

            this.viewModel.SelectedVisualRegion = null;

            var a = e.GetPosition(this.canvas);
            var b = e.GetPosition(this.scrollViewer);

            if (e.Delta > 0)
                this.scale += scaleFactor;
            else
                // Ограничиваем зум аут. Пусть оно будет не меньше фактора масштабирования.
                if (this.scale - scaleFactor > scaleFactor)
                    this.scale -= scaleFactor;

            this.scaleTransform.ScaleX = this.scale;
            this.scaleTransform.ScaleY = this.scale;

            var newPosition = scaleTransform.Transform(a);

            this.scrollViewer.ScrollToHorizontalOffset(newPosition.X - b.X);
            this.scrollViewer.ScrollToVerticalOffset(newPosition.Y - b.Y);
        }

        #endregion
    }
}