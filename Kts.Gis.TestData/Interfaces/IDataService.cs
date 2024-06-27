using Kts.Gis.Data.Interfaces;
using Kts.Gis.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет интерфейс сервиса данных.
    /// </summary>
    [ServiceContract]
    public interface IDataService
    {
        
        string ConnectiorServerName
        {
            get;
            set;
        }

        #region Свойства

        /// <summary>
        /// Возвращает сервис доступа к данным объектов, представляемых значками на карте.
        /// </summary>
        IChildAccessService<BadgeModel> BadgeAccessService
        {
            get;
        }
        
        /// <summary>
        /// Возвращает геометрии значков.
        /// </summary>
        ReadOnlyCollection<BadgeGeometryModel> BadgeGeometries
        {
            get;
        }

        /// <summary>
        /// Возвращает сервис доступа к информации о котельной.
        /// </summary>
        IBoilerInfoAccessService BoilerInfoAccessService
        {
            get;
        }

        /// <summary>
        /// Возвращает сервис доступа к данным кастомных слоев.
        /// </summary>
        ICustomLayerAccessService CustomLayerAccessService
        {
            get;
        }

        /// <summary>
        /// Возвращает сервис доступа к данным кастомных объектов.
        /// </summary>
        ICustomObjectAccessService CustomObjectAccessService
        {
            get;
        }
        
        /// <summary>
        /// Возвращает сервис доступа к данным документов.
        /// </summary>
        IDocumentAccessService DocumentAccessService
        {
            get;
        }

        /// <summary>
        /// Возвращает полный путь к папке, в которой находятся логи об ошибках.
        /// </summary>
        string ErrorFolderName
        {
            get;
        }
        
        TableModel getTableMeterParamsForRegion(int regionID);

        /// <summary>
        /// Возвращает сервис доступа к данным объектов, представляемых фигурами на карте.
        /// </summary>
        IFigureAccessService FigureAccessService
        {
            get;
        }
        
        /// <summary>
        /// Возвращает сервис доступа к данным топлива котельных.
        /// </summary>
        IFuelAccessService FuelAccessService
        {
            get;
        }

        IMeterAccessService MeterAccessService
        {
            get;
        }

        /// <summary>
        /// Возвращает сервис доступа к данным глобальной карты.
        /// </summary>
        IGlobalMapAccessService GlobalMapAccessService
        {
            get;
        }
        
        /// <summary>
        /// Возвращает сервис доступа к данным базовых программ КТС.
        /// </summary>
        IKtsAccessService KtsAccessService
        {
            get;
        }

        /// <summary>
        /// Возвращает сервис доступа к данным надписей.
        /// </summary>
        ILabelAccessService LabelAccessService
        {
            get;
        }
        
        /// <summary>
        /// Возвращает сервис доступа к данным объектов, представляемых линиями на карте.
        /// </summary>
        ILineAccessService LineAccessService
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает идентификатор залогиненного пользователя.
        /// </summary>
        int LoggedUserId
        {
            get;
            set;
        }

        [OperationContract]
        ILoginAccessService getLoginAccessService();
        /// <summary>
        /// Возвращает сервис доступа к данным логинов.
        /// </summary>
        ILoginAccessService LoginAccessService
        {
            get;
        }

        /// <summary>
        /// Возвращает сервис доступа к данным карты.
        /// </summary>
        IMapAccessService MapAccessService
        {
            get;
        }
        
        /// <summary>
        /// Возвращает сервис доступа к данным объектов, представляемых узлами на карте.
        /// </summary>
        INodeAccessService NodeAccessService
        {
            get;
        }

        /// <summary>
        /// Возвращает сервис доступа к данным невизуальных объектов.
        /// </summary>
        IChildAccessService<NonVisualObjectModel> NonVisualObjectAccessService
        {
            get;
        }

        /// <summary>
        /// Возвращает коллекцию типов объектов.
        /// </summary>
        ReadOnlyCollection<ObjectType> ObjectTypes
        {
            get;
        }

        /// <summary>
        /// Возвращает сервис доступа к данным параметров объектов.
        /// </summary>
        IParameterAccessService ParameterAccessService
        {
            get;
        }

        /// <summary>
        /// Возвращает сервис доступа к данным отчетов.
        /// </summary>
        IReportAccessService ReportAccessService
        {
            get;
        }

        /// <summary>
        /// Возвращает список схем.
        /// </summary>
        List<SchemaModel> Schemas
        {
            get;
        }

        /// <summary>
        /// Возвращает сервис поиска объектов.
        /// </summary>
        ISearchService SearchService
        {
            get;
        }

        /// <summary>
        /// Возвращает название сервера.
        /// </summary>
        string ServerName
        {
            get;
        }

        /// <summary>
        /// Возвращает полный путь к папке, содержащей файлы-изображений подложки.
        /// </summary>
        string SubstrateFolderName
        {
            get;
        }

        /// <summary>
        /// Возвращает сервис доступа к данным территориальных единиц.
        /// </summary>
        
        ITerritorialEntityAccessService TerritorialEntityAccessService
        {
            get;
        }

        /// <summary>
        /// Возвращает полный путь к папке, содержащей миниатюры подложек.
        /// </summary>
        string ThumbnailFolderName
        {
            get;
        }

        #endregion

        #region Методы

        /// <summary>
        /// Начинает транзакцию сохранения.
        /// </summary>
        [OperationContract]
        void BeginSaveTransaction();

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли подключиться к источнику данных.
        /// </summary>
        /// <returns>true, если можно подключиться, иначе - false.</returns>
        [OperationContract]
        bool CanConnect();

        /// <summary>
        /// Завершает транзакцию сохранения.
        /// </summary>
        /// <returns>true, если сохранение выполнено, иначе - false.</returns>
        [OperationContract]
        bool EndSaveTransaction();

        /// <summary>
        /// Возвращает логи изменений данных.
        /// </summary>
        /// <returns>Таблица логов изменений данных.</returns>
        [OperationContract]
        DataTable GetLogs();

        /// <summary>
        /// Возвращает тип объекта по его идентификатору.
        /// </summary>
        /// <param name="typeId">Идентификатор типа объекта.</param>
        /// <returns>Тип объекта.</returns>
        [OperationContract]
        ObjectType GetObjectType(int typeId);

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли заданный тип типом котельной.
        /// </summary>
        /// <param name="type">Проверяемый тип.</param>
        /// <returns>true, если заданный тип является типом котельной, иначе - false.</returns>
        [OperationContract]
        bool IsBoilerType(ObjectType type);
        [OperationContract]
        bool IsTrashStorageType(ObjectType type);

        /// <summary>
        /// Проверяет, совместима ли заданная версия приложения с источником данных.
        /// </summary>
        /// <param name="version">Версия приложения.</param>
        /// <returns>true, если версия приложения совместима, иначе - false.</returns>
        [OperationContract]
        bool IsCompatible(string version);

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли заданный тип типом складом.
        /// </summary>
        /// <param name="type">Проверяемый тип.</param>
        /// <returns>true, если заданный тип является типом складом, иначе - false.</returns>
        [OperationContract]
        bool IsStorageType(ObjectType type);

        /// <summary>
        /// Асинхронно загружает критически важные данные.
        /// </summary>
        [OperationContract]
        Task LoadDataAsync();

        /// <summary>
        /// Подготавливает схему к работе.
        /// </summary>
        /// <param name="year">Год.</param>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        [OperationContract]
        void PrepareSchema(int year, int cityId);

        /// <summary>
        /// Обновляет логи изменений данных.
        /// </summary>
        /// <param name="dataTable">Таблица логов изменений данных.</param>
        [OperationContract]
        void UpdateLogs(DataTable dataTable);

        /// <summary>
        /// Обновляет все перезаписываемые таблицы.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="type">Тип объекта.</param>
        /// <param name="schema">Схема.</param>
        /// <param name="loadLevel">Уровень загрузки справочников.</param>
        [OperationContract]
        void UpdateTables(int cityId, ObjectType type, SchemaModel schema, LoadLevel loadLevel);
        [OperationContract]
        void UpdateTables(int cityID);

        /// <summary>
        /// Обновляет параметры, отвечающие за составление названия объектов.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пунктаю</param>
        [OperationContract]
        void UpdateTypeCaptionParams(int cityId);

        [OperationContract]
        List<ObjectType> GetFillTypes();

        [OperationContract]
        List<TableModel> GetFillTables();

        [OperationContract]
        List<ParameterModel> GetFillParameters();

        [OperationContract]
        List<BadgeGeometryModel> GetFillBadgeGeometries();

        [OperationContract]
        List<SchemaModel> GetFillSchemas();

        #endregion
    }
}