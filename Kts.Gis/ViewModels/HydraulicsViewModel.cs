using Kts.Gis.Services;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления слоя гидравлики.
    /// </summary>
    internal sealed class HydraulicsViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Идентификатор слоя гидравлики.
        /// </summary>
        private Guid id;

        /// <summary>
        /// Видимость слоя гидравлики.
        /// </summary>
        private bool isVisible;

        /// <summary>
        /// Линии слоя гидравлики.
        /// </summary>
        private List<LineViewModel> lines = new List<LineViewModel>();

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис привязки представлений карты с моделями представлений.
        /// </summary>
        private readonly IMapBindingService mapBindingService;

        #endregion

        #region Закрытые статические поля

        /// <summary>
        /// Ручка обводки линий.
        /// </summary>
        private static Pen pen;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="HydraulicsViewModel"/>.
        /// </summary>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        public HydraulicsViewModel(IMapBindingService mapBindingService)
        {
            this.mapBindingService = mapBindingService;
        }

        #endregion

        #region Статические конструкторы

        /// <summary>
        /// Инициализирует статические члены класса <see cref="HydraulicsViewModel"/>.
        /// </summary>
        static HydraulicsViewModel()
        {
            pen = new Pen(new SolidColorBrush(Colors.Yellow), 2);

            if (pen.CanFreeze)
                pen.Freeze();
        }

        #endregion

        #region Закрытые свойства
        
        /// <summary>
        /// Возвращает или задает список линий слоя гидравлики.
        /// </summary>
        private List<LineViewModel> Lines
        {
            get
            {
                return this.lines;
            }
            set
            {
                this.lines = value;

                // Если линии сейчас видны, то нужно вручную скрыть и отобразить слой.
                if (this.IsVisible)
                {
                    this.IsVisible = false;

                    this.IsVisible = true;
                }
            }
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает видимость слоя гидравлики.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                return this.isVisible;
            }
            set
            {
                if (this.IsVisible != value)
                {
                    this.isVisible = value;

                    if (value)
                        this.id = this.mapBindingService.AddLayer(this.Lines.ConvertAll(x => x as IObjectViewModel), pen, false);
                    else
                        this.mapBindingService.RemoveLayer(this.id);

                    this.NotifyPropertyChanged(nameof(this.IsVisible));
                }
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Задает линии слоя гидравлики.
        /// </summary>
        /// <param name="lines">Идентификаторы линий слоя гидравлики.</param>
        /// <param name="layerHolder">Хранитель слоев.</param>
        public void SetLines(List<Guid> lines, ILayerHolder layerHolder)
        {
            var result = new List<LineViewModel>();

            LineViewModel wanted;

            foreach (var line in lines)
            {
                wanted = layerHolder.GetObject(line) as LineViewModel;

                if (wanted != null)
                    result.Add(wanted);
            }

            this.Lines = result;
        }

        /// <summary>
        /// Убирает линии со слоя гидравлики.
        /// </summary>
        public void UnsetLines()
        {
            this.Lines = new List<LineViewModel>();
        }

        #endregion
    }
}