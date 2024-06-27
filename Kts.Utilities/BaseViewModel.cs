using System.ComponentModel;
using System;

namespace Kts.Utilities
{
    /// <summary>
    /// Представляет базовую модель представления.
    /// </summary>
    [Serializable]
    public abstract partial class BaseViewModel : INotifyPropertyChanged
    {
        #region Защищенные методы

        /// <summary>
        /// Уведомляет об изменении свойства модели представления.
        /// </summary>
        /// <param name="propertyName">Имя измененного свойства.</param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    // Реализация INotifyPropertyChanged.
    public abstract partial class BaseViewModel
    {
        #region Открытые события

        /// <summary>
        /// Событие изменения свойства модели представления.
        /// </summary>
        
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}