using System.Drawing;

using MicrosoftExcel = Microsoft.Office.Interop.Excel;

namespace Kts.Excel
{
    /// <summary>
    /// Представляет базовый ряд ячеек рабочего листа.
    /// </summary>
    public abstract class BaseRange
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Рабочий лист.
        /// </summary>
        private readonly MicrosoftExcel.Worksheet worksheet;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="BaseRange"/>.
        /// </summary>
        /// <param name="startColumn">Индекс начального столбца ячейки.</param>
        /// <param name="startRow">Индекс начальной строки ячейки.</param>
        /// <param name="endColumn">Индекс конечного столбца ячейки.</param>
        /// <param name="endRow">Индекс конечной строки ячейки.</param>
        /// <param name="worksheet">Рабочий лист документа Excel.</param>
        public BaseRange(int startColumn, int startRow, int endColumn, int endRow, MicrosoftExcel.Worksheet worksheet)
        {
            this.RawRange = worksheet.Range[worksheet.Cells[startRow, startColumn], worksheet.Cells[endRow, endColumn]];

            this.worksheet = worksheet;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Задает цвет заливки ячейки.
        /// </summary>
        public Color FillColor
        {
            set
            {
                this.RawRange.Interior.Color = value;
            }
        }

        /// <summary>
        /// Задает цвет шрифта значения ячейки.
        /// </summary>
        public Color FontColor
        {
            set
            {
                this.RawRange.Font.Color = value;
            }
        }

        /// <summary>
        /// Задает название шрифта.
        /// </summary>
        public string FontName
        {
            set
            {
                this.RawRange.Font.Name = value;
            }
        }

        /// <summary>
        /// Задает размер шрифта.
        /// </summary>
        public int FontSize
        {
            set
            {
                this.RawRange.Font.Size = value;
            }
        }

        /// <summary>
        /// Задает значение, указывающее на то, что нужно ли выделить значение ячейки жирным шрифтом.
        /// </summary>
        public bool IsBold
        {
            set
            {
                this.RawRange.Font.Bold = value;
            }
        }

        /// <summary>
        /// Задает значение, указывающее на то, что нужно ли нарисовать границы вокруг ячейки.
        /// </summary>
        public bool IsBordered
        {
            set
            {
                if (value)
                    this.RawRange.BorderAround(MicrosoftExcel.XlLineStyle.xlContinuous, MicrosoftExcel.XlBorderWeight.xlMedium, Color: Color.FromArgb(255, 0, 0, 0));
            }
        }

        /// <summary>
        /// Задает значение, указывающее на то, что нужно ли нарисовать границы внутри ячейки.
        /// </summary>
        public bool IsInnerBordered
        {
            set
            {
                if (value)
                {
                    this.RawRange.Borders[MicrosoftExcel.XlBordersIndex.xlEdgeLeft].LineStyle = MicrosoftExcel.XlLineStyle.xlContinuous;
                    this.RawRange.Borders[MicrosoftExcel.XlBordersIndex.xlEdgeTop].LineStyle = MicrosoftExcel.XlLineStyle.xlContinuous;
                    this.RawRange.Borders[MicrosoftExcel.XlBordersIndex.xlEdgeRight].LineStyle = MicrosoftExcel.XlLineStyle.xlContinuous;
                    this.RawRange.Borders[MicrosoftExcel.XlBordersIndex.xlEdgeBottom].LineStyle = MicrosoftExcel.XlLineStyle.xlContinuous;

                    this.RawRange.Borders[MicrosoftExcel.XlBordersIndex.xlEdgeLeft].Weight = MicrosoftExcel.XlBorderWeight.xlMedium;
                    this.RawRange.Borders[MicrosoftExcel.XlBordersIndex.xlEdgeTop].Weight = MicrosoftExcel.XlBorderWeight.xlMedium;
                    this.RawRange.Borders[MicrosoftExcel.XlBordersIndex.xlEdgeRight].Weight = MicrosoftExcel.XlBorderWeight.xlMedium;
                    this.RawRange.Borders[MicrosoftExcel.XlBordersIndex.xlEdgeBottom].Weight = MicrosoftExcel.XlBorderWeight.xlMedium;

                    this.RawRange.Borders.Color = Color.FromArgb(255, 0, 0, 0);
                }
            }
        }

        /// <summary>
        /// Задает ширину столбца ячейки.
        /// </summary>
        public double Width
        {
            set
            {
                this.RawRange.ColumnWidth = value;
            }
        }

        /// <summary>
        /// Задает значение, указывающее на то, что нужно ли переносить значение ячейки по словам.
        /// </summary>
        public bool WordWrap
        {
            set
            {
                this.RawRange.WrapText = value;
            }
        }

        #endregion

        #region Защищенные свойства

        /// <summary>
        /// Возвращает или задает ряд ячеек.
        /// </summary>
        protected MicrosoftExcel.Range RawRange
        {
            get;
            set;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Выравнивает ячейку по центру.
        /// </summary>
        public void AlignToCenter()
        {
            this.RawRange.HorizontalAlignment = MicrosoftExcel.XlHAlign.xlHAlignCenter;
        }

        /// <summary>
        /// Выравнивает ячейку по левой границе.
        /// </summary>
        public void AlignToLeft()
        {
            this.RawRange.HorizontalAlignment = MicrosoftExcel.XlHAlign.xlHAlignLeft;
        }

        /// <summary>
        /// Выравнивает ячейку по правой границе.
        /// </summary>
        public void AlignToRight()
        {
            this.RawRange.HorizontalAlignment = MicrosoftExcel.XlHAlign.xlHAlignRight;
        }

        /// <summary>
        /// Объединяет ячейки.
        /// </summary>
        /// <param name="columnCount">Количество столбцов.</param>
        /// <param name="rowCount">Количество строк.</param>
        public void Merge(int columnCount, int rowCount)
        {
            this.RawRange = this.worksheet.Range[this.worksheet.Cells[this.RawRange.Row, this.RawRange.Column], this.worksheet.Cells[this.RawRange.Row + this.RawRange.Rows.Count + rowCount - 1, this.RawRange.Column + this.RawRange.Columns.Count + columnCount - 1]];

            this.RawRange.Merge();

            this.RawRange.HorizontalAlignment = MicrosoftExcel.XlVAlign.xlVAlignCenter;
            this.RawRange.VerticalAlignment = MicrosoftExcel.XlVAlign.xlVAlignCenter;
        }

        #endregion
    }
}