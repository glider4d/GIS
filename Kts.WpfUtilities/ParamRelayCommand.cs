using System;
using System.Windows.Input;

namespace Kts.WpfUtilities
{
    /// <summary>
    /// Представляет параметризованную команду.
    /// </summary>
    /// <typeparam name="T">Тип параметра.</typeparam>
    public sealed partial class ParamRelayCommand<T> : ICommand
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Возможность выполнения команды.
        /// </summary>
        private readonly Func<bool> canExecute;

        /// <summary>
        /// Выполняемое действие.
        /// </summary>
        private readonly Action<T> execute;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ParamRelayCommand{T}"/>.
        /// </summary>
        /// <param name="execute">Выполняемое действие.</param>
        public ParamRelayCommand(Action<T> execute) : this(execute, null)
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ParamRelayCommand{T}"/>.
        /// </summary>
        /// <param name="execute">Выполняемое действие.</param>
        /// <param name="canExecute">Возможность выполнения команды.</param>
        public ParamRelayCommand(Action<T> execute, Func<bool> canExecute)
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
    public sealed partial class ParamRelayCommand<T>
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
            this.execute((T)parameter);
        }

        #endregion
    }
}