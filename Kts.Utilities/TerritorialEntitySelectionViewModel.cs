using System;
using System.Collections.Generic;

namespace Kts.Utilities
{
    /// <summary>
    /// Представляет базовую модель представления выбора территориальной единицы.
    /// </summary>
    [Serializable]
    public abstract class TerritorialEntitySelectionViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Выбранный населенный пункт.
        /// </summary>
        private CityViewModel selectedCity;

        /// <summary>
        /// Выбранный регион.
        /// </summary>
        private RegionViewModel selectedRegion;

        /// <summary>
        /// Выбранная улица.
        /// </summary>
        private StreetViewModel selectedStreet;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Глубина выбора.
        /// </summary>
        //[NonSerialized]
        private readonly SelectionDepth selectionDepth;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие изменения выбора конечной территориальной единицы.
        /// </summary>
        public event EventHandler SelectionChanged;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TerritorialEntitySelectionViewModel"/>.
        /// </summary>
        /// <param name="depth">Глубина выбора территориальной единицы.</param>
        public TerritorialEntitySelectionViewModel(SelectionDepth depth)
        {
            this.selectionDepth = depth;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает коллекцию населенных пунктов.
        /// </summary>
        public AdvancedObservableCollection<CityViewModel> Cities
        {
            get;
        } = new AdvancedObservableCollection<CityViewModel>();

        /// <summary>
        /// Возвращает коллекцию регионов.
        /// </summary>
        public AdvancedObservableCollection<RegionViewModel> Regions
        {
            get;
        } = new AdvancedObservableCollection<RegionViewModel>();

        /// <summary>
        /// Возвращает коллекцию улиц.
        /// </summary>
        public AdvancedObservableCollection<StreetViewModel> Streets
        {
            get;
        } = new AdvancedObservableCollection<StreetViewModel>();

        /// <summary>
        /// Возвращает или задает выбранный населенный пункт.
        /// </summary>
        public CityViewModel SelectedCity
        {
            get
            {
                return this.selectedCity;
            }
            set
            {
                this.selectedCity = value;

                this.NotifyPropertyChanged(nameof(this.SelectedCity));

                if (value != null)
                    if (this.selectionDepth > SelectionDepth.City)
                        this.FillStreets(this.GetStreets(value));
                    else
                        if (this.selectionDepth == SelectionDepth.City && this.SelectionChanged != null)
                            this.SelectionChanged(this, EventArgs.Empty);

                this.OnSelectionChanged();
            }
        }

        /// <summary>
        /// Возвращает или задает выбранный регион.
        /// </summary>
        public RegionViewModel SelectedRegion
        {
            get
            {
                return this.selectedRegion;
            }
            set
            {
                this.selectedRegion = value;

                this.NotifyPropertyChanged(nameof(this.SelectedRegion));

                if (value != null)
                    if (this.selectionDepth > SelectionDepth.Region)
                        this.FillCities(this.GetCities(value));
            }
        }

        /// <summary>
        /// Возвращает или задает выбранную улицу.
        /// </summary>
        public StreetViewModel SelectedStreet
        {
            get
            {
                return this.selectedStreet;
            }
            set
            {
                this.selectedStreet = value;

                this.NotifyPropertyChanged(nameof(this.SelectedStreet));

                if (this.selectionDepth == SelectionDepth.Street && this.SelectionChanged != null)
                    this.SelectionChanged(this, EventArgs.Empty);

                this.OnSelectionChanged();
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Заполняет коллекцию населенных пунктов.
        /// </summary>
        /// <param name="cities">Населенные пункты.</param>
        private void FillCities(IEnumerable<CityViewModel> cities)
        {
            this.Cities.Clear();

            this.Cities.AddRange(cities);

            this.OnCitiesLoaded();
        }
        
        /// <summary>
        /// Заполняет коллекцию регионов.
        /// </summary>
        /// <param name="regions">Регионы.</param>
        private void FillRegions(IEnumerable<RegionViewModel> regions)
        {
            this.Regions.Clear();

            this.Regions.AddRange(regions);

            this.OnRegionsLoaded();
        }

        /// <summary>
        /// Заполняет коллекцию улиц.
        /// </summary>
        /// <param name="streets">Улицы.</param>
        private void FillStreets(IEnumerable<StreetViewModel> streets)
        {
            this.Streets.Clear();

            this.Streets.AddRange(streets);

            this.OnStreetsLoaded();
        }

        #endregion

        #region Защищенные абстрактные методы

        /// <summary>
        /// Возвращает коллекцию населенных пунктов.
        /// </summary>
        /// <param name="region">Регион.</param>
        /// <returns>Коллекция населенных пунктов.</returns>
        protected abstract IEnumerable<CityViewModel> GetCities(RegionViewModel region);

        /// <summary>
        /// Возвращает коллекцию регионов.
        /// </summary>
        /// <returns>Коллекция регионов.</returns>
        protected abstract IEnumerable<RegionViewModel> GetRegions();

        /// <summary>
        /// Возвращает коллекцию улиц.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <returns>Коллекция улиц.</returns>
        protected abstract IEnumerable<StreetViewModel> GetStreets(CityViewModel city);

        #endregion

        #region Защищенные виртуальные методы

        /// <summary>
        /// Выполняет действия, связанные с загрузкой населенных пунктов.
        /// </summary>
        protected virtual void OnCitiesLoaded()
        {
            this.SelectedCity = this.Cities[0];
        }
        
        /// <summary>
        /// Выполняет действия, связанные с загрузкой регионов.
        /// </summary>
        protected virtual void OnRegionsLoaded()
        {
            this.SelectedRegion = this.Regions[0];
        }

        /// <summary>
        /// Выполняет действия, связанные с изменением выбора.
        /// </summary>
        protected virtual void OnSelectionChanged()
        {
            // Ничего не делаем.
        }

        /// <summary>
        /// Выполняет действия, связанные с загрузкой улиц.
        /// </summary>
        protected virtual void OnStreetsLoaded()
        {
            this.SelectedStreet = this.Streets[0];
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Инициализирует модель представления.
        /// </summary>
        public void Init()
        {
            // Заполняем коллекцию регионов.
            this.FillRegions(this.GetRegions());
        }

        #endregion
    }
}