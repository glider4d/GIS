using System.Collections.Generic;

namespace Kts.Gis.Reports.ViewModels
{
    /// <summary>
    /// Представляет модель представления элемента дерева, используемого для моделирования вложенности <see cref="ReportViewModel"/>.
    /// </summary>
    public sealed partial class MockViewModel : ITreeItemViewModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MockViewModel"/>.
        /// </summary>
        /// <param name="name">Название элемента дерева.</param>
        public MockViewModel(string name)
        {
            this.Name = name;
        }

        #endregion
    }

    // Реализация ITreeItemViewModel.
    public sealed partial class MockViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает дочерние объекты элемента дерева.
        /// </summary>
        public List<ITreeItemViewModel> Children
        {
            get;
        } = new List<ITreeItemViewModel>();

        /// <summary>
        /// Возвращает название элемента дерева.
        /// </summary>
        public string Name
        {
            get;
        }

        #endregion
    }
}