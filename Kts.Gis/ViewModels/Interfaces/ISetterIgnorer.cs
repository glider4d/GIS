namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет интерфейс игнорщика сеттеров свойств.
    /// </summary>
    internal interface ISetterIgnorer
    {
        #region Методы

        /// <summary>
        /// Задает значение заданного свойства в обход его сеттера.
        /// </summary>
        /// <param name="propertyName">Название свойства.</param>
        /// <param name="value">Значение.</param>
        void SetValue(string propertyName, object value);

        #endregion
    }
}