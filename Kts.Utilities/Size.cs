using System;
namespace Kts.Utilities
{
    /// <summary>
    /// Представляет размер.
    /// </summary>
    [Serializable]
    public sealed class Size
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Size"/>.
        /// </summary>
        /// <param name="width">Ширина.</param>
        /// <param name="height">Высота.</param>
        public Size(double width, double height)
        {
            this.Width = width;
            this.Height = height;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает высоту.
        /// </summary>
        public double Height
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает ширину.
        /// </summary>
        public double Width
        {
            get;
            set;
        }

        #endregion

        #region Открытые статические свойства

        /// <summary>
        /// Возвращает пустой размер.
        /// </summary>
        public static Size Empty
        {
            get
            {
                return new Size(0, 0);
            }
        }

        #endregion
    }
}