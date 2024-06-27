using System.Collections.Generic;

namespace Kts.Gis.Reports.ViewModels
{
    /// <summary>
    /// Представляет интерфейс модели представления элемента дерева.
    /// </summary>
    public interface ITreeItemViewModel
    {
        #region Свойства

        /// <summary>
        /// Возвращает дочерние объекты элемента дерева.
        /// </summary>
        List<ITreeItemViewModel> Children
        {
            get;
        }

        /// <summary>
        /// Возвращает название элемента дерева.
        /// </summary>
        string Name
        {
            get;
        }

        #endregion
    }
}