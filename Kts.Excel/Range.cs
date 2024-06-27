using MicrosoftExcel = Microsoft.Office.Interop.Excel;

namespace Kts.Excel
{
    /// <summary>
    /// Представляет ряд ячеек рабочего листа.
    /// </summary>
    public sealed class Range : BaseRange
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Range"/>.
        /// </summary>
        /// <param name="startColumn">Индекс начального столбца ячейки.</param>
        /// <param name="startRow">Индекс начальной строки ячейки.</param>
        /// <param name="endColumn">Индекс конечного столбца ячейки.</param>
        /// <param name="endRow">Индекс конечной строки ячейки.</param>
        /// <param name="worksheet">Рабочий лист документа Excel.</param>
        public Range(int startColumn, int startRow, int endColumn, int endRow, MicrosoftExcel.Worksheet worksheet) : base(startColumn, startRow, endColumn, endRow, worksheet)
        {
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает конечный столбец ряда ячеек.
        /// </summary>
        public int EndColumn
        {
            get
            {
                return this.RawRange.Columns.Count;
            }
        }

        /// <summary>
        /// Возвращает конечную строку ряда ячеек.
        /// </summary>
        public int EndRow
        {
            get
            {
                return this.RawRange.Rows.Count;
            }
        }

        /// <summary>
        /// Возвращает начальный столбец ряда ячеек.
        /// </summary>
        public int StartColumn
        {
            get
            {
                return this.RawRange.Column;
            }
        }

        /// <summary>
        /// Возвращает начальную строку ряда ячеек.
        /// </summary>
        public int StartRow
        {
            get
            {
                return this.RawRange.Row;
            }
        }

        /// <summary>
        /// Возвращает или задает значения ряда ячеек.
        /// </summary>
        public object[,] Values
        {
            get
            {
                return (object[,])this.RawRange.Value2;
            }
            set
            {
                this.RawRange.Value2 = value;
            }
        }

        #endregion
    }
}