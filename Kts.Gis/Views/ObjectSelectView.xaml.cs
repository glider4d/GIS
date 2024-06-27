using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.Services;
using Kts.Gis.Substrates;
using Kts.Gis.ViewModels;
using Kts.Messaging;
using Kts.Settings;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление выбора объекта.
    /// </summary>
    internal sealed partial class ObjectSelectView : Window, IDisposable
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что высвобождены ли ресурсы.
        /// </summary>
        private bool isDisposed;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Идентификатор населенного пункта.
        /// </summary>
        private readonly int cityId;

        /// <summary>
        /// Сервис привязки представлений карты с моделями представлений.
        /// </summary>
        private readonly ShapeMapBindingService mapBindingService;

        /// <summary>
        /// Модель представления.
        /// </summary>
        private readonly ObjectSelectViewModel viewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ObjectSelectView"/>.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        /// <param name="settingService">Сервис настроек.</param>
        /// <param name="substrateService">Сервис подложек.</param>
        public ObjectSelectView(int cityId, IDataService dataService, IMessageService messageService, ISettingService settingService, SubstrateService substrateService)
        {
            this.InitializeComponent();

            this.cityId = cityId;

            // Составляем словари с данными геометрий значков.
            var geometries = new Dictionary<ObjectType, Geometry>();
            var hotPoints = new Dictionary<ObjectType, Point>();
            var originPoints = new Dictionary<ObjectType, Point>();
            foreach (var entry in dataService.BadgeGeometries)
            {
                geometries.Add(entry.Type, (PathGeometry)XamlReader.Parse(entry.Geometry));
                hotPoints.Add(entry.Type, new Point(entry.HotPoint.X, entry.HotPoint.Y));
                originPoints.Add(entry.Type, new Point(entry.OriginPoint.X, entry.OriginPoint.Y));
            }

            // Замораживаем геометрии для повышения производительности.
            foreach (var geometry in geometries.Values)
                if (geometry.CanFreeze)
                    geometry.Freeze();

            var accessService = new AccessService(new List<int>(), new List<int>(), false, "Демо", false, false);
            
#warning Нужно будет переделать так, чтобы настройки вида карты подхватывались из источника данных
            this.mapBindingService = new ShapeMapBindingService(this.map, geometries, hotPoints, originPoints, accessService, new SqlMapSettingService(dataService));

            this.viewModel = new ObjectSelectViewModel(accessService, dataService, this.mapBindingService, messageService, settingService, substrateService);

            this.DataContext = this.viewModel;

            this.viewModel.CloseRequested += this.MainViewModel_CloseRequested;
            this.viewModel.LongTimeTaskRequested += this.MainViewModel_LongTimeTaskRequested;
            this.viewModel.MapLoaded += this.MainViewModel_MapLoaded;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.CloseRequested"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_CloseRequested(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.LongTimeTaskRequested"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_LongTimeTaskRequested(object sender, LongTimeTaskRequestedEventArgs e)
        {
            var view = new WaitView(e.WaitViewModel, this.Icon)
            {
                Owner = this
            };

            view.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="MainViewModel.MapLoaded"/> главной модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainViewModel_MapLoaded(object sender, EventArgs e)
        {
            this.map.Scale = 1;

            this.map.ScrollToCenter();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Window.Closed"/> представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void window_Closed(object sender, EventArgs e)
        {
            Console.WriteLine(this.viewModel.Result != null ? this.viewModel.Result.Id.ToString() : "");

            Application.Current.Shutdown();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="FrameworkElement.Loaded"/> представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            this.viewModel.Load(new CityViewModel(new Utilities.TerritorialEntityModel(this.cityId, "Демо"), true));
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
                    this.mapBindingService.Dispose();

                this.isDisposed = true;
            }
        }

        #endregion
    }

    // Реализация IDisposable.
    internal sealed partial class ObjectSelectView
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
}