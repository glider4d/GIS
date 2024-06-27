using System;

using MicrosoftExcel = Microsoft.Office.Interop.Excel;

namespace Kts.Excel
{
    /// <summary>
    /// Представляет ячейку рабочего листа.
    /// </summary>
    public sealed class Cell : BaseRange
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Cell"/>.
        /// </summary>
        /// <param name="column">Индекс столбца ячейки.</param>
        /// <param name="row">Индекс строки ячейки.</param>
        /// <param name="worksheet">Рабочий лист документа Excel.</param>
        public Cell(int column, int row, MicrosoftExcel.Worksheet worksheet) : base(column, row, column, row, worksheet)
        {
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает столбец ячейки.
        /// </summary>
        public int Column
        {
            get
            {
                return this.RawRange.Column;
            }
        }

        /// <summary>
        /// Возвращает строку ячейки.
        /// </summary>
        public int Row
        {
            get
            {
                return this.RawRange.Row;
            }
        }

        /// <summary>
        /// Возвращает или задает значение ячейки.
        /// </summary>
        public string Value
        {
            get
            {
                return Convert.ToString(this.RawRange.Value2);
            }
            set
            {
                this.RawRange.Value2 = value;
            }
        }

        #endregion
    }
}