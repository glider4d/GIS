using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления управления слоем кастомного объекта.
    /// </summary>
    internal sealed class ManageCustomObjectLayerViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Выбранный кастомный слой.
        /// </summary>
        private CustomLayerViewModel selectedLayer;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Кастомный объект.
        /// </summary>
        private readonly ICustomLayerObject customLayerObject;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса закрытия представления.
        /// </summary>
        public event EventHandler CloseRequested;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ManageCustomObjectLayerViewModel"/>.
        /// </summary>
        /// <param name="obj">Кастомный объект.</param>
        /// <param name="customLayers">Кастомные слои.</param>
        public ManageCustomObjectLayerViewModel(ICustomLayerObject obj, List<CustomLayerViewModel> customLayers)
        {
            this.customLayerObject = obj;
            this.CustomLayers = customLayers.ToList();

            this.CustomLayers.Insert(0, new CustomLayerViewModel());

            this.OKCommand = new RelayCommand(this.ExecuteOK);

            CustomLayerViewModel layer;

            if (customLayerObject.CustomLayer == null)
                layer = this.CustomLayers[0];
            else
            {
                layer = customLayers.FirstOrDefault(x => x.Id == customLayerObject.CustomLayer.Id);

                if (layer == null)
                    layer = this.CustomLayers[0];
            }

            this.SelectedLayer = layer;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает кастомные слои.
        /// </summary>
        public List<CustomLayerViewModel> CustomLayers
        {
            get;
        }

        /// <summary>
        /// Возвращает команду ОК.
        /// </summary>
        public RelayCommand OKCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает выбранный кастомный слой.
        /// </summary>
        public CustomLayerViewModel SelectedLayer
        {
            get
            {
                return this.selectedLayer;
            }
            set
            {
                this.selectedLayer = value;

                this.NotifyPropertyChanged(nameof(this.SelectedLayer));
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выполняет ОК.
        /// </summary>
        private void ExecuteOK()
        {
            if (this.SelectedLayer != null && this.SelectedLayer.IsEmpty)
                customLayerObject.SetCustomLayer(null);
            else
                customLayerObject.SetCustomLayer(this.SelectedLayer);

            this.CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}