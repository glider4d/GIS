using Kts.Utilities;
using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления ошибок сохраненных значений параметров объектов.
    /// </summary>
    internal sealed class SavedErrorsViewModel
    {
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
        /// Возвращает объекты, содержащие ошибки.
        /// </summary>
        public AdvancedObservableCollection<IObjectViewModel> Items
        {
            get;
        } = new AdvancedObservableCollection<IObjectViewModel>();

        /// <summary>
        /// Возвращает или задает выбранный объект.
        /// </summary>
        public IObjectViewModel SelectedItem
        {
            get;
            set;
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
        /// Отображает объект, содержащий ошибку.
        /// </summary>
        public void ShowSelectedItem()
        {
            this.ObjectOpenRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}