using Kts.WpfUtilities;
using System;
using System.Collections.Generic;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления редактирования таблицы.
    /// </summary>
    internal sealed class TableEditViewModel
    {
        #region Закрытые константы

        /// <summary>
        /// Максимальный размер шрифта.
        /// </summary>
        private const int maxSize = 50;

        /// <summary>
        /// Минимальный размер шрифта.
        /// </summary>
        private const int minSize = 5;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Таблица.
        /// </summary>
        private readonly LengthPerDiameterTableViewModel table;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса закрытия представления.
        /// </summary>
        public event EventHandler CloseRequested;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TableEditViewModel"/>.
        /// </summary>
        /// <param name="table">Таблица.</param>
        public TableEditViewModel(LengthPerDiameterTableViewModel table)
        {
            this.table = table;

            for (int i = minSize; i <= maxSize; i++)
                this.Sizes.Add(i);

            this.title = table.Title;
            this.SelectedSize = table.FontSize;

            this.SaveCommand = new RelayCommand(this.ExecuteSave, this.CanExecuteSave);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает команду сохранения.
        /// </summary>
        public RelayCommand SaveCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает выбранный размер.
        /// </summary>
        public int SelectedSize
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает размеры.
        /// </summary>
        public List<int> Sizes
        {
            get;
        } = new List<int>();

        /// <summary>
        /// Заголовок таблицы.
        /// </summary>
        private string title = "";

        /// <summary>
        /// Возвращает или задает заголовок таблицы.
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;

                this.SaveCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить команду сохранения.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteSave()
        {
            return this.Title.Length > 0;
        }

        /// <summary>
        /// Выполняет сохранение.
        /// </summary>
        private void ExecuteSave()
        {
            this.table.Title = this.Title;
            this.table.FontSize = this.SelectedSize;

            this.CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}