namespace Kts.History
{
    /// <summary>
    /// Представляет интерфейс возвратимого действия.
    /// </summary>
    public interface IRevertibleAction
    {
        #region Методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        void Do();

        /// <summary>
        /// Возвращает строковое представление действия.
        /// </summary>
        /// <returns>Строковое представление действия.</returns>
        string GetString();

        /// <summary>
        /// Выполняет действие, обратное прямому действию.
        /// </summary>
        void Revert();

        #endregion
    }
}