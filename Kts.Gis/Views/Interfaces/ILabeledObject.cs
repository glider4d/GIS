namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет интерфейс объекта с надписью.
    /// </summary>
    internal interface ILabeledObject
    {
        #region Свойства

        /// <summary>
        /// Возвращает надпись.
        /// </summary>
        SmartLabel Label
        {
            get;
        }

        #endregion

        #region Методы

        /// <summary>
        /// Выполняет действия, связанные с изменением надписи.
        /// </summary>
        /// <param name="newAngle">Новый угол поворота надписи.</param>
        /// <param name="newPosition">Новое положение надписи.</param>
        /// <param name="newSize">Новый размер шрифта надписи.</param>
        void OnLabelChanged(int? newAngle, int? newPosition, int? newSize);

        #endregion
    }
}