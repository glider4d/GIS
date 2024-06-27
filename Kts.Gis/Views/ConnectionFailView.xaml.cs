using Kts.Gis.ViewModels;
using System;
using System.Data.SqlClient;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление ожидания окончания выполнения какой-либо задачи.
    /// </summary>
    internal sealed partial class ConnectionFailView : Window, IDisposable
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что высвобождены ли ресурсы.
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// Значение, указывающее на то, что было ли отображено представление.
        /// </summary>
        private bool isShown;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Коннектор с базой данных SQL с возможностью переподключения.
        /// </summary>
        private readonly SqlReconnector reconnector;

        /// <summary>
        /// Таймер.
        /// </summary>
        private readonly Timer timer = new Timer(500);

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ConnectionFailView"/>.
        /// </summary>
        /// <param name="reconnector">Коннектор с базой данных SQL с возможностью переподключения.</param>
        public ConnectionFailView(SqlReconnector reconnector)
        {
            this.InitializeComponent();

            this.reconnector = reconnector;
        }

        public ConnectionFailView(SqlReconnector reconnector, string skipButtonText)
        {
            this.InitializeComponent();

            Button skipButton = new Button();
            skipButton.Content = skipButtonText;
            stackPanel.Children.Add(skipButton);

            skipButton.Click += Button_Click;

            this.reconnector = reconnector;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает соединение с базой данных.
        /// </summary>
        public SqlConnection Connection
        {
            get;
            private set;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="Timer.Elapsed"/> таймера.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.timer.Stop();

            this.timer.Elapsed -= this.timer_Elapsed;

            SqlConnection connection;

            while (true)
            {
                connection = this.reconnector.TryGetConnection();

                if (connection != null)
                {
                    this.Connection = connection;

                    break;
                }
            }

            Application.Current.Dispatcher.BeginInvoke(new Action(() => this.Close()));
        }

        #endregion

        #region Закрытые методы
        
        /// <summary>
        /// Высвобождает ресурсы.
        /// </summary>
        /// <param name="isDisposing">Значение, указывающее на то, что нужно ли высвободить ресурсы.</param>
        private void Dispose(bool isDisposing)
        {
            if (!this.isDisposed)
            {
                if (isDisposing)
                    this.timer.Dispose();

                this.isDisposed = true;
            }
        }

        #endregion

        #region Защищенные переопределенные методы

        /// <summary>
        /// Выполняет действия, связанные с рендерингом содержимого представления.
        /// </summary>
        /// <param name="e">Аргументы.</param>
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            if (this.isShown)
                return;

            this.isShown = true;

            timer.Elapsed += this.timer_Elapsed;

            timer.Start();
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => this.Close()));
            //MessageBox.Show("click");
        }
    }

    // Реализация IDisposable.
    internal sealed partial class ConnectionFailView
    {
        #region Открытые методы

        /// <summary>
        /// Высвобождает ресурсы.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}