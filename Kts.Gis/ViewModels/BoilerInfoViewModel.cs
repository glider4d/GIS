using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления информации о котельной.
    /// </summary>
    [Serializable]
    internal sealed class BoilerInfoViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Котельная.
        /// </summary>
        private FigureViewModel boiler;

        /// <summary>
        /// Сигнализатор токена отмена загрузки информации о котельной.
        /// </summary>
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Значение, указывающее на то, что отображается ли информация о котельной.
        /// </summary>
        private bool isBoilerInfoVisible;

        /// <summary>
        /// Значение, указывающее на то, что идет ли загрузка данных.
        /// </summary>
        private bool isLoading;

        /// <summary>
        /// Значение, указывающее на то, что идет ли подзагрузка данных.
        /// </summary>
        private bool isSubLoading;

        /// <summary>
        /// Последний выбранный тип трубы.
        /// </summary>
        private ObjectType lastSelectedPipeType;

        /// <summary>
        /// Схема.
        /// </summary>
        private SchemaModel schema;

        /// <summary>
        /// Выбранный тип трубы.
        /// </summary>
        private ObjectType selectedPipeType;

        /// <summary>
        /// Общая протяженность труб по дате.
        /// </summary>
        private double totalLengthByDate = 0;

        /// <summary>
        /// Общая протяженность труб по диаметрам.
        /// </summary>
        private double totalLengthByDiameter = 0;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="BoilerInfoViewModel"/>.
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="dataService">Сервис данных.</param>
        public BoilerInfoViewModel(IDataService dataService)
        {
            this.dataService = dataService;

            paramNames = new AdvancedObservableCollection<string>();

            paramNames.Add("Температура");
            paramNames.Add("Объем");
            paramNames.Add("Теплоэнергия");
            paramNames.Add("Масса");



            this.FuelInfo = new FuelInfoViewModel(dataService);
            this.meterInfo = new MeterInfoViewModel(dataService);

            this.PropertyChanged += this.BoilerInfoViewModel_PropertyChanged;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает даты ввода труб и их протяженность.
        /// </summary>
        public AdvancedObservableCollection<Tuple<int, double>> Dates
        {
            get;
        } = new AdvancedObservableCollection<Tuple<int, double>>();

        /// <summary>
        /// Возвращает информацию о топливе котельной.
        /// </summary>
        public FuelInfoViewModel FuelInfo
        {
            get;
        }

        public MeterInfoViewModel meterInfo
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что отображается ли информация о котельной.
        /// </summary>
        public bool IsBoilerInfoVisible
        {
            get
            {
                return this.isBoilerInfoVisible;
            }
            set
            {
                this.isBoilerInfoVisible = value;

                this.NotifyPropertyChanged(nameof(this.IsBoilerInfoVisible));
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
        /// Возвращает длины труб.
        /// </summary>
        public AdvancedObservableCollection<Tuple<int, double>> Lengths
        {
            get;
        } = new AdvancedObservableCollection<Tuple<int, double>>();

        /// <summary>
        /// Возвращает типы труб, которые подсоединены к котельной.
        /// </summary>
        public AdvancedObservableCollection<ObjectType> PipeTypes
        {
            get;
        } = new AdvancedObservableCollection<ObjectType>();

        public AdvancedObservableCollection<MeterInfoModel> MeterInfoModels
        {
            get;
        } = new AdvancedObservableCollection<MeterInfoModel>();

        public AdvancedObservableCollection<string> paramNames
        {
            get; set;
        }



        string m_paramSelected = "Температура";

        private int indexForParamName(string paramName)
        {
            return paramNames.IndexOf(paramName);
        }

        private int indexParams = 0;

        public string paramSelected//0 - температура, 1 - объем, 2 - теплота, 3 - масса
        {
            get
            {
                return m_paramSelected;
            }
            set
            {
                indexParams = indexForParamName(value);
                foreach (var meterModel in MeterInfoModels)
                    meterModel.selectParam(indexParams);
                m_paramSelected = value;
            }
        }

        /// <summary>
        /// Возвращает или задает выбранный тип трубы.
        /// </summary>
        public ObjectType SelectedPipeType
        {
            get
            {
                return this.selectedPipeType;
            }
            set
            {
                this.selectedPipeType = value;

                if (value != null)
                    this.lastSelectedPipeType = value;

                this.NotifyPropertyChanged(nameof(this.SelectedPipeType));
            }
        }

        /// <summary>
        /// Возвращает или задает общую протяженность труб по дате.
        /// </summary>
        public double TotalLengthByDate
        {
            get
            {
                return this.totalLengthByDate;
            }
            private set
            {
                this.totalLengthByDate = Math.Round(value, 2);

                this.NotifyPropertyChanged(nameof(this.TotalLengthByDate));
            }
        }

        /// <summary>
        /// Возвращает или задает общую протяженность труб по диаметру.
        /// </summary>
        public double TotalLengthByDiameter
        {
            get
            {
                return this.totalLengthByDiameter;
            }
            private set
            {
                this.totalLengthByDiameter = Math.Round(value, 2);

                this.NotifyPropertyChanged(nameof(this.TotalLengthByDiameter));
            }
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="INotifyPropertyChanged.PropertyChanged"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private async void BoilerInfoViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.SelectedPipeType))
            {
                
                this.Lengths.Clear();
                this.Dates.Clear();

                var pipeType = this.SelectedPipeType;
                var boiler = this.boiler;
                var schema = this.schema;

                if (pipeType != null && boiler != null && schema != null)
                {
                    this.IsSubLoading = true;

                    var ct = this.cancellationTokenSource.Token;

                    List<Tuple<int, double>> lengths = null;
                    List<Tuple<int, double>> dates = null;

                    if (pipeType.TypeId == -1)
                        await Task.Factory.StartNew(new Action(() =>
                        {
                            lengths = this.dataService.BoilerInfoAccessService.GetPipeLengths(boiler.Id, schema);
                            dates = this.dataService.BoilerInfoAccessService.GetPipeDates(boiler.Id, schema);
                        }));
                    else
                        await Task.Factory.StartNew(new Action(() =>
                        {
                            lengths = this.dataService.BoilerInfoAccessService.GetPipeLengths(boiler.Id, pipeType.TypeId, schema);
                            dates = this.dataService.BoilerInfoAccessService.GetPipeDates(boiler.Id, pipeType.TypeId, schema);
                        }));

                    if (!ct.IsCancellationRequested)
                    {
                        this.Lengths.Clear();
                        this.Dates.Clear();
                        this.Lengths.AddRange(lengths);
                        this.Dates.AddRange(dates);

                        // Вычисляем общую протяженность труб по диаметрам и датам.
                        var sumLength = 0.0;
                        var sumDate = 0.0;
                        await Task.Factory.StartNew(new Action(() =>
                        {
                            foreach (var entry in lengths)
                                sumLength += entry.Item2;
                            foreach (var entry in dates)
                                sumDate += entry.Item2;
                        }));

                        if (!ct.IsCancellationRequested)
                        {
                            this.TotalLengthByDiameter = sumLength;
                            this.TotalLengthByDate = sumDate;

                            this.IsSubLoading = false;
                        }
                    }
                }
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Асинхронно задает котельную.
        /// </summary>
        /// <param name="boiler">Котельная.</param>
        /// <param name="schema">Схема.</param>
        public async Task SetBoilerAsync(FigureViewModel boiler, SchemaModel schema)
        {
            try
            {
                


                // Если уже имеется сигнализатор токена отмены, то пробуем отменить задачу.
                if (this.cancellationTokenSource != null)
                    this.cancellationTokenSource.Cancel();

                this.IsLoading = true;
                this.FuelInfo.IsLoading = true;
                this.meterInfo.IsLoading = true;

                // Создаем новый сигнализатор и запоминаем токен.
                var cts = new CancellationTokenSource();
                this.cancellationTokenSource = cts;
                var ct = cts.Token;

                if (!ct.IsCancellationRequested)
                {
                    this.boiler = boiler;
                    this.schema = schema;
                    MeterInfoModels.Clear();
                    this.IsBoilerInfoVisible = true;

                    // Получаем типы труб котельной.
                    List<ObjectType> pipeTypes = null;
                    await Task.Factory.StartNew(new Action(() => pipeTypes = this.dataService.BoilerInfoAccessService.GetPipeTypes(boiler.Id, schema)));

                    if (!ct.IsCancellationRequested)
                    {
                        // Заполняем типы труб.
                        this.PipeTypes.Clear();
                        this.PipeTypes.Add(new ObjectType(-1, "Все", "Все", ObjectKind.None, new Color(0, 0, 0), new Color(0, 0, 0), false));
                        this.PipeTypes.AddRange(pipeTypes);
                        // И выбираем один из них.
                        if (this.lastSelectedPipeType != null && this.PipeTypes.Contains(this.lastSelectedPipeType))
                            this.SelectedPipeType = this.lastSelectedPipeType;
                        else
                            this.SelectedPipeType = this.PipeTypes[0];

                        if (!ct.IsCancellationRequested)
                        {
                            // Передаем котельную модели представления информации о топливе.
                            await this.FuelInfo.SetBoilerAsync(boiler);
                            //await this.meterInfo.SetBoilerAsync(boiler);
                            if (boiler.ParameterValuesViewModel != null)
                            {
                                foreach (var value in boiler.ParameterValuesViewModel.Parameters)
                                {
                                    ParameterModel param = value.getParam();

                                    //1005 номер параметра со счетчиками
                                    //int paramValue = (int)value.Value;
                                    if (param.Id == 1005)
                                    {
                                        int paramValue = Convert.ToInt32(value.Value);
                                        //await this.meterInfo.GetRegion(boiler.CityId);
                                        await this.meterInfo.SetBoilerAsync(paramValue, boiler.CityId);
                                        MeterInfoModels.AddRange(meterInfo.InfoMeters);
                                    }


                                }
                            }
                            //boiler.ParameterValuesViewModel.Parameters[]
                            this.IsLoading = false;
                            this.FuelInfo.IsLoading = false;
                            this.meterInfo.IsLoading = false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("message : " + e.Message);
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

            this.IsBoilerInfoVisible = false;

            this.boiler = null;

            this.SelectedPipeType = null;
            
            this.PipeTypes.Clear();

            this.TotalLengthByDiameter = 0;
            this.TotalLengthByDate = 0;

            // Убираем котельную с модели представления информации о топливе.
            this.FuelInfo.UnsetBoiler();

            this.IsLoading = false;
            this.FuelInfo.IsLoading = false;
        }

        #endregion
    }
}