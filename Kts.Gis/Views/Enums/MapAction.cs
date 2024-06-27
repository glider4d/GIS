namespace Kts.Gis.Views
{
    /// <summary>
    /// Действие над картой.
    /// </summary>
    internal enum MapAction
    {
        /// <summary>
        /// Рисование.
        /// </summary>
        Draw = 0,

        /// <summary>
        /// Редактирование.
        /// </summary>
        Edit = 1,

        /// <summary>
        /// Редактирование группы объектов.
        /// </summary>
        EditGroup = 5,

        /// <summary>
        /// Перемещение.
        /// </summary>
        Move = 2,

        /// <summary>
        /// Выбор.
        /// </summary>
        Select = 3,

        /// <summary>
        /// Задание области печати.
        /// </summary>
        SetPrintArea = 6,

        /// <summary>
        /// Добавление надписи.
        /// </summary>
        Text = 4
    }
}