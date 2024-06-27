using Kts.WpfUtilities;
using System;
using System.Collections.Generic;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления редактирования заголовка утверждения/согласования.
    /// </summary>
    internal sealed class ApprovedHeaderEditViewModel
    {
        #region Закрытые константы

        /// <summary>
        /// Максимальный размер шрифта.
        /// </summary>
        private const int maxSize = 100;

        /// <summary>
        /// Минимальный размер шрифта.
        /// </summary>
        private const int minSize = 5;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Редактируемая модель представления.
        /// </summary>
        private readonly ApprovedHeaderViewModel viewModel;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса закрытия представления.
        /// </summary>
        public event EventHandler CloseRequested;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ApprovedHeaderEditViewModel"/>.
        /// </summary>
        /// <param name="viewModel">Редактируемая модель представления.</param>
        public ApprovedHeaderEditViewModel(ApprovedHeaderViewModel viewModel)
        {
            this.viewModel = viewModel;

            for (int i = minSize; i <= maxSize; i++)
                this.Sizes.Add(i);

            this.post = viewModel.Post;
            this.name = viewModel.Name;
            this.year = viewModel.Year;
            this.SelectedSize = viewModel.FontSize;

            this.SaveCommand = new RelayCommand(this.ExecuteSave, this.CanExecuteSave);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// ФИО.
        /// </summary>
        private string name = "";

        /// <summary>
        /// Возвращает или задает ФИО.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;

                this.SaveCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Должность.
        /// </summary>
        private string post = "";

        /// <summary>
        /// Возвращает или задает должность.
        /// </summary>
        public string Post
        {
            get
            {
                return this.post;
            }
            set
            {
                this.post = value;

                this.SaveCommand.RaiseCanExecuteChanged();
            }
        }

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
        /// Год.
        /// </summary>
        private int year;

        /// <summary>
        /// Возвращает или задает год.
        /// </summary>
        public int Year
        {
            get
            {
                return this.year;
            }
            set
            {
                this.year = value;

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
            return this.Post.Length > 0 && this.Name.Length > 0 && this.Year > 0;
        }

        /// <summary>
        /// Выполняет сохранение.
        /// </summary>
        private void ExecuteSave()
        {
            this.viewModel.Post = this.Post;
            this.viewModel.Name = this.Name;
            this.viewModel.Year = this.Year;
            this.viewModel.FontSize = this.SelectedSize;

            this.CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}