namespace Kts.Gis.Views
{
    /// <summary>
    /// Тип точки области редактирования группы объектов.
    /// </summary>
    internal enum GroupAreaPointType
    {
        /// <summary>
        /// Точка перемещения объектов.
        /// </summary>
        Move,

        /// <summary>
        /// Точка изменения размеров объектов.
        /// </summary>
        Resize,

        /// <summary>
        /// Точка вращения объектов.
        /// </summary>
        Rotate
    }
}