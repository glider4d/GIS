using Kts.Gis.Data;
using Kts.Gis.Services;
using Kts.Messaging;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления полилайна.
    /// </summary>
    internal sealed partial class PolylineViewModel : ServicedViewModel, IMapObjectViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Коэффициент направления отступа параллельных линий.
        /// </summary>
        private double coeff = 1;

        /// <summary>
        /// Значение, указывающее на то, что был ли размещен полилайн на карте.
        /// </summary>
        private bool isPlaced;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Информатор о рисуемом полилайне.
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
        /// Инициализирует новый экземпляр класса <see cref="PolylineViewModel"/>.
        /// </summary>
        /// <param name="id">Идентификатор полилайна.</param>
        /// <param name="isPlanning">Значение, указывающее на то, что является ли полилайн планируемым.</param>
        /// <param name="points">Коллекция точек, из которых состоит полилайн.</param>
        /// <param name="brush">Кисть полилайна.</param>
        /// <param name="lineInfo">Информатор о рисуемом полилайне.</param>
        /// <param name="scale">Масштаб линий.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        public PolylineViewModel(object id, bool isPlanning, PointCollection points, SolidColorBrush brush, LineInfoViewModel lineInfo, double scale, IDataService dataService, IMessageService messageService, IMapBindingService mapBindingService) : base(dataService, messageService)
        {
            this.Id = id;
            this.IsPlanning = isPlanning;
            this.Points = points;
            this.Brush = brush;
            this.lineInfo = lineInfo;
            this.scale = scale;
            this.mapBindingService = mapBindingService;

            this.RegisterBinding();
        }

        public PolylineViewModel(object id, bool isPlanning, PointCollection points, SolidColorBrush brush, LineInfoViewModel lineInfo, double scale, IDataService dataService, IMessageService messageService, IMapBindingService mapBindingService, double thickness ) : base(dataService, messageService)
        {
            this.m_customThickness = true;
            this.m_doubleCustomThickness = thickness;
            this.Id = id;
            this.IsPlanning = isPlanning;
            this.Points = points;
            this.Brush = brush;
            this.lineInfo = lineInfo;
            this.scale = scale;
            this.mapBindingService = mapBindingService;

            this.RegisterBinding();
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает кисть полилайна.
        /// </summary>
        public SolidColorBrush Brush
        {
            get;
        }
        
        /// <summary>
        /// Возвращает идентификатор полилайна.
        /// </summary>
        public object Id
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли полилайн планируемым.
        /// </summary>
        public bool IsPlanning
        {
            get;
        }

        /// <summary>
        /// Возвращает параллельные полилайны.
        /// </summary>
        public List<PolylineViewModel> ParallelPolylines
        {
            get;
        } = new List<PolylineViewModel>();

        /// <summary>
        /// Возвращает коллекцию точек, из которых состоит полилайн.
        /// </summary>
        public PointCollection Points
        {
            get;
        }

        /// <summary>
        /// Возвращает толщину полилайна.
        /// </summary>
        public double Thickness
        {
            get
            {
                if (!m_customThickness)
                    return this.mapBindingService.MapSettingService.PolylineThickness;
                else
                    return m_doubleCustomThickness;
            }
            set
            {
                if (m_customThickness)
                    m_doubleCustomThickness = value;
            }
        }

        private double m_doubleCustomThickness;

        public bool m_customThickness;

        /// <summary>
        /// Возвращает общую толщину полилайна (сумма толщины полилайна, толщин параллельных полилайнов и отступов между ними).
        /// </summary>
        public double TotalThickness
        {
            get
            {
                return this.Thickness * (this.ParallelPolylines.Count + 1) + (this.mapBindingService.MapSettingService.PolylineOffset - this.Thickness) * this.ParallelPolylines.Count;
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Возвращает строковое представление точек изгиба полилайна.
        /// </summary>
        /// <param name="pointCount">Количество учитываемых точек.</param>
        /// <returns>Строковое представление точек изгиба полилайна.</returns>
        public string GetBendPoints(int pointCount)
        {
            var list = new List<Point>();

            for (int i = 1; i < pointCount - 1; i++)
                list.Add(this.Points[i]);

            return new PointCollection(list).ToString();
        }

        /// <summary>
        /// Выполняет действия, вызванные изменением точек.
        /// </summary>
        public void OnPointsChanged()
        {
            if (this.Points.Count == 2)
                if (this.Points[1].X < this.Points[0].X)
                    coeff = -1;
                else
                    coeff = 1;

            var offset = this.mapBindingService.MapSettingService.PolylineOffset;

            for (int i = 0; i < this.ParallelPolylines.Count; i++)
            {
                this.ParallelPolylines[i].Points.Clear();

                var points = GetOffsettedPoints(this.Points, coeff * (i * offset + offset));

                foreach (var point in points)
                    this.ParallelPolylines[i].Points.Add(point);
            }

            // Высчитываем полную протяженность полилайна и последнего его сегмента.
            var pointCount = this.Points.Count;
            var totalLength = 0.0;
            for (int i = 1; i < pointCount; i++)
                totalLength += PointHelper.GetDistance(this.Points[i - 1], this.Points[i]);
            var segmentLength = PointHelper.GetDistance(this.Points[pointCount - 2], this.Points[pointCount - 1]);

            this.lineInfo.Length = Math.Round(totalLength * this.scale, 2);
            this.lineInfo.SegmentLength = Math.Round(segmentLength * this.scale, 2);
        }

        #endregion

        #region Открытые статические методы

        /// <summary>
        /// Возвращает коллекцию точек полилайна с отступом.
        /// </summary>
        /// <param name="points">Точки без отступа.</param>
        /// <param name="offset">Отступ.</param>
        /// <returns>Коллекция точек.</returns>
        public static PointCollection GetOffsettedPoints(IList<Point> points, double offset)
        {
            int n = points.Count;
            var p = points;
            var u = new List<Vector>();

            for (int k = 0; k < n - 1; k++)
            {
                double c = p[k + 1].X - p[k].X;
                double s = p[k + 1].Y - p[k].Y;
                double l = Math.Sqrt(c * c + s * s);

                u.Add(new Vector(c / l, s / l));
            }

            var result = new PointCollection();

            result.Add(new Point(p[0].X - offset * u[0].Y, p[0].Y + offset * u[0].X));

            for (int k = 1; k < n - 1; k++)
            {
                double l = offset / (1 + u[k].X * u[k - 1].X + u[k].Y * u[k - 1].Y);

                result.Add(new Point(p[k].X - l * (u[k].Y + u[k - 1].Y), p[k].Y + l * (u[k].X + u[k - 1].X)));
            }

            result.Add(new Point(p[n - 1].X - offset * u[n - 2].Y, p[n - 1].Y + offset * u[n - 2].X));

            return result;
        }

        #endregion
    }

    // Реализация IMapObjectViewModel.
    internal sealed partial class PolylineViewModel
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