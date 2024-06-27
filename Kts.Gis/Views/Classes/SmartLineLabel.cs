using Kts.Gis.Services;
using Kts.Gis.ViewModels;
using Kts.WpfUtilities;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет умную надпись, которая автоматически определяет свое положение относительно родителя-линии.
    /// </summary>
    internal sealed partial class SmartLineLabel : SmartLabel
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что анимирована ли линия-родитель надписи.
        /// </summary>
        private bool isLineAnimated;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Родитель-линия.
        /// </summary>
        private readonly InteractiveLine line;

        /// <summary>
        /// Сервис настроек вида карты.
        /// </summary>
        private readonly IMapSettingService mapSettingService;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SmartLineLabel"/>.
        /// </summary>
        /// <param name="line">Родитель-линия.</param>
        /// <param name="savedAngle">Сохраненный угол поворота надписи.</param>
        /// <param name="savedPosition">Сохраненное положение надписи.</param>
        /// <param name="savedSize">Сохраненный размер надписи.</param>
        /// <param name="offset">Отступ надписи.</param>
        /// <param name="mapSettingService">Сервис настроек вида карты.</param>
        public SmartLineLabel(InteractiveLine line, double? savedAngle, Point? savedPosition, int? savedSize, double offset, IMapSettingService mapSettingService) : base(savedAngle, savedPosition, savedSize, mapSettingService.LineLabelDefaultSize, false)
        {
            this.line = line;
            this.mapSettingService = mapSettingService;

            this.NormalBrush = line.GetStrokePen().Brush;

            this.Foreground = this.NormalBrush;

            this.Offset = offset;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает отступ надписи.
        /// </summary>
        public double Offset
        {
            get;
            set;
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

        #endregion

        #region Открытые методы

        /// <summary>
        /// Задает цвет надписи.
        /// </summary>
        /// <param name="brush">Кисть.</param>
        public void ChangeColor(SolidColorBrush brush)
        {
            this.NormalBrush = brush;

            this.Foreground = this.NormalBrush;
        }

        #endregion
    }

    // Реализация SmartLabel.
    internal sealed partial class SmartLineLabel
    {
        #region Открытые переопределенные методы

        /// <summary>
        /// Возвращает размер надписи по умолчанию.
        /// </summary>
        public override int DefaultSize
        {
            get
            {
                return this.mapSettingService.LineLabelDefaultSize;
            }
        }

        #endregion

        #region Защищенные переопределенные методы

        /// <summary>
        /// Выполняется при требовании контекстного меню.
        /// </summary>
        protected override void OnContextMenuRequested()
        {
            this.ContextMenu = Application.Current.Resources["LineLabelContextMenu"] as ContextMenu;

            // Задаем контекст данных, предварительно обнуляя его.
            this.ContextMenu.DataContext = null;
            this.ContextMenu.DataContext = this.line.ViewModel;

#warning Нужно в будущем переделать вообще работу с контекстными меню. В данном случае пришлось использовать костыль для того, чтобы модель представления знала, с какой надписью сейчас ведется работа.
            (this.line.ViewModel as LineViewModel).SelectedLabelIndex = line.GetLabelIndex(this);

            this.ContextMenu.Closed += this.ContextMenu_Closed;

            this.ContextMenu.IsOpen = true;
        }

        /// <summary>
        /// Выполняется при нажатии левой кнопки мыши по надписи.
        /// </summary>
        protected override void OnMouseClick()
        {
            if (this.isLineAnimated)
                return;

            this.isLineAnimated = true;

            this.line.StartAnimation();
        }

        /// <summary>
        /// Выполняет действия, связанные с выходом курсора мыши с надписи.
        /// </summary>
        protected override void OnMouseLeaved()
        {
            if (!this.isLineAnimated)
                return;

            this.isLineAnimated = false;

            this.line.StopAnimation();
        }

        /// <summary>
        /// Выполняет действия, связанные с изменением положения надписи.
        /// </summary>
        /// <param name="prevPosition">Предыдущее положение надписи.</param>
        protected override void OnPositionChanged(Point prevPosition)
        {
            if (!this.HasManualPosition)
                this.InitialPosition = prevPosition;

            this.line.SetLabelMoved(this, prevPosition);
        }

        /// <summary>
        /// Выполняет действия, связанные с изменением размера надписи.
        /// </summary>
        protected override void OnSizeChanged()
        {
            this.line.SetLabelSizeChanged(this);
        }

        #endregion

        #region Открытые переопределенные методы

        /// <summary>
        /// Выполняет действия, связанные с изменением угла поворота надписи.
        /// </summary>
        /// <param name="prevAngle">Предыдущий угол поворота надписи.</param>
        public override void OnAngleChanged(double prevAngle)
        {
            if (!this.HasManualAngle)
                this.InitialAngle = prevAngle;

            this.line.SetLabelAngleChanged(this, prevAngle);
        }

        /// <summary>
        /// Переопределяет положение надписи.
        /// </summary>
        /// <param name="positionDelta">Разница между предыдущим и текущим положением фигуры.</param>
        public override void Relocate(Point positionDelta)
        {
            var points = this.line.GetLabelPoints(this);

            if (points == null)
                return;
                
            var a = points.Item1;
            var b = points.Item2;

            if (b.X < a.X)
            {
                // Меняем точки местами, чтобы точка a была левее точки b.
                var temp = a;
                a = b;
                b = temp;
            }

            if (!this.HasManualPosition)
            {
                var offset = -this.DesiredSize.Height - 1 * this.Offset;

                // Вычисляем положение надписи.
                var len = PointHelper.GetDistance(a, b);
                var ax = len / 2 - this.DesiredSize.Width / 2;
                var bx = len - ax;
                var coeff = ax / bx;
                var newX = (a.X + coeff * b.X) / (1 + coeff);
                var newY = (a.Y + coeff * b.Y) / (1 + coeff);

                // Вычисляем отступ.
                var dX = offset * (a.Y - b.Y) / len;
                var dY = offset * (b.X - a.X) / len;

                this.LeftTopCorner = new Point(newX + dX, newY + dY);
            }

            if (!this.HasManualAngle)
            {
                // Вычисляем угол поворота надписи.
                var c = new Vector(1, 0);
                var d = new Vector(b.X - a.X, b.Y - a.Y);
                var angle = Vector.AngleBetween(c, d);

                // Задаем его.
                var rotateTransform = this.RenderTransform as RotateTransform;
                if (rotateTransform == null)
                {
                    rotateTransform = new RotateTransform(0);

                    this.RenderTransform = rotateTransform;
                }
                rotateTransform.Angle = angle;
            }
        }

        #endregion
    }
}