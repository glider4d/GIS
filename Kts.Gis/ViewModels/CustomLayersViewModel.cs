using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.RevertibleActions;
using Kts.Gis.Services;
using Kts.History;
using Kts.Messaging;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления кастомных слоев.
    /// </summary>
    /// 
    [Serializable]
    internal sealed class CustomLayersViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Текущий населенный пункт.
        /// </summary>
        private CityViewModel city;
    
        /// <summary>
        /// Текущая схема.
        /// </summary>
        private SchemaModel schema;

        /// <summary>
        /// Выбранный кастомный слой.
        /// </summary>
        private CustomLayerViewModel selectedLayer;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис доступа к функциям приложения.
        /// </summary>
        //[NonSerialized]
        private readonly AccessService accessService;

        /// <summary>
        /// Сервис доступа к данным.
        /// </summary>
        //[NonSerialized]
        private readonly IDataService dataService;

        /// <summary>
        /// Сервис истории.
        /// </summary>
        //[NonSerialized]
        private readonly HistoryService historyService;

        /// <summary>
        /// Главная модель представления.
        /// </summary>
        //[NonSerialized]
        private readonly MainViewModel mainViewModel;

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        //[NonSerialized]
        private readonly IMessageService messageService;

        /// <summary>
        /// Кастомные слои, на изменения которых мы подписаны.
        /// </summary>
        private List<CustomLayerViewModel> subscribers = new List<CustomLayerViewModel>();

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие надобности котельной.
        /// </summary>
        public event EventHandler<NeedBoilerEventArgs> BoilerNeeded;

        /// <summary>
        /// Событие запроса ввода значения.
        /// </summary>
        public event EventHandler<ValueInputViewRequestedEventArgs> ValueInputRequested;

        /// <summary>
        /// Событие запроса отображения представления.
        /// </summary>
        public event EventHandler ViewRequested;

        /// <summary>
        /// Событие изменения видимости кастомного слоя.
        /// </summary>
        public event EventHandler<CustomLayerVisibilityChangedEventArgs> VisibilityChanged;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CustomLayersViewModel"/>.
        /// </summary>
        /// <param name="mainViewModel">Главная модель представления.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="dataService">Сервис доступа к данным.</param>
        /// <param name="historyService">Сервис истории.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public CustomLayersViewModel(MainViewModel mainViewModel, AccessService accessService, IDataService dataService, HistoryService historyService, IMessageService messageService)
        {
            this.mainViewModel = mainViewModel;
            this.accessService = accessService;
            this.dataService = dataService;
            this.historyService = historyService;
            this.messageService = messageService;

            this.AddLayerCommand = new RelayCommand(this.ExecuteAddLayer);
            this.ChangeLayerCommand = new RelayCommand(this.ExecuteChangeLayer, this.CanExecuteChangeLayer);
            this.DeleteLayerCommand = new RelayCommand(this.ExecuteDeleteLayer, this.CanExecuteDeleteLayer);
            this.EditCommand = new RelayCommand(this.ExecuteEdit, this.CanExecuteEdit);
            this.PasteAgreedHeaderCommand = new RelayCommand(this.ExecutePasteAgreedHeader, this.CanExecutePasteAgreedHeader);
            this.PasteApprovedHeaderCommand = new RelayCommand(this.ExecutePasteApprovedHeader, this.CanExecutePasteApprovedHeader);
            this.PasteLengthPerDiameterCommand = new RelayCommand(this.ExecutePasteLengthPerDiameter, this.CanExecutePasteLengthPerDiameter);

            this.Layers.CollectionChanged += this.Layers_CollectionChanged;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает команду добавления кастомного слоя.
        /// </summary>
        public RelayCommand AddLayerCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду изменения кастомного слоя.
        /// </summary>
        public RelayCommand ChangeLayerCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает удаляемые кастомные слои.
        /// </summary>
        public List<CustomLayerViewModel> DeletedLayers
        {
            get;
        } = new List<CustomLayerViewModel>();

        /// <summary>
        /// Возвращает команду удаления кастомного слоя.
        /// </summary>
        public RelayCommand DeleteLayerCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду редактирования кастомных слоев.
        /// </summary>
        public RelayCommand EditCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеются ли кастомные слои.
        /// </summary>
        public bool HasLayers
        {
            get
            {
                return this.Layers.Count > 0;
            }
        }

        /// <summary>
        /// Возвращает кастомные слои.
        /// </summary>
        public AdvancedObservableCollection<CustomLayerViewModel> Layers
        {
            get;
        } = new AdvancedObservableCollection<CustomLayerViewModel>();

        /// <summary>
        /// Возвращает команду вставки заголовка согласования.
        /// </summary>
        public RelayCommand PasteAgreedHeaderCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду вставки заголовка утверждения.
        /// </summary>
        public RelayCommand PasteApprovedHeaderCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду вставки свода труб по диаметрам.
        /// </summary>
        public RelayCommand PasteLengthPerDiameterCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает выбранный кастомный слой.
        /// </summary>
        public CustomLayerViewModel SelectedLayer
        {
            get
            {
                return this.selectedLayer;
            }
            set
            {
                this.selectedLayer = value;

                this.NotifyPropertyChanged(nameof(this.SelectedLayer));

                this.ChangeLayerCommand.RaiseCanExecuteChanged();
                this.DeleteLayerCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="INotifyPropertyChanged"/> кастомного слоя.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Layer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CustomLayerViewModel.IsVisible))
                this.VisibilityChanged?.Invoke(this, new CustomLayerVisibilityChangedEventArgs(sender as CustomLayerViewModel));
        }

        /// <summary>
        /// Обрабатывает событие <see cref="INotifyCollectionChanged.CollectionChanged"/> кастомных слоев.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Layers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                        foreach (CustomLayerViewModel layer in e.NewItems)
                        {
                            this.subscribers.Add(layer);

                            layer.PropertyChanged += this.Layer_PropertyChanged;
                        }

                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                        foreach (CustomLayerViewModel layer in e.OldItems)
                        {
                            this.subscribers.Remove(layer);

                            layer.PropertyChanged -= this.Layer_PropertyChanged;
                        }

                    break;

                case NotifyCollectionChangedAction.Reset:
                    if (this.Layers.Count == 0)
                    {
                        foreach (var layer in this.subscribers)
                            layer.PropertyChanged -= this.Layer_PropertyChanged;

                        this.subscribers.Clear();
                    }
                    else
                    {
                        this.subscribers.AddRange(this.Layers);

                        foreach (var layer in this.subscribers)
                            layer.PropertyChanged += this.Layer_PropertyChanged;
                    }

                    break;
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить изменение кастомного слоя.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteChangeLayer()
        {
            return this.SelectedLayer != null;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить удаление кастомного слоя.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteDeleteLayer()
        {
            return this.SelectedLayer != null;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить редактирование кастомного слоя.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteEdit()
        {
            return this.schema != null && (this.schema.IsActual && this.accessService.CanDraw || this.schema.IsIS && this.accessService.CanDrawIS);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить вставку заголовка согласования.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecutePasteAgreedHeader()
        {
            return this.schema != null && (this.schema.IsActual && this.accessService.CanDraw || this.schema.IsIS && this.accessService.CanDrawIS);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить вставку заголовка утверждения.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecutePasteApprovedHeader()
        {
            return this.schema != null && (this.schema.IsActual && this.accessService.CanDraw || this.schema.IsIS && this.accessService.CanDrawIS);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить вставку свода труб по диаметрам.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecutePasteLengthPerDiameter()
        {
            return this.schema != null && (this.schema.IsActual && this.accessService.CanDraw || this.schema.IsIS && this.accessService.CanDrawIS);
        }

        /// <summary>
        /// Выполняет добавление нового слоя.
        /// </summary>
        private void ExecuteAddLayer()
        {
            var layer = new CustomLayerViewModel(new CustomLayerModel(Guid.Empty, this.schema, this.city.Id, "Новый слой"), this.dataService);

            var action = new AddRemoveCustomLayerAction(layer, this, true);

            this.historyService.Add(new HistoryEntry(action, Target.Data, "добавление нового пользовательского слоя"));

            action.Do();

            this.SelectedLayer = layer;
        }

        /// <summary>
        /// Выполняет удаление слоя.
        /// </summary>
        private void ExecuteChangeLayer()
        {
            var layer = this.SelectedLayer;

            var eventArgs = new ValueInputViewRequestedEventArgs(new ValueInputViewModel("Название пользовательского слоя:", "Редактирование пользовательского слоя", typeof(string), layer.Name, null, this.messageService));

            this.ValueInputRequested?.Invoke(this, eventArgs);

            if (eventArgs.HasResult)
            {
                var value = Convert.ToString(eventArgs.ViewModel.Result);

                if (string.IsNullOrEmpty(value))
                {
                    this.messageService.ShowMessage("Не удалось изменить название пользовательского слоя, так как оно не должно быть пустым", "Редактирование пользовательского слоя", MessageType.Error);

                    return;
                }

                var action = new RenameCustomLayerAction(layer, layer.Name, value);

                this.historyService.Add(new HistoryEntry(action, Target.Data, "изменение названия пользовательского слоя"));

                action.Do();
            }
        }

        /// <summary>
        /// Выполняет удаление слоя.
        /// </summary>
        private void ExecuteDeleteLayer()
        {
            var layer = this.SelectedLayer;

            this.SelectedLayer = null;

            var action = new AddRemoveCustomLayerAction(layer, this, false);

            this.historyService.Add(new HistoryEntry(action, Target.Data, "удаление пользовательского слоя"));

            action.Do();
        }

        /// <summary>
        /// Выполняет редактирование кастомных слоев.
        /// </summary>
        private void ExecuteEdit()
        {
            this.ViewRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Выполняет вставку заголовка согласования.
        /// </summary>
        private void ExecutePasteAgreedHeader()
        {
            this.mainViewModel.InsertApprovedHeader(ApprovedHeaderType.Agreed);
        }

        /// <summary>
        /// Выполняет вставку заголовка утверждения.
        /// </summary>
        private void ExecutePasteApprovedHeader()
        {
            this.mainViewModel.InsertApprovedHeader(ApprovedHeaderType.Approved);
        }

        /// <summary>
        /// Выполняет вставку свода труб по диаметрам.
        /// </summary>
        private void ExecutePasteLengthPerDiameter()
        {
            var boilers = new List<SelectableBoilerViewModel>();

            foreach (var boiler in city.GetBoilers(schema, dataService))
                boilers.Add(new SelectableBoilerViewModel(boiler.Item1, boiler.Item2));

            if (boilers.Count == 0)
            {
                this.messageService.ShowMessage("Котельные не найдены!", "Выбор котельной", MessageType.Error);

                return;
            }

            var eventArgs = new NeedBoilerEventArgs(new SelectBoilerViewModel(boilers));

            this.BoilerNeeded?.Invoke(this, eventArgs);

            if (eventArgs.ViewModel.IsBoilerSelected)
                this.mainViewModel.InsertLengthPerDiameterTable(eventArgs.ViewModel.SelectedBoiler.Id);
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Начинает применение изменений слоев.
        /// </summary>
        public void BeginApplyChanges()
        {
            foreach (var layer in this.Layers)
                layer.BeginSave();

            foreach (var layer in this.DeletedLayers)
                layer.DeleteFromSource();
        }

        /// <summary>
        /// Заканчивает применение изменений слоев.
        /// </summary>
        public void EndApplyChanges()
        {
            foreach (var layer in this.Layers)
                layer.EndSave();

            this.DeletedLayers.Clear();
        }

        /// <summary>
        /// Инициализирует кастомные слои.
        /// </summary>
        /// <param name="schema">Схема.</param>
        /// <param name="city">Населенный пункт.</param>
        public void Init(SchemaModel schema, CityViewModel city)
        {
            this.schema = schema;
            this.city = city;

            var layers = new List<CustomLayerViewModel>();

            if (city != null)
                foreach (var layer in this.dataService.CustomLayerAccessService.GetCustomLayers(schema, city.Id))
                    layers.Add(new CustomLayerViewModel(layer, this.dataService));

            this.Layers.AddRange(layers);

            this.EditCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Обновляет состояния кастомных слоев.
        /// </summary>
        public void RefreshLayers()
        {
            this.NotifyPropertyChanged(nameof(this.HasLayers));
        }

        /// <summary>
        /// Отменяет применение изменений слоев.
        /// </summary>
        public void RevertApplyChanges()
        {
            foreach (var layer in this.Layers)
                layer.RevertSave();
        }

        /// <summary>
        /// Убивает кастомные слои.
        /// </summary>
        public void Terminate()
        {
            this.schema = null;
            
            this.Layers.Clear();

            this.DeletedLayers.Clear();

            this.EditCommand.RaiseCanExecuteChanged();
        }

        #endregion
    }
}