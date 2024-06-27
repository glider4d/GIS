using System;

namespace Kts.Utilities
{
    /// <summary>
    /// Представляет цвет в цветовой модели RGB.
    /// </summary>
    [Serializable]
    public sealed class Color
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Color"/>.
        /// </summary>
        /// <param name="r">Значение R.</param>
        /// <param name="g">Значение G.</param>
        /// <param name="b">Значение B.</param>
        public Color(byte r, byte g, byte b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение B.
        /// </summary>
        public byte B
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает значение G.
        /// </summary>
        public byte G
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает значение R.
        /// </summary>
        public byte R
        {
            get;
            set;
        }

        #endregion
    }
}