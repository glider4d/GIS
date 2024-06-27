using Kts.Gis.Data;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления списка объектов.
    /// </summary>
    internal sealed class ObjectListViewModel : BaseViewModel
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
            Kvp,

            /// <summary>
            /// Другое.
            /// </summary>
            Other
        }

        #endregion

        #region Закрытые поля

        /// <summary>
        /// Объекты для сопоставления.
        /// </summary>
        private List<Tuple<string, string>> compareObjects;

        

        /// <summary>
        /// Объекты.
        /// </summary>
        private List<Tuple<Guid, Guid, string, Guid>> objects;

        /// <summary>
        /// Подсказка представления списка объектов.
        /// </summary>
        private string tip;

        /// <summary>
        /// Заголовок представления списка объектов.
        /// </summary>
        private string title;

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
        /// Инициализирует новый экземпляр класса <see cref="ObjectListViewModel"/>.
        /// </summary>
        /// <param name="layerHolder">Хранитель слоев.</param>
        /// <param name="dataService">Сервис данных.</param>
        public ObjectListViewModel(ILayerHolder layerHolder, IDataService dataService)
        {
            this.layerHolder = layerHolder;
            this.dataService = dataService;

            this.EqualCommand = new RelayCommand(this.ExecuteEqual, this.CanExecuteEqual);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает список котельных.
        /// </summary>
        public AdvancedObservableCollection<Tuple<Guid, string>> Boilers
        {
            get;
        } = new AdvancedObservableCollection<Tuple<Guid, string>>();

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что можно ли сопоставлять объекты.
        /// </summary>
        public bool CanCompare
        {
            get;
            set;
        } = false;

        /// <summary>
        /// Представление коллекции объектов для сопоставления.
        /// </summary>
        private ICollectionView compareObjectsView;

        /// <summary>
        /// Возвращает представление коллекции объектов для сопоставления.
        /// </summary>
        public ICollectionView CompareObjectsView
        {
            get
            {
                return this.compareObjectsView;
            }
            private set
            {
                this.compareObjectsView = value;

                this.NotifyPropertyChanged(nameof(this.CompareObjectsView));
            }
        }

        /// <summary>
        /// Возвращает или задает режим.
        /// </summary>
        public Mode CurrentMode
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает команду приравнивания.
        /// </summary>
        public RelayCommand EqualCommand
        {
            get;
        }

        /// <summary>
        /// Текст фильтра.
        /// </summary>
        private string filterText = "";

        /// <summary>
        /// Возвращает или задает текст фильтра.
        /// </summary>
        public string FilterText
        {
            get
            {
                return this.filterText;
            }
            set
            {
                this.filterText = value;

                this.compareObjectsView.Refresh();
            }
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
        /// Представление коллекции объектов.
        /// </summary>
        private ICollectionView objectsView;
        
        /// <summary>
        /// Возвращает представление коллекции объектов.
        /// </summary>
        public ICollectionView ObjectsView
        {
            get
            {
                return this.objectsView;
            }
            private set
            {
                this.objectsView = value;

                this.NotifyPropertyChanged(nameof(this.ObjectsView));
            }
        }

        /// <summary>
        /// Выбранная котельная.
        /// </summary>
        private Guid selectedBoiler;

        /// <summary>
        /// Возвращает или задает выбранную котельную.
        /// </summary>
        public Guid SelectedBoiler
        {
            get
            {
                return this.selectedBoiler;
            }
            set
            {
                this.selectedBoiler = value;

                this.NotifyPropertyChanged(nameof(this.SelectedBoiler));

                this.objectsView.Refresh();
            }
        }

        /// <summary>
        /// Выбранный объект.
        /// </summary>
        private Tuple<Guid, Guid, string, Guid> selectedObject;

        /// <summary>
        /// Возвращает или задает выбранный объект.
        /// </summary>
        public Tuple<Guid, Guid, string, Guid> SelectedObject
        {
            get
            {
                return this.selectedObject;
            }
            set
            {
                this.selectedObject = value;

                this.NotifyPropertyChanged(nameof(this.SelectedObject));

                this.EqualCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Выбранный объект сравнения.
        /// </summary>
        private Tuple<string, string> selectedCompareObject;

        /// <summary>
        /// Возвращает или задает выбранный объект.
        /// </summary>
        public Tuple<string, string> SelectedCompareObject
        {
            get
            {
                return this.selectedCompareObject;
            }
            set
            {
                this.selectedCompareObject = value;

                this.NotifyPropertyChanged(nameof(this.SelectedCompareObject));

                this.EqualCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Возвращает или задает подсказку представления списка объектов.
        /// </summary>
        public string Tip
        {
            get
            {
                return this.tip;
            }
            set
            {
                this.tip = value;

                this.NotifyPropertyChanged(nameof(this.Tip));
            }
        }

        /// <summary>
        /// Возвращает или задает заголовок представления списка объектов.
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;

                this.NotifyPropertyChanged(nameof(this.Title));
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Применяет фильтр к объекту.
        /// </summary>
        /// <param name="item">Объект.</param>
        /// <returns>true, если объект подходит, иначе - false.</returns>
        private bool ApplyBoilerFilter(object item)
        {
            var obj = item as Tuple<Guid, Guid, string, Guid>;

            return obj.Item4 == this.SelectedBoiler;
        }

        /// <summary>
        /// Применяет фильтр к объекту.
        /// </summary>
        /// <param name="item">Объект.</param>
        /// <returns>true, если объект подходит, иначе - false.</returns>
        private bool ApplyFilter(object item)
        {
            var obj = item as Tuple<string, string>;

            return obj.Item2.ToLower().Contains(this.FilterText.ToLower());
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить команду приравнивания.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteEqual()
        {
            return this.SelectedObject != null && this.SelectedCompareObject != null;
        }

        /// <summary>
        /// Выполняет команду приравнивания.
        /// </summary>
        private void ExecuteEqual()
        {
            var a = this.SelectedObject;
            var b = this.SelectedCompareObject;

            this.objects.Remove(a);
            this.compareObjects.Remove(b);

            this.compareObjectsView.Refresh();
            this.objectsView.Refresh();

            switch (this.CurrentMode)
            {
                case Mode.Jur:
                    this.dataService.FigureAccessService.SetJurId(a.Item1, b.Item1, this.layerHolder.CurrentSchema);

                    break;

                case Mode.Kvp:
                    this.dataService.FigureAccessService.SetKvpId(a.Item1, b.Item1.ToString(), this.layerHolder.CurrentSchema);

                    break;
            }

            this.HasChanges = true;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Очищает объекты для сопоставления.
        /// </summary>
        public void ClearCompareObjects()
        {
            if (this.compareObjects == null)
                return;

            this.compareObjects.Clear();

            this.CompareObjectsView.Refresh();
        }

        /// <summary>
        /// Очищает объекты.
        /// </summary>
        public void ClearObjects()
        {
            if (this.objects == null)
                return;

            this.objects.Clear();

            this.ObjectsView.Refresh();
        }

        /// <summary>
        /// Закрывает представление.
        /// </summary>
        public void Close()
        {
            this.CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Задает объекты для сопоставления.
        /// </summary>
        /// <param name="compareObjects">Объекты для сопоставления.</param>
        public void SetCompareObjects(List<Tuple<string, string>> compareObjects)
        {
            this.compareObjects = compareObjects;

            if (this.CompareObjectsView != null)
                this.CompareObjectsView.Filter = null;
            this.CompareObjectsView = CollectionViewSource.GetDefaultView(this.compareObjects);
            this.CompareObjectsView.Filter = this.ApplyFilter;
        }

        public void SetCompareObjectsJur(List<Tuple<string, string>> compareObjects)
        {
            this.compareObjects = compareObjects;

            if (this.CompareObjectsView != null)
                this.CompareObjectsView.Filter = null;
            this.CompareObjectsView = CollectionViewSource.GetDefaultView(this.compareObjects);
            this.CompareObjectsView.Filter = this.ApplyFilter;
        }

        /// <summary>
        /// Задает объекты.
        /// </summary>
        /// <param name="objects">Объекты.</param>
        public void SetObjects(List<Tuple<Guid, Guid, string, Guid>> objects)
        {
            this.objects = objects;

            if (this.ObjectsView != null)
                this.ObjectsView.Filter = null;
            this.ObjectsView = CollectionViewSource.GetDefaultView(this.objects);
            this.ObjectsView.Filter = this.ApplyBoilerFilter;
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