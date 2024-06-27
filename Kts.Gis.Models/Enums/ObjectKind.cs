namespace Kts.Gis.Models
{
    /// <summary>
    /// Вид объекта.
    /// </summary>
    public enum ObjectKind
    {
        /// <summary>
        /// Значок.
        /// </summary>
        Badge = 1,

        /// <summary>
        /// Фигура.
        /// </summary>
        Figure,

        /// <summary>
        /// Линия.
        /// </summary>
        Line,

        /// <summary>
        /// Узел.
        /// </summary>
        Node,

        /// <summary>
        /// Незаданный.
        /// </summary>
        None,

        /// <summary>
        /// Невизуальный объект.
        /// </summary>
        NonVisualObject
    }
}