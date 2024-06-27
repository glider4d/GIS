using Kts.Gis.ViewModels;
using Kts.Gis.Views;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Kts.Gis.Services
{
    /// <summary>
    /// Представляет интерфейс сервиса привязки представлений карты с моделями представлений.
    /// </summary>
    internal interface IMapBindingService
    {
        #region События

        /// <summary>
        /// Событие изменения масштаба.
        /// </summary>
        event EventHandler ScaleChanged;

        #endregion

        #region Свойства
        Map getMap();
        /// <summary>
        /// Возвращает сервис настроек вида карты.
        /// </summary>
        IMapSettingService MapSettingService
        {
            get;
        }

        /// <summary>
        /// Возвращает масштаб карты.
        /// </summary>
        double Scale
        {
            get;
        }

        #endregion

        #region Методы

        /// <summary>
        /// Добавляет дополнительный слой на карту с заданными объектами и ручками.
        /// </summary>
        /// <param name="objects">Словарь, где объекты разделены по ручкам.</param>
        /// <param name="front">true, если слой должен располагаться спереди объектов, иначе - false.</param>
        /// <returns>Идентификатор добавленного слоя.</returns>
        Guid AddLayer(Dictionary<Pen, List<IObjectViewModel>> objects, bool front);

        /// <summary>
        /// Добавляет дополнительный слой на карту с заданными объектами и ручкой.
        /// </summary>
        /// <param name="objects">Объекты.</param>
        /// <param name="pen">Ручка обводки объектов.</param>
        /// <param name="front">true, если слой должен располагаться спереди объектов, иначе - false.</param>
        /// <returns>Идентификатор добавленного слоя.</returns>
        Guid AddLayer(List<IObjectViewModel> objects, Pen pen, bool front);

        /// <summary>
        /// Отключает анимацию объектов.
        /// </summary>
        void AnimateOff();

        /// <summary>
        /// Включает анимацию заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        void AnimateOn(IObjectViewModel obj);

        /// <summary>
        /// Включает анимацию заданных объектов.
        /// </summary>
        /// <param name="objects">Объекты.</param>
        void AnimateOn(List<IObjectViewModel> objects);

        /// <summary>
        /// Проверяет наличие коллизии узла с другими объектами карты.
        /// </summary>
        /// <param name="node">Узел.</param>
        /// <param name="mode">Режим поиска коллизии.</param>
        /// <param name="radius">Радиус поиска.</param>
        /// <returns>Первый подходящий объект карты.</returns>
        IMapObjectViewModel CheckCollision(NodeViewModel node, CollisionSearchMode mode, double radius);

        /// <summary>
        /// Убирает очертания объектов с карты.
        /// </summary>
        void ClearOutlines();

        /// <summary>
        /// Создает очертания объектов на карте.
        /// </summary>
        /// <param name="objects">Объекты.</param>
        void CreateOutlines(List<IMapObjectViewModel> objects);

        /// <summary>
        /// Насильно скрывает надписи.
        /// </summary>
        void ForceHideLabels();

        /// <summary>
        /// Насильно отображает надписи.
        /// </summary>
        void ForceShowLabels();

        /// <summary>
        /// Возвращает кисть заданного цвета.
        /// </summary>
        /// <param name="color">Цвет.</param>
        /// <returns>Кисть.</returns>
        SolidColorBrush GetBrush(Utilities.Color color);

        /// <summary>
        /// Возвращает положение центра текущего куска карты.
        /// </summary>
        /// <returns>Положение центра.</returns>
        Point GetCurrentCenter();

        /// <summary>
        /// Возвращает представление заданной модели представления объекта карты.
        /// </summary>
        /// <param name="mapObject">Модель представления объекта карты.</param>
        /// <returns>Объект карты.</returns>
        IMapObject GetMapObjectView(IMapObjectViewModel mapObject);

        /// <summary>
        /// Возвращает модель представления заданного представления объекта карты.
        /// </summary>
        /// <param name="mapObject">Объект карты.</param>
        /// <returns>Модель представления объекта карты.</returns>
        IMapObjectViewModel GetMapObjectViewModel(IMapObject mapObject);

        /// <summary>
        /// Возвращает ручку заданного цвета.
        /// </summary>
        /// <param name="color">Цвет.</param>
        /// <param name="thickness">Толщина .</param>
        /// <returns>Ручка.</returns>
        Pen GetPen(Utilities.Color color, double thickness);

        /// <summary>
        /// Возвращает положение указателя на карте.
        /// </summary>
        /// <returns>Положение указателя на карте.</returns>
        Point GetPointerPosition();

        /// <summary>
        /// Уведомляет модель представления объекта карты об изменении его представления.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="sourceMemberName">Название члена-источника.</param>
        void NotifyMapObjectViewModel(IMapObject source, string sourceMemberName);

        /// <summary>
        /// Регистрирует связь между моделью представления объекта карты и его представлением.
        /// </summary>
        /// <param name="obj">Модель представления объекта карты.</param>
        void RegisterBinding(IMapObjectViewModel mapObject);

        /// <summary>
        /// Убирает с карты дополнительный слой с заданным идентификатором.
        /// </summary>
        /// <param name="id">Идентификатор слоя.</param>
        void RemoveLayer(Guid id);

        /// <summary>
        /// Сбрасывает настройки надписи.
        /// </summary>
        /// <param name="obj">Объект.</param>
        void ResetLabel(IMapObjectViewModel obj);

        /// <summary>
        /// Задает значение, указывающее на то, что должны ли надписи автоматически скрываться.
        /// </summary>
        /// <param name="value">Значение, указывающее на то, что должны ли надписи автоматически скрываться.</param>
        void SetAutoHideLabels(bool value);

        /// <summary>
        /// Задает значение, указывающее на то, что должны ли узлы автоматически скрываться.
        /// </summary>
        /// <param name="value">Значение, указывающее на то, что должны ли узлы автоматически скрываться.</param>
        void SetAutoHideNodes(bool value);

        /// <summary>
        /// Задает масштаб значков.
        /// </summary>
        /// <param name="scale">Масштаб.</param>
        void SetBadgeScale(double scale);

        /// <summary>
        /// Задает значение представлению группы.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="sourcePropertyName">Название свойства-источника.</param>
        /// <param name="value">Значение.</param>
        void SetGroupViewValue(GroupViewModel source, string sourcePropertyName, object value);

        /// <summary>
        /// Задает значение представлению слоя.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="sourcePropertyName">Название свойства-источника.</param>
        /// <param name="value">Значение.</param>
        void SetLayerViewValue(LayerViewModel source, string sourcePropertyName, object value);

        /// <summary>
        /// Задает значение модели представления объекта карты.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="sourcePropertyName">Название свойства-источника.</param>
        /// <param name="value">Значение.</param>
        void SetMapObjectViewModelValue(IMapObject source, string sourcePropertyName, object value);

        /// <summary>
        /// Задает значение представлению объекта карты.
        /// </summary>
        /// <param name="source">Источник.</param>
        /// <param name="sourcePropertyName">Название свойства-источника.</param>
        /// <param name="value">Значение.</param>
        void SetMapObjectViewValue(IMapObjectViewModel source, string sourcePropertyName, object value);

        /// <summary>
        /// Начинает анимирование объекта.
        /// </summary>
        /// <param name="mapObject">Анимируемый объект.</param>
        void StartAnimation(IMapObject mapObject);

        /// <summary>
        /// Заканчивает анимирование объекта.
        /// </summary>
        /// <param name="mapObject">Анимируемый объект.</param>
        void StopAnimation(IMapObject mapObject);

        /// <summary>
        /// Пробует уменьшить размер шрифта надписи линии с заданным индексом.
        /// </summary>
        /// <param name="line">Линия.</param>
        /// <param name="index">Индекс надписи.</param>
        /// <returns>null, если не удалось изменить размер шрифта, иначе - новый размер.</returns>
        int? TryDecreaseLabelSize(LineViewModel line, int index);

        /// <summary>
        /// Пробует уменьшить размер шрифта надписи заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <returns>null, если не удалось изменить размер шрифта, иначе - новый размер.</returns>
        int? TryDecreaseLabelSize(IMapObjectViewModel obj);

        /// <summary>
        /// Пробует увеличить размер шрифта надписи линии с заданным индексом.
        /// </summary>
        /// <param name="obj">Линия.</param>
        /// <param name="index">Индекс надписи.</param>
        /// <returns>null, если не удалось изменить размер шрифта, иначе - новый размер.</returns>
        int? TryIncreaseLabelSize(LineViewModel line, int index);

        /// <summary>
        /// Пробует увеличить размер шрифта надписи заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <returns>null, если не удалось изменить размер шрифта, иначе - новый размер.</returns>
        int? TryIncreaseLabelSize(IMapObjectViewModel obj);

        /// <summary>
        /// Убирает связь между моделью представления объекта карты и его представлением.
        /// </summary>
        /// <param name="obj">Модель представления объекта карты.</param>
        void UnregisterBinding(IMapObjectViewModel obj);

        /// <summary>
        /// Убирает связи между моделями представлений объектов карты и их представлениями.
        /// </summary>
        void UnregisterBindings();

        /// <summary>
        /// Обновляет размер шрифта надписей фигур по умолчанию.
        /// </summary>
        /// <param name="size">Размер.</param>
        /// <param name="prevSize">Предыдущий размер.</param>
        void UpdateFigureLabelDefaultSize(int size, int prevSize);

        /// <summary>
        /// Обновляет отступ внутри границы планируемых фигур.
        /// </summary>
        /// <param name="offset">Отступ.</param>
        void UpdateFigurePlanningOffset(double offset);

        /// <summary>
        /// Обновляет толщину границы фигур.
        /// </summary>
        /// <param name="thickness">Толщина.</param>
        void UpdateFigureThickness(double thickness);

        /// <summary>
        /// Обновляет размер шрифта независимых надписей по умолчанию.
        /// </summary>
        /// <param name="size">Размер.</param>
        /// <param name="prevSize">Предыдущий размер.</param>
        void UpdateIndependentLabelDefaultSize(int size, int prevSize);

        /// <summary>
        /// Обновляет отступ внутри отключенных линий.
        /// </summary>
        /// <param name="offset">Отступ.</param>
        void UpdateLineDisabledOffset(double offset);

        /// <summary>
        /// Обновляет размер шрифта надписей линий по умолчанию.
        /// </summary>
        /// <param name="size">Размер.</param>
        /// <param name="prevSize">Предыдущий размер.</param>
        void UpdateLineLabelDefaultSize(int size, int prevSize);

        /// <summary>
        /// Обновляет надписи линии.
        /// </summary>
        /// <param name="line">Линия.</param>
        void UpdateLineLabels(LineViewModel line);

        /// <summary>
        /// Обновляет отступ внутри планируемых линий.
        /// </summary>
        /// <param name="offset">Отступ.</param>
        void UpdateLinePlanningOffset(double offset);

        /// <summary>
        /// Обновляет толщину линий.
        /// </summary>
        /// <param name="thickness">Толщина.</param>
        /// <param name="prevThickness">Предыдущая толщина.</param>
        void UpdateLineThickness(double thickness, double prevThickness);

        #endregion
    }
}