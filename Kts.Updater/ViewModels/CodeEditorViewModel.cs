namespace Kts.Updater.ViewModels
{
    /// <summary>
    /// Представляет модель представления редактирования кода.
    /// </summary>
    internal sealed class CodeEditorViewModel
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Редактируемый объект SQL.
        /// </summary>
        private readonly CodeObjectViewModel obj;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CodeEditorViewModel"/>.
        /// </summary>
        /// <param name="obj">Редактируемый объект SQL.</param>
        public CodeEditorViewModel(CodeObjectViewModel obj)
        {
            this.obj = obj;
        }

        #endregion
    }
}