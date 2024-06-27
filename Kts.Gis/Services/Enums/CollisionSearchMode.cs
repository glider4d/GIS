namespace Kts.Gis.Services
{
    /// <summary>
    /// Режим поиска коллизии.
    /// </summary>
    internal enum CollisionSearchMode
    {
        /// <summary>
        /// Поиск первого попавшегося объекта.
        /// </summary>
        First,

        /// <summary>
        /// Поиск первого и ближайшего попавшегося объекта.
        /// </summary>
        FirstAndNearest
    }
}