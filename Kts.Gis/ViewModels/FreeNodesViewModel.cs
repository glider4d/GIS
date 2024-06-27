using Kts.Utilities;
using System;

namespace Kts.Gis.ViewModels
{
    internal sealed class FreeNodesViewModel
    {
        #region Открытые события

        /// <summary>
        /// Событие запроса закрытия представления.
        /// </summary>
        public event EventHandler CloseRequested;

        /// <summary>
        /// Событие запроса открытия узла.
        /// </summary>
        public event EventHandler OpenNodeRequested;

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает коллекцию узлов.
        /// </summary>
        public AdvancedObservableCollection<NodeViewModel> Nodes
        {
            get;
        } = new AdvancedObservableCollection<NodeViewModel>();

        /// <summary>
        /// Возвращает или задает выбранный узел.
        /// </summary>
        public NodeViewModel SelectedNode
        {
            get;
            set;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Закрывает представление.
        /// </summary>
        public void Close()
        {
            this.CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Отображает выбранный узел.
        /// </summary>
        public void ShowSelectedNode()
        {
            this.OpenNodeRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}