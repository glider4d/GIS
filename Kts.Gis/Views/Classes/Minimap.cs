using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет миникарту.
    /// </summary>
    internal sealed partial class Minimap : Control
    {
        #region Закрытые поля

        /// <summary>
        /// Бордер, представляющий границы миникарты.
        /// </summary>
        private Border border;

        /// <summary>
        /// Изображение миникарты.
        /// </summary>
        private Image image;

        /// <summary>
        /// Значение, указывающее на то, что начато ли перемещение по миникарте.
        /// </summary>
        private bool isMoveStarted;

        /// <summary>
        /// Прямоугольник, указывающий на текущее положение внутри миникарты.
        /// </summary>
        private Rectangle rect;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Карта.
        /// </summary>
        private readonly Map map;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Minimap"/>.
        /// </summary>
        /// <param name="map">Карта.</param>
        public Minimap(Map map)
        {
            this.map = map;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает максимальный размер миникарты.
        /// </summary>
        public Size MaxSize
        {
            get;
            set;
        } = new Size(180, 180);

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.MouseEnter"/> бордера.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void border_MouseEnter(object sender, MouseEventArgs e)
        {
            var animation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromSeconds(0.3),
                From = this.border.Opacity,
                To = 1
            };

            this.border.BeginAnimation(OpacityProperty, animation);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.MouseLeave"/> бордера.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void border_MouseLeave(object sender, MouseEventArgs e)
        {
            var animation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromSeconds(0.3),
                From = this.border.Opacity,
                To = 0.5
            };

            this.border.BeginAnimation(OpacityProperty, animation);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseLeftButtonDown"/> бордера.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!this.map.CanSelect && !this.map.CanEdit)
                return;

            this.MoveView(e.GetPosition(this.border));

            this.isMoveStarted = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseLeftButtonUp"/> бордера.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void border_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.isMoveStarted = false;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseMove"/> бордера.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void border_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!this.isMoveStarted)
                return;

            this.MoveView(e.GetPosition(this.border));
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Передвигает вид карты.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        private void MoveView(Point mousePosition)
        {
            var widthCoeff = this.map.SubstrateSize.Width / this.border.Width;
            var heightCoeff = this.map.SubstrateSize.Height / this.border.Height;

            var position = mousePosition;

            var x = position.X * widthCoeff;
            var y = position.Y * heightCoeff;

            this.map.ScrollToAndCenter(new Point(x, y));
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Подгоняет размер миникарты под максимальный.
        /// </summary>
        public void FitToMaxSize()
        {
            // Определяем размер бордера, исходя из размера подложки карты.
            var mapSize = map.SubstrateSize;

            if (mapSize.Width > mapSize.Height)
            {
                var coeff = mapSize.Width / mapSize.Height;

                this.border.Width = this.MaxSize.Width;
                this.border.Height = this.MaxSize.Width / coeff;
            }
            else
            {
                var coeff = mapSize.Height / mapSize.Width;

                this.border.Width = this.MaxSize.Height / coeff;
                this.border.Height = this.MaxSize.Height;
            }
        }

        /// <summary>
        /// Загружает изображение миникарты.
        /// </summary>
        /// <param name="path">Путь к изображению.</param>
        public void LoadImage(string path)
        {
            // Загружаем изображение.
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
            bitmapImage.UriSource = new Uri(path);
            bitmapImage.EndInit();

            if (bitmapImage.CanFreeze)
                bitmapImage.Freeze();

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.image.Source = bitmapImage;
            }));
        }

        /// <summary>
        /// Обновляет положение внутри миникарты.
        /// </summary>
        public void UpdateLocation()
        {
            if (double.IsNaN(this.border.Width) || double.IsNaN(this.border.Height))
                return;

            var viewSize = this.map.MapViewSize;

            var widthCoeff = this.map.SubstrateSize.Width / this.border.Width;
            var heightCoeff = this.map.SubstrateSize.Height / this.border.Height;

            // Вычисляем положение прямоугольника на карте.
            Canvas.SetLeft(this.rect, this.map.MapHorizontalOffset / this.map.Scale / widthCoeff);
            Canvas.SetTop(this.rect, this.map.MapVerticalOffset / this.map.Scale / heightCoeff);

            // Определяем размеры прямоугольника. Также отнимаем размер границы прямоугольника.
            var borderWidth = this.rect.StrokeThickness * 2;
            var newWidth = this.map.VisibleSize.Width / widthCoeff - borderWidth;
            var newHeight = this.map.VisibleSize.Height / heightCoeff - borderWidth;
            newWidth = newWidth > 0 && newWidth > borderWidth ? newWidth : borderWidth;
            newHeight = newHeight > 0 && newHeight > borderWidth ? newHeight : borderWidth;
            this.rect.Width = newWidth > this.border.ActualWidth ? this.border.ActualWidth : newWidth;
            this.rect.Height = newHeight > this.border.ActualHeight ? this.border.ActualHeight : newHeight;
        }

        #endregion
    }

    // Реализация Control.
    internal sealed partial class Minimap
    {
        #region Открытые переопределенные методы

        /// <summary>
        /// Выполняется при применении шаблона.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.border = this.GetTemplateChild("border") as Border;
            this.image = this.GetTemplateChild("image") as Image;
            this.rect = this.GetTemplateChild("rect") as Rectangle;

            this.border.MouseEnter += this.border_MouseEnter;
            this.border.MouseLeave += this.border_MouseLeave;
            this.border.PreviewMouseLeftButtonDown += this.border_PreviewMouseLeftButtonDown;
            this.border.PreviewMouseMove += this.border_PreviewMouseMove;
            this.border.PreviewMouseLeftButtonUp += this.border_PreviewMouseLeftButtonUp;
        }

        #endregion
    }
}