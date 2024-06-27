using Kts.Updater.Models;
using Kts.WpfUtilities;

namespace Kts.Updater.ViewModels
{
    /// <summary>
    /// Представляет модель представления объекта-кода SQL.
    /// </summary>
    internal sealed class CodeObjectViewModel : SqlObjectViewModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CodeObjectViewModel"/>.
        /// </summary>
        /// <param name="model">Модель объекта SQL.</param>
        public CodeObjectViewModel(SqlObjectModel model) : base(model)
        {
            this.ChangeCommand = new RelayCommand(this.ExecuteChange);
            this.DeleteCommand = new RelayCommand(this.ExecuteDelete);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает команду изменения объекта-кода SQL.
        /// </summary>
        public RelayCommand ChangeCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду удаления объекта-кода SQL.
        /// </summary>
        public RelayCommand DeleteCommand
        {
            get;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выполняет изменение объекта-кода SQL.
        /// </summary>
        private void ExecuteChange()
        {
#warning Нужно дописать код
        }

        /// <summary>
        /// Выполняет удаление объекта-кода SQL.
        /// </summary>
        private void ExecuteDelete()
        {
#warning Нужно дописать код
        }

        #endregion
    }
}