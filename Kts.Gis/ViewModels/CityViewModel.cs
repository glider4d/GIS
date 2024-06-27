using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.Substrates;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления населенного пункта.
    /// </summary>
    [Serializable]
    internal sealed partial class CityViewModel : Utilities.CityViewModel
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Значение, указывающее на то, что нужно ли отображать идентификатор населенного пункта.
        /// </summary>
        private readonly bool showId;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CityViewModel"/>.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <param name="showId">Значение, указывающее на то, что нужно ли отображать идентификатор населенного пункта.</param>
        public CityViewModel(TerritorialEntityModel city, bool showId) : base(city)
        {
            this.showId = showId;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Удаляет кешированные файлы-изображений подложки населенного пункта.
        /// </summary>
        /// <param name="substrateService">Сервис подложек.</param>
        /// <returns>Возвращает значение, указывающее на то, что удалены ли кешированные файлы-изображения.</returns>
        public bool DeleteCachedImages(SubstrateService substrateService)
        {
            return substrateService.DeleteCachedImages(this.TerritorialEntity);
        }

        /// <summary>
        /// Возвращает список заголовков "Утверждено"/"Согласовано".
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Список заголовков "Утверждено"/"Согласовано".</returns>
        public List<ApprovedHeaderModel> GetApprovedHeader(SchemaModel schema, IDataService dataService)
        {
            return dataService.CustomObjectAccessService.GetApprovedHeaders(schema, this.Id);
        }

        /// <summary>
        /// Возвращает таблицу данных объектов населенного пункта, представляемых значками на карте, в необработанном виде.
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Таблица данных объектов, представляемых значками на карте, в необработанном виде.</returns>
        public DataTable GetBadgesRaw(SchemaModel schema, IDataService dataService)
        {
            return dataService.BadgeAccessService.GetAllRaw(schema, this.Id);
        }

        /// <summary>
        /// Возвращает список котельных населенного пункта.
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Список котельных.</returns>
        public List<Tuple<Guid, string>> GetBoilers(SchemaModel schema, IDataService dataService)
        {
            return dataService.TerritorialEntityAccessService.GetBoilers(this.TerritorialEntity, schema);
        }

        /// <summary>
        /// Возвращает названия кешированных файлов-изображений населенного пункта.
        /// </summary>
        /// <param name="substrateService">Сервис подложек.</param>
        /// <returns>Названия кешированных файлов-изображений.</returns>
        public string[][] GetCachedImageFileNames(SubstrateService substrateService)
        {
            return substrateService.GetCachedImageFileNames(this.TerritorialEntity);
        }
        
        /// <summary>
        /// Возвращает список объектов населенного пункта, представляемых фигурами на карте.
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Список объектов, представляемых фигурами на карте.</returns>
        public List<FigureModel> GetFigures(SchemaModel schema, IDataService dataService)
        {
            return dataService.FigureAccessService.GetAll(this.Id, schema);
        }

        /// <summary>
        /// Возвращает список объектов населенного пункта, представляемых фигурами на карте и входящими в одну сеть с заданным объектом.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="objectIds">Идентификаторы объектов.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список объектов, представляемых фигурами на карте.</returns>
        public List<FigureModel> GetFigures(IDataService dataService, List<Guid> objectIds, SchemaModel schema)
        {
            return dataService.FigureAccessService.GetAll(this.Id, objectIds, schema);
        }

        /// <summary>
        /// Возвращает список надписей населенного пункта.
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Список надписей.</returns>
        public List<LabelModel> GetLabels(SchemaModel schema, IDataService dataService)
        {
            return dataService.LabelAccessService.GetAll(this.Id, schema);
        }

        /// <summary>
        /// Возвращает список объектов населенного пункта, представляемых линиями на карте.
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Список объектов, представляемых линиями на карте.</returns>
        public List<LineModel> GetLines(SchemaModel schema, IDataService dataService)
        {
            return dataService.LineAccessService.GetAll(this.Id, schema);
        }

        /// <summary>
        /// Возвращает список объектов населенного пункта, представляемых линиями на карте и входящими в одну сеть с заданным объектом.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="objectIds">Идентификаторы объектов.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список объектов, представляемых линиями на карте.</returns>
        public List<LineModel> GetLines(IDataService dataService, List<Guid> objectIds, SchemaModel schema)
        {
            return dataService.LineAccessService.GetAll(this.Id, objectIds, schema);
        }

        /// <summary>
        /// Возвращает список объектов населенного пункта, представляемых узлами на карте.
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Список объектов, представляемых узлами на карте.</returns>
        public List<NodeModel> GetNodes(SchemaModel schema, IDataService dataService)
        {
            return dataService.NodeAccessService.GetAll(this.Id, schema);
        }

        /// <summary>
        /// Возвращает список объектов населенного пункта, представляемых узлами на карте и входящими в одну сеть с заданным объектом.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="objectIds">Идентификаторы объектов.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Список объектов, представляемых узлами на карте.</returns>
        public List<NodeModel> GetNodes(IDataService dataService, List<Guid> objectIds, SchemaModel schema)
        {
            return dataService.NodeAccessService.GetAll(this.Id, objectIds, schema);
        }

        /// <summary>
        /// Возвращает масштаб линий населенного пункта.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Масштаб линий.</returns>
        public double GetScale(IDataService dataService)
        {
            return dataService.MapAccessService.GetScale(this.TerritorialEntity);
        }
        
        /// <summary>
        /// Возвращает подложку населенного пункта.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Подложка.</returns>
        public SubstrateModel GetSubstrate(IDataService dataService)
        {
            return dataService.MapAccessService.GetSubstrate(this.TerritorialEntity);
        }

        /// <summary>
        /// Возвращает размерность кешированной подложки населенного пункта.
        /// </summary>
        /// <param name="substrateService">Сервис подложек.</param>
        /// <returns>Размерность кешированной подложки.</returns>
        public System.Windows.Size GetSubstrateDimension(SubstrateService substrateService)
        {
            var dimension = substrateService.GetSubstrateDimension(this.TerritorialEntity);

            return new System.Windows.Size(dimension.Width, dimension.Height);
        }

        /// <summary>
        /// Возвращает размер кешированной подложки населенного пункта.
        /// </summary>
        /// <param name="substrateService">Сервис подложек.</param>
        /// <returns>Размер кешированной подложки.</returns>
        public System.Windows.Size GetSubstrateSize(SubstrateService substrateService)
        {
            var size = substrateService.GetSubstrateSize(this.TerritorialEntity);

            return new System.Windows.Size(size.Width, size.Height);
        }

        /// <summary>
        /// Возвращает список таблиц населенного пункта.
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Список таблиц.</returns>
        public List<LengthPerDiameterTableModel> GetTables(SchemaModel schema, IDataService dataService)
        {
            return dataService.CustomObjectAccessService.GetLengthPerDiameterTables(schema, this.Id);
        }

        /// <summary>
        /// Возвращает путь к миниатюре подложки населенного пункта.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Путь к миниатюре подложки карты.</returns>
        public string GetThumbnailPath(IDataService dataService)
        {
            return Directory.GetFiles(dataService.ThumbnailFolderName).FirstOrDefault(x =>
            {
                try
                {
                    var id = Convert.ToInt32(Path.GetFileNameWithoutExtension(x));

                    return id == this.Id;
                }
                catch
                {
                }

                return false;
            });
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеет ли населенный пункт файл-изображение подложки.
        /// </summary>
        /// <param name="substrateService">Сервис подложек.</param>
        /// <returns>Значение, указывающее на то, что имеет ли населенный пункт файл-изображение подложки.</returns>
        public bool HasImageSource(SubstrateService substrateService)
        {
            return substrateService.HasImageSource(this.TerritorialEntity);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что зафиксирован ли населенный пункт.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>true, если зафиксирован, иначе - false.</returns>
        public bool IsFixed(IDataService dataService)
        {
            return dataService.TerritorialEntityAccessService.IsCityFixed(this.TerritorialEntity);
        }

        /// <summary>
        /// Удаляет подложку из кеша.
        /// </summary>
        /// <param name="substrateService">Сервис подложек.</param>
        public void RemoveSubstrate(SubstrateService substrateService)
        {
            substrateService.RemoveSubstrate(this.TerritorialEntity);
        }

        /// <summary>
        /// Обновляет масштаб линий населенного пункта.
        /// </summary>
        /// <param name="scale">Масштаб линий.</param>
        /// <param name="dataService">Сервис данных.</param>
        public void UpdateScale(double scale, IDataService dataService)
        {
            dataService.MapAccessService.UpdateScale(scale, this.TerritorialEntity);
        }

        /// <summary>
        /// Обновляет подложку.
        /// </summary>
        /// <param name="substrate">Подложка.</param>
        /// <param name="substrateService">Сервис подложек.</param>
        public void UpdateSubstrate(SubstrateModel substrate, SubstrateService substrateService)
        {
            substrateService.AddSubstrate(substrate);
        }

        #endregion
    }

    // Реализация Utilities.CityViewModel.
    internal sealed partial class CityViewModel
    {
        #region Открытые переопределенные свойства

        /// <summary>
        /// Возвращает название территориальной единицы.
        /// </summary>
        public override string Name
        {
            get
            {
                if (this.showId)
                    return base.Name + " (" + this.Id + ")";

                return base.Name;
            }
        }

        #endregion
    }
}