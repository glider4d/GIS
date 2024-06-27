using Kts.Gis.Services;
using Kts.Settings;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления слоя несопоставленных объектов.
    /// </summary>
    internal sealed class IJSViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Идентификатор слоя.
        /// </summary>
        private Guid id;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис привязки представлений карты с моделями представлений.
        /// </summary>
        private readonly IMapBindingService mapBindingService;

        /// <summary>
        /// Ручка.
        /// </summary>
        private readonly Pen pen;

        /// <summary>
        /// Сервис настроек.
        /// </summary>
        private readonly ISettingService settingService;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="UOViewModel"/>.
        /// </summary>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        /// <param name="settingService">Сервис настроек.</param>
        public IJSViewModel(IMapBindingService mapBindingService, ISettingService settingService, string color = "#FF9800FF")
        {
            this.mapBindingService = mapBindingService;
            this.settingService = settingService;

            // Получаем кисть.
            var brush = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(color));
            if (brush.CanFreeze)
                brush.Freeze();
            this.pen = new Pen(brush, 8);
            if (this.pen.CanFreeze)
                this.pen.Freeze();
        }

        #endregion

        #region Закрытые свойства

        /// <summary>
        /// Список фигур.
        /// </summary>
        private List<FigureViewModel> figures;

        /// <summary>
        /// Возвращает или задает список фигур.
        /// </summary>
        private List<FigureViewModel> Figures
        {
            get
            {
                return this.figures;
            }
            set
            {
                this.figures = value;

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
        /// Видимость слоя.
        /// </summary>
        private bool isVisible;

        /// <summary>
        /// Возвращает или задает видимость слоя.
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
                        this.id = this.mapBindingService.AddLayer(this.Figures.Cast<IObjectViewModel>().ToList(), this.pen, true);
                    else
                        this.mapBindingService.RemoveLayer(this.id);

                    this.NotifyPropertyChanged(nameof(this.IsVisible));
                }
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Задает фигуры слоя.
        /// </summary>
        /// <param name="figures">Идентификаторы фигур.</param>
        /// <param name="layerHolder">Хранитель слоев.</param>
        public void SetFigures(List<Guid> figures, ILayerHolder layerHolder)
        {
            var result = new List<FigureViewModel>();

            FigureViewModel wanted;

            foreach (var figure in figures)
            {
                wanted = layerHolder.GetObject(figure) as FigureViewModel;

                if (wanted != null)
                    result.Add(wanted);
            }

            this.Figures = result;
        }

        /// <summary>
        /// Убирает фигуры со слоя.
        /// </summary>
        public void UnsetFigures()
        {
            this.Figures = new List<FigureViewModel>();
        }

        #endregion
    }
}