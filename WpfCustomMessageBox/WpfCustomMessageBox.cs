using System.Windows;
using WpfCustomMessageBox.Windows;

namespace WpfCustomMessageBox
{
    /// <summary>
    /// Представляет кастомное окно сообщения WPF.
    /// </summary>
    public static class WpfCustomMessageBox
    {
        #region Открытые статические методы

        /// <summary>
        /// Отображает окно сообщения.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        /// <param name="owner">Окно-владелец.</param>
        /// <returns>Результат окна сообщения.</returns>
        public static MessageBoxResult Show(string message, Window owner)
        {
            var msg = new WpfCustomMessageBoxWindow(message);

            if (owner != null)
            {
                msg.Icon = owner.Icon;
                msg.Owner = owner;
            }

            msg.ShowDialog();

            return msg.Result;
        }

        /// <summary>
        /// Отображает окно сообщения.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        /// <param name="caption">Заголовок сообщения.</param>
        /// <param name="owner">Окно-владелец.</param>
        /// <returns>Результат окна сообщения.</returns>
        public static MessageBoxResult Show(string message, string caption, Window owner)
        {
            var msg = new WpfCustomMessageBoxWindow(message, caption);

            if (owner != null)
            {
                msg.Icon = owner.Icon;
                msg.Owner = owner;
            }

            msg.ShowDialog();

            return msg.Result;
        }

        /// <summary>
        /// Отображает окно сообщения.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        /// <param name="caption">Заголовок сообщения.</param>
        /// <param name="button">Кнопка(и) сообщения.</param>
        /// <param name="icon">Иконка сообщения.</param>
        /// <param name="owner">Окно-владелец.</param>
        /// <returns>Результат окна сообщения.</returns>
        public static MessageBoxResult Show(string message, string caption, MessageBoxButton button, MessageBoxImage icon, Window owner)
        {
            var msg = new WpfCustomMessageBoxWindow(message, caption, button, icon);

            if (owner != null)
            {
                msg.Icon = owner.Icon;
                msg.Owner = owner;
            }

            msg.ShowDialog();

            return msg.Result;
        }
        
        /// <summary>
        /// Отображает окно сообщения.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        /// <param name="caption">Заголовок сообщения.</param>
        /// <param name="button">Кнопка(и) сообщения.</param>
        /// <param name="owner">Окно-владелец.</param>
        /// <returns>Результат окна сообщения.</returns>
        public static MessageBoxResult Show(string message, string caption, MessageBoxButton button, Window owner)
        {
            var msg = new WpfCustomMessageBoxWindow(message, caption, button);

            if (owner != null)
            {
                msg.Icon = owner.Icon;
                msg.Owner = owner;
            }

            msg.ShowDialog();

            return msg.Result;
        }

        #endregion
    }
}