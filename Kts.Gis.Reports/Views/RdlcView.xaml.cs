using System.Windows;

namespace Kts.Gis.Reports.Views
{
    /// <summary>
    /// Представляет представление просмотра отчета в формате RDLC.
    /// </summary>
    public sealed partial class RdlcView : Window
    {
        #region Конструкторы

#warning Тут были репорты
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RdlcView"/>.
        /// </summary>
        /// <param name="displayName">Отображаемое название отчета.</param>
        /// <param name="reportEmbeddedResource">Название ресурса с отчетом.</param>
        /// <param name="dataSource">Источник данных отчета.</param>
        public RdlcView(string displayName, string reportEmbeddedResource)//, ReportDataSource dataSource)
        {
            this.InitializeComponent();

            //this.reportViewer.LocalReport.DisplayName = displayName;
            //this.reportViewer.LocalReport.ReportEmbeddedResource = reportEmbeddedResource;
            //this.reportViewer.LocalReport.DataSources.Clear();
            //this.reportViewer.LocalReport.DataSources.Add(dataSource);
            //this.reportViewer.RefreshReport();
        }

        #endregion
    }
}