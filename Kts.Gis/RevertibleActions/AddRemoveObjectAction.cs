using Kts.History;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие добавления/удаления объекта.
    /// </summary>
    internal abstract partial class AddRemoveObjectAction : IRevertibleAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Значение, указывающее на то, что выполняется ли добавление.
        /// </summary>
        private readonly bool isAdding;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AddRemoveObjectAction"/>.
        /// </summary>
        /// <param name="isAdding">Значение, указывающее на то, что выполняется ли добавление.</param>
        public AddRemoveObjectAction(bool isAdding)
        {
            this.isAdding = isAdding;
        }

        #endregion

        #region Защищенные абстрактные методы

        /// <summary>
        /// Добавляет объект.
        /// </summary>
        protected abstract void Add();

        /// <summary>
        /// Удаляет объект.
        /// </summary>
        protected abstract void Remove();

        #endregion
    }

    // Реализация IRevertibleAction.
    internal abstract partial class AddRemoveObjectAction
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        public void Do()
        {
            if (this.isAdding)
                this.Add();
            else
                this.Remove();
        }

        /// <summary>
        /// Выполняет действие, обратное прямому действию.
        /// </summary>
        public void Revert()
        {
            if (this.isAdding)
                this.Remove();
            else
                this.Add();
        }

        #endregion

        #region Открытые абстрактные методы

        /// <summary>
        /// Возвращает строковое представление действия.
        /// </summary>
        /// <returns>Строковое представление действия.</returns>
        public abstract string GetString();

        #endregion
    }
}