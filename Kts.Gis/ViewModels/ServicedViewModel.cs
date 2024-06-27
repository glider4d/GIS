using Kts.Gis.Data;
using Kts.Messaging;
using Kts.Utilities;
using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет базовую модель представления, использующую основные сервисы.
    /// </summary>
    [Serializable]
    internal class ServicedViewModel : BaseViewModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ServicedViewModel"/>.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public ServicedViewModel(IDataService dataService, IMessageService messageService)
        {
            this.DataService = dataService;
            this.MessageService = messageService;
        }

        #endregion

        #region Защищенные свойства

        /// <summary>
        /// Возвращает сервис данных.
        /// </summary>
        
        protected IDataService DataService
        {
            get
            {
                return m_dataService;
            }
            private set
            {
                m_dataService = value;
            }
        }
        //[NonSerialized]
        private IDataService m_dataService;

        //[NonSerialized]
        private IMessageService m_messageService;
        /// <summary>
        /// Возвращает сервис сообщений.
        /// </summary>
        protected IMessageService MessageService
        {
            get
            {
                return m_messageService;
            }
            set
            {
                m_messageService = value;
            }
        }

        #endregion
    }
}