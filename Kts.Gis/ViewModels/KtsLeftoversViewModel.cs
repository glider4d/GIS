using Kts.Gis.Data;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления остатков сопоставления с базовыми программами КТС.
    /// </summary>
    internal sealed class KtsLeftoversViewModel : BaseViewModel
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Идентификатор программы.
        /// </summary>
        private readonly int appId;

        /// <summary>
        /// Идентификатор населенного пункта.
        /// </summary>
        private readonly int cityId;

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="KtsLeftoversViewModel"/>.
        /// </summary>
        /// <param name="hidden">Скрытые объекты.</param>
        /// <param name="visible">Отображаемые объекты.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="appId">Идентификатор программы.</param>
        /// <param name="dataService">Сервис данных.</param>
        public KtsLeftoversViewModel(List<Tuple<long, string>> hidden, List<Tuple<long, string>> visible, int cityId, int appId, IDataService dataService)
        {
            this.HiddenObjects.AddRange(hidden);
            this.VisibleObjects.AddRange(visible);
            this.cityId = cityId;
            this.appId = appId;
            this.dataService = dataService;

            this.HideCommand = new RelayCommand(this.ExecuteHide, this.CanExecuteHide);
            this.ShowCommand = new RelayCommand(this.ExecuteShow, this.CanExecuteShow);

            this.SelectedHidden = this.HiddenObjects.FirstOrDefault();
            this.SelectedVisible = this.VisibleObjects.FirstOrDefault();
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает список скрытых объектов.
        /// </summary>
        public AdvancedObservableCollection<Tuple<long, string>> HiddenObjects
        {
            get;
        } = new AdvancedObservableCollection<Tuple<long, string>>();

        /// <summary>
        /// Возвращает команду скрытия объекта.
        /// </summary>
        public RelayCommand HideCommand
        {
            get;
        }

        private Tuple<long, string> selectedHidden;
        /// <summary>
        /// Возвращает или задает выбранный скрытый объект.
        /// </summary>
        public Tuple<long, string> SelectedHidden
        {
            get
            {
                return this.selectedHidden;
            }
            set
            {
                this.selectedHidden = value;

                this.NotifyPropertyChanged(nameof(this.SelectedHidden));

                this.HideCommand.RaiseCanExecuteChanged();
            }
        }

        private Tuple<long, string> selectedVisible;
        /// <summary>
        /// Возвращает или задает выбранный отображаемый объект.
        /// </summary>
        public Tuple<long, string> SelectedVisible
        {
            get
            {
                return this.selectedVisible;
            }
            set
            {
                this.selectedVisible = value;

                this.NotifyPropertyChanged(nameof(this.SelectedVisible));

                this.ShowCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Возвращает команду отображения объекта.
        /// </summary>
        public RelayCommand ShowCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает список отображаемых объектов.
        /// </summary>
        public AdvancedObservableCollection<Tuple<long, string>> VisibleObjects
        {
            get;
        } = new AdvancedObservableCollection<Tuple<long, string>>();

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает true, если можно выполнить команду <see cref="HideCommand"/>, иначе - false.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteHide()
        {
            return this.SelectedVisible != null;
        }

        /// <summary>
        /// Возвращает true, если можно выполнить команду <see cref="ShowCommand"/>, иначе - false.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteShow()
        {
            return this.SelectedHidden != null;
        }

        /// <summary>
        /// Выполняет команду <see cref="HideCommand"/>.
        /// </summary>
        private void ExecuteHide()
        {
            this.dataService.KtsAccessService.HideObj(this.SelectedVisible.Item1, this.cityId, this.appId);

            var sel = this.SelectedVisible;

            this.VisibleObjects.Remove(sel);
            this.HiddenObjects.Add(sel);
            
            this.SelectedVisible = null;
        }

        /// <summary>
        /// Выполняет команду <see cref="ShowCommand"/>.
        /// </summary>
        private void ExecuteShow()
        {
            this.dataService.KtsAccessService.ShowObj(this.SelectedHidden.Item1, this.cityId, this.appId);

            var sel = this.SelectedHidden;

            this.HiddenObjects.Remove(sel);
            this.VisibleObjects.Add(sel);
            
            this.SelectedHidden = null;
        }

        #endregion
    }
}