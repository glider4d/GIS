using Kts.Excel;
using Kts.Gis.Data;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace Kts.Gis.Reports.ViewModels
{
    /// <summary>
    /// Представляет модель представления составления отчета КТС по КВП.
    /// </summary>
    public sealed class KtsReportViewModel
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса выполнения долгодлительной задачи.
        /// </summary>
        public event EventHandler<LongTimeTaskRequestedEventArgs> LongTimeTaskRequested;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="KtsKvpReportViewModel"/>.
        /// </summary>
        /// <param name="permittedRegionIds">Идентификаторы разрешенных к выбору регионов.</param>
        /// <param name="dataService">Сервис данных.</param>
        public KtsReportViewModel(List<int> permittedRegionIds, IDataService dataService)
        {
            this.dataService = dataService;

            this.GenerateCommand = new RelayCommand(this.ExecuteGenerate);

            // Инициализируем модель представления выбора региона.
            this.RegionSelectionViewModel = new RegionSelectionViewModel(permittedRegionIds, dataService);
            this.RegionSelectionViewModel.Init();
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает команду формирования отчета.
        /// </summary>
        public RelayCommand GenerateCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает модель представления выбора региона.
        /// </summary>
        public RegionSelectionViewModel RegionSelectionViewModel
        {
            get;
        }

        /// <summary>
        /// Возвращает заголовок представления.
        /// </summary>
        public string Title
        {
            get
            {
                return "О жилищном фонде и договорных подключениях";
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выполняет формирование отчета.
        /// </summary>
        private void ExecuteGenerate()
        {
            this.LongTimeTaskRequested?.Invoke(this, new LongTimeTaskRequestedEventArgs(new WaitViewModel("Составление отчета", "Пожалуйста подождите, идет составление отчета...", () => this.GenerateReportAsync())));
        }

        /// <summary>
        /// Асинхронно составляет отчет.
        /// </summary>
        private async Task GenerateReportAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                // Загружаем шаблон.
                var xsl = new XslCompiledTransform();
                using (var stringReader = new StringReader(this.dataService.ReportAccessService.GetTemplate((int)ReportKind.Kts)))
                    using (var xmlReader = new XmlTextReader(stringReader))
                        xsl.Load(xmlReader);
                
                // Получаем данные отчета в XML-разметке.
                var xml = new XmlDocument();
                var dataSet = this.dataService.ReportAccessService.GetKts(this.RegionSelectionViewModel.SelectedRegion.Id, this.RegionSelectionViewModel.SelectedCity.Id, this.RegionSelectionViewModel.NormalSchemas.First(s => s.IsActual && !s.IsIS), this.RegionSelectionViewModel.SelectedBoiler.Id);
                xml.LoadXml(dataSet.GetXml());

                // Получаем название временного XML-файла.
                var temp = Path.GetTempFileName();

                using (var fileStream = new FileStream(temp, FileMode.Create))
                    xsl.Transform(xml, null, fileStream);

                // Получаем название временного шаблона Excel.
                var docName = "Отчет о жилищном фонде и договорных подключениях.xlt";
                var tempTemplate = Path.GetTempPath() + docName;

                // Сохраняем сгенерированный XML-документ как шаблон Excel.
                var excel = ExcelDocument.OpenXml(temp, false, false, false);
                excel.SaveAsXlt(tempTemplate);
                excel.Close();

                // Удаляем временный XML-файл.
                if (File.Exists(temp))
                    File.Delete(temp);

                // Открываем созданный шаблон Excel и удаляем его файл.
                Process.Start(tempTemplate);
                File.Delete(tempTemplate);
            });
        }

        #endregion
    }
}