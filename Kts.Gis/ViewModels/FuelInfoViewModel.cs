using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления информации о топливе котельной.
    /// </summary>
    internal sealed class FuelInfoViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Наличие на начало месяца.
        /// </summary>
        private double availableCount;

        /// <summary>
        /// Котельная.
        /// </summary>
        private FigureViewModel m_boiler;

        private FigureViewModel boiler
        {
            get
            {
                return m_boiler;
            }
            set
            {
                m_boiler = value;
            }
        }


        /// <summary>
        /// Сигнализатор токена отмена загрузки информации о топливе котельной.
        /// </summary>
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Расход.
        /// </summary>
        private double consumption;

        /// <summary>
        /// Остаток дней.
        /// </summary>
        private int dayBalance;

        /// <summary>
        /// Лимит (в сутки).
        /// </summary>
        private double dayLimit;

        /// <summary>
        /// Остаток на конец периода.
        /// </summary>
        private double endBalance;

        /// <summary>
        /// Дата с.
        /// </summary>
        private DateTime fromDate;

        /// <summary>
        /// Значение, указывающее на то, что имеется ли остаток дней.
        /// </summary>
        private bool hasDayBalance;

        /// <summary>
        /// Значение, указывающее на то, что имеется ли значение обеспеченности.
        /// </summary>
        private bool hasProvision;

        /// <summary>
        /// Приход.
        /// </summary>
        private double incoming;

        /// <summary>
        /// Значение, указывающее на то, что идет ли загрузка данных.
        /// </summary>
        private bool isLoading;

        /// <summary>
        /// Значение, указывающее на то, что идет ли подзагрузка данных.
        /// </summary>
        private bool isSubLoading;

        /// <summary>
        /// Лимит (в месяц).
        /// </summary>
        private double monthLimit;

        /// <summary>
        /// Значение перемещения.
        /// </summary>
        private double moving;

        /// <summary>
        /// Обеспеченность.
        /// </summary>
        private string provision;

        /// <summary>
        /// Выбранный тип топлива.
        /// </summary>
        private FuelModel selectedFuelType;

        /// <summary>
        /// Дата по.
        /// </summary>
        private DateTime toDate;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="FuelInfoViewModel"/>.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        public FuelInfoViewModel(IDataService dataService)
        {
            this.dataService = dataService;

            this.toDate = DateTime.Now;
            this.fromDate = new DateTime(this.ToDate.Year, this.ToDate.Month, 1);

            this.PropertyChanged += this.FuelInfoViewModel_PropertyChanged;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает наличие на начало месяца.
        /// </summary>
        public double AvailableCount
        {
            get
            {
                return this.availableCount;
            }
            private set
            {
                this.availableCount = value;

                this.NotifyPropertyChanged(nameof(this.AvailableCount));
            }
        }

        /// <summary>
        /// Возвращает или задает расход.
        /// </summary>
        public double Consumption
        {
            get
            {
                return this.consumption;
            }
            private set
            {
                this.consumption = value;

                this.NotifyPropertyChanged(nameof(this.Consumption));
            }
        }

        /// <summary>
        /// Возвращает или задает остаток дней.
        /// </summary>
        public int DayBalance
        {
            get
            {
                return this.dayBalance;
            }
            private set
            {
                this.dayBalance = value;

                this.NotifyPropertyChanged(nameof(this.DayBalance));
            }
        }

        /// <summary>
        /// Возвращает или задает лимит (в сутки).
        /// </summary>
        public double DayLimit
        {
            get
            {
                return this.dayLimit;
            }
            private set
            {
                this.dayLimit = value;

                this.NotifyPropertyChanged(nameof(this.DayLimit));
            }
        }

        /// <summary>
        /// Возвращает или задает остаток на конец периода.
        /// </summary>
        public double EndBalance
        {
            get
            {
                return this.endBalance;
            }
            private set
            {
                this.endBalance = value;

                this.NotifyPropertyChanged(nameof(this.EndBalance));
            }
        }

        /// <summary>
        /// Возвращает или задает дату с.
        /// </summary>
        public DateTime FromDate
        {
            get
            {
                return this.fromDate;
            }
            set
            {
                this.fromDate = value;

                this.NotifyPropertyChanged(nameof(this.FromDate));
            }
        }

        /// <summary>
        /// Возвращает виды топлива.
        /// </summary>
        public AdvancedObservableCollection<FuelModel> FuelTypes
        {
            get;
        } = new AdvancedObservableCollection<FuelModel>();

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что имеется ли остаток дней.
        /// </summary>
        public bool HasDayBalance
        {
            get
            {
                return this.hasDayBalance;
            }
            private set
            {
                this.hasDayBalance = value;

                this.NotifyPropertyChanged(nameof(this.HasDayBalance));
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеются ли виды топлива.
        /// </summary>
        public bool HasFuelTypes
        {
            get
            {
                return this.FuelTypes.Count > 0;
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что имеется ли значение обеспеченности.
        /// </summary>
        public bool HasProvision
        {
            get
            {
                return this.hasProvision;
            }
            private set
            {
                this.hasProvision = value;

                this.NotifyPropertyChanged(nameof(this.HasProvision));
            }
        }

        /// <summary>
        /// Возвращает или задает приход.
        /// </summary>
        public double Incoming
        {
            get
            {
                return this.incoming;
            }
            private set
            {
                this.incoming = value;

                this.NotifyPropertyChanged(nameof(this.Incoming));
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что идет ли загрузка данных.
        /// </summary>
        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }
            set
            {
                this.isLoading = value;

                this.NotifyPropertyChanged(nameof(this.IsLoading));
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что идет ли подзагрузка данных.
        /// </summary>
        public bool IsSubLoading
        {
            get
            {
                return this.isSubLoading;
            }
            set
            {
                this.isSubLoading = value;

                this.NotifyPropertyChanged(nameof(this.IsSubLoading));
            }
        }

        /// <summary>
        /// Возвращает или задает лимит (в месяц).
        /// </summary>
        public double MonthLimit
        {
            get
            {
                return this.monthLimit;
            }
            private set
            {
                this.monthLimit = value;

                this.NotifyPropertyChanged(nameof(this.MonthLimit));
            }
        }

        /// <summary>
        /// Возвращает или задает значение перемещения.
        /// </summary>
        public double Moving
        {
            get
            {
                return this.moving;
            }
            private set
            {
                this.moving = value;

                this.NotifyPropertyChanged(nameof(this.Moving));
            }
        }

        /// <summary>
        /// Возвращает или задает обеспеченность.
        /// </summary>
        public string Provision
        {
            get
            {
                return this.provision;
            }
            private set
            {
                this.provision = value;

                this.NotifyPropertyChanged(nameof(this.Provision));
            }
        }

        /// <summary>
        /// Возвращает или задает выбранный тип топлива.
        /// </summary>
        public FuelModel SelectedFuelType
        {
            get
            {
                return this.selectedFuelType;
            }
            set
            {
                this.selectedFuelType = value;

                this.NotifyPropertyChanged(nameof(this.SelectedFuelType));
            }
        }

        /// <summary>
        /// Возвращает информацию о складах топлива.
        /// </summary>
        public AdvancedObservableCollection<FuelStorageModel> Storages
        {
            get;
        } = new AdvancedObservableCollection<FuelStorageModel>();

        /// <summary>
        /// Возвращает или задает дату по.
        /// </summary>
        public DateTime ToDate
        {
            get
            {
                return this.toDate;
            }
            set
            {
                this.toDate = value;

                this.NotifyPropertyChanged(nameof(this.ToDate));
            }
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="INotifyPropertyChanged.PropertyChanged"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private async void FuelInfoViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(this.FromDate):
                    if (this.FromDate > this.ToDate)
                        this.ToDate = this.FromDate;
                    else
                        await this.UpdateFuelTypesAsync();

                    break;

                case nameof(this.SelectedFuelType):
                    await this.UpdateAsync();

                    break;

                case nameof(this.ToDate):
                    await this.UpdateFuelTypesAsync();

                    break;
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Асинхронно обновляет данные о топливе котельной.
        /// </summary>
        private async Task UpdateAsync()
        {
            this.IsSubLoading = true;

            if (this.SelectedFuelType == null)
            {
                this.AvailableCount = 0;
                this.MonthLimit = 0;
                this.DayLimit = 0;
                this.Incoming = 0;
                this.Consumption = 0;
                this.EndBalance = 0;
                this.DayBalance = 0;
                this.Moving = 0;
                this.Provision = "";

                this.Storages.Clear();

                this.IsSubLoading = false;
            }
            else
            {
                var ct = this.cancellationTokenSource.Token;

                var boiler = this.boiler;
                var selectedFuelType = this.SelectedFuelType;

                FuelInfoModel fuelInfo = null;
                List<FuelStorageModel> storages = null;

                await Task.Factory.StartNew(new Action(() =>
                {
                    fuelInfo = this.dataService.FuelAccessService.GetFuelInfo(boiler.Id, selectedFuelType.Id, this.FromDate, this.ToDate);
                    storages = this.dataService.FuelAccessService.GetStoragesInfo(boiler.Id, selectedFuelType.Id, this.FromDate, this.ToDate);
                }));

                if (!ct.IsCancellationRequested)
                {
                    if (fuelInfo != null)
                    {
                        this.AvailableCount = fuelInfo.AvailableCount;
                        this.MonthLimit = fuelInfo.MonthLimit;
                        this.DayLimit = fuelInfo.DayLimit;
                        this.Incoming = fuelInfo.Incoming;
                        this.Consumption = fuelInfo.Consumption;
                        this.EndBalance = fuelInfo.EndBalance;
                        this.DayBalance = fuelInfo.DayBalance;
                        this.HasDayBalance = fuelInfo.HasDayBalance;
                        this.Moving = fuelInfo.Moving;
                        this.Provision = fuelInfo.Provision;
                        this.HasProvision = !string.IsNullOrEmpty(fuelInfo.Provision);
                    }
                    else
                    {
                        this.AvailableCount = 0;
                        this.MonthLimit = 0;
                        this.DayLimit = 0;
                        this.Incoming = 0;
                        this.Consumption = 0;
                        this.EndBalance = 0;
                        this.DayBalance = 0;
                        this.HasDayBalance = false;
                        this.Moving = 0;
                        this.Provision = "";
                        this.HasProvision = false;
                    }

                    this.Storages.AddRange(storages);

                    this.IsSubLoading = false;
                }
            }
        }

        /// <summary>
        /// Асинхронно обновляет виды топлива.
        /// </summary>
        private async Task UpdateFuelTypesAsync()
        {
            if (this.cancellationTokenSource != null)
            {
                if (this.boiler == null)
                    return;
                this.IsLoading = true;

                var ct = this.cancellationTokenSource.Token;

                var boiler = this.boiler;

                List<FuelModel> fuelTypes = null;

                await Task.Factory.StartNew(new Action(() => fuelTypes = this.dataService.FuelAccessService.GetFuelTypes(boiler.Id, this.FromDate, this.ToDate)));

                if (!ct.IsCancellationRequested)
                {
                    this.FuelTypes.Clear();
                    this.FuelTypes.AddRange(fuelTypes);

                    this.NotifyPropertyChanged(nameof(this.HasFuelTypes));

                    this.SelectedFuelType = this.FuelTypes.FirstOrDefault();

                    this.IsLoading = false;
                }
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Асинхронно задает котельную.
        /// </summary>
        /// <param name="boiler">Котельная.</param>
        public async Task SetBoilerAsync(FigureViewModel boiler)
        {
            // Если уже имеется сигнализатор токена отмены, то пробуем отменить задачу.
            if (this.cancellationTokenSource != null)
                this.cancellationTokenSource.Cancel();

            // Создаем новый сигнализатор и запоминаем токен.
            var cts = new CancellationTokenSource();
            this.cancellationTokenSource = cts;
            var ct = cts.Token;

            this.boiler = boiler;

            if (this.dataService.FuelAccessService.CanAccessFuelInfo())
                await this.UpdateFuelTypesAsync();
            else
            {
                this.FuelTypes.Clear();

                this.NotifyPropertyChanged(nameof(this.HasFuelTypes));

                this.SelectedFuelType = null;
            }
        }

        /// <summary>
        /// Убирает котельную.
        /// </summary>
        public void UnsetBoiler()
        {
            // Если уже имеется сигнализатор токена отмены, то пробуем отменить задачу.
            if (this.cancellationTokenSource != null)
                this.cancellationTokenSource.Cancel();

            this.boiler = null;

            this.IsLoading = false;
            this.IsSubLoading = false;
        }

        #endregion
    }
}