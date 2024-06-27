using Kts.Gis.Data;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления списка сопоставленных объектов с программой "Расчеты с юридическими лицами/Квартплата".
    /// </summary>
    internal sealed class JurKvpCompletedListViewModel : BaseViewModel
    {
        #region Открытые перечисления

        /// <summary>
        /// Режим.
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// Расчеты с юридическими лицами.
            /// </summary>
            Jur,

            /// <summary>
            /// Квартплата.
            /// </summary>
            Kvp
        }

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        /// <summary>
        /// Хранитель слоев.
        /// </summary>
        private readonly ILayerHolder layerHolder;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса закрытия представления.
        /// </summary>
        public event EventHandler CloseRequested;

        /// <summary>
        /// Событие запроса открытия объекта.
        /// </summary>
        public event EventHandler OpenObjectRequested;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="JurKvpCompletedListViewModel"/>.
        /// </summary>
        /// <param name="layerHolder">Хранитель слоев.</param>
        /// <param name="dataService">Сервис данных.</param>
        public JurKvpCompletedListViewModel(ILayerHolder layerHolder, IDataService dataService)
        {
            this.layerHolder = layerHolder;
            this.dataService = dataService;

            this.DisbandCommand = new RelayCommand(this.ExecuteDisband, this.CanExecuteDisband);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает текущий режим.
        /// </summary>
        public Mode CurrentMode
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает команду убирания связи.
        /// </summary>
        public RelayCommand DisbandCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что есть ли изменения.
        /// </summary>
        public bool HasChanges
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает коллекцию объектов. Первый <see cref="Guid"/> - это идентификатор объекта, второй <see cref="Guid"/> - идентификатор родителя, а третье и четвертое значения - это наименования объекта в "ИКС" и в "Расчеты с юридическими лицами" соответственно.
        /// </summary>
        public AdvancedObservableCollection<Tuple<Guid, Guid, string, string>> Objects
        {
            get;
        } = new AdvancedObservableCollection<Tuple<Guid, Guid, string, string>>();

        /// <summary>
        /// Выбранный объект.
        /// </summary>
        private Tuple<Guid, Guid, string, string> selectedObject;

        /// <summary>
        /// Возвращает или задает выбранный объект.
        /// </summary>
        public Tuple<Guid, Guid, string, string> SelectedObject
        {
            get
            {
                return this.selectedObject;
            }
            set
            {
                this.selectedObject = value;

                this.NotifyPropertyChanged(nameof(this.SelectedObject));

                this.DisbandCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Закрытые методы
        
        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить лишение связи между объектами.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteDisband()
        {
            return this.SelectedObject != null;
        }

        /// <summary>
        /// Выполняет лишение связи между объектами.
        /// </summary>
        private void ExecuteDisband()
        {
            var obj = this.SelectedObject;

            this.Objects.Remove(obj);

            if (this.CurrentMode == Mode.Jur)
                this.dataService.FigureAccessService.ResetJurId(obj.Item1, this.layerHolder.CurrentSchema);
            else
                if (this.CurrentMode == Mode.Kvp)
                this.dataService.FigureAccessService.ResetKvpId(obj.Item1, this.layerHolder.CurrentSchema);

            this.HasChanges = true;
        }

        #endregion

        #region Открытые методы
        
        /// <summary>
        /// Закрывает представление.
        /// </summary>
        public void Close()
        {
            this.CloseRequested?.Invoke(this, EventArgs.Empty);
        }
        
        /// <summary>
        /// Отображает выбранный объект.
        /// </summary>
        public void ShowSelectedObject()
        {
            this.OpenObjectRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}