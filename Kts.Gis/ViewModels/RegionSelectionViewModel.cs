using Kts.Gis.Data;
using Kts.Utilities;
using System.Collections.Generic;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления выбора региона, позволяющую выбирать все регионы и все населенные пункты.
    /// </summary>
    internal sealed partial class RegionSelectionViewModel : Reports.ViewModels.RegionSelectionViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что можно ли выбрать населенный пункт.
        /// </summary>
        private bool canSelectCity;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RegionSelectionViewModel"/>.
        /// </summary>
        /// <param name="permittedRegionIds">Идентификаторы разрешенных к выбору регионов.</param>
        /// <param name="dataService">Сервис данных.</param>
        public RegionSelectionViewModel(List<int> permittedRegionIds, IDataService dataService) : base(permittedRegionIds, dataService)
        {
            this.dataService = dataService;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает список, состоящий из искусственнего региона, представляющий все регионы.
        /// </summary>
        public List<Reports.ViewModels.RegionViewModel> AllRegions
        {
            get;
        } = new List<Reports.ViewModels.RegionViewModel>()
        {
            new Reports.ViewModels.RegionViewModel(new TerritorialEntityModel(-1, "Все"))
        };

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что можно ли выбрать населенный пункт.
        /// </summary>
        public bool CanSelectCity
        {
            get
            {
                return this.canSelectCity;
            }
            private set
            {
                this.canSelectCity = value;

                this.NotifyPropertyChanged(nameof(this.CanSelectCity));
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеется ли выбранный населенный пункт.
        /// </summary>
        public bool HasSelectedCity
        {
            get
            {
                return this.CanSelectCity && this.SelectedCity != this.AllCities[0];
            }
        }
        
        #endregion
    }

    // Реализация TerritorialEntitySelectionViewModel.
    internal sealed partial class RegionSelectionViewModel
    {
        #region Защищенные переопределенные методы

        /// <summary>
        /// Возвращает коллекцию населенных пунктов.
        /// </summary>
        /// <param name="region">Регион.</param>
        /// <returns>Коллекция населенных пунктов.</returns>
        protected override IEnumerable<Utilities.CityViewModel> GetCities(Utilities.RegionViewModel region)
        {
            var result = new List<Reports.ViewModels.CityViewModel>();

            if (region != this.AllRegions[0])
            {
                foreach (var city in (region as Reports.ViewModels.RegionViewModel).GetCities(this.dataService))
                    result.Add(city);

                this.CanSelectCity = true;
            }
            else
                this.CanSelectCity = false;

            return result;
        }

        /// <summary>
        /// Выполняет действия, связанные с загрузкой регионов.
        /// </summary>
        protected override void OnRegionsLoaded()
        {
            //System.Windows.MessageBox.Show("OnRegionsLoaded in 1");
            this.SelectedRegion = this.AllRegions[0];
            //System.Windows.MessageBox.Show("OnRegionsLoaded out 2");
        }

        #endregion
    }
}