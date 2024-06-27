using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Utilities;
using System.Threading;
using System.Threading.Tasks;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления информации о складе.
    /// </summary>
    internal sealed class StorageInfoViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Сигнализатор токена отмена загрузки информации о складе.
        /// </summary>
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Значение, указывающее на то, что отображается ли информация о складе.
        /// </summary>
        private bool isStorageInfoVisible;

        /// <summary>
        /// Схема.
        /// </summary>
        private SchemaModel schema;

        /// <summary>
        /// Склад.
        /// </summary>
        private FigureViewModel storage;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="StorageInfoViewModel"/>.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        public StorageInfoViewModel(IDataService dataService)
        {
            this.dataService = dataService;

            this.FuelInfo = new FuelInfoViewModel(dataService);
        }

        #endregion

        #region Открытые свойства
        
        /// <summary>
        /// Возвращает информацию о топливе склада.
        /// </summary>
        public FuelInfoViewModel FuelInfo
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что отображается ли информация о складе.
        /// </summary>
        public bool IsStorageInfoVisible
        {
            get
            {
                return this.isStorageInfoVisible;
            }
            set
            {
                this.isStorageInfoVisible = value;

                this.NotifyPropertyChanged(nameof(this.IsStorageInfoVisible));
            }
        }
        
        #endregion

        #region Открытые методы

        /// <summary>
        /// Асинхронно задает склад.
        /// </summary>
        /// <param name="storage">Склад.</param>
        /// <param name="schema">Схема.</param>
        public async Task SetStorageAsync(FigureViewModel storage, SchemaModel schema)
        {
            // Если уже имеется сигнализатор токена отмены, то пробуем отменить задачу.
            if (this.cancellationTokenSource != null)
                this.cancellationTokenSource.Cancel();
                
            this.FuelInfo.IsLoading = true;

            // Создаем новый сигнализатор и запоминаем токен.
            var cts = new CancellationTokenSource();
            this.cancellationTokenSource = cts;
            var ct = cts.Token;

            if (!ct.IsCancellationRequested)
            {
                this.storage = storage;
                this.schema = schema;

                this.IsStorageInfoVisible = true;

                // Передаем склад модели представления информации о топливе.
                await this.FuelInfo.SetBoilerAsync(storage);

                if (!ct.IsCancellationRequested)
                    this.FuelInfo.IsLoading = false;
            }
        }

        /// <summary>
        /// Убирает склад.
        /// </summary>
        public void UnsetStorage()
        {
            // Если уже имеется сигнализатор токена отмены, то пробуем отменить задачу.
            if (this.cancellationTokenSource != null)
                this.cancellationTokenSource.Cancel();

            this.IsStorageInfoVisible = false;

            this.storage = null;
            
            // Убираем склад с модели представления информации о топливе.
            this.FuelInfo.UnsetBoiler();
            
            this.FuelInfo.IsLoading = false;
        }

        #endregion
    }
}