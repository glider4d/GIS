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
    /// Представляет модель представления слоя ремонтной программы.
    /// </summary>
    internal sealed class RPViewModel : BaseViewModel
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
        /// Инициализирует новый экземпляр класса <see cref="RPViewModel"/>.
        /// </summary>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        /// <param name="settingService">Сервис настроек.</param>
        public RPViewModel(IMapBindingService mapBindingService, ISettingService settingService)
        {
            this.mapBindingService = mapBindingService;
            this.settingService = settingService;

            // Получаем кисть.
            var brush = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(settingService.Settings["RPColor"].ToString()));
            if (brush.CanFreeze)
                brush.Freeze();
            this.pen = new Pen(brush, 1.25);
            if (this.pen.CanFreeze)
                this.pen.Freeze();
        }

        #endregion

        #region Закрытые свойства

        /// <summary>
        /// Список линий.
        /// </summary>
        private List<LineViewModel> lines;

        /// <summary>
        /// Возвращает или задает список линий.
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
        /// Видимость слоя ремонтной программы.
        /// </summary>
        private bool isVisible;

        /// <summary>
        /// Возвращает или задает видимость слоя ремонтной программы.
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
                        this.id = this.mapBindingService.AddLayer(this.Lines.Cast<IObjectViewModel>().ToList(), this.pen, true);
                    else
                        this.mapBindingService.RemoveLayer(this.id);

                    this.NotifyPropertyChanged(nameof(this.IsVisible));
                }
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Задает линии слоя.
        /// </summary>
        /// <param name="lines">Идентификаторы линий.</param>
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
        /// Убирает линии со слоя.
        /// </summary>
        public void UnsetLines()
        {
            this.Lines = new List<LineViewModel>();
        }

        #endregion
    }
}