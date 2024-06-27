using Kts.Gis.Models;
using Kts.Gis.ViewModels;
using Kts.Gis.Views;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Kts.Gis.Services
{
    /// <summary>
    /// Представляет базовый сервис привязки представлений карты с моделями представлений.
    /// </summary>
    [Serializable]
    internal abstract partial class BaseMapBindingService : IDisposable, IMapBindingService
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что должны ли надписи автоматически скрываться.
        /// </summary>
        private bool autoHideLabels;

        /// <summary>
        /// Значение, указывающее на то, что должны ли узлы автоматически скрываться.
        /// </summary>
        private bool autoHideNodes;

        /// <summary>
        /// Значение, указывающее на то, что высвобождены ли ресурсы.
        /// </summary>
        private bool isDisposed;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Аниматор.
        /// </summary>
        //[NonSerialized]
        private readonly Animator animator = new Animator();

        /// <summary>
        /// Кисти.
        /// </summary>
        private readonly Dictionary<Utilities.Color, SolidColorBrush> brushes = new Dictionary<Utilities.Color, SolidColorBrush>();

        /// <summary>
        /// Кисти.
        /// </summary>
        private readonly Dictionary<Utilities.Color, Dictionary<double, Pen>> pens = new Dictionary<Utilities.Color, Dictionary<double, Pen>>();

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="BaseMapBindingService"/>.
        /// </summary>
        /// <param name="map">Карта.</param>
        /// <param name="badgeGeometries">Геометрии значков.</param>
        /// <param name="badgeHotPoints">Главные точки геометрий значков.</param>
        /// <param name="badgeOriginPoints">Точки поворотов геометрий значков.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="mapSettingService">Сервис настроек вида карты.</param>
        public BaseMapBindingService(Map map, Dictionary<ObjectType, Geometry> badgeGeometries, Dictionary<ObjectType, Point> badgeHotPoints, Dictionary<ObjectType, Point> badgeOriginPoints, AccessService accessService, IMapSettingService mapSettingService)
        {

            this.Map = map;
            this.BadgeGeometries = badgeGeometries;
            this.BadgeHotPoints = badgeHotPoints;
            this.BadgeOriginPoints = badgeOriginPoints;
            this.AccessService = accessService;
            this.MapSettingService = mapSettingService;

            this.Map.ScaleChanged += this.map_ScaleChanged;
        }

        #endregion

        #region Деструкторы

        /// <summary>
        /// Финализирует экземпляр класса <see cref="BaseMapBindingService"/>.
        /// </summary>
        ~BaseMapBindingService()
        {
            this.Dispose(false);
        }

        #endregion

        #region Защищенные свойства

        /// <summary>
        /// Возвращает сервис доступа к функциям приложения.
        /// </summary>
        protected AccessService AccessService
        {
            get;
        }

        /// <summary>
        /// Возвращает аниматор.
        /// </summary>
        
        protected Animator Animator
        {
            get
            {
                return this.animator;
            }
        }

        /// <summary>
        /// Возвращает геометрии значков.
        /// </summary>
        protected Dictionary<ObjectType, Geometry> BadgeGeometries
        {
            get;
        }
        
        /// <summary>
        /// Возвращает главные точки геометрий значков.
        /// </summary>
        protected Dictionary<ObjectType, Point> BadgeHotPoints
        {
            get;
        }

        /// <summary>
        /// Возвращает точки поворотов геометрий значков.
        /// </summary>
        protected Dictionary<ObjectType, Point> BadgeOriginPoints
        {
            get;
        }

        /// <summary>
        /// Трансформация масштабирования значков.
        /// </summary>
        //[NonSerialized]
        readonly ScaleTransform m_badgeScaleTransform = new ScaleTransform();

        protected ScaleTransform BadgeScaleTransform
        {
            get
            {
                return m_badgeScaleTransform;
            }
        }//;= new ScaleTransform();

        /// <summary>
        /// Возвращает карту.
        /// </summary>
        protected Map Map
        {
            get
            {
                return m_map;
            }
            private set
            {
                m_map = value;
            }
        }

        public Map getMap()
        {
            return m_map;
        }

        //[NonSerialized]
        Map m_map;


        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="Map.ScaleChanged"/> карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void map_ScaleChanged(object sender, EventArgs e)
        {
            this.ShowHideLabelsNodes();

            this.ScaleChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Высвобождает ресурсы.
        /// </summary>
        /// <param name="isDisposing">Значение, указывающее на то, что нужно ли высвободить ресурсы.</param>
        private void Dispose(bool isDisposing)
        {
            if (!this.isDisposed)
            {
                if (isDisposing)
                    this.animator.Dispose();

                this.isDisposed = true;
            }
        }

        /// <summary>
        /// Отображает или скрывает надписи и узлы.
        /// </summary>
        private void ShowHideLabelsNodes()
        {
            if (this.Map.Scale < 1)
            {
                if (this.autoHideLabels)
                    this.HideLabels();
                else
                    this.ShowLabels();

                if (this.autoHideNodes)
                    this.HideNodes();
                else
                    this.ShowNodes();
            }
            else
            {
                this.ShowLabels();

                this.ShowNodes();
            }
        }

        #endregion

        #region Защищенные абстрактные методы

        /// <summary>
        /// Скрывает надписи объектов.
        /// </summary>
        protected abstract void HideLabels();

        /// <summary>
        /// Скрывает узлы.
        /// </summary>
        protected abstract void HideNodes();

        /// <summary>
        /// Отображает надписи объектов.
        /// </summary>
        protected abstract void ShowLabels();

        /// <summary>
        /// Отображает узлы.
        /// </summary>
        protected abstract void ShowNodes();

        #endregion
    }

    // Реализация IDisposable.
    internal abstract partial class BaseMapBindingService
    {
        #region Открытые методы

        /// <summary>
        /// Высвобождает ресурсы.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion
    }

    // Реализация IMapBindingService.
    internal abstract partial class BaseMapBindingService
    {
        #region Открытые события

        /// <summary>
        /// Событие изменения масштаба.
        /// </summary>
        public event EventHandler ScaleChanged;

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает сервис настроек вида карты.
        /// </summary>
        public IMapSettingService MapSettingService
        {
            get
            {
                return m_mapSettingService;
            }
            private set
            {
                m_mapSettingService = value;
            }
        }

        //[NonSerialized]
        IMapSettingService m_mapSettingService;

        /// <summary>
        /// Возвращает масштаб карты.
        /// </summary>
        public double Scale
        {
            get
            {
                return this.Map.Scale;
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Отключает анимацию объектов.
        /// </summary>
        public void AnimateOff()
        {
            this.Map.AnimateOff();
        }
        
        /// <summary>
        /// Возвращает кисть заданного цвета.
        /// </summary>
        /// <param name="color">Цвет.</param>
        /// <returns>Кисть.</returns>
        public SolidColorBrush GetBrush(Utilities.Color color)
        {
            if (this.brushes.ContainsKey(color))
                return this.brushes[color];

            var brush = new SolidColorBrush(Color.FromRgb(color.R, color.G, color.B));

            if (brush.CanFreeze)
                brush.Freeze();

            this.brushes.Add(color, brush);

            return brush;
        }

        /// <summary>
        /// Возвращает ручку заданного цвета.
        /// </summary>
        /// <param name="color">Цвет.</param>
        /// <param name="thickness">Толщина .</param>
        /// <returns>Кисть.</returns>
        public Pen GetPen(Utilities.Color color, double thickness)
        {
            if (this.pens.ContainsKey(color))
            {
                if (this.pens[color].ContainsKey(thickness))
                    return this.pens[color][thickness];
            }
            else
                this.pens.Add(color, new Dictionary<double, Pen>());

            var pen = new Pen(this.GetBrush(color), thickness);

            if (pen.CanFreeze)
                pen.Freeze();

            this.pens[color].Add(thickness, pen);

            return pen;
        }

        /// <summary>
        /// Возвращает положение указателя на карте.
        /// </summary>
        /// <returns>Положение указателя на карте.</returns>
        public Point GetPointerPosition()
        {
            return this.Map.MousePosition;
        }

        /// <summary>
        /// Задает значение, указывающее на то, что должны ли надписи автоматически скрываться.
        /// </summary>
        /// <param name="value">Значение, указывающее на то, что должны ли надписи автоматически скрываться.</param>
        public void SetAutoHideLabels(bool value)
        {
            this.autoHideLabels = value;

            this.ShowHideLabelsNodes();
        }

        /// <summary>
        /// Задает значение, указывающее на то, что должны ли узлы автоматически скрываться.
        /// </summary>
        /// <param name="value">Значение, указывающее на то, что должны ли узлы автоматически скрываться.</param>
        public void SetAutoHideNodes(bool value)
        {
            this.autoHideNodes = value;

            this.ShowHideLabelsNodes();
        }

        /// <summary>
        /// Задает масштаб значков.
        /// </summary>
        /// <param name="scale">Масштаб.</param>
        public void SetBadgeScale(double scale)
        {
            this.BadgeScaleTransform.ScaleX = scale;
            this.BadgeScaleTransform.ScaleY = scale;
        }

        #endregion

        #region Открытые абстрактные методы

        /// <summary>
        /// Добавляет дополнительный слой на карту с заданными объектами и ручками.
        /// </summary>
        /// <param name="objects">Словарь, где объекты разделены по ручкам.</param>
        /// <param name="front">true, если слой должен располагаться спереди объектов, иначе - false.</param>
        /// <returns>Идентификатор добавленного слоя.</returns>
        public abstract Guid AddLayer(Dictionary<Pen, List<IObjectViewModel>> objects, bool front);

        /// <summary>
        /// Добавляет дополнительный слой на карту с заданными объектами и ручкой.
        /// </summary>
        /// <param name="objects">Объекты.</param>
        /// <param name="pen">Ручка обводки объектов.</param>
        /// <param name="front">true, если слой должен располагаться спереди объектов, иначе - false.</param>
        /// <returns>Идентификатор добавленного слоя.</returns>
        public abstract Guid AddLayer(List<IObjectViewModel> objects, Pen pen, bool front);

        /// <summary>
        /// Включает анимацию заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        public abstract void AnimateOn(IObjectViewModel obj);

        /// <summary>
        /// Включает анимацию заданных объектов.
        /// </summary>
        /// <param name="objects">Объекты.</param>
        public abstract void AnimateOn(List<IObjectViewModel> objects);

        /// <summary>
        /// Проверяет наличие коллизии узла с другими объектами карты.
        /// </summary>
        /// <param name="node">Узел.</param>
        /// <param name="mode">Режим поиска коллизии.</param>
        /// <param name="radius">Радиус поиска.</param>
        /// <returns>Первый подходящий объект карты.</returns>
        public abstract IMapObjectViewModel CheckCollision(NodeViewModel node, CollisionSearchMode mode, double radius);

        /// <summary>
        /// Убирает очертания объектов с карты.
        /// </summary>
        public abstract void ClearOutlines();

        /// <summary>
        /// Создает очертания объектов на карте.
        /// </summary>
        /// <param name="objects">Объекты.</param>
        public abstract void CreateOutlines(List<IMapObjectViewModel> objects);

        /// <summary>
        /// Насильно скрывает надписи.
        /// </summary>
        public abstract void ForceHideLabels();

        /// <summary>
        /// Насильно отображает надписи.
        /// </summary>
        public abstract void ForceShowLabels();

        /// <summary>
        /// Возвращает положение центра текущего куска карты.
        /// </summary>
        /// <returns>Положение центра.</returns>
        public abstract Point GetCurrentCenter();

        /// <summary>
        /// Возвращает представление заданной модели представления объекта карты.
        /// </summary>
        /// <param name="mapObject">Модель представления объекта карты.</param>
        /// <returns>Объект карты.</returns>
        public abstract IMapObject GetMapObjectView(IMapObjectViewModel mapObject);

        /// <summary>
        /// Возвращает модель представления заданного представления объекта карты.
        /// </summary>
        /// <param name="mapObject">Объект карты.</param>
        /// <returns>Модель представления объекта карты.</returns>
        public abstract IMapObjectViewModel GetMapObjectViewModel(IMapObject mapObject);

        /// <summary>
        /// Уведомляет модель представления объекта карты об изменении его представления.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="sourceMemberName">Название члена-источника.</param>
        public abstract void NotifyMapObjectViewModel(IMapObject source, string sourceMemberName);

        /// <summary>
        /// Регистрирует связь между моделью представления объекта карты и его представлением.
        /// </summary>
        /// <param name="obj">Модель представления объекта карты.</param>
        public abstract void RegisterBinding(IMapObjectViewModel mapObject);

        /// <summary>
        /// Убирает с карты дополнительный слой с заданным идентификатором.
        /// </summary>
        /// <param name="id">Идентификатор слоя.</param>
        public abstract void RemoveLayer(Guid id);

        /// <summary>
        /// Сбрасывает настройки надписи.
        /// </summary>
        /// <param name="obj">Объект.</param>
        public abstract void ResetLabel(IMapObjectViewModel obj);

        /// <summary>
        /// Задает значение представлению группы.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="sourcePropertyName">Название свойства-источника.</param>
        /// <param name="value">Значение.</param>
        public abstract void SetGroupViewValue(GroupViewModel source, string sourcePropertyName, object value);

        /// <summary>
        /// Задает значение представлению слоя.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="sourcePropertyName">Название свойства-источника.</param>
        /// <param name="value">Значение.</param>
        public abstract void SetLayerViewValue(LayerViewModel source, string sourcePropertyName, object value);

        /// <summary>
        /// Задает значение модели представления объекта карты.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="sourcePropertyName">Название свойства-источника.</param>
        /// <param name="value">Значение.</param>
        public abstract void SetMapObjectViewModelValue(IMapObject source, string sourcePropertyName, object value);

        /// <summary>
        /// Задает значение представлению объекта карты.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="sourcePropertyName">Название свойства-источника.</param>
        /// <param name="value">Значение.</param>
        public abstract void SetMapObjectViewValue(IMapObjectViewModel source, string sourcePropertyName, object value);

        /// <summary>
        /// Начинает анимирование объекта.
        /// </summary>
        /// <param name="mapObject">Анимируемый объект.</param>
        public abstract void StartAnimation(IMapObject mapObject);

        /// <summary>
        /// Заканчивает анимирование объекта.
        /// </summary>
        /// <param name="mapObject">Анимируемый объект.</param>
        public abstract void StopAnimation(IMapObject mapObject);

        /// <summary>
        /// Пробует уменьшить размер шрифта надписи линии с заданным индексом.
        /// </summary>
        /// <param name="line">Линия.</param>
        /// <param name="index">Индекс надписи.</param>
        /// <returns>null, если не удалось изменить размер шрифта, иначе - новый размер.</returns>
        public abstract int? TryDecreaseLabelSize(LineViewModel line, int index);

        /// <summary>
        /// Пробует уменьшить размер шрифта надписи заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <returns>null, если не удалось изменить размер шрифта, иначе - новый размер.</returns>
        public abstract int? TryDecreaseLabelSize(IMapObjectViewModel obj);

        /// <summary>
        /// Пробует увеличить размер шрифта надписи линии с заданным индексом.
        /// </summary>
        /// <param name="obj">Линия.</param>
        /// <param name="index">Индекс надписи.</param>
        /// <returns>null, если не удалось изменить размер шрифта, иначе - новый размер.</returns>
        public abstract int? TryIncreaseLabelSize(LineViewModel line, int index);

        /// <summary>
        /// Пробует увеличить размер шрифта надписи заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <returns>null, если не удалось изменить размер шрифта, иначе - новый размер.</returns>
        public abstract int? TryIncreaseLabelSize(IMapObjectViewModel obj);

        /// <summary>
        /// Убирает связь между моделью представления объекта карты и его представлением.
        /// </summary>
        /// <param name="obj">Модель представления объекта карты.</param>
        public abstract void UnregisterBinding(IMapObjectViewModel obj);

        /// <summary>
        /// Убирает связи между моделями представлений объектов карты и их представлениями.
        /// </summary>
        public abstract void UnregisterBindings();

        /// <summary>
        /// Обновляет размер шрифта надписей фигур по умолчанию.
        /// </summary>
        /// <param name="size">Размер.</param>
        /// <param name="prevSize">Предыдущий размер.</param>
        public abstract void UpdateFigureLabelDefaultSize(int size, int prevSize);

        /// <summary>
        /// Обновляет отступ внутри границы планируемых фигур.
        /// </summary>
        /// <param name="offset">Отступ.</param>
        public abstract void UpdateFigurePlanningOffset(double offset);

        /// <summary>
        /// Обновляет толщину границы фигур.
        /// </summary>
        /// <param name="thickness">Толщина.</param>
        public abstract void UpdateFigureThickness(double thickness);

        /// <summary>
        /// Обновляет размер шрифта независимых надписей по умолчанию.
        /// </summary>
        /// <param name="size">Размер.</param>
        /// <param name="prevSize">Предыдущий размер.</param>
        public abstract void UpdateIndependentLabelDefaultSize(int size, int prevSize);

        /// <summary>
        /// Обновляет отступ внутри отключенных линий.
        /// </summary>
        /// <param name="offset">Отступ.</param>
        public abstract void UpdateLineDisabledOffset(double offset);

        /// <summary>
        /// Обновляет размер шрифта надписей линий по умолчанию.
        /// </summary>
        /// <param name="size">Размер.</param>
        /// <param name="prevSize">Предыдущий размер.</param>
        public abstract void UpdateLineLabelDefaultSize(int size, int prevSize);

        /// <summary>
        /// Обновляет надписи линии.
        /// </summary>
        /// <param name="line">Линия.</param>
        public abstract void UpdateLineLabels(LineViewModel line);

        /// <summary>
        /// Обновляет отступ внутри планируемых линий.
        /// </summary>
        /// <param name="offset">Отступ.</param>
        public abstract void UpdateLinePlanningOffset(double offset);

        /// <summary>
        /// Обновляет толщину линий.
        /// </summary>
        /// <param name="thickness">Толщина.</param>
        /// <param name="prevThickness">Предыдущая толщина.</param>
        public abstract void UpdateLineThickness(double thickness, double prevThickness);

        #endregion
    }
}