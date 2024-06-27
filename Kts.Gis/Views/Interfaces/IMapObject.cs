namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет интерфейс объекта карты.
    /// </summary>
    internal interface IMapObject
    {
        #region Свойства
    
        /// <summary>
        /// Возвращает холст, на котором расположен объект.
        /// </summary>
        IndentableCanvas Canvas
        {
            get;
        }

        #endregion

        #region Методы

        /// <summary>
        /// Добавляет объект на холст.
        /// </summary>
        /// <param name="canvas">Холст.</param>
        /// <returns>true, если объект был добавлен, иначе - false.</returns>
        bool AddToCanvas(IndentableCanvas canvas);

        /// <summary>
        /// Удаляет объект с холста.
        /// </summary>
        /// <returns>true, если объект был удален, иначе - false.</returns>
        bool RemoveFromCanvas();

        #endregion
    }
}