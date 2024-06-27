using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.Services;
using Kts.History;
using Kts.Messaging;
using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления объекта.
    /// </summary>
    [Serializable]
    internal abstract partial class ObjectViewModel : ServicedViewModel, IObjectViewModel
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Объект.
        /// </summary>
        private readonly IObjectModel obj;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ObjectViewModel"/>.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="historyService">Сервис истории изменений.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public ObjectViewModel(IObjectModel obj, AccessService accessService, IDataService dataService, HistoryService historyService, IMessageService messageService) : base(dataService, messageService)
        {
            this.obj = obj;
            this.AccessService = accessService;
            this.HistoryService = historyService;
        }

        #endregion

        #region Защищенные свойства
        //[NonSerialized]
        private HistoryService m_historyService;
        /// <summary>
        /// Возвращает сервис истории изменений.
        /// </summary>
        protected HistoryService HistoryService
        {
            get
            {
                return m_historyService;
            }
            private set
            {
                m_historyService = value;
            }

        }

        #endregion

        #region Открытые статические свойства

        /// <summary>
        /// Возвращает менеджер буфера обмена.
        /// </summary>
        public static ClipboardManager ClipboardManager
        {
            get
            {
                return m_clipboardManager; 
            }
        }// = new ClipboardManager();
        //[NonSerialized]
        static ClipboardManager m_clipboardManager = new ClipboardManager();
        #endregion
    }

    // Реализация IObjectViewModel.
    internal abstract partial class ObjectViewModel
    {
        #region Открытые свойства


        //[NonSerialized]
        private AccessService m_accessService;
        /// <summary>
        /// Возвращает сервис доступа к функциям приложения.
        /// </summary>
        
        public AccessService AccessService
        {
            get
            {
                return m_accessService;
            }
            private set
            {
                m_accessService = value;
            }
        }

        /// <summary>
        /// Возвращает идентификатор населенного пункта, в котором находится объект.
        /// </summary>
        public int CityId
        {
            get
            {
                return this.obj.CityId;
            }
        }

       


        /// <summary>
        /// Возвращает или задает идентификатор объекта.
        /// </summary>
        public Guid Id
        {
            get
            {
                return this.obj.Id;
            }
            private set
            {
                this.obj.Id = value;
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что сохранен ли объект в источнике данных.
        /// </summary>
        public bool IsSaved
        {
            get
            {
                return this.Id != ObjectModel.DefaultId;
            }
        }

        /// <summary>
        /// Возвращает идентификатор родителя объекта.
        /// </summary>
        public Guid? ParentId
        {
            get
            {
                return this.obj.ParentId;
            }
        }

        #endregion

        #region Открытые абстрактные свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли объект активным.
        /// </summary>
        public abstract bool IsActive
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что инициализирован ли объект.
        /// </summary>
        public abstract bool IsInitialized
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли объект планируемым.
        /// </summary>
        public abstract bool IsPlanning
        {
            get;
            set;
        }
        
        /// <summary>
        /// Возвращает или задает тип объекта.
        /// </summary>
        public abstract ObjectType Type
        {
            get;
            set;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Вызывается при изменении свойства.
        /// </summary>
        /// <param name="propertyName">Название измененного свойства.</param>
        public void OnPropertyChanged(string propertyName)
        {
            this.NotifyPropertyChanged(propertyName);
        }

        #endregion

        #region Открытые абстрактные методы
        
        /// <summary>
        /// Начинает сохранение объекта в источнике данных.
        /// </summary>
        public abstract void BeginSave();

        /// <summary>
        /// Завершает сохранение объекта в источнике данных.
        /// </summary>
        public abstract void EndSave();

        /// <summary>
        /// Отменяет сохранение объекта в источнике данных.
        /// </summary>
        public abstract void RevertSave();

        #endregion
    }
}