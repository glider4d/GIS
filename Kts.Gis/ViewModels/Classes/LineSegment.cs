namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет сегмент линии.
    /// </summary>
    internal sealed class LineSegment
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LineSegment"/>.
        /// </summary>
        /// <param name="length">Длина сегмента линии.</param>
        public LineSegment(double length)
        {
            this.Length = length;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает длину сегмента линии.
        /// </summary>
        public double Length
        {
            get;
            set;
        }

        #endregion
    }
}