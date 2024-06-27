using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Kts.History
{
    /// <summary>
    /// Представляет сервис истории изменений.
    /// </summary>
    [Serializable]
    public sealed partial class HistoryService : INotifyPropertyChanged
    {
        #region Закрытые поля
    
        /// <summary>
        /// Указатель на текущую позицию в истории изменений.
        /// </summary>
        private int pointer = -1;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// История изменений.
        /// </summary>
        private readonly List<HistoryEntry> history = new List<HistoryEntry>();

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие изменения истории изменений.
        /// </summary>
        public event EventHandler HistoryChanged;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="HistoryService"/>.
        /// </summary>
        public HistoryService()
        {
            this.RedoCommand = new RelayCommand(this.ExecuteRedo, this.CanExecuteRedo);
            this.UndoCommand = new RelayCommand(this.ExecuteUndo, this.CanExecuteUndo);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли отменить последнее действие.
        /// </summary>
        public bool CanGoBack
        {
            get
            {
                return this.pointer > -1;
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли повторить последнее действие.
        /// </summary>
        public bool CanGoForward
        {
            get
            {
                return this.pointer < this.history.Count - 1;
            }
        }

        /// <summary>
        /// Возвращает команду повтора последнего действия.
        /// </summary>
        public RelayCommand RedoCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает описание повторяемого действия.
        /// </summary>
        public string RedoDescription
        {
            get
            {
                if (this.CanGoForward)
                    return "Повторить " + this.history[this.pointer + 1].Description;

                return "Повторить";
            }
        }

        /// <summary>
        /// Возвращает команду отмены последнего действия.
        /// </summary>
        public RelayCommand UndoCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает описание отменяемого действия.
        /// </summary>
        public string UndoDescription
        {
            get
            {
                if (this.CanGoBack)
                    return "Отменить " + this.GetCurrentEntry().Description;

                return "Отменить";
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить повтор последнего действия.
        /// </summary>
        /// <returns>Значение, указывающее на то, что можно ли выполнить повтор последнего действия.</returns>
        private bool CanExecuteRedo()
        {
            return this.CanGoForward;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить отмену последнего действия.
        /// </summary>
        /// <returns>Значение, указывающее на то, что можно ли выполнить отмену последнего действия.</returns>
        private bool CanExecuteUndo()
        {
            return this.CanGoBack;
        }

        /// <summary>
        /// Выполняет повтор последнего действия.
        /// </summary>
        private void ExecuteRedo()
        {
            this.GoForward();
        }

        /// <summary>
        /// Выполняет отмену последнего действия.
        /// </summary>
        private void ExecuteUndo()
        {
            this.GoBack();
        }

        /// <summary>
        /// Уведомляет об изменениях возможности отмены/повтора последнего действия.
        /// </summary>
        private void RaiseChanges()
        {
            this.UndoCommand.RaiseCanExecuteChanged();
            this.RedoCommand.RaiseCanExecuteChanged();

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.UndoDescription)));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.RedoDescription)));

            this.HistoryChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Добавляет новую запись в историю изменений.
        /// </summary>
        /// <param name="entry">Запись.</param>
        public void Add(HistoryEntry entry)
        {
            // Если указатель ссылается на непоследний элемент, то удаляем образовавшийся хвост из истории изменений.
            if (this.pointer != this.history.Count - 1)
                this.history.RemoveRange(this.pointer + 1, this.history.Count - this.pointer - 1);

            // Вставляем запись в историю изменений и меняем указатель.
            this.history.Add(entry);
            this.pointer++;

            this.RaiseChanges();
        }

        /// <summary>
        /// Очищает историю изменений.
        /// </summary>
        public void Clear()
        {
            this.history.Clear();

            this.pointer = -1;

            this.RaiseChanges();
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеется ли в истории изменений неотмененная запись о действии, целью которого является заданная цель.
        /// </summary>
        /// <param name="target">Цель действия.</param>
        /// <returns>true, если такая запись имеется, иначе - false.</returns>
        public bool Contains(Target target)
        {
            for (int i = 0; i <= this.pointer; i++)
                if (this.history[i].Target == target)
                    return true;

            return false;
        }

        /// <summary>
        /// Возвращает текущую запись в истории изменений.
        /// </summary>
        /// <returns>Запись.</returns>
        public HistoryEntry GetCurrentEntry()
        {
            if (this.pointer > -1)
                return this.history[this.pointer];

            return null;
        }

        /// <summary>
        /// Отменяет последнее действие.
        /// </summary>
        public void GoBack()
        {
            if (!this.CanGoBack)
                return;

            this.history[this.pointer].Action.Revert();

            this.pointer--;

            this.RaiseChanges();
        }

        /// <summary>
        /// Повторяет последнее действие.
        /// </summary>
        public void GoForward()
        {
            if (!this.CanGoForward)
                return;

            this.pointer++;

            this.history[this.pointer].Action.Do();

            this.RaiseChanges();
        }

        #endregion
    }

    // Реализация INotifyPropertyChanged.
    public sealed partial class HistoryService
    {
        #region Открытые события

        /// <summary>
        /// Событие изменения свойства истории изменений.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}