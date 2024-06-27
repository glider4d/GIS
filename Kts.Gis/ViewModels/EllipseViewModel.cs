using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.Services;
using Kts.History;
using Kts.Messaging;
using Kts.Utilities;
using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления эллипса.
    /// </summary>
    [Serializable]
    internal sealed partial class EllipseViewModel : FigureViewModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EllipseViewModel"/>.
        /// </summary>
        /// <param name="ellipse">Эллипс.</param>
        /// <param name="layerHolder">Хранитель слоев.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="historyService">Сервис истории изменений.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public EllipseViewModel(FigureModel ellipse, ILayerHolder layerHolder, AccessService accessService, IDataService dataService, HistoryService historyService, IMapBindingService mapBindingService, IMessageService messageService) : base(ellipse, layerHolder, accessService, dataService, historyService, mapBindingService, messageService)
        {
            this.RegisterBinding();
        }

        #endregion
    }

    // Реализация FigureViewModel.
    internal sealed partial class EllipseViewModel
    {
        #region Открытые переопределенные свойства

        /// <summary>
        /// Возвращает или задает текстовое представление последовательности точек, из которых состоит фигура.
        /// </summary>
        public override string Points
        {
            get
            {
                return "";
            }
            set
            {
                throw new NotImplementedException("Не реализована возможность задания последовательности точек эллипсу");
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

            var viewModel = new EllipseViewModel(model, this.layerHolder, this.AccessService, this.DataService, this.HistoryService, this.MapBindingService, this.MessageService);

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