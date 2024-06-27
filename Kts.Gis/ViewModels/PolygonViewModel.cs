using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.RevertibleActions;
using Kts.Gis.Services;
using Kts.History;
using Kts.Messaging;
using Kts.Utilities;
using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления многоугольника.
    /// </summary>
    [Serializable]
    internal sealed partial class PolygonViewModel : FigureViewModel
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Многоугольник.
        /// </summary>
        private readonly FigureModel polygon;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PolygonViewModel"/>.
        /// </summary>
        /// <param name="polygon">Многоугольник.</param>
        /// <param name="layerHolder">Хранитель слоев.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="historyService">Сервис истории изменений.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public PolygonViewModel(FigureModel polygon, ILayerHolder layerHolder, AccessService accessService, IDataService dataService, HistoryService historyService, IMapBindingService mapBindingService, IMessageService messageService) : base(polygon, layerHolder, accessService, dataService, historyService, mapBindingService, messageService)
        {
            this.polygon = polygon;

            this.RegisterBinding();
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Выполняет действия, связанные с изменением структуры вершин многоугольника.
        /// </summary>
        /// <param name="prevPoints">Предыдущая структура вершин.</param>
        /// <param name="prevPosition">Предыдущее положение многоугольника.</param>
        /// <param name="prevSize">Предыдущий размер многоугольника.</param>
        public void OnPointsChanged(string prevPoints, System.Windows.Point prevPosition, System.Windows.Size prevSize)
        {
            // При изменении структуры вершин необходимо проверять историю изменений, чтобы не создавать в ней кучу записей, так как вершины могут часто меняться.
            var exists = false;
            var entry = this.HistoryService.GetCurrentEntry();
            if (entry != null)
            {
                var action = entry.Action as ChangePolygonPointsAction;

                if (action != null && action.Polygon == this)
                {
                    action.NewPoints = this.Points;
                    action.NewPosition = this.Position;
                    action.NewSize = this.Size;

                    exists = true;
                }
            }
            if (!exists)
                this.HistoryService.Add(new HistoryEntry(new ChangePolygonPointsAction(this, prevPoints, prevPosition, prevSize, this.Points, this.Position, this.Size), Target.Data, "изменение вершин многоугольника"));
        }

        #endregion
    }

    // Реализация FigureViewModel.
    internal sealed partial class PolygonViewModel
    {
        #region Открытые переопределенные свойства

        /// <summary>
        /// Возвращает или задает текстовое представление последовательности точек, из которых состоит фигура.
        /// </summary>
        public override string Points
        {
            get
            {
                return this.polygon.Points;
            }
            set
            {
                if (this.Points != value)
                {
                    this.polygon.Points = value;

                    this.MapBindingService.SetMapObjectViewValue(this, nameof(this.Points), value);
                }
            }
        }

        #endregion

        #region Открытые переопределенные методы

        /// <summary>
        /// Возвращает копию объекта.
        /// </summary>
        /// <returns>Копия объекта.</returns>
        public override ICopyableObjectViewModel Copy()
        {
            var model = this.Figure.Clone();

            var pointerPosition = this.MapBindingService.GetPointerPosition();

            model.Id = ObjectModel.DefaultId;
            model.Position = new Point(pointerPosition.X - model.Size.Width / 2, pointerPosition.Y - model.Size.Height / 2);

            var viewModel = new PolygonViewModel(model, this.layerHolder, this.AccessService, this.DataService, this.HistoryService, this.MapBindingService, this.MessageService);

            // Добавляем значения редактируемых параметров копии объекта.
            this.LoadParameterValues();
            foreach (var entry in this.ParameterValuesViewModel.ParameterValueSet.ParameterValues)
                if (!string.IsNullOrEmpty(Convert.ToString(entry.Value)) && !this.Type.IsParameterReadonly(entry.Key))
                    viewModel.ChangeChangedValue(entry.Key, entry.Value);
            this.UnloadParameterValues();

            return viewModel;
        }

        #endregion
    }
}