using Kts.Gis.Services;
using Kts.Gis.ViewModels;
using Kts.WpfUtilities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет интерактивный значок.
    /// </summary>
    [Serializable]
    internal sealed partial class InteractiveBadge : InteractiveShape
    {
        #region Закрытые поля

        /// <summary>
        /// Положение значка на карте.
        /// </summary>
        private Point leftTopCorner;

        /// <summary>
        /// Трансформация поворота значка.
        /// </summary>
        private RotateTransform rotateTransform = new RotateTransform();

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Главная точка значка.
        /// </summary>
        private readonly Point hotPoint;

        /// <summary>
        /// Точка поворота значка.
        /// </summary>
        private readonly Point originPoint;

        /// <summary>
        /// Путь, представляющий значок.
        /// </summary>
        private readonly Path path;

        #endregion

        #region Закрытые статические поля

        /// <summary>
        /// Кисть границы значка.
        /// </summary>
        private static SolidColorBrush strokeBrush = new SolidColorBrush(Color.FromRgb(52, 52, 52));

        /// <summary>
        /// Кисть границы значка при его выделении.
        /// </summary>
        private static Pen strokePen;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="InteractiveBadge"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        /// <param name="hotPoint">Главная точка значка.</param>
        /// <param name="originPoint">Точка поворота значка.</param>
        /// <param name="badgeGeometry">Геометрия значка.</param>
        /// <param name="scaleTransform">Трансформация масштабирования.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        public InteractiveBadge(BadgeViewModel viewModel, Point hotPoint, Point originPoint, Geometry badgeGeometry, ScaleTransform scaleTransform, IMapBindingService mapBindingService) : base(viewModel, new Path(), mapBindingService)
        {
            this.path = this.Shape as Path;

            this.hotPoint = hotPoint;
            this.originPoint = originPoint;
            this.path.Data = badgeGeometry;

            this.path.Stroke = strokeBrush;
            this.path.Fill = strokeBrush;

            this.path.RenderTransformOrigin = new Point(this.originPoint.X, this.originPoint.Y);

            // Добавляем трансформации.
            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(scaleTransform);
            transformGroup.Children.Add(this.rotateTransform);
            this.path.RenderTransform = transformGroup;

            this.RelocateBadge();

            // Добавляем значок линии, чтобы та могла обновлять положение значка.
            (this.MapBindingService.GetMapObjectView(viewModel.Parent as LineViewModel) as InteractiveLine).Badges.Add(this);
        }

        #endregion

        #region Статические конструкторы

        /// <summary>
        /// Инициализирует статические члены класса <see cref="InteractiveBadge"/>.
        /// </summary>
        static InteractiveBadge()
        {
            if (strokeBrush.CanFreeze)
                strokeBrush.Freeze();

            strokePen = new Pen(strokeBrush, 2);

            if (strokePen.CanFreeze)
                strokePen.Freeze();
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
            if (this.Shape.ContextMenu != null)
            {
                this.Shape.ContextMenu.Closed -= this.ContextMenu_Closed;

                this.Shape.ContextMenu = null;
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Скрывает значок.
        /// </summary>
        public void Hide()
        {
            this.path.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Переопределяет положение значка.
        /// </summary>
        public void RelocateBadge()
        {
            var badge = this.ViewModel as BadgeViewModel;
            var line = badge.Parent as LineViewModel;

            double x = 0;
            double y = 0;

            var a = line.LeftPoint;
            var b = line.RightPoint;

            if (badge.Distance > 0 && badge.Distance < line.Length)
            {
                // Находим отрезок линии, на котором должен находиться значок. Для этого сперва получаем общую длину линии в пикселях.
                var totalLength = 0.0;
                var points = line.GetAllPoints();
                for (int i = 1; i < points.Count; i++)
                    totalLength += PointHelper.GetDistance(points[i - 1], points[i]);
                // Находим расстояние в пикселях, на которое должен быть отдален значок.
                var offset = badge.Distance * (totalLength / line.Length);
                // Теперь приступаем к поиску отрезка линии, на котором будет находиться значок.
                var curOffset = 0.0;
                for (int i = 1; i < points.Count; i++)
                {
                    var curLength = PointHelper.GetDistance(points[i - 1], points[i]);

                    if ((curOffset + curLength) >= offset)
                    {
                        a = points[i - 1];
                        b = points[i];

                        var badgeDistance = offset - curOffset;
                        
                        // В данном случае мы нашли нужный нам отрезок. Теперь находим соотношение, на которое значок должен делить отрезок.
                        var coeff = badgeDistance / (curLength - badgeDistance);
                        if (double.IsNaN(coeff))
                            coeff = 0;
                        x = (a.X + coeff * b.X) / (1 + coeff);
                        y = (a.Y + coeff * b.Y) / (1 + coeff);
                        if (double.IsNaN(x) || double.IsNaN(y))
                        {
                            MessageBox.Show("isNan");
                        }


                        break;
                    }

                    curOffset += curLength;
                }

                if (double.IsNaN(x) || double.IsNaN(y))
                    throw new Exception(string.Format("Возникла проблема с расчетом положения значка. Идентификатор трубы: {0}. Идентификатор значка: {1}", line.Id, badge.Id));
            }
            else
                if (badge.Distance <= 0)
                {
                    x = a.X;
                    y = a.Y;
                }
                else
                {
                    x = b.X;
                    y = b.Y;
                }

            // Смещаем к главной точке значка.
            x -= this.hotPoint.X;
            y -= this.hotPoint.Y;

            this.leftTopCorner = new Point(x, y);

            System.Windows.Controls.Canvas.SetLeft(this.path, this.leftTopCorner.X);
            System.Windows.Controls.Canvas.SetTop(this.path, this.leftTopCorner.Y);

            // Поворачиваем значок.
            var c = a.X < b.X ? new Vector(b.X - a.X, b.Y - a.Y) : new Vector(a.X - b.X, a.Y - b.Y);
            var d = new Vector(1, 0);
            this.rotateTransform.Angle = -Vector.AngleBetween(c, d);
        }

        /// <summary>
        /// Показывает значок.
        /// </summary>
        public void Show()
        {
            this.path.Visibility = Visibility.Visible;
        }

        #endregion
    }

    // Реализация InteractiveShape.
    internal sealed partial class InteractiveBadge
    {
        #region Открытые переопределенные свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что может ли фигура быть выделенной.
        /// </summary>
        public override bool CanBeHighlighted
        {
            get
            {
                return false;
            }
            set
            {
                // Ничего не делаем.
            }
        }

        /// <summary>
        /// Возвращает центральную точку фигуры.
        /// </summary>
        public override Point CenterPoint
        {
            get
            {
                return this.leftTopCorner;
            }
        }

        /// <summary>
        /// Возвращает важную точку фигуры.
        /// </summary>
        public override Point MajorPoint
        {
            get
            {
                return this.leftTopCorner;
            }
        }

        #endregion

        #region Защищенные переопределенные методы

        /// <summary>
        /// Показывает элементы управления свойствами фигуры.
        /// </summary>
        protected override void HideUI()
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Выполняет действия, связанные с запросом контекстного меню.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        protected override void OnContextMenuRequested(Point mousePosition)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Выполняет действия, связанные с запросом контекстного меню редактирования.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        protected override void OnEditContextMenuRequested(Point mousePosition)
        {
            this.Shape.ContextMenu = Application.Current.Resources["BadgeEditContextMenu"] as ContextMenu;

            // Задаем контекст данных, предварительно обнуляя его.
            this.Shape.ContextMenu.DataContext = null;
            this.Shape.ContextMenu.DataContext = this.ViewModel;

            this.Shape.ContextMenu.Closed += this.ContextMenu_Closed;

            this.Shape.ContextMenu.IsOpen = true;
        }

        /// <summary>
        /// Выполняет действия, связанные с двойным нажатием мыши по фигуре.
        /// </summary>
        /// <param name="mousePosition">Положение мыши.</param>
        protected override void OnMouseDoubleClick(Point mousePosition)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Выполняет действия, связанные с перемещением фигуры.
        /// </summary>
        /// <param name="deltaX">Изменение положения фигуры по X.</param>
        /// <param name="deltaY">Изменение положения фигуры по Y.</param>
        protected override void OnMoving(double deltaX, double deltaY)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Выполняет действия, связанные с завершением перемещения фигуры.
        /// </summary>
        protected override void OnMovingCompleted()
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Выполняет действия, связанные с началом перемещения фигуры.
        /// </summary>
        protected override void OnMovingStarted()
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Показывает элементы управления свойствами фигуры.
        /// </summary>
        protected override void ShowUI()
        {
            // Ничего не делаем.
        }

        #endregion

        #region Открытые переопределенные методы

        /// <summary>
        /// Завершает перемещение фигуры. Используется для закрепления результата работы метода <see cref="MoveTo(Point)"/>.
        /// </summary>
        public override void EndMoveTo()
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Возвращает геометрию фигуры.
        /// </summary>
        /// <returns>Геометрия.</returns>
        public override Geometry GetGeometry()
        {
            return this.path.Data;
        }

        /// <summary>
        /// Возвращает кисть обводки фигуры.
        /// </summary>
        /// <returns>Кисть обводки.</returns>
        public override Pen GetStrokePen()
        {
            return strokePen;
        }

        /// <summary>
        /// Перемещает фигуру в заданную точку.
        /// </summary>
        /// <param name="point">Точка.</param>
        public override void MoveTo(Point point)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Начинает перемещение фигуры.
        /// </summary>
        public override void StartMoveTo()
        {
            // Ничего не делаем.
        }

        #endregion
    }
}