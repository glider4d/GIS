using Kts.Gis.Services;
using Kts.WpfUtilities;
using System;
using System.Windows;
using System.Windows.Media;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления новой линейки, которая тупо показывает отмеченное ей расстояние.
    /// </summary>
    internal sealed partial class NewRulerViewModel : IMapObjectViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что была ли размещена линейка на карте.
        /// </summary>
        private bool isPlaced;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Информатор о рисуемой линейке.
        /// </summary>
        private readonly LineInfoViewModel lineInfo;

        /// <summary>
        /// Сервис привязки представлений карты с моделями представлений.
        /// </summary>
        private readonly IMapBindingService mapBindingService;

        /// <summary>
        /// Масштаб линий.
        /// </summary>
        private readonly double scale;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NewRulerViewModel"/>.
        /// </summary>
        /// <param name="startPoint">Начальная точка линейки.</param>
        /// <param name="scale">Масштаб линий.</param>
        /// <param name="lineInfo">Информатор о рисуемой линейке.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        public NewRulerViewModel(Point startPoint, double scale, LineInfoViewModel lineInfo, IMapBindingService mapBindingService)
        {
            this.Points = new PointCollection()
            {
                startPoint,
                new Point(startPoint.X, startPoint.Y)
            };

            this.scale = scale;
            this.lineInfo = lineInfo;
            this.mapBindingService = mapBindingService;

            this.RegisterBinding();
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает коллекцию точек, из которых состоит линейка.
        /// </summary>
        public PointCollection Points
        {
            get;
        } = new PointCollection();

        #endregion

        #region Открытые методы
        
        /// <summary>
        /// Выполняет действия, вызванные изменением точек.
        /// </summary>
        public void OnPointsChanged()
        {
            // Высчитываем полную протяженность линейки и последнего его сегмента.
            var pointCount = this.Points.Count;
            var totalLength = 0.0;
            for (int i = 1; i < pointCount; i++)
                totalLength += PointHelper.GetDistance(this.Points[i - 1], this.Points[i]);
            var segmentLength = PointHelper.GetDistance(this.Points[pointCount - 2], this.Points[pointCount - 1]);

            this.lineInfo.Length = Math.Round(totalLength * this.scale, 2);
            this.lineInfo.SegmentLength = Math.Round(segmentLength * this.scale, 2);
        }

        #endregion
    }

    // Реализация IMapObjectViewModel.
    internal sealed partial class NewRulerViewModel
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