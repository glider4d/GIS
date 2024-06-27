using System;
using System.Windows.Input;

namespace Kts.WpfUtilities
{
    /// <summary>
    /// Представляет команду.
    /// </summary>
    [Serializable]
    public sealed partial class RelayCommand : ICommand
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Возможность выполнения команды.
        /// </summary>
        private readonly Func<bool> canExecute;

        /// <summary>
        /// Выполняемое действие.
        /// </summary>
        private readonly Action execute;
        private readonly Action<object> pExecute;
        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">Выполняемое действие.</param>
        public RelayCommand(Action execute) : this(execute, null)
        {
        }

        public RelayCommand(Action<object> pExecute) : this(pExecute, null)
        {
        }


        public RelayCommand(Action<object> execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            this.pExecute = execute;
            this.canExecute = canExecute;
        }
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">Выполняемое действие.</param>
        /// <param name="canExecute">Возможность выполнения команды.</param>
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            this.execute = execute;
            this.canExecute = canExecute;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Уведомляет об изменении возможности выполнения команды.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }

    // Реализация ICommand.
    public sealed partial class RelayCommand
    {
        #region Открытые события

        /// <summary>
        /// Событие изменения возможности выполнения команды.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        #endregion

        #region Открытые методы

        /// <summary>
        /// Возвращает возможность выполнения команды.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <returns>Возможность выполнения команды.</returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null ? true : this.canExecute();
        }

        /// <summary>
        /// Выполняет команду.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        public void Execute(object parameter)
        {
            if (parameter == null)
                this.execute();
            else
                this.pExecute(parameter);
        }

        #endregion
    }
}