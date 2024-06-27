using Kts.Gis.Data;
using Kts.Gis.Data.Interfaces;
using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kts.Gis.ViewModels
{
    internal sealed class BuildingInfoViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Котельная.
        /// </summary>
        private FigureViewModel building;

        /// <summary>
        /// Сигнализатор токена отмена загрузки информации о котельной.
        /// </summary>
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Значение, указывающее на то, что отображается ли информация о котельной.
        /// </summary>
        private bool isBuildingInfoVisible;

        /// <summary>
        /// Значение, указывающее на то, что идет ли загрузка данных.
        /// </summary>
        private bool isLoading;

        /// <summary>
        /// Значение, указывающее на то, что идет ли подзагрузка данных.
        /// </summary>
        private bool isSubLoading;

       

        /// <summary>
        /// Схема.
        /// </summary>
        private SchemaModel schema;

     

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        #endregion


        public AdvancedObservableCollection<string> paramNames
        {
            get; set;
        }

        private async void BuildingInfoViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var building = this.building;
            var schema = this.schema;


            if (building != null && schema != null)
            {
                //this.IsSubLoading = true;

                var ct = this.cancellationTokenSource.Token; 


                await Task.Factory.StartNew(new Action(() =>
                {
                    //lengths = this.dataService.BoilerInfoAccessService.GetPipeLengths(boiler.Id, schema);
                    //dates = this.dataService.BoilerInfoAccessService.GetPipeDates(boiler.Id, schema);
                }));


            }
        }

        public BuildingInfoViewModel(IDataService dataService)
        {
            this.dataService = dataService;

            paramNames = new AdvancedObservableCollection<string>();

            paramNames.Add("Общее потребление");
            paramNames.Add("Потери для прочих");
            paramNames.Add("Отказы");

            


            this.PropertyChanged += this.BuildingInfoViewModel_PropertyChanged;
        }



        public AdvancedObservableCollection<BuilderInfo> builderInfo
        {
            get;
        } = new AdvancedObservableCollection<BuilderInfo>();


        public async Task SetBuildingAsync(FigureViewModel building, SchemaModel schema)
        {
            try
            {



                // Если уже имеется сигнализатор токена отмены, то пробуем отменить задачу.
                if (this.cancellationTokenSource != null)
                    this.cancellationTokenSource.Cancel();

                this.IsLoading = true;


                // Создаем новый сигнализатор и запоминаем токен.
                var cts = new CancellationTokenSource();
                this.cancellationTokenSource = cts;
                var ct = cts.Token;

                if (!ct.IsCancellationRequested)
                {
                    this.building = building;
                    this.schema = schema; 
                    this.IsBuildingInfoVisible = true;

                    // Получаем типы труб котельной.
                    List<BuilderInfo> builderInfo = null;
                    await Task.Factory.StartNew(new Action(() => builderInfo = this.dataService.BuilderInfoAccessService.GetBuilderInfo(building.Id, schema)));
                    this.builderInfo.Clear();
                    this.builderInfo.AddRange(builderInfo);



                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("message : " + e.Message);
            }
        }


        public void UnsetBuilding()
        {
            // Если уже имеется сигнализатор токена отмены, то пробуем отменить задачу.
            if (this.cancellationTokenSource != null)
                this.cancellationTokenSource.Cancel();

            this.IsBuildingInfoVisible = false;

            this.building = null;

        }


        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что отображается ли информация о здании.
        /// </summary>
        public bool IsBuildingInfoVisible
        {
            get
            {
                return this.isBuildingInfoVisible;
            }
            set
            {
                this.isBuildingInfoVisible = value;

                this.NotifyPropertyChanged(nameof(this.IsBuildingInfoVisible));
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






















    }
}
