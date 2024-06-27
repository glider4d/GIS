using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kts.Gis.ViewModels
{
    internal sealed class MeterInfoViewModel : BaseViewModel
    {
        private FigureViewModel boiler;
        private CancellationTokenSource cancellationTokenSource;
        private DateTime fromDate;
        private DateTime toDate;
        private bool isLoading;
        private bool isSubLoading;
        private readonly IDataService dataService;

        public MeterInfoViewModel(IDataService dataService)
        {
            this.dataService = dataService;

            this.toDate = DateTime.Now;
            
            this.fromDate = new DateTime(this.ToDate.Year, this.ToDate.Month, 1);

            this.PropertyChanged += this.MeterInfoViewModel_PropertyChanged;
        }

        private async void MeterInfoViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

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

        public int region
        {
            get;set;
        }

        public AdvancedObservableCollection<MeterInfoModel> InfoMeters
        {
            get;
        } = new AdvancedObservableCollection<MeterInfoModel>();
        /*
        public async Task UpdateRegionDataAsync(int cityID)
        {
            this.IsLoading = true;
            await Task.Factory.StartNew(new Action(() => region = this.dataService.MeterAccessService.getRegionID(cityID)));

        }*/

        private List<MeterInfoModel> getInfoMeters(int idcity, int meterID)
        {
            //int region = this.dataService.MeterAccessService.getRegionID(idcity);
            return this.dataService.MeterAccessService.GetMeterInfo(meterID, this.FromDate, this.ToDate);
        }
        private async Task UpdateMetersDataAsync(int meterID, int idcity)
        {
            //if (this.cancellationTokenSource != null)
            {
                this.IsLoading = true;

                //var ct = this.cancellationTokenSource.Token;
                /*
                if (this.boiler == null)
                    return;*/
                var boiler = this.boiler;

                List<MeterInfoModel> infoMeters = null;

                await Task.Factory.StartNew(new Action(() =>
                        infoMeters = getInfoMeters(idcity, meterID)
                ));

                //if (!ct.IsCancellationRequested)
                {
                    InfoMeters.Clear();
                    //this.FuelTypes.AddRange(fuelTypes);
                    InfoMeters.AddRange(infoMeters);
                    //this.NotifyPropertyChanged(nameof(this.HasFuelTypes));

                    //this.SelectedFuelType = this.FuelTypes.FirstOrDefault();

                    this.IsLoading = false;
                    this.IsSubLoading = false;
                }
            }
        }
        /*
        public async Task GetRegion(int idcity)
        {
            await this.UpdateRegionDataAsync(idcity);
        }*/
        public async Task SetBoilerAsync(int meterId, int idcity)//FigureViewModel boiler)
        {
            await this.UpdateMetersDataAsync(meterId, idcity);
            /*
            // Если уже имеется сигнализатор токена отмены, то пробуем отменить задачу.
            if (this.cancellationTokenSource != null)
                this.cancellationTokenSource.Cancel();

            // Создаем новый сигнализатор и запоминаем токен.
            var cts = new CancellationTokenSource();
            this.cancellationTokenSource = cts;
            var ct = cts.Token;

            this.boiler = boiler;
            if (this.dataService.MeterAccessService.CanAccessMeterInfo())
            {
            }
            
            if (this.dataService.FuelAccessService.CanAccessFuelInfo())
                await this.UpdateMetersDataAsync();
            else
            {
                

                

                
            }*/
        }

        public void UnsetBoiler()
        {
            // Если уже имеется сигнализатор токена отмены, то пробуем отменить задачу.
            if (this.cancellationTokenSource != null)
                this.cancellationTokenSource.Cancel();

            this.boiler = null;

            this.IsLoading = false;
            this.IsSubLoading = false;
        }

    }
}
