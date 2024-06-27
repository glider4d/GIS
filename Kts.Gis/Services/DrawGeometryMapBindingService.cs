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
    /// Представляет сервис привязки представлений карты с моделями представлений, где представления отображаются при помощи <see cref="DrawingContext.DrawGeometry(Brush, Pen, Geometry)"/>.
    /// </summary>
    internal sealed partial class DrawGeometryMapBindingService : BaseMapBindingService
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DrawGeometryMapBindingService"/>.
        /// </summary>
        /// <param name="map">Карта.</param>
        /// <param name="badgeGeometries">Геометрии значков.</param>
        /// <param name="badgeHotPoints">Главные точки геометрий значков.</param>
        /// <param name="badgeOriginPoints">Точки поворотов геометрий значков.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="mapSettingService">Сервис доступа к функциям приложения.</param>
        public DrawGeometryMapBindingService(Map map, Dictionary<ObjectType, Geometry> badgeGeometries, Dictionary<ObjectType, Point> badgeHotPoints, Dictionary<ObjectType, Point> badgeOriginPoints, AccessService accessService, IMapSettingService mapSettingService) : base(map, badgeGeometries, badgeHotPoints, badgeOriginPoints, accessService, mapSettingService)
        {
        }

        #endregion
    }

    // Реализация BaseMapBindingService.
    internal sealed partial class DrawGeometryMapBindingService
    {
        #region Защищенные переопределенные методы

        /// <summary>
        /// Скрывает надписи объектов.
        /// </summary>
        protected override void HideLabels()
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Скрывает узлы.
        /// </summary>
        protected override void HideNodes()
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Отображает надписи объектов.
        /// </summary>
        protected override void ShowLabels()
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Отображает узлы.
        /// </summary>
        protected override void ShowNodes()
        {
            // Ничего не делаем.
        }

        #endregion

        #region Открытые переопределенные методы

        /// <summary>
        /// Добавляет дополнительный слой на карту с заданными объектами и ручками.
        /// </summary>
        /// <param name="objects">Словарь, где объекты разделены по ручкам.</param>
        /// <param name="front">true, если слой должен располагаться спереди объектов, иначе - false.</param>
        /// <returns>Идентификатор добавленного слоя.</returns>
        public override Guid AddLayer(Dictionary<Pen, List<IObjectViewModel>> objects, bool front)
        {
            return Guid.Empty;
        }

        /// <summary>
        /// Добавляет дополнительный слой на карту с заданными объектами и ручкой.
        /// </summary>
        /// <param name="objects">Объекты.</param>
        /// <param name="pen">Ручка обводки объектов.</param>
        /// <param name="front">true, если слой должен располагаться спереди объектов, иначе - false.</param>
        /// <returns>Идентификатор добавленного слоя.</returns>
        public override Guid AddLayer(List<IObjectViewModel> objects, Pen pen, bool front)
        {
            return Guid.Empty;
        }

        /// <summary>
        /// Включает анимацию заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        public override void AnimateOn(IObjectViewModel obj)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Включает анимацию заданных объектов.
        /// </summary>
        /// <param name="objects">Объекты.</param>
        public override void AnimateOn(List<IObjectViewModel> objects)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Проверяет наличие коллизии узла с другими объектами карты.
        /// </summary>
        /// <param name="node">Узел.</param>
        /// <param name="mode">Режим поиска коллизии.</param>
        /// <param name="radius">Радиус поиска.</param>
        /// <returns>Первый подходящий объект карты.</returns>
        public override IMapObjectViewModel CheckCollision(NodeViewModel node, CollisionSearchMode mode, double radius)
        {
            return null;
        }

        /// <summary>
        /// Убирает очертания объектов с карты.
        /// </summary>
        public override void ClearOutlines()
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Создает очертания объектов на карте.
        /// </summary>
        /// <param name="objects">Объекты.</param>
        public override void CreateOutlines(List<IMapObjectViewModel> objects)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Насильно скрывает надписи.
        /// </summary>
        public override void ForceHideLabels()
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Насильно отображает надписи.
        /// </summary>
        public override void ForceShowLabels()
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Возвращает положение центра текущего куска карты.
        /// </summary>
        /// <returns>Положение центра.</returns>
        public override Point GetCurrentCenter()
        {
            return new Point();
        }

        /// <summary>
        /// Возвращает представление заданной модели представления объекта карты.
        /// </summary>
        /// <param name="mapObject">Модель представления объекта карты.</param>
        /// <returns>Объект карты.</returns>
        public override IMapObject GetMapObjectView(IMapObjectViewModel mapObject)
        {
            return null;
        }

        /// <summary>
        /// Возвращает модель представления заданного представления объекта карты.
        /// </summary>
        /// <param name="mapObject">Объект карты.</param>
        /// <returns>Модель представления объекта карты.</returns>
        public override IMapObjectViewModel GetMapObjectViewModel(IMapObject mapObject)
        {
            return null;
        }

        /// <summary>
        /// Уведомляет модель представления объекта карты об изменении его представления.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="sourceMemberName">Название члена-источника.</param>
        public override void NotifyMapObjectViewModel(IMapObject source, string sourceMemberName)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Регистрирует связь между моделью представления объекта карты и его представлением.
        /// </summary>
        /// <param name="obj">Модель представления объекта карты.</param>
        public override void RegisterBinding(IMapObjectViewModel mapObject)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Убирает с карты дополнительный слой с заданным идентификатором.
        /// </summary>
        /// <param name="id">Идентификатор слоя.</param>
        public override void RemoveLayer(Guid id)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Сбрасывает настройки надписи.
        /// </summary>
        /// <param name="obj">Объект.</param>
        public override void ResetLabel(IMapObjectViewModel obj)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Задает значение представлению группы.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="sourcePropertyName">Название свойства-источника.</param>
        /// <param name="value">Значение.</param>
        public override void SetGroupViewValue(GroupViewModel source, string sourcePropertyName, object value)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Задает значение представлению слоя.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="sourcePropertyName">Название свойства-источника.</param>
        /// <param name="value">Значение.</param>
        public override void SetLayerViewValue(LayerViewModel source, string sourcePropertyName, object value)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Задает значение модели представления объекта карты.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="sourcePropertyName">Название свойства-источника.</param>
        /// <param name="value">Значение.</param>
        public override void SetMapObjectViewModelValue(IMapObject source, string sourcePropertyName, object value)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Задает значение представлению объекта карты.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="sourcePropertyName">Название свойства-источника.</param>
        /// <param name="value">Значение.</param>
        public override void SetMapObjectViewValue(IMapObjectViewModel source, string sourcePropertyName, object value)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Начинает анимирование объекта.
        /// </summary>
        /// <param name="mapObject">Анимируемый объект.</param>
        public override void StartAnimation(IMapObject mapObject)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Заканчивает анимирование объекта.
        /// </summary>
        /// <param name="mapObject">Анимируемый объект.</param>
        public override void StopAnimation(IMapObject mapObject)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Пробует уменьшить размер шрифта надписи линии с заданным индексом.
        /// </summary>
        /// <param name="line">Линия.</param>
        /// <param name="index">Индекс надписи.</param>
        /// <returns>null, если не удалось изменить размер шрифта, иначе - новый размер.</returns>
        public override int? TryDecreaseLabelSize(LineViewModel line, int index)
        {
            return null;
        }

        /// <summary>
        /// Пробует уменьшить размер шрифта надписи заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <returns>null, если не удалось изменить размер шрифта, иначе - новый размер.</returns>
        public override int? TryDecreaseLabelSize(IMapObjectViewModel obj)
        {
            return null;
        }

        /// <summary>
        /// Пробует увеличить размер шрифта надписи линии с заданным индексом.
        /// </summary>
        /// <param name="obj">Линия.</param>
        /// <param name="index">Индекс надписи.</param>
        /// <returns>null, если не удалось изменить размер шрифта, иначе - новый размер.</returns>
        public override int? TryIncreaseLabelSize(LineViewModel line, int index)
        {
            return null;
        }

        /// <summary>
        /// Пробует увеличить размер шрифта надписи заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <returns>null, если не удалось изменить размер шрифта, иначе - новый размер.</returns>
        public override int? TryIncreaseLabelSize(IMapObjectViewModel obj)
        {
            return null;
        }

        /// <summary>
        /// Убирает связь между моделью представления объекта карты и его представлением.
        /// </summary>
        /// <param name="obj">Модель представления объекта карты.</param>
        public override void UnregisterBinding(IMapObjectViewModel obj)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Убирает связи между моделями представлений объектов карты и их представлениями.
        /// </summary>
        public override void UnregisterBindings()
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Обновляет размер шрифта надписей фигур по умолчанию.
        /// </summary>
        /// <param name="size">Размер.</param>
        /// <param name="prevSize">Предыдущий размер.</param>
        public override void UpdateFigureLabelDefaultSize(int size, int prevSize)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Обновляет отступ внутри границы планируемых фигур.
        /// </summary>
        /// <param name="offset">Отступ.</param>
        public override void UpdateFigurePlanningOffset(double offset)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Обновляет толщину границы фигур.
        /// </summary>
        /// <param name="thickness">Толщина.</param>
        public override void UpdateFigureThickness(double thickness)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Обновляет размер шрифта независимых надписей по умолчанию.
        /// </summary>
        /// <param name="size">Размер.</param>
        /// <param name="prevSize">Предыдущий размер.</param>
        public override void UpdateIndependentLabelDefaultSize(int size, int prevSize)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Обновляет отступ внутри отключенных линий.
        /// </summary>
        /// <param name="offset">Отступ.</param>
        public override void UpdateLineDisabledOffset(double offset)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Обновляет размер шрифта надписей линий по умолчанию.
        /// </summary>
        /// <param name="size">Размер.</param>
        /// <param name="prevSize">Предыдущий размер.</param>
        public override void UpdateLineLabelDefaultSize(int size, int prevSize)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Обновляет надписи линии.
        /// </summary>
        /// <param name="line">Линия.</param>
        public override void UpdateLineLabels(LineViewModel line)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Обновляет отступ внутри планируемых линий.
        /// </summary>
        /// <param name="offset">Отступ.</param>
        public override void UpdateLinePlanningOffset(double offset)
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Обновляет толщину линий.
        /// </summary>
        /// <param name="thickness">Толщина.</param>
        /// <param name="prevThickness">Предыдущая толщина.</param>
        public override void UpdateLineThickness(double thickness, double prevThickness)
        {
            // Ничего не делаем.
        }

        #endregion
    }
}