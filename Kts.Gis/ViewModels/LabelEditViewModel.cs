using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления редактирования надписи.
    /// </summary>
    internal sealed class LabelEditViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Содержимое надписи.
        /// </summary>
        private string content;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Надпись.
        /// </summary>
        private readonly LabelViewModel label;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса закрытия представления.
        /// </summary>
        public event EventHandler CloseRequested;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LabelEditViewModel"/>.
        /// </summary>
        /// <param name="label">Надпись.</param>
        /// <param name="sizes">Допустимые размеры надписи.</param>
        public LabelEditViewModel(LabelViewModel label, List<int> sizes)
        {
            this.label = label;

            this.SaveCommand = new RelayCommand(this.ExecuteSave, this.CanExecuteSave);

            this.Content = label.Content;
            this.Sizes = sizes;
            this.SelectedSize = label.Size;
            this.IsBold = label.IsBold;
            this.IsItalic = label.IsItalic;
            this.IsUnderline = label.IsUnderline;
            this.AlignHorizontal = label.Angle == 0;
            this.Angle = label.Angle;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Значение, указывающее на то, что должна ли быть надпись выровнена по горизонтали.
        /// </summary>
        private bool alignHorizontal;

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что должна ли быть надпись выровнена по горизонтали.
        /// </summary>
        public bool AlignHorizontal
        {
            get
            {
                return this.alignHorizontal;
            }
            set
            {
                this.alignHorizontal = value;

                this.CanSetAngle = !value;
            }
        }

        /// <summary>
        /// Возвращает или задает угол поворота надписи.
        /// </summary>
        public double Angle
        {
            get;
            set;
        }

        /// <summary>
        /// Значение, указывающее на то, что можно ли задать угол поворота надписи.
        /// </summary>
        private bool canSetAngle;

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что можно ли задать угол поворота надписи.
        /// </summary>
        public bool CanSetAngle
        {
            get
            {
                return this.canSetAngle;
            }
            set
            {
                this.canSetAngle = value;

                this.NotifyPropertyChanged(nameof(this.CanSetAngle));
            }
        }

        /// <summary>
        /// Возвращает или задает содержимое надписи.
        /// </summary>
        public string Content
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = value;

                this.SaveCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли шрифт надписи полужирным.
        /// </summary>
        public bool IsBold
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли шрифт надписи курсивным.
        /// </summary>
        public bool IsItalic
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли шрифт надписи подчеркнутым.
        /// </summary>
        public bool IsUnderline
        {
            get;
            set;
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
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить сохранение изменений.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteSave()
        {
            return this.Content.Length > 0;
        }

        /// <summary>
        /// Выполняет сохранение изменений.
        /// </summary>
        private void ExecuteSave()
        {
            this.label.ChangeStyle(this.Content, this.SelectedSize, this.IsBold, this.IsItalic, this.IsUnderline, this.AlignHorizontal, this.Angle);

            this.CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}