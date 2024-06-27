using Kts.Gis.Models;
using Kts.Importer.Data;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kts.Importer.ViewModels
{
    /// <summary>
    /// Представляет главную модель представления.
    /// </summary>
    internal sealed class MainViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Выбранный тип объектов.
        /// </summary>
        private ObjectType selectedType;

        /// <summary>
        /// Путь к файлу-источнику данных.
        /// </summary>
        private string sourcePath = "";

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие изменения возможности начала импортирования.
        /// </summary>
        public event EventHandler CanStartImportChanged;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MainViewModel"/>.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        public MainViewModel(IDataService dataService)
        {
            this.StartImportCommand = new RelayCommand(this.ExecuteStartImport, this.CanExecuteStartImport);

            this.Types.AddRange(dataService.ObjectTypes.Where(x => x.ObjectKind == ObjectKind.Figure || x.ObjectKind == ObjectKind.NonVisualObject));

            this.SelectedType = this.Types[0];
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает выбранный тип объектов.
        /// </summary>
        public ObjectType SelectedType
        {
            get
            {
                return this.selectedType;
            }
            set
            {
                this.selectedType = value;

                this.NotifyPropertyChanged(nameof(this.SelectedType));
            }
        }

        /// <summary>
        /// Возвращает или задает путь к файлу-источнику данных.
        /// </summary>
        public string SourcePath
        {
            get
            {
                return this.sourcePath;
            }
            set
            {
                this.sourcePath = value;

                this.NotifyPropertyChanged(nameof(this.SourcePath));

                this.StartImportCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Возвращает команду начала импортирования.
        /// </summary>
        public RelayCommand StartImportCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает список типов объектов.
        /// </summary>
        public List<ObjectType> Types
        {
            get;
        } = new List<ObjectType>();

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить начало импортирования.
        /// </summary>
        /// <returns>Значение, указывающее на то, что можно ли выполнить начало импортирования.</returns>
        private bool CanExecuteStartImport()
        {
            return !string.IsNullOrEmpty(this.SourcePath);
        }

        /// <summary>
        /// Выполняет начало импортирования.
        /// </summary>
        private void ExecuteStartImport()
        {
            this.CanStartImportChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}