using Kts.Gis.Models;
using Kts.Importer.Data;
using Kts.Utilities;
using System;
using System.Linq;

namespace Kts.Importer.ViewModels
{
    /// <summary>
    /// Представляет модель представления импортируемого объекта.
    /// </summary>
    internal sealed class ImportingObjectViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Населенный пункт.
        /// </summary>
        private CityViewModel city;

        /// <summary>
        /// Идентификатор объекта.
        /// </summary>
        private Guid? id;

        /// <summary>
        /// Значение, указывающее на то, что является ли населенный пункт обязательным.
        /// </summary>
        private bool isCityNecessary;

        /// <summary>
        /// Значение, указывающее на то, что является ли идентификатор объекта-родителя обязательным.
        /// </summary>
        private bool isParentNecessary;

        /// <summary>
        /// Значение, указывающее на то, что является ли улица обязательной.
        /// </summary>
        private bool isStreetNecessary;

        /// <summary>
        /// Идентификатор объекта-родителя.
        /// </summary>
        private Guid? parentId;

        /// <summary>
        /// Регион.
        /// </summary>
        private RegionViewModel region;

        /// <summary>
        /// Улица.
        /// </summary>
        private StreetViewModel street;

        /// <summary>
        /// Идентификатор параметра, представляющего улицу.
        /// </summary>
        private int streetParamId;

        /// <summary>
        /// Тип объекта.
        /// </summary>
        private ObjectType type;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ImportingObjectViewModel"/>.
        /// </summary>
        /// <param name="type">Тип объекта.</param>
        /// <param name="paramValues">Значения параметров объекта.</param>
        /// <param name="rawRegionName">Неообработанное название региона.</param>
        /// <param name="rawCityName">Неообработанное название населенного пункта.</param>
        public ImportingObjectViewModel(ObjectType type, ParameterValueSetModel paramValues, string rawRegionName, string rawCityName)
        {
            this.type = type;
            this.ParameterValueSetViewModel = new ParameterValueSetViewModel(paramValues);
            this.RawRegionName = rawRegionName;
            this.RawCityName = rawCityName;

            this.isCityNecessary = true;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ImportingObjectViewModel"/>.
        /// </summary>
        /// <param name="type">Тип объекта.</param>
        /// <param name="paramValues">Значения параметров объекта.</param>
        /// <param name="rawRegionName">Неообработанное название региона.</param>
        /// <param name="rawCityName">Неообработанное название населенного пункта.</param>
        /// <param name="rawParentName">Неообработанное название объекта-родителя.</param>
        public ImportingObjectViewModel(ObjectType type, ParameterValueSetModel paramValues, string rawRegionName, string rawCityName, string rawParentName) : this(type, paramValues, rawRegionName, rawCityName)
        {
            this.RawParentName = rawParentName;

            this.isParentNecessary = true;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ImportingObjectViewModel"/>.
        /// </summary>
        /// <param name="type">Тип объекта.</param>
        /// <param name="paramValues">Значения параметров объекта.</param>
        /// <param name="rawRegionName">Неообработанное название региона.</param>
        /// <param name="rawCityName">Неообработанное название населенного пункта.</param>
        /// <param name="rawStreetName">Неообработанное название улицы.</param>
        /// <param name="streetParamId">Идентификатор параметра, представляющего улицу.</param>
        public ImportingObjectViewModel(ObjectType type, ParameterValueSetModel paramValues, string rawRegionName, string rawCityName, string rawStreetName, int streetParamId) : this(type, paramValues, rawRegionName, rawCityName)
        {
            this.RawStreetName = rawStreetName;
            this.streetParamId = streetParamId;

            this.isStreetNecessary = true;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли импортировать объект.
        /// </summary>
        public bool CanBeImported
        {
            get
            {
                if (this.isCityNecessary && this.City == null || this.isStreetNecessary && this.Street == null || this.isParentNecessary && !this.ParentId.HasValue)
                    return false;

                return true;
            }
        }

        /// <summary>
        /// Возвращает или задает населенный пункт.
        /// </summary>
        public CityViewModel City
        {
            get
            {
                return this.city;
            }
            set
            {
                this.city = value;

                this.NotifyPropertyChanged(nameof(this.City));
            }
        }

        /// <summary>
        /// Возвращает или задает идентификатор объекта.
        /// </summary>
        public Guid? Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;

                this.NotifyPropertyChanged(nameof(this.Id));
            }
        }

        /// <summary>
        /// Возвращает модель представления набора значений параметров.
        /// </summary>
        public ParameterValueSetViewModel ParameterValueSetViewModel
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает идентификатор объекта-родителя.
        /// </summary>
        public Guid? ParentId
        {
            get
            {
                return this.parentId;
            }
            set
            {
                this.parentId = value;

                this.NotifyPropertyChanged(nameof(this.ParentId));
            }
        }

        /// <summary>
        /// Возвращает неообработанное название населенного пункта.
        /// </summary>
        public string RawCityName
        {
            get;
        }

        /// <summary>
        /// Возвращает необработанное название объекта-родителя.
        /// </summary>
        public string RawParentName
        {
            get;
        }

        /// <summary>
        /// Возвращает неообработанное название региона.
        /// </summary>
        public string RawRegionName
        {
            get;
        }

        /// <summary>
        /// Возвращает неообработанное название улицы.
        /// </summary>
        public string RawStreetName
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает регион.
        /// </summary>
        public RegionViewModel Region
        {
            get
            {
                return this.region;
            }
            set
            {
                this.region = value;

                this.NotifyPropertyChanged(nameof(this.Region));
            }
        }

        /// <summary>
        /// Возвращает или задает улицу.
        /// </summary>
        public StreetViewModel Street
        {
            get
            {
                return this.street;
            }
            set
            {
                this.street = value;

                this.NotifyPropertyChanged(nameof(this.Street));
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Сохраняет данные импортируемого объекта в источнике данных.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        public void SaveToDataSource(IDataService dataService)
        {
            if (this.isStreetNecessary)
                this.ParameterValueSetViewModel.ParameterValueSet.ParameterValues.Add(this.type.Parameters.First(x => x.Id == this.streetParamId), this.Street.Id);

            // Получаем дефолтную схему населенного пункта.
            var schema = dataService.GetDefaultSchema(this.City.Id);

            if (this.Id.HasValue && this.Id.Value != Guid.Empty)
                dataService.SaveObjectValues(this.Id.Value, this.City.Id, this.ParentId, this.ParameterValueSetViewModel.ParameterValueSet, schema);
            else
                dataService.SaveObjectValues(this.City.Id, this.ParentId, this.ParameterValueSetViewModel.ParameterValueSet, schema);
        }

        #endregion
    }
}