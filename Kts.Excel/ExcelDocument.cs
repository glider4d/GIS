using MicrosoftExcel = Microsoft.Office.Interop.Excel;

namespace Kts.Excel
{
    /// <summary>
    /// Представляет документ Excel.
    /// </summary>
    public sealed class ExcelDocument
    {
        #region Закрытые поля

        /// <summary>
        /// Рабочая книга.
        /// </summary>
        private MicrosoftExcel.Workbook workbook;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Приложение.
        /// </summary>
        private readonly MicrosoftExcel.Application application = new MicrosoftExcel.Application();

        /// <summary>
        /// Значение, указывающее на то, что открыт ли документ только для чтения.
        /// </summary>
        private readonly bool isReadOnly;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ExcelDocument"/>.
        /// </summary>
        /// <param name="isReadOnly">Значение, указывающее на то, что создается ли документ только для чтения.</param>
        /// <param name="isVisible">Значение, указывающее на то, что видим ли документ.</param>
        public ExcelDocument(bool isReadOnly, bool isVisible)
        {
            this.isReadOnly = isReadOnly;
            this.IsVisible = isVisible;

            this.workbook = this.application.Workbooks.Add();
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Задает значение, указывающее на то, что нужно ли отображать уведомления.
        /// </summary>
        public bool DisplayAlerts
        {
            set
            {
                this.application.DisplayAlerts = value;
            }
        }

        /// <summary>
        /// Задает видимость документа Excel.
        /// </summary>
        public bool IsVisible
        {
            set
            {
                this.application.Visible = value;
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Добавляет новый рабочий лист.
        /// </summary>
        /// <param name="name">Название рабочей книги.</param>
        /// <returns>Рабочий лист документа Excel.</returns>
        public Worksheet AddWorksheet(string name)
        {
            this.application.Worksheets.Add(After: this.application.Worksheets[this.application.Worksheets.Count]);
            var tt = this.workbook.Worksheets[0];
            var worksheet = new Worksheet(this.workbook.Worksheets[this.workbook.Worksheets.Count]);

            worksheet.Name = name;

            return worksheet;
        }

        /// <summary>
        /// Закрывает документ и высвобождает ресурсы.
        /// </summary>
        public void Close()
        {
            for (int i = 1; i <= this.application.Workbooks.Count; i++)
                this.application.Workbooks[i].Close(false);

            this.application.Quit();
        }

        /// <summary>
        /// Удаляет рабочие книги, оставляя самую последнюю.
        /// </summary>
        public void DeleteWorksheets()
        {
            for (int i = 1; i < this.workbook.Worksheets.Count; i++)
            {
                this.workbook.Worksheets[i].Delete();

                i--;
            }
        }
        
        /// <summary>
        /// Возвращает рабочий лист документа Excel.
        /// </summary>
        /// <param name="index">Индекс рабочего листа.</param>
        /// <returns>Рабочий лист документа Excel.</returns>
        public Worksheet GetWorksheet(int index)
        {
            return new Worksheet(this.workbook.Worksheets[index]);
        }

        /// <summary>
        /// Открывает заданную рабочую книгу Excel.
        /// </summary>
        /// <param name="fileName">Путь к рабочей книге Excel.</param>
        public void Open(string fileName)
        {
            for (int i = 1; i <= this.application.Workbooks.Count; i++)
                this.application.Workbooks[i].Close(false);

            this.workbook = this.application.Workbooks.Open(fileName, ReadOnly: this.isReadOnly);
        }

        /// <summary>
        /// Открывает заданную рабочую книгу Excel в формате XML.
        /// </summary>
        /// <param name="fileName">Путь к рабочей книге Excel в формате XML.</param>
        public void OpenXml(string fileName)
        {
            for (int i = 1; i <= this.application.Workbooks.Count; i++)
                this.application.Workbooks[i].Close(false);

            this.workbook = this.application.Workbooks.OpenXML(fileName);
        }

        /// <summary>
        /// Сохраняет документ как XLT-документ.
        /// </summary>
        /// <param name="newFileName">Новый путь к рабочей книге Excel.</param>
        public void SaveAsXlt(string newFileName)
        {
            this.workbook.SaveAs(newFileName, MicrosoftExcel.XlFileFormat.xlTemplate);
        }

        #endregion

        #region Открытые статические методы

        /// <summary>
        /// Открывает документ Excel.
        /// </summary>
        /// <param name="fileName">Путь к файлу.</param>
        /// <param name="isReadOnly">Значение, указывающее на то, что открывается ли документ только для чтения.</param>
        /// <param name="isVisible">Значение, указывающее на то, что видим ли документ.</param>
        /// <returns>Документ Excel.</returns>
        public static ExcelDocument Open(string fileName, bool isReadOnly, bool isVisible)
        {
            var doc = new ExcelDocument(isReadOnly, isVisible);

            doc.Open(fileName);

            return doc;
        }

        /// <summary>
        /// Открывает документ Excel в формате XML.
        /// </summary>
        /// <param name="fileName">Путь к файлу.</param>
        /// <param name="isReadOnly">Значение, указывающее на то, что открывается ли документ только для чтения.</param>
        /// <param name="isVisible">Значение, указывающее на то, что видим ли документ.</param>
        /// <param name="displayAlerts">Значение, указывающее на то, что нужно ли отображать уведомления.</param>
        /// <returns>Документ Excel.</returns>
        public static ExcelDocument OpenXml(string fileName, bool isReadOnly, bool isVisible, bool displayAlerts)
        {
            var doc = new ExcelDocument(isReadOnly, isVisible);

            doc.DisplayAlerts = displayAlerts;

            doc.OpenXml(fileName);

            return doc;
        }

        #endregion
    }
}