using Kts.Gis.Services;
using Kts.WpfUtilities;
using System.Windows;
using System.Windows.Media;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления линейки.
    /// </summary>
    internal sealed partial class RulerViewModel : IMapObjectViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что была ли размещена линейка на карте.
        /// </summary>
        private bool isPlaced;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис привязки представлений карты с моделями представлений.
        /// </summary>
        private readonly IMapBindingService mapBindingService;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RulerViewModel"/>.
        /// </summary>
        /// <param name="startPoint">Начальная точка линейки.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        public RulerViewModel(Point startPoint, IMapBindingService mapBindingService)
        {
            this.StartPoint = startPoint;
            this.EndPoint = startPoint;
            this.mapBindingService = mapBindingService;

            this.RegisterBinding();
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает кисть линейки.
        /// </summary>
        public SolidColorBrush Brush
        {
            get
            {
                return new SolidColorBrush(Colors.Black);
            }
        }

        /// <summary>
        /// Возвращает или задает конечную точку линейки.
        /// </summary>
        public Point EndPoint
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает длину между начальной и конечной точек линейки в пикселях.
        /// </summary>
        public double Length
        {
            get
            {
                return PointHelper.GetDistance(this.StartPoint, this.EndPoint);
            }
        }

        /// <summary>
        /// Возвращает или задает начальную точку линейки.
        /// </summary>
        public Point StartPoint
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает толщину линейки.
        /// </summary>
        public int Thickness
        {
            get
            {
                return 1;
            }
        }

        #endregion
    }

    // Реализация IMapObjectViewModel.
    internal sealed partial class RulerViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что размещен ли объект на карте.
        /// </summary>
        public bool IsPlaced
        {
            get
            {
                return this.isPlaced;
            }
            set
            {
                this.isPlaced = value;

                this.mapBindingService.SetMapObjectViewValue(this, nameof(this.IsPlaced), value); 
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Регистрирует связь с представлением.
        /// </summary>
        public void RegisterBinding()
        {
            this.mapBindingService.RegisterBinding(this);
        }

        /// <summary>
        /// Вращает объект карты на заданный угол.
        /// </summary>
        /// <param name="angle">Угол.</param>
        /// <param name="origin">Точка, относительно которой выполняется поворот.</param>
        public void Rotate(double angle, Point origin)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Задает новый масштаб объекта карты.
        /// </summary>
        /// <param name="angle">Масштаб.</param>
        /// <param name="origin">Точка, относительно которой выполняется масштабирование.</param>
        public void Scale(double scale, Point origin)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Сдвигает объект карты на заданную дельту.
        /// </summary>
        /// <param name="delta">Дельта.</param>
        public void Shift(Point delta)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Убирает связь с представлением.
        /// </summary>
        public void UnregisterBinding()
        {
            this.mapBindingService.UnregisterBinding(this);
        }

        #endregion
    }
}