using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Kts.WpfUtilities
{
    /// <summary>
    /// Представляет модель представления отладки.
    /// </summary>
    public sealed class DebugViewModel
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Максимальное количество отображаемых элементов.
        /// </summary>
        private readonly int maxItemCount;

        /// <summary>
        /// Наблюдения.
        /// </summary>
        private readonly List<Tuple<INotifyPropertyChanged, string>> watches = new List<Tuple<INotifyPropertyChanged, string>>();

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие добавления элемента.
        /// </summary>
        public event EventHandler ItemAdded;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DebugViewModel"/>.
        /// </summary>
        /// <param name="maxItemCount">Максимальное количество отображаемых элементов.</param>
        public DebugViewModel(int maxItemCount)
        {
            this.maxItemCount = maxItemCount;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает элементы.
        /// </summary>
        public ObservableCollection<DebuggingVariableViewModel> Items
        {
            get;
        } = new ObservableCollection<DebuggingVariableViewModel>();

        #endregion

        #region Обработчики событий
        
        /// <summary>
        /// Обрабатывает событие <see cref="INotifyPropertyChanged.PropertyChanged"/> объекта наблюдения.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Obj_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!this.watches.Any(x => x.Item1 == (INotifyPropertyChanged)sender && x.Item2 == e.PropertyName))
                return;

            if (this.Items.Count == this.maxItemCount)
                this.Items.RemoveAt(0);

            this.Items.Add(new DebuggingVariableViewModel(e.PropertyName, DateTime.Now, Convert.ToString(sender.GetType().GetProperty(e.PropertyName).GetValue(sender, null))));

            this.ItemAdded?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Добавляет наблюдение.
        /// </summary>
        /// <param name="obj">Объект, за свойством которого следует наблюдать.</param>
        /// <param name="propertyName">Название свойства, за которым следует наблюдать.</param>
        public void AddWatch(INotifyPropertyChanged obj, string propertyName)
        {
            if (this.watches.Any(x => x.Item1 == obj && x.Item2 == propertyName))
                return;

            if (!this.watches.Any(x => x.Item1 == obj))
                obj.PropertyChanged += this.Obj_PropertyChanged;

            this.watches.Add(new Tuple<INotifyPropertyChanged, string>(obj, propertyName));
        }

        /// <summary>
        /// Очищает отображаемые элементы.
        /// </summary>
        public void ClearItems()
        {
            this.Items.Clear();
        }

        /// <summary>
        /// Убирает наблюдения.
        /// </summary>
        public void ClearWatches()
        {
            foreach (var watch in this.watches)
                watch.Item1.PropertyChanged -= this.Obj_PropertyChanged;

            this.watches.Clear();
        }

        /// <summary>
        /// Убирает наблюдение.
        /// </summary>
        /// <param name="obj">Объект, за свойством которого велось наблюдение.</param>
        /// <param name="propertyName">Название свойства, за которым велось наблюдение.</param>
        public void RemoveWatch(INotifyPropertyChanged obj, string propertyName)
        {
            var watch = this.watches.FirstOrDefault(x => x.Item1 == obj && x.Item2 == propertyName);

            if (watch != null)
            {
                this.watches.Remove(watch);

                if (!this.watches.Any(x => x.Item1 == obj))
                    watch.Item1.PropertyChanged -= this.Obj_PropertyChanged;
            }
        }

        #endregion
    }
}