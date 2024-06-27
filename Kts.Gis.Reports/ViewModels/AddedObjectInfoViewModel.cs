using Kts.Excel;
using Kts.Gis.Data;
using Kts.WpfUtilities;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace Kts.Gis.Reports.ViewModels
{
    /// <summary>
    /// Представляет модель представления отчета с информацией о количестве введенных объектов.
    /// </summary>
    public sealed class AddedObjectInfoViewModel
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
        /// Инициализирует новый экземпляр класса <see cref="AddedObjectInfoViewModel"/>.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        public AddedObjectInfoViewModel(IDataService dataService)
        {
            this.dataService = dataService;

            this.GenerateCommand = new RelayCommand(this.ExecuteGenerate);

            // Выбираем первый день текущей недели.
            this.StartDateTime = DateTime.Now.AddDays(DayOfWeek.Monday - DateTime.Now.DayOfWeek);
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
        /// Возвращает или задает начальную дату.
        /// </summary>
        public DateTime StartDateTime
        {
            get;
            set;
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
                using (var stringReader = new StringReader(this.dataService.ReportAccessService.GetTemplate((int)ReportKind.AddedObjectInfo)))
                    using (var xmlReader = new XmlTextReader(stringReader))
                        xsl.Load(xmlReader);
                
#warning Тут вручную выбирается определенная схема, возможно надо ее переделать под все схемы
                // Получаем данные отчета в XML-разметке.
                var xml = new XmlDocument();
                var dataSet = this.dataService.ReportAccessService.GetAddedObjectInfo(this.StartDateTime, this.dataService.Schemas.First(x => x.IsActual));
                xml.LoadXml(dataSet.GetXml());

                // Получаем название временного XML-файла.
                var temp = Path.GetTempFileName();

                using (var fileStream = new FileStream(temp, FileMode.Create))
                    xsl.Transform(xml, null, fileStream);

                // Получаем название временного шаблона Excel.
                var tempTemplate = Path.GetTempPath() + "Отчет с информацией о количестве введенных объектов.xlt";

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