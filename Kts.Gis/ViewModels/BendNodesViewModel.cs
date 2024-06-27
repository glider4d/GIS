using Kts.Messaging;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления узлов поворота.
    /// </summary>
    [Serializable]
    internal sealed class BendNodesViewModel
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        private readonly IMessageService messageService;
    
        /// <summary>
        /// Главная модель представления.
        /// </summary>
        private readonly MainViewModel viewModel;

        #endregion

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

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="BendNodesViewModel"/>.
        /// </summary>
        /// <param name="viewModel">Главная модель представления.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public BendNodesViewModel(MainViewModel viewModel, IMessageService messageService)
        {
            this.viewModel = viewModel;
            this.messageService = messageService;

            this.DeleteAllCommand = new RelayCommand(this.ExecuteDeleteAll);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает команду удаления всех узлов.
        /// </summary>
        public RelayCommand DeleteAllCommand
        {
            get;
        }

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

        #region Закрытые методы

        /// <summary>
        /// Выполняет удаление всех узлов.
        /// </summary>
        private void ExecuteDeleteAll()
        {
            var undeletedCount = 0;

            foreach (var node in this.Nodes)
                if (node.DeleteCommand.CanExecute(null))
                    node.DeleteCommand.Execute(null);
                else
                    undeletedCount++;

            if (undeletedCount == 0)
                this.messageService.ShowMessage("Все узлы успешно удалены", "Удаление узлов", MessageType.Information);
            else
                this.messageService.ShowMessage("Не удалось удалить некоторые узлы (" + undeletedCount + ")", "Удаление узлов", MessageType.Error);

            this.Close();
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