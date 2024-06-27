using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Kts.Utilities
{
    /// <summary>
    /// Представляет улучшенную версию <see cref="ObservableCollection{T}"/>, которая позволяет добавлять ряд записей.
    /// </summary>
    /// <typeparam name="T">Тип записи.</typeparam>
    /// <remarks>
    /// Стандартный класс <see cref="ObservableCollection{T}"/> не годится для добавления большого числа записей, так как каждый раз он уведомляет об изменении коллекции, что значительно сказывается на производительности.
    /// </remarks>
    [Serializable]
    public sealed class AdvancedObservableCollection<T> : ObservableCollection<T>
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AdvancedObservableCollection{T}"/>.
        /// </summary>
        public AdvancedObservableCollection() : base()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AdvancedObservableCollection{T}"/>.
        /// </summary>
        /// <param name="collection">Коллекция.</param>
        public AdvancedObservableCollection(IEnumerable<T> collection) : base(collection)
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AdvancedObservableCollection{T}"/>.
        /// </summary>
        /// <param name="list">Список.</param>
        public AdvancedObservableCollection(List<T> list) : base(list)
        {
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Добавляет ряд записей в коллекцию.
        /// </summary>
        /// <param name="range">Ряд записей.</param>
        public void AddRange(IEnumerable<T> range)
        {
            foreach (var item in range)
                this.Items.Add(item);

            // Уведомляем об изменении коллекции.
            this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(this.Count)));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        #endregion
    }
}