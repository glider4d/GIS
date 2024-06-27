using Kts.Gis.Models;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет интерфейс модели представления объекта, имеющего тип.
    /// </summary>
    public interface ITypedObjectViewModel
    {
        #region Свойства

        /// <summary>
        /// Возвращает тип объекта.
        /// </summary>
        ObjectType Type
        {
            get;
        }

        #endregion
    }
}