using Kts.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Printing;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления выбора принтера.
    /// </summary>
    internal sealed class PrinterSelectionViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Размеры страниц.
        /// </summary>
        private IEnumerable<PageMediaSize> pageSizes;

        /// <summary>
        /// Выбранный размер страницы.
        /// </summary>
        private PageMediaSize selectedPageSize;
        
        /// <summary>
        /// Выбранный принтер.
        /// </summary>
        private PrintQueue selectedPrinter;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PrinterSelectionViewModel"/>.
        /// </summary>
        public PrinterSelectionViewModel()
        {
            var printers = new LocalPrintServer().GetPrintQueues(new[]
            {
                EnumeratedPrintQueueTypes.Connections,
                EnumeratedPrintQueueTypes.Local
            });

            var notOrdered = new List<PrintQueue>();

            foreach (var printer in printers)
                notOrdered.Add(printer);

            this.Printers = notOrdered.OrderBy(x => x.FullName);

            var defaultPrinter = LocalPrintServer.GetDefaultPrintQueue();

            if (this.Printers.Any(x => x.FullName == defaultPrinter.FullName))
                this.SelectedPrinter = this.Printers.First(x => x.FullName == defaultPrinter.FullName);
            else
                this.SelectedPrinter = this.Printers.First();
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает размеры страниц.
        /// </summary>
        public IEnumerable<PageMediaSize> PageSizes
        {
            get
            {
                return this.pageSizes;
            }
            private set
            {
                this.pageSizes = value;

                this.NotifyPropertyChanged(nameof(this.PageSizes));
            }
        }

        /// <summary>
        /// Возвращает список принтеров.
        /// </summary>
        public IEnumerable<PrintQueue> Printers
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает выбранный размер страницы.
        /// </summary>
        public PageMediaSize SelectedPageSize
        {
            get
            {
                return this.selectedPageSize;
            }
            set
            {
                this.selectedPageSize = value;

                this.NotifyPropertyChanged(nameof(this.SelectedPageSize));

                this.SelectedPrinter.UserPrintTicket.PageMediaSize = value;

                this.SelectedPrinter.Commit();
            }
        }

        /// <summary>
        /// Возвращает или задает выбранный принтер.
        /// </summary>
        public PrintQueue SelectedPrinter
        {
            get
            {
                return this.selectedPrinter;
            }
            set
            {
                this.selectedPrinter = value;

                this.PageSizes = value.GetPrintCapabilities(value.UserPrintTicket).PageMediaSizeCapability.Where(x => x.PageMediaSizeName == PageMediaSizeName.ISOA2 || x.PageMediaSizeName == PageMediaSizeName.ISOA3 || x.PageMediaSizeName == PageMediaSizeName.ISOA4).OrderBy(x => x.PageMediaSizeName);

                this.SelectedPageSize = this.PageSizes.First();
            }
        }

        #endregion
    }
}