namespace Kts.Gis.Models
{
    /// <summary>
    /// Оператор.
    /// </summary>
    public enum Operator
    {
        /// <summary>
        /// Содержит.
        /// </summary>
        Contains = 0,

        /// <summary>
        /// Без значения.
        /// </summary>
        Empty = 6,

        /// <summary>
        /// Равно.
        /// </summary>
        Equal = 1,

        /// <summary>
        /// Меньше.
        /// </summary>
        Less = 2,

        /// <summary>
        /// Больше.
        /// </summary>
        More = 3,

        /// <summary>
        /// Не содержит.
        /// </summary>
        NotContains = 4,

        /// <summary>
        /// Не равно.
        /// </summary>
        NotEqual = 5
    }
}