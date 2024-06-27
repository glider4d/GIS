using Kts.Gis.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление смены пароля.
    /// </summary>
    internal sealed partial class ChangePasswordView : Window
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Модель представления.
        /// </summary>
        private readonly ChangePasswordViewModel viewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ChangePasswordView"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public ChangePasswordView(ChangePasswordViewModel viewModel)
        {
            this.InitializeComponent();

            this.viewModel = viewModel;

            this.DataContext = viewModel;

            viewModel.CloseRequested += this.ViewModel_CloseRequested;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.KeyDown"/> поля ввода нового пароля.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void passwordBoxNew_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                this.passwordBoxNewNew.Focus();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="PasswordBox.PasswordChanged"/> поля ввода нового пароля.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void passwordBoxNew_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.viewModel.NewPassword = (sender as PasswordBox).Password;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="PasswordBox.PasswordChanged"/> поля повторного ввода нового пароля.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void passwordBoxNewNew_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.viewModel.NewNewPassword = (sender as PasswordBox).Password;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.KeyDown"/> поля ввода старого пароля.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void passwordBoxOld_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                this.passwordBoxNew.Focus();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="PasswordBox.PasswordChanged"/> поля ввода старого пароля.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void passwordBoxOld_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.viewModel.OldPassword = (sender as PasswordBox).Password;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="ChangePasswordViewModel.CloseRequested"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ViewModel_CloseRequested(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}