using Kts.Gis.Models;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления сгрупированного типа объекта.
    /// </summary>
    internal sealed class GroupedTypeViewModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="GroupedTypeViewModel"/>.
        /// </summary>
        /// <param name="groupName">Название группы.</param>
        /// <param name="type">Тип объекта.</param>
        public GroupedTypeViewModel(string groupName, ObjectType type)
        {
            this.GroupName = groupName;
            this.ObjectType = type;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает название группы.
        /// </summary>
        public string GroupName
        {
            get;
        }

        /// <summary>
        /// Возвращает название типа объекта.
        /// </summary>
        public string Name
        {
            get
            {
                return this.ObjectType.Name;
            }
        }

        /// <summary>
        /// Возвращает тип объекта.
        /// </summary>
        public ObjectType ObjectType
        {
            get;
        }

        #endregion
    }
}