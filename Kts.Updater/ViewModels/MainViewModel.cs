using Kts.Messaging;
using Kts.Updater.Models;
using Kts.Updater.Services;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Kts.Updater.ViewModels
{
    /// <summary>
    /// Представляет главную модель представления.
    /// </summary>
    internal sealed class MainViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Текущая страница.
        /// </summary>
        private IPageViewModel curPage;

        /// <summary>
        /// Текст "Далее".
        /// </summary>
        private string forwardText;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly SqlDataService dataService;

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        //[NonSerialized]
        private readonly IMessageService messageService;

        /// <summary>
        /// Список страниц.
        /// </summary>
        private readonly List<IPageViewModel> pages = new List<IPageViewModel>();

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса представления.
        /// </summary>
        public event EventHandler CloseRequested;

        /// <summary>
        /// Событие создания пакета обновления.
        /// </summary>
        public event EventHandler<PackagingEventArgs> Packaging;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MainViewModel"/>.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public MainViewModel(SqlConnectionString connectionString, SqlDataService dataService, IMessageService messageService)
        {
            this.dataService = dataService;
            this.messageService = messageService;

            dataService.SetConnectionString(connectionString);

            this.Target = connectionString.Name + " - " + connectionString.Database;

            this.BackCommand = new RelayCommand(this.ExecuteBack, this.CanExecuteBack);
            this.ForwardCommand = new RelayCommand(this.ExecuteForward);

            this.TablesPage = new TablesViewModel(dataService);
            this.CatalogsPage = new CatalogsViewModel(dataService);
            this.ViewsPage = new ViewsViewModel(dataService);
            this.StoredProceduresPage = new StoredProceduresViewModel(dataService);
            this.ScalarFunctionsPage = new ScalarFunctionsViewModel(dataService);
            this.TableFunctionsPage = new TableFunctionsViewModel(dataService);
            this.CodePage = new CodeViewModel(dataService);

            this.TablesPage.PropertyChanged += this.Page_PropertyChanged;
            this.CatalogsPage.PropertyChanged += this.Page_PropertyChanged;
            this.ViewsPage.PropertyChanged += this.Page_PropertyChanged;
            this.StoredProceduresPage.PropertyChanged += this.Page_PropertyChanged;
            this.ScalarFunctionsPage.PropertyChanged += this.Page_PropertyChanged;
            this.TableFunctionsPage.PropertyChanged += this.Page_PropertyChanged;
            this.CodePage.PropertyChanged += this.Page_PropertyChanged;

            this.pages.Add(this.TablesPage);
            this.pages.Add(this.CatalogsPage);
            this.pages.Add(this.ViewsPage);
            this.pages.Add(this.StoredProceduresPage);
            this.pages.Add(this.ScalarFunctionsPage);
            this.pages.Add(this.TableFunctionsPage);
            this.pages.Add(this.CodePage);

            this.TablesPage.IsSelected = true;

            this.UpdateForward();
        }
        
        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает команду "Назад".
        /// </summary>
        public RelayCommand BackCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает страницу с каталогами.
        /// </summary>
        public CatalogsViewModel CatalogsPage
        {
            get;
        }

        /// <summary>
        /// Возвращает страницу с кодом.
        /// </summary>
        public CodeViewModel CodePage
        {
            get;
        }

        /// <summary>
        /// Возвращает команду "Вперед".
        /// </summary>
        public RelayCommand ForwardCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает текст "Далее".
        /// </summary>
        public string ForwardText
        {
            get
            {
                return this.forwardText;
            }
            private set
            {
                if (this.ForwardText != value)
                {
                    this.forwardText = value;

                    this.NotifyPropertyChanged(nameof(this.ForwardText));
                }
            }
        }

        /// <summary>
        /// Возвращает страницу скалярных функций.
        /// </summary>
        public ScalarFunctionsViewModel ScalarFunctionsPage
        {
            get;
        }

        /// <summary>
        /// Возвращает страницу хранимых процедур.
        /// </summary>
        public StoredProceduresViewModel StoredProceduresPage
        {
            get;
        }

        /// <summary>
        /// Возвращает страницу табличных функций.
        /// </summary>
        public TableFunctionsViewModel TableFunctionsPage
        {
            get;
        }

        /// <summary>
        /// Возвращает страницу таблиц.
        /// </summary>
        public TablesViewModel TablesPage
        {
            get;
        }

        /// <summary>
        /// Возвращает цель обновления.
        /// </summary>
        public string Target
        {
            get;
        }

        /// <summary>
        /// Возвращает страницу представлений.
        /// </summary>
        public ViewsViewModel ViewsPage
        {
            get;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="INotifyPropertyChanged.PropertyChanged"/> модели представления страницы.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Page_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IPageViewModel.IsSelected))
                if ((sender as IPageViewModel).IsSelected)
                    this.curPage = sender as IPageViewModel;

            this.UpdateForward();

            this.BackCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить "Назад".
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteBack()
        {
            return this.pages.IndexOf(this.curPage) > 0;
        }

        /// <summary>
        /// Выполняет "Назад".
        /// </summary>
        private void ExecuteBack()
        {
            var index = this.pages.IndexOf(this.curPage) - 1;

            this.curPage.IsSelected = false;

            this.pages[index].IsSelected = true;
        }

        /// <summary>
        /// Выполняет "Вперед".
        /// </summary>
        private void ExecuteForward()
        {
            if (!this.IsPageLast(this.curPage))
            {
                var index = this.pages.IndexOf(this.curPage) + 1;

                this.curPage.IsSelected = false;

                this.pages[index].IsSelected = true;
            }
            else
            {
                // Подготавливаем справочники.
                this.CatalogsPage.PrepareData();
                
                // Собираем объекты.
                var objects = new List<SqlObjectModel>();
                objects.AddRange(this.TablesPage.GetSelectedObjects());
                objects.AddRange(this.CatalogsPage.GetSelectedObjects());
                objects.AddRange(this.ViewsPage.GetSelectedObjects());
                objects.AddRange(this.StoredProceduresPage.GetSelectedObjects());
                objects.AddRange(this.ScalarFunctionsPage.GetSelectedObjects());
                objects.AddRange(this.TableFunctionsPage.GetSelectedObjects());
                objects.AddRange(this.CodePage.GetSelectedObjects());

                // Создаем новый пакет обновления.
                var eventArgs = new PackagingEventArgs(PackageViewModel.CreateNew(objects, this.dataService, this.messageService));
                this.Packaging?.Invoke(this, eventArgs);

                if (eventArgs.IsCreated)
                    this.CloseRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли заданная страница последней в списке.
        /// </summary>
        /// <param name="page">Проверяемая страница.</param>
        /// <returns>true, если является, иначе - false.</returns>
        private bool IsPageLast(IPageViewModel page)
        {
            return this.pages.IndexOf(page) == this.pages.Count - 1;
        }

        /// <summary>
        /// Обновляет текст "Далее".
        /// </summary>
        private void UpdateForward()
        {
            if (this.IsPageLast(this.curPage))
                this.ForwardText = "Готово";
            else
                this.ForwardText = "Далее";
        }

        #endregion
    }
}