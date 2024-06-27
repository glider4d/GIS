using Kts.Excel;
using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.Reports.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace Kts.Gis.Reports
{
    /// <summary>
    /// Представляет сервис отчетов.
    /// </summary>
    public sealed class ReportService
    {
        #region Открытые события

        /// <summary>
        /// Событие запроса отображения представления формирования отчета с информацией о количестве введенных объектов.
        /// </summary>
        public event EventHandler AddedObjectInfoViewShowRequested;

        /// <summary>
        /// Событие запроса отображения представления формирования отчета о жилищном фонде и договорных подключениях.
        /// </summary>
        public event EventHandler KtsViewShowRequested;

        /// <summary>
        /// Событие запроса отображения представления формирования отчета о технических характеристиках.
        /// </summary>
        public event EventHandler TechSpecViewShowRequested;

        #endregion

        #region Открытые методы

        /// <summary>
        /// Генерирует отчет о проценте сопоставления.
        /// </summary>
        /// <param name="all">По всем объектам.</param>
        /// <param name="dataService">Сервис данных.</param>
        public void GenerateDiffObjects(bool all, IDataService dataService)
        {
            // Загружаем шаблон.
            StringReader stringReader = null;
            var xsl = new XslCompiledTransform();
            try
            {
                stringReader = new StringReader(dataService.ReportAccessService.GetTemplate((int)ReportKind.DiffObjects));

                using (var xmlReader = new XmlTextReader(stringReader))
                {
                    stringReader = null;

                    xsl.Load(xmlReader);
                }
            }
            catch
            {
                if (stringReader != null)
                    stringReader.Dispose();
            }

            // Получаем данные отчета в XML-разметке.
            var xml = new XmlDocument();
            var dataSet = dataService.ReportAccessService.GetDiffObjects(all);
            xml.LoadXml(dataSet.GetXml());

            // Получаем название временного XML-файла.
            var temp = Path.GetTempFileName();

            using (var fileStream = new FileStream(temp, FileMode.Create))
                xsl.Transform(xml, null, fileStream);

            // Получаем название временного шаблона Excel.
            var tempTemplate = Path.GetTempPath() + "Отчет о несопоставленных объектах.xlt";

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
        }

        /// <summary>
        /// Генерирует отчет о гидравлике.
        /// </summary>
        /// <param name="boilerId">Идентификатор котельной.</param>
        /// <param name="schema">Схема.</param>
        /// <param name="dataService">Сервис данных.</param>
        public void GenerateHydraulics(Guid boilerId, SchemaModel schema, IDataService dataService)
        {
            // Загружаем шаблон.
            StringReader stringReader = null;
            var xsl = new XslCompiledTransform();
            try
            {
                stringReader = new StringReader(dataService.ReportAccessService.GetTemplate((int)ReportKind.Hydraulics));
                
                using (var xmlReader = new XmlTextReader(stringReader))
                {
                    stringReader = null;

                    xsl.Load(xmlReader);
                }
            }
            catch
            {
                if (stringReader != null)
                    stringReader.Dispose();
            }

            // Получаем данные отчета в XML-разметке.
            var xml = new XmlDocument();
            var dataSet = dataService.ReportAccessService.GetHydraulics(boilerId, schema);
            xml.LoadXml(dataSet.GetXml());

            // Получаем название временного XML-файла.
            var temp = Path.GetTempFileName();

            using (var fileStream = new FileStream(temp, FileMode.Create))
                xsl.Transform(xml, null, fileStream);

            // Получаем название временного шаблона Excel.
            var tempTemplate = Path.GetTempPath() + "Отчет о гидравлике.xlt";

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
        }

        /// <summary>
        /// Генерирует отчет о проценте сопоставления.
        /// </summary>
        /// <param name="all">По всем объектам.</param>
        /// <param name="dataService">Сервис данных.</param>
        public void GenerateIntegrationStats(bool all, IDataService dataService)
        {
            // Загружаем шаблон.
            StringReader stringReader = null;
            var xsl = new XslCompiledTransform();
            try
            {
                stringReader = new StringReader(dataService.ReportAccessService.GetTemplate((int)ReportKind.IntegrationStats));

                using (var xmlReader = new XmlTextReader(stringReader))
                {
                    stringReader = null;

                    xsl.Load(xmlReader);
                }
            }
            catch
            {
                if (stringReader != null)
                    stringReader.Dispose();
            }

            // Получаем данные отчета в XML-разметке.
            var xml = new XmlDocument();
            var dataSet = dataService.ReportAccessService.GetIntegrationStats(all);
            xml.LoadXml(dataSet.GetXml());

            // Получаем название временного XML-файла.
            var temp = Path.GetTempFileName();

            using (var fileStream = new FileStream(temp, FileMode.Create))
                xsl.Transform(xml, null, fileStream);

            // Получаем название временного шаблона Excel.
            var tempTemplate = Path.GetTempPath() + "Отчет о сопоставлении.xlt";

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
        }

        /// <summary>
        /// Возвращает дерево отчетов.
        /// </summary>
        /// <returns>Дерево отчетов.</returns>
        public List<ITreeItemViewModel> GetReportTree()
        {
            var result = new List<ITreeItemViewModel>();

            result.Add(new ReportViewModel("О технических характеристиках...", ReportKind.TechSpec, this));
            result.Add(new ReportViewModel("О количестве введенных объектов...", ReportKind.AddedObjectInfo, this));
            result.Add(new ReportViewModel("О жилищном фонде и договорных подключениях...", ReportKind.Kts, this));
            //result.Add(new ReportViewModel("О измерительных приборах...", ReportKind.MetersStats, this));

            return result;
        }

        /// <summary>
        /// Запрашивает отображение представления формирования отчета с информацией о количестве введенных объектов.
        /// </summary>
        public void RequestAddedObjectInfoViewShow()
        {
            this.AddedObjectInfoViewShowRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Запрашивает отображение представления составления отчета о жилищном фонде и договорных подключениях.
        /// </summary>
        public void RequestKtsViewShow()
        {
            this.KtsViewShowRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Запрашивает отображение представления формирования технических характеристик.
        /// </summary>
        public void RequestTechSpecViewShow()
        {
            this.TechSpecViewShowRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}