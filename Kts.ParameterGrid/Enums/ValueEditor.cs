namespace Kts.ParameterGrid
{
    /// <summary>
    /// Редактор значения.
    /// </summary>
    public enum ValueEditor
    {
        /// <summary>
        /// Флажок.
        /// </summary>
        CheckBox,

        /// <summary>
        /// Комбобокс.
        /// </summary>
        ComboBox,

        /// <summary>
        /// Выбиральщик даты.
        /// </summary>
        DatePicker,

        /// <summary>
        /// Отсутствие редактора.
        /// </summary>
        None,

        /// <summary>
        /// Текстовое поле.
        /// </summary>
        TextBox,

        /// <summary>
        /// Выбиральщик года.
        /// </summary>
        YearPicker
    }
}