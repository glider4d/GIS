using Kts.Excel;
using Kts.Gis.Data;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace Kts.Gis.Reports.ViewModels
{
    /// <summary>
    /// Представляет модель представления формирования отчета о технических характеристиках.
    /// </summary>
    public sealed class TechSpecViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Конечная дата.
        /// </summary>
        private DateTime endDateTime;

        /// <summary>
        /// Значение, указывающее на то, что выбраны ли все внутренние отчеты.
        /// </summary>
        private bool isAllChecked;

        /// <summary>
        /// Начальная дата.
        /// </summary>
        private DateTime startDateTime;

        #endregion

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
        /// Инициализирует новый экземпляр класса <see cref="TechSpecViewModel"/>.
        /// </summary>
        /// <param name="permittedRegionIds">Идентификаторы разрешенных к выбору регионов.</param>
        /// <param name="dataService">Сервис данных.</param>
        public TechSpecViewModel(List<int> permittedRegionIds, IDataService dataService)
        {
            this.dataService = dataService;

            this.GenerateCommand = new RelayCommand(this.ExecuteGenerate, this.CanExecuteGenerate);
            
            this.Reports.Add(new TechSpecPartViewModel("ТХ 1", new List<int>()
            {
                
                1,11,12
            }));
            this.Reports.Add(new TechSpecPartViewModel("ТХ 2", new List<int>()
            {
                
                2
            }));
            this.Reports.Add(new TechSpecPartViewModel("ТХ 3", new List<int>()
            {
                
                
                3,
                31,
                32
                
                
            }));
            this.Reports.Add(new TechSpecPartViewModel("ТХ 4", new List<int>()
            {
                
                4
            }));
            this.Reports.Add(new TechSpecPartViewModel("ТХ 5", new List<int>()
            {
                
                5
            }));
            this.Reports.Add(new TechSpecPartViewModel("ТХ 6", new List<int>()
            {
                
                6
            }));
            this.Reports.Add(new TechSpecPartViewModel("ТХ 7", new List<int>()
            {
                7
            }));
            this.Reports.Add(new TechSpecPartViewModel("ТХ 8", new List<int>()
            {
                81,
                82,
                83,
                84
            }));
            this.Reports.Add(new TechSpecPartViewModel("ТХ 9", new List<int>()
            {
                9
            }));
            this.Reports.Add(new TechSpecPartViewModel("ТХ 10", new List<int>()
            {
                10
            }));

            foreach (var entry in this.Reports)
                entry.PropertyChanged += this.Report_PropertyChanged;

            // Инициализируем модель представления выбора региона.
            this.RegionSelectionViewModel = new RegionSelectionViewModel(permittedRegionIds, dataService);
            this.RegionSelectionViewModel.Init();
            
            this.StartDateTime = new DateTime(DateTime.Now.Year, 1, 1);
            this.EndDateTime = DateTime.Now;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает конечную дату.
        /// </summary>
        public DateTime EndDateTime
        {
            get
            {
                return this.endDateTime;
            }
            set
            {
                this.endDateTime = value;
                
                this.GenerateCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Возвращает команду формирования отчета.
        /// </summary>
        public RelayCommand GenerateCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что выбраны ли все внутренние отчеты.
        /// </summary>
        public bool IsAllChecked
        {
            get
            {
                return this.isAllChecked;
            }
            set
            {
                this.isAllChecked = value;

                foreach (var report in this.Reports)
                    report.IsSelected = value;
            }
        }

        /// <summary>
        /// Возвращает модель представления выбора региона.
        /// </summary>
        public RegionSelectionViewModel RegionSelectionViewModel
        {
            get;
        }

        /// <summary>
        /// Возвращает список частей отчета о технических характеристиках.
        /// </summary>
        public List<TechSpecPartViewModel> Reports
        {
            get;
        } = new List<TechSpecPartViewModel>();

        /// <summary>
        /// Возвращает или задает начальную дату.
        /// </summary>
        public DateTime StartDateTime
        {
            get
            {
                return this.startDateTime;
            }
            set
            {
                this.startDateTime = value;

                this.GenerateCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="INotifyPropertyChanged.PropertyChanged"/> внутреннего отчета.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Report_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.GenerateCommand.RaiseCanExecuteChanged();
        }

        #endregion
        
        #region Закрытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить формирование отчета.
        /// </summary>
        /// <returns>Значение, указывающее на то, что можно ли выполнить формирование отчета.</returns>
        private bool CanExecuteGenerate()
        {
            if (this.StartDateTime < this.EndDateTime)
                return this.Reports.Any(x => x.IsSelected);

            return false;
        }

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
                using (var stringReader = new StringReader(this.dataService.ReportAccessService.GetTemplate((int)ReportKind.TechSpec)))
                    using (var xmlReader = new XmlTextReader(stringReader))
                        xsl.Load(xmlReader);

                // Получаем номера частей отчета.
                var ids = new List<int>();
                foreach (var report in this.Reports)
                    if (report.IsSelected)
                        ids.AddRange(report.Ids);

                // Получаем данные отчета в XML-разметке.
                var xml = new XmlDocument();
                var dataSet = this.dataService.ReportAccessService.GetTechSpec(ids, this.RegionSelectionViewModel.SelectedRegion.Id, this.RegionSelectionViewModel.SelectedCity.Id, this.RegionSelectionViewModel.SelectedSchema, this.RegionSelectionViewModel.SelectedBoiler.Id, this.StartDateTime, this.EndDateTime);
                // Создаем таблицу, где будут храниться настройки отчета.
                var reportTable = dataSet.Tables.Add("Sheets");
                // Добавляем в нее поля, отвечающие за отображение листов.
                foreach (var report in this.Reports)
                    foreach (var id in report.Ids)
                        reportTable.Columns.Add("TS" + id);
                // Добавляем значения.
                foreach (var report in this.Reports)
                    foreach (var id in report.Ids)
                    {
                        var row = reportTable.NewRow();
                        foreach (var r in this.Reports)
                            if (r != report)
                                foreach (var i in r.Ids)
                                    row["TS" + i] = 0;
                            else
                                foreach (var i in r.Ids)
                                    if (i != id)
                                        row["TS" + i] = 0;
                                    else
                                        row["TS" + i] = report.IsSelected ? 1 : 0;

                        reportTable.Rows.Add(row);
                    }

                xml.LoadXml(dataSet.GetXml());
                
                // Получаем название временного XML-файла.
                var temp = Path.GetTempFileName();

                using (var fileStream = new FileStream(temp, FileMode.Create))
                    xsl.Transform(xml, null, fileStream);

                // Получаем название временного шаблона Excel.
                var tempTemplate = Path.GetTempPath() + "Отчет о технических характеристиках - " + this.RegionSelectionViewModel.SelectedRegion.Name + ".xlt";

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