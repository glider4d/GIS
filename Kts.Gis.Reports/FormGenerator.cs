using Kts.Gis.Reports.Models;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Kts.Gis.Reports
{
    /// <summary>
    /// Представляет генератор различных форм на основе RDLC.
    /// </summary>
    public static class FormGenerator
    {
        #region Открытые статические события

        /// <summary>
        /// Событие запроса закрытия всех форм.
        /// </summary>
        public static event EventHandler CloseAllFormsRequested;

        /// <summary>
        /// Событие отображения диалога акта раздела границ.
        /// </summary>
        public static event EventHandler<ViewRequestedEventArgs<PartitionActModel>> PartitionActDialog;

        #endregion

        #region Статические конструкторы

        static FormGenerator()
        {
            Forms = new List<Tuple<string, RelayCommand>>();

            Forms.Add(new Tuple<string, RelayCommand>("Акт раздела границ", new RelayCommand(ExecuteGeneratePartitionAct)));
        }

        #endregion

        #region Открытые статические свойства

        /// <summary>
        /// Возвращает список генерируемых форм.
        /// </summary>
        public static List<Tuple<string, RelayCommand>> Forms
        {
            get;
        }

        #endregion

        #region Закрытые статические методы

        /// <summary>
        /// Выполняет генерацию акта раздела границ.
        /// </summary>
        private static void ExecuteGeneratePartitionAct()
        {
            CloseAllForms();

            var eventArgs = new ViewRequestedEventArgs<PartitionActModel>(new PartitionActModel());

            PartitionActDialog?.Invoke(null, eventArgs);

            if (eventArgs.Result)
            {
                var window = GetActiveWindow();

#warning Тут были репорты
                //var view = new RdlcView("Акт раздела границ", "Kts.Gis.Reports.Reports.PartitionAct.rdlc", new ReportDataSource("Model", eventArgs.ViewModel.GetDataTable()))
                //{
                //    Icon = window.Icon,
                //    Owner = window
                //};

                //view.ShowDialog();
            }

            var model = new PartitionActModel()
            {
                Abonent = "ФИО абонента",
                BoilerName = "Котельная",
                BoilerCode = "Код котельной",
                City = "Город",
                DocApprover = "_____________ ФИО",
                DocApproverPost = "Главный инженер ХХХ филиала ГУП \"ЖКХ РС(Я)\"",
                Flat = "_",
                House = "1",
                Image = new byte[0],
                ObjName = "Название объекта",
                Street = "Название улицы"
            };

            model.Diameters1[0] = 1;
        }

        /// <summary>
        /// Возвращает активное окно приложения.
        /// </summary>
        /// <returns>Окно.</returns>
        private static Window GetActiveWindow()
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

            // Если такого окна нет, то пробуем активировать главное окно приложения и выбрать его.
            if (window == null && Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Activate();

                window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            }

            return window;
        }

        #endregion

        #region Открытые статические методы

        /// <summary>
        /// Закрывает все открытые окна.
        /// </summary>
        public static void CloseAllForms()
        {
            CloseAllFormsRequested?.Invoke(null, EventArgs.Empty);
        }

        #endregion
    }
}