using System.Drawing;
using System.Windows;
using WpfCustomMessageBox.Classes;

namespace WpfCustomMessageBox.Windows
{
    /// <summary>
    /// Представляет представление окна сообщения WPF.
    /// </summary>
    internal sealed partial class WpfCustomMessageBoxWindow : Window
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="WpfCustomMessageBoxWindow"/>.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        public WpfCustomMessageBoxWindow(string message)
        {
            this.InitializeComponent();

            this.Message = message;

            this.imageMessageBox.Visibility = Visibility.Collapsed;

            this.DisplayButtons(MessageBoxButton.OK);
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="WpfCustomMessageBoxWindow"/>.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        /// <param name="caption">Заголовок сообщения.</param>
        public WpfCustomMessageBoxWindow(string message, string caption)
        {
            this.InitializeComponent();

            this.Message = message;
            this.Caption = caption;

            this.imageMessageBox.Visibility = Visibility.Collapsed;

            this.DisplayButtons(MessageBoxButton.OK);
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="WpfCustomMessageBoxWindow"/>.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        /// <param name="caption">Заголовок сообщения.</param>
        /// <param name="button">Кнопка(и) сообщения.</param>
        public WpfCustomMessageBoxWindow(string message, string caption, MessageBoxButton button)
        {
            this.InitializeComponent();

            this.Message = message;
            this.Caption = caption;

            this.DisplayButtons(button);

            this.imageMessageBox.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="WpfCustomMessageBoxWindow"/>.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        /// <param name="caption">Заголовок сообщения.</param>
        /// <param name="button">Кнопка(и) сообщения.</param>
        /// <param name="image">Изображение сообщения.</param>
        public WpfCustomMessageBoxWindow(string message, string caption, MessageBoxButton button, MessageBoxImage image)
        {
            this.InitializeComponent();

            this.Message = message;
            this.Caption = caption;

            this.DisplayButtons(button);
            this.DisplayImage(image);
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="WpfCustomMessageBoxWindow"/>.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        /// <param name="caption">Заголовок сообщения.</param>
        /// <param name="image">Изображение сообщения.</param>
        public WpfCustomMessageBoxWindow(string message, string caption, MessageBoxImage image)
        {
            this.InitializeComponent();

            this.Message = message;
            this.Caption = caption;

            this.DisplayImage(image);

            this.DisplayButtons(MessageBoxButton.OK);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает текст кнопки "Отмена".
        /// </summary>
        public string ButtonCancelText
        {
            get
            {
                return this.labelCancel.Content.ToString();
            }
            set
            {
                this.labelCancel.Content = value.TryAddKeyboardAccellerator();
            }
        }

        /// <summary>
        /// Возвращает или задает текст кнопки "Нет".
        /// </summary>
        public string ButtonNoText
        {
            get
            {
                return this.labelNo.Content.ToString();
            }
            set
            {
                this.labelNo.Content = value.TryAddKeyboardAccellerator();
            }
        }

        /// <summary>
        /// Возвращает или задает текст кнопки "ОК".
        /// </summary>
        public string ButtonOKText
        {
            get
            {
                return this.labelOK.Content.ToString();
            }
            set
            {
                this.labelOK.Content = value.TryAddKeyboardAccellerator();
            }
        }

        /// <summary>
        /// Возвращает или задает текст кнопки "Да".
        /// </summary>
        public string ButtonYesText
        {
            get
            {
                return this.labelYes.Content.ToString();
            }
            set
            {
                this.labelYes.Content = value.TryAddKeyboardAccellerator();
            }
        }

        /// <summary>
        /// Возвращает или задает заголовок сообщения.
        /// </summary>
        public string Caption
        {
            get
            {
                return this.Title;
            }
            set
            {
                this.Title = value;
            }
        }

        /// <summary>
        /// Возвращает или задает текст сообщения.
        /// </summary>
        public string Message
        {
            get
            {
                return this.textBlockMessage.Text;
            }
            set
            {
                this.textBlockMessage.Text = value;
            }
        }

        /// <summary>
        /// Возвращает или задает результат выбора.
        /// </summary>
        public MessageBoxResult Result
        {
            get;
            set;
        } = MessageBoxResult.Cancel;

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="System.Windows.Controls.Primitives.ButtonBase.Click"/> кнопки "Отмена".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageBoxResult.Cancel;

            this.Close();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="System.Windows.Controls.Primitives.ButtonBase.Click"/> кнопки "Нет".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void buttonNo_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageBoxResult.No;

            this.Close();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="System.Windows.Controls.Primitives.ButtonBase.Click"/> кнопки "ОК".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageBoxResult.OK;

            this.Close();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="System.Windows.Controls.Primitives.ButtonBase.Click"/> кнопки "Да".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void buttonYes_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageBoxResult.Yes;

            this.Close();
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Отображает заданную(ые) кнопку(и).
        /// </summary>
        /// <param name="button">Кнопка(и).</param>
        private void DisplayButtons(MessageBoxButton button)
        {
            switch (button)
            {
                case MessageBoxButton.OKCancel:
                    this.buttonOK.Visibility = Visibility.Visible;
                    this.buttonCancel.Visibility = Visibility.Visible;

                    this.buttonOK.Focus();

                    this.buttonYes.Visibility = Visibility.Collapsed;
                    this.buttonNo.Visibility = Visibility.Collapsed;

                    break;

                case MessageBoxButton.YesNo:
                    this.buttonYes.Visibility = Visibility.Visible;
                    this.buttonNo.Visibility = Visibility.Visible;

                    this.buttonYes.Focus();

                    this.buttonOK.Visibility = Visibility.Collapsed;
                    this.buttonCancel.Visibility = Visibility.Collapsed;

                    break;

                case MessageBoxButton.YesNoCancel:
                    this.buttonYes.Visibility = Visibility.Visible;
                    this.buttonNo.Visibility = Visibility.Visible;
                    this.buttonCancel.Visibility = Visibility.Visible;

                    this.buttonYes.Focus();

                    this.buttonOK.Visibility = Visibility.Collapsed;

                    break;

                default:
                    this.buttonOK.Visibility = Visibility.Visible;

                    this.buttonOK.Focus();

                    this.buttonYes.Visibility = Visibility.Collapsed;
                    this.buttonNo.Visibility = Visibility.Collapsed;
                    this.buttonCancel.Visibility = Visibility.Collapsed;

                    break;
            }
        }

        /// <summary>
        /// Отображает заданное изображение.
        /// </summary>
        /// <param name="image">Изображение.</param>
        private void DisplayImage(MessageBoxImage image)
        {
            Icon icon;

            switch (image)
            {
                case MessageBoxImage.Error:
                    icon = SystemIcons.Hand;

                    break;

                case MessageBoxImage.Exclamation:
                    icon = SystemIcons.Exclamation;

                    break;

                case MessageBoxImage.Information:
                    icon = SystemIcons.Information;

                    break;

                case MessageBoxImage.Question:
                    icon = SystemIcons.Question;

                    break;

                default:
                    icon = SystemIcons.Information;

                    break;
            }

            this.imageMessageBox.Source = icon.ToImageSource();
            this.imageMessageBox.Visibility = Visibility.Visible;
        }

        #endregion
    }
}