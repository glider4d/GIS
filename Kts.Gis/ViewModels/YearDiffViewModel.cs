using Kts.Gis.Services;
using Kts.Settings;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления слоя разделения линий по годам.
    /// </summary>
    internal sealed class YearDiffViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Идентификатор слоя разделения линий по годам.
        /// </summary>
        private Guid id;

        /// <summary>
        /// Видимость слоя разделения линий по годам.
        /// </summary>
        private bool isVisible;

        /// <summary>
        /// Линии слоя разделения линий по годам. 0 - новые линии, 1 - старые.
        /// </summary>
        private Dictionary<int, List<LineViewModel>> lines = new Dictionary<int, List<LineViewModel>>();

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис привязки представлений карты с моделями представлений.
        /// </summary>
        private readonly IMapBindingService mapBindingService;

        /// <summary>
        /// Ручка новой линии.
        /// </summary>
        private readonly Pen newLinePen;

        /// <summary>
        /// Ручка старой линии.
        /// </summary>
        private readonly Pen oldLinePen;

        /// <summary>
        /// Сервис настроек.
        /// </summary>
        private readonly ISettingService settingService;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="YearDiffViewModel"/>.
        /// </summary>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        /// <param name="settingService">Сервис настроек.</param>
        public YearDiffViewModel(IMapBindingService mapBindingService, ISettingService settingService)
        {
            this.mapBindingService = mapBindingService;
            this.settingService = settingService;

            // Получаем кисти для новых и старых линий.
            var newLineBrush = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(settingService.Settings["NewLineColor"].ToString()));
            var oldLineBrush = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(settingService.Settings["OldLineColor"].ToString()));
            if (newLineBrush.CanFreeze)
                newLineBrush.Freeze();
            if (oldLineBrush.CanFreeze)
                oldLineBrush.Freeze();
            this.newLinePen = new Pen(newLineBrush, 1.25);
            this.oldLinePen = new Pen(oldLineBrush, 1.25);
            if (this.newLinePen.CanFreeze)
                this.newLinePen.Freeze();
            if (this.oldLinePen.CanFreeze)
                this.oldLinePen.Freeze();
        }

        #endregion

        #region Закрытые свойства

        /// <summary>
        /// Возвращает или задает список линий слоя разделения линий по годам.
        /// </summary>
        private Dictionary<int, List<LineViewModel>> Lines
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
        /// Возвращает или задает видимость слоя разделения линий по годам.
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
                    {
                        // Переводим линии в словарь с ручками и объектами.
                        var result = new Dictionary<Pen, List<IObjectViewModel>>();
                        result.Add(this.newLinePen, this.Lines[0].ConvertAll(x => x as IObjectViewModel));
                        result.Add(this.oldLinePen, this.Lines[1].ConvertAll(x => x as IObjectViewModel));

                        this.id = this.mapBindingService.AddLayer(result, true);
                    }
                    else
                        this.mapBindingService.RemoveLayer(this.id);

                    this.NotifyPropertyChanged(nameof(this.IsVisible));
                }
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Задает линии слоя разделения линий по годам.
        /// </summary>
        /// <param name="newLines">Идентификаторы новых линий слоя разделения линий по годам.</param>
        /// <param name="oldLines">Идентификаторы старых линий слоя разделения линий по годам.</param>
        /// <param name="layerHolder">Хранитель слоев.</param>
        public void SetLines(List<Guid> newLines, List<Guid> oldLines, ILayerHolder layerHolder)
        {
            var result = new Dictionary<int, List<LineViewModel>>();

            result.Add(0, new List<LineViewModel>());
            result.Add(1, new List<LineViewModel>());

            LineViewModel wanted;

            foreach (var line in newLines)
            {
                wanted = layerHolder.GetObject(line) as LineViewModel;

                if (wanted != null)
                    result[0].Add(wanted);
            }
            
            foreach (var line in oldLines)
            {
                wanted = layerHolder.GetObject(line) as LineViewModel;

                if (wanted != null)
                    result[1].Add(wanted);
            }

            this.Lines = result;
        }

        /// <summary>
        /// Убирает линии со слоя разделения линий по годам.
        /// </summary>
        public void UnsetLines()
        {
            this.Lines = new Dictionary<int, List<LineViewModel>>();
        }

        #endregion
    }
}