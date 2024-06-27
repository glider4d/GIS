using Kts.Utilities;
using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления ошибок.
    /// </summary>
    [Serializable]
    internal sealed class ErrorsViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Возвращает содержимое.
        /// </summary>
        private string content;

        /// <summary>
        /// Возвращает заголовок.
        /// </summary>
        private string title;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса закрытия представления.
        /// </summary>
        public event EventHandler CloseRequested;

        /// <summary>
        /// Событие запроса открытия объекта.
        /// </summary>
        public event EventHandler ObjectOpenRequested;

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает содержимое.
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

                this.NotifyPropertyChanged(nameof(this.Content));
            }
        }

        /// <summary>
        /// Возвращает ошибки.
        /// </summary>
        public AdvancedObservableCollection<ErrorViewModel> Items
        {
            get;
        } = new AdvancedObservableCollection<ErrorViewModel>();

        /// <summary>
        /// Возвращает или задает выбранную ошибку.
        /// </summary>
        public ErrorViewModel SelectedItem
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает заголовок.
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

                this.NotifyPropertyChanged(nameof(this.Title));
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Закрывает представление.
        /// </summary>
        public void Close()
        {
            this.CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Отображает объект, содержащий выбранную ошибку.
        /// </summary>
        public void ShowSelectedItem()
        {
            this.ObjectOpenRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}