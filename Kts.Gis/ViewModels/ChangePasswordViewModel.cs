using Kts.Gis.Data;
using Kts.Messaging;
using Kts.WpfUtilities;
using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления смены пароля.
    /// </summary>
    internal sealed class ChangePasswordViewModel
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;
        public IDataService m_dataService;

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        private readonly IMessageService messageService;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса закрытия представления.
        /// </summary>
        public event EventHandler CloseRequested;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ChangePasswordViewModel"/>.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public ChangePasswordViewModel(IDataService dataService, IMessageService messageService)
        {
            this.dataService = dataService;
            this.messageService = messageService;

            this.ChangePasswordCommand = new RelayCommand(this.ExecuteChangePassword);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает команду смены пароля.
        /// </summary>
        public RelayCommand ChangePasswordCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает повторно введенный новый пароль.
        /// </summary>
        public string NewNewPassword
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает новый пароль.
        /// </summary>
        public string NewPassword
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает старый пароль.
        /// </summary>
        public string OldPassword
        {
            get;
            set;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выполняет смену пароля.
        /// </summary>
        private void ExecuteChangePassword()
        {
            if (string.IsNullOrEmpty(this.OldPassword) || string.IsNullOrEmpty(this.NewPassword) || string.IsNullOrEmpty(this.NewNewPassword))
            {
                this.messageService.ShowMessage("Необходимо заполнить все поля", "Смена пароля", MessageType.Error);

                return;
            }

            if (this.NewPassword != this.NewNewPassword)
            {
                this.messageService.ShowMessage("Новые пароли не совпадают", "Смена пароля", MessageType.Error);

                return;
            }

            if (this.dataService.LoginAccessService.ChangePassword(this.dataService.LoggedUserId, AuthorizationViewModel.EncryptSHA256(this.OldPassword), AuthorizationViewModel.EncryptSHA256(this.NewPassword)))
                this.messageService.ShowMessage("Пароль успешно изменен", "Смена пароля", MessageType.Information);
            else
            {
                this.messageService.ShowMessage("Неверно введен старый пароль", "Смена пароля", MessageType.Error);

                return;
            }

            this.CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}