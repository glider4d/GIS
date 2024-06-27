using Kts.Gis.Models;
using Kts.Importer.Data;
using Kts.Importer.ViewModels;
using Kts.Messaging;
using System.Windows;

namespace Kts.Importer.Views
{
    /// <summary>
    /// Представляет представление импортирования.
    /// </summary>
    internal sealed partial class ImportationView : Window
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ImportationView"/>.
        /// </summary>
        /// <param name="sourcePath">Путь к файлу-источнику данных.</param>
        /// <param name="type">Тип импортируемых объектов.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public ImportationView(string sourcePath, ObjectType type, IDataService dataService, IMessageService messageService)
        {
            this.InitializeComponent();

            this.DataContext = new ImportationViewModel(sourcePath, type, dataService, messageService);
        }

        #endregion
    }
}