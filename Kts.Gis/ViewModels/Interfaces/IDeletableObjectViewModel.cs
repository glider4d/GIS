using Kts.WpfUtilities;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет интерфейс модели представления удаляемого из источника данных объекта.
    /// </summary>
    internal interface IDeletableObjectViewModel
    {
        #region Свойства

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли удалить объект.
        /// </summary>
        bool CanBeDeleted
        {
            get;
        }

        /// <summary>
        /// Возвращает команду удаления объекта.
        /// </summary>
        RelayCommand DeleteCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду полного удаления объекта.
        /// </summary>
        RelayCommand FullDeleteCommand
        {
            get;
        }

        #endregion

        #region Методы

        /// <summary>
        /// Выполняет полное удаление объекта из источника данных.
        /// </summary>
        void FullDelete();

        /// <summary>
        /// Помечает объект на полное удаление из источника данных.
        /// </summary>
        void MarkFullDelete();

        #endregion
    }
}