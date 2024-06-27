using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Utilities;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления визуального региона.
    /// </summary>
    internal sealed class VisualRegionViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Информация о визуальном регионе.
        /// </summary>
        private VisualRegionInfoViewModel info;

        /// <summary>
        /// Значение, указывающее на то, что выбран ли визуальный регион.
        /// </summary>
        private bool isSelected;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        /// <summary>
        /// Визуальный регион.
        /// </summary>
        private readonly VisualRegionModel visualRegion;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="VisualRegionViewModel"/>.
        /// </summary>
        /// <param name="visualRegion">Визуальный регион.</param>
        /// <param name="dataService">Сервис данных.</param>
        public VisualRegionViewModel(VisualRegionModel visualRegion, IDataService dataService)
        {
            this.visualRegion = visualRegion;
            this.dataService = dataService;

            this.Path = Geometry.Parse(visualRegion.Path);
            this.Transform = System.Windows.Media.Transform.Parse(visualRegion.Transform) as MatrixTransform;

            if (this.Path.CanFreeze)
                this.Path.Freeze();

            if (this.Transform.CanFreeze)
                this.Transform.Freeze();
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли визуальный регион активным.
        /// </summary>
        public bool IsActive
        {
            get
            {
                return this.visualRegion.Id != -1;
            }
        }

        /// <summary>
        /// Возвращает или задает информацию о визуальном регионе.
        /// </summary>
        public VisualRegionInfoViewModel Info
        {
            get
            {
                return this.info;
            }
            private set
            {
                this.info = value;

                this.NotifyPropertyChanged(nameof(this.Info));
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что выбран ли визуальный регион.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                if (this.IsActive)
                {
                    this.isSelected = value;

                    this.NotifyPropertyChanged(nameof(this.IsSelected));
                }
            }
        }

        /// <summary>
        /// Возвращает название визуального региона.
        /// </summary>
        public string Name
        {
            get
            {
                return this.visualRegion.Name;
            }
        }

        /// <summary>
        /// Возвращает путь, из которого состоит фигура визуального региона.
        /// </summary>
        public Geometry Path
        {
            get;
        }

        /// <summary>
        /// Возвращает матричную трансформацию фигуры визуального региона.
        /// </summary>
        public MatrixTransform Transform
        {
            get;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Асинхронно загружает информацию о визуальном регионе.
        /// </summary>
        /// <param name="token">Токен отмены.</param>
        public async Task LoadInfoAsync(CancellationToken token)
        {
            var info = await this.dataService.GlobalMapAccessService.GetVisualRegionInfoAsync(this.visualRegion, token);

            if (!token.IsCancellationRequested)
                this.Info = new VisualRegionInfoViewModel(info);
        }

        #endregion
    }
}