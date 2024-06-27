using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет многостраничный документ из карты.
    /// </summary>
    internal sealed partial class MapPaginator : DocumentPaginator
    {
        #region Закрытые поля

        /// <summary>
        /// Словарь соответствия номеров страниц с их строками и столбцами.
        /// </summary>
        private Dictionary<int, Tuple<int, int>> pages = new Dictionary<int, Tuple<int, int>>();

        /// <summary>
        /// Предыдущая страница.
        /// </summary>
        private DocumentPage prevPage;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Контейнер.
        /// </summary>
        private readonly Grid container;

        /// <summary>
        /// Значение, указывающее на то, что нужно ли игнорировать отступы.
        /// </summary>
        private readonly bool ignoreMargins;

        /// <summary>
        /// Карта.
        /// </summary>
        private readonly Map map;

        /// <summary>
        /// Область печати.
        /// </summary>
        private readonly PrintArea printArea;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MapPaginator"/>.
        /// </summary>
        /// <param name="map">Карта.</param>
        /// <param name="container">Контейнер.</param>
        /// <param name="printArea">Область печати.</param>
        /// <param name="ignoreMargins">Значение, указывающее на то, что нужно ли игнорировать отступы.</param>
        public MapPaginator(Map map, Grid container, PrintArea printArea, bool ignoreMargins = false)
        {
            this.map = map;
            this.container = container;
            this.printArea = printArea;
            this.ignoreMargins = ignoreMargins;

            this.PageSize = printArea.PageSize;

            // Заполняем словарь соответствия номеров страниц с их строками и столбцами.
            var column = 0;
            var row = 0;
            var index = 0;
            for (int i = 0; i < this.printArea.TotalPageCount; i++)
            {
                if (!this.printArea.IsPageIgnored(row, column))
                {
                    pages.Add(index, new Tuple<int, int>(row, column));

                    index++;
                }

                column++;

                if (column / this.ColumnCount == 1)
                {
                    column = 0;

                    row++;
                }
            }
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает количество столбцов.
        /// </summary>
        public int ColumnCount
        {
            get
            {
                return (int)Math.Round(this.printArea.Width / this.printArea.AreaPageSize.Width);
            }
        }
        
        /// <summary>
        /// Возвращает количество строк.
        /// </summary>
        public int RowCount
        {
            get
            {
                return (int)Math.Round(this.printArea.Height / this.printArea.AreaPageSize.Height);
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Освобождает ресурсы последней страницы.
        /// </summary>
        public void ReleaseLastPage()
        {
            if (this.prevPage != null)
            {
                this.prevPage.Dispose();

                GC.SuppressFinalize(this);
            }
        }

        #endregion
    }

    // Реализация DocumentPaginator.
    internal sealed partial class MapPaginator
    {
        #region Открытые переопределенные свойства

        /// <summary>
        /// Возвращает значение, указывающее на то, что возвращает ли <see cref="PageCount"/> правильное значение.
        /// </summary>
        public override bool IsPageCountValid
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Возвращает количество страниц.
        /// </summary>
        public override int PageCount
        {
            get
            {
                return this.printArea.PageCount;
            }
        }

        /// <summary>
        /// Возвращает или задает размер страницы.
        /// </summary>
        public override Size PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает объект, который подвергается делению на страницы.
        /// </summary>
        public override IDocumentPaginatorSource Source
        {
            get
            {
                return null;
            }
        }

        #endregion

        #region Открытые переопределенные методы

        /// <summary>
        /// Возвращает страницу документа по ее номеру.
        /// </summary>
        /// <param name="pageNumber">Номер страницы.</param>
        /// <returns>Страница документа.</returns>
        public override DocumentPage GetPage(int pageNumber)
        {
            // Если имеется предыдущая страница, то необходимо ее очистить.
            this.ReleaseLastPage();
            
            // Не знаю как, но это работает.
            var scale = this.map.Scale * (this.printArea.PageSize.Width / (this.printArea.AreaPageSize.Width * this.map.Scale));

            // Применяем трансформации масштабирования, поворота (если это необходимо) и сдвига.
            var marginX = this.ignoreMargins ? 0 : this.printArea.PageMargin.Left;
            var marginY = this.ignoreMargins ? 0 : this.printArea.PageMargin.Top;
            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(new ScaleTransform(scale, scale));
            if (this.printArea.CustomAngle != 0)
            {
                var centerX = this.printArea.Position.X * scale + this.printArea.ActualWidth * scale / 2;
                var centerY = this.printArea.Position.Y * scale + this.printArea.ActualHeight * scale / 2;

                transformGroup.Children.Add(new RotateTransform(-this.printArea.CustomAngle, centerX, centerY));
            }
            transformGroup.Children.Add(new TranslateTransform(-this.printArea.AreaPageSize.Width * scale * this.pages[pageNumber].Item2 - this.printArea.Position.X * scale + marginX, -this.printArea.AreaPageSize.Height * scale * this.pages[pageNumber].Item1 - this.printArea.Position.Y * scale + marginY));
            this.container.RenderTransform = transformGroup;

            var size = new Size(this.container.ActualWidth, this.container.ActualHeight);

            this.container.Measure(size);
            this.container.Arrange(new Rect(new Point(0, 0), size));

            var page = new DocumentPage(this.container, this.PageSize, new Rect(this.PageSize), new Rect(this.PageSize));

            this.container.RenderTransform = null;

            this.prevPage = page;

            return page;
        }

        #endregion
    }
}