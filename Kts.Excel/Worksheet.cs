using MicrosoftExcel = Microsoft.Office.Interop.Excel;

namespace Kts.Excel
{
    /// <summary>
    /// Представляет рабочий лист документа Excel.
    /// </summary>
    public sealed class Worksheet
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Рабочий лист.
        /// </summary>
        private readonly MicrosoftExcel.Worksheet worksheet;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Worksheet"/>.
        /// </summary>
        /// <param name="worksheet">Рабочий лист документа Excel.</param>
        public Worksheet(MicrosoftExcel.Worksheet worksheet)
        {
            this.worksheet = worksheet;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Задает название рабочего листа.
        /// </summary>
        public string Name
        {
            set
            {
                this.worksheet.Name = value;
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Возвращает ячейку рабочего листа.
        /// </summary>
        /// <param name="column">Индекс столбца ячейки.</param>
        /// <param name="row">Индекс строки ячейки.</param>
        /// <returns>Ячейка рабочего листа.</returns>
        public Cell GetCell(int column, int row)
        {
            return new Cell(column, row, this.worksheet);
        }

        /// <summary>
        /// Возвращает ряд ячеек рабочего листа.
        /// </summary>
        /// <param name="startColumn">Индекс начального столбца ячейки.</param>
        /// <param name="startRow">Индекс начальной строки ячейки.</param>
        /// <param name="endColumn">Индекс конечного столбца ячейки.</param>
        /// <param name="endRow">Индекс конечной строки ячейки.</param>
        /// <returns>Ряд ячеек рабочего листа.</returns>
        public Range GetRange(int startColumn, int startRow, int endColumn, int endRow)
        {
            return new Range(startColumn, startRow, endColumn, endRow, this.worksheet);
        }

        #endregion
    }
}