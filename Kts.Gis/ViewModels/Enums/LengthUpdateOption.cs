namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Опция обновления длин линий.
    /// </summary>
    internal enum LengthUpdateOption
    {
        /// <summary>
        /// Обновить все линии.
        /// </summary>
        All,

        /// <summary>
        /// Обновить только несохраненные линии.
        /// </summary>
        OnlyNonSaved,

        /// <summary>
        /// Обновить только выбранный линии.
        /// </summary>
        OnlySelected
    }
}