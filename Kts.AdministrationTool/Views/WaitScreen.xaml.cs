using Kts.AdministrationTool.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kts.AdministrationTool.Views
{
    /// <summary>
    /// Interaction logic for WaitScreen.xaml
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public partial class WaitScreen : Window
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что может ли представление быть закрытым.
        /// </summary>
        private bool canClose;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Модель представления ожидания окончания выполнения какой-либо задачи.
        /// </summary>
        private readonly WaitScreenViewModel waitViewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="WaitView"/>.
        /// </summary>
        /// <param name="waitViewModel">Модель представления ожидания окончания выполнения какой-либо задачи.</param>
        /// <param name="icon">Иконка представления.</param>
        public WaitScreen(WaitScreenViewModel waitViewModel, ImageSource icon)
        {
            this.InitializeComponent();

            this.waitViewModel = waitViewModel;
            this.Icon = icon;

            this.DataContext = waitViewModel;

            waitViewModel.CloseRequested += this.WaitViewModel_CloseRequested;

            this.Loaded += Window_Loaded;
        }
        
        public WaitScreen(WaitScreenViewModel waitViewModel)
        {
            this.InitializeComponent();

            this.waitViewModel = waitViewModel;


            this.DataContext = waitViewModel;

            waitViewModel.CloseRequested += this.WaitViewModel_CloseRequested;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="WaitViewModel.CloseRequested"/> модели представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void WaitViewModel_CloseRequested(object sender, EventArgs e)
        {
            this.canClose = true;

            Application.Current.Dispatcher.BeginInvoke(new Action(() => this.Close()));
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Window.Closing"/> представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = !this.canClose;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="FrameworkElement.Loaded"/> представления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.waitViewModel.Action != null)
                await this.waitViewModel.StartActionAsync();
            else
                await this.waitViewModel.StartFuncAsync();
        }

        #endregion
    }
}

