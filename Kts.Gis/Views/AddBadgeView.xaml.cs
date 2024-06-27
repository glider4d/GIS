using Kts.Gis.Services;
using Kts.Gis.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление добавления объекта, представляемого значком на карте.
    /// </summary>
    internal sealed partial class AddBadgeView : Window
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис привязки представлений карты с моделями представлений.
        /// </summary>
        private readonly IMapBindingService mapBindingService;

        /// <summary>
        /// Модель представления.
        /// </summary>
        private readonly AddBadgeViewModel viewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AddBadgeView"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        public AddBadgeView(AddBadgeViewModel viewModel, IMapBindingService mapBindingService)
        {
            this.InitializeComponent();

            this.viewModel = viewModel;
            this.mapBindingService = mapBindingService;

            this.DataContext = this.viewModel;

            this.viewModel.CloseRequested += this.ViewModel_CloseRequested;
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
            this.DrawLine();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.GotFocus"/> поля ввода значения.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() => this.textBox.SelectAll()), null);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="AddBadgeViewModel.CloseRequested"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_CloseRequested(object sender, System.EventArgs e)
        {
            this.DialogResult = true;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Рисует линию и выделяет ее начальную точку.
        /// </summary>
        private void DrawLine()
        {
            // Получаем кисть будущего полилайна.
            var brush = this.mapBindingService.GetBrush(this.viewModel.Line.Type.Color);

            // Получаем точки.
            var points = this.viewModel.Line.GetAllPoints();

            // Находим минимальные X и Y.
            var minX = this.GetMinXPoint(points);
            var minY = this.GetMinYPoint(points);

            // Сдвигаем все точки.
            for (int i = 0; i < points.Count; i++)
                points[i] = new Point(points[i].X - minX, points[i].Y - minY);

            // Вычисляем коэффициенты, на которые будем уменьшать точки.
            var coeff = 0.0;
            var maxX = this.GetMaxXPoint(points);
            if (this.grid.ActualWidth < maxX)
            {
                coeff = maxX / this.grid.ActualWidth;

                for (int i = 0; i < points.Count; i++)
                    points[i] = new Point(points[i].X / coeff, points[i].Y / coeff);
            }
            var maxY = this.GetMaxYPoint(points);
            if (this.grid.ActualHeight < maxY)
            {
                coeff = maxY / this.grid.ActualHeight;

                for (int i = 0; i < points.Count; i++)
                    points[i] = new Point(points[i].X / coeff, points[i].Y / coeff);
            }
                
            // Задаем размер холста.
            this.canvas.Width = this.GetMaxXPoint(points);
            this.canvas.Height = this.GetMaxYPoint(points);

            // Создаем полилайн и добавляем его на холст.
            var polyline = new System.Windows.Shapes.Polyline()
            {
                Stroke = brush,
                StrokeEndLineCap = PenLineCap.Round,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeThickness = 4
            };
            polyline.Points = new PointCollection(points);
            this.canvas.Children.Add(polyline);

            // Выделяем начальную точку полилайна.
            var ellipse = new Ellipse()
            {
                Fill = new SolidColorBrush(brush.Color),
                Height = 12,
                Width = 12,
            };
            Canvas.SetLeft(ellipse, polyline.Points[0].X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, polyline.Points[0].Y - ellipse.Height / 2);
            this.canvas.Children.Add(ellipse);

            // Добавляем анимацию выделенной точке.
            var animation = new ColorAnimation()
            {
                Duration = TimeSpan.FromSeconds(1),
                From = brush.Color,
                To = Colors.Transparent,
                RepeatBehavior = RepeatBehavior.Forever
            };
            ellipse.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }

        /// <summary>
        /// Возвращает максимальный X в заданном перечислении точек.
        /// </summary>
        /// <param name="points">Точки.</param>
        /// <returns>Максимальный X.</returns>
        private double GetMaxXPoint(IEnumerable<Point> points)
        {
            var max = double.MinValue;

            foreach (var point in points)
                if (point.X > max)
                    max = point.X;

            return max;
        }

        /// <summary>
        /// Возвращает максимальный Y в заданном перечислении точек.
        /// </summary>
        /// <param name="points">Точки.</param>
        /// <returns>Максимальный Y.</returns>
        private double GetMaxYPoint(IEnumerable<Point> points)
        {
            var max = double.MinValue;

            foreach (var point in points)
                if (point.Y > max)
                    max = point.Y;

            return max;
        }

        /// <summary>
        /// Возвращает минимальный X в заданном перечислении точек.
        /// </summary>
        /// <param name="points">Точки.</param>
        /// <returns>Минимальный Y.</returns>
        private double GetMinXPoint(IEnumerable<Point> points)
        {
            var min = double.MaxValue;

            foreach (var point in points)
                if (point.X < min)
                    min = point.X;

            return min;
        }

        /// <summary>
        /// Возвращает минимальный Y в заданном перечислении точек.
        /// </summary>
        /// <param name="points">Точки.</param>
        /// <returns>Минимальный Y.</returns>
        private double GetMinYPoint(IEnumerable<Point> points)
        {
            var min = double.MaxValue;

            foreach (var point in points)
                if (point.Y < min)
                    min = point.Y;

            return min;
        }

        #endregion
    }
}