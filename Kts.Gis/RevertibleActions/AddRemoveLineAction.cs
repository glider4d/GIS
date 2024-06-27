using Kts.Gis.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие добавления/удаления линии.
    /// </summary>
    internal sealed partial class AddRemoveLineAction : AddRemoveObjectAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Добавляемая/удаляемая линия.
        /// </summary>
        private readonly LineViewModel line;

        /// <summary>
        /// Линии.
        /// </summary>
        private readonly List<LineViewModel> lines;

        /// <summary>
        /// Главная модель представления.
        /// </summary>
        private readonly MainViewModel mainViewModel;

        /// <summary>
        /// Узлы линии(й).
        /// </summary>
        private readonly Dictionary<LineViewModel, Tuple<NodeViewModel, NodeViewModel>> nodes = new Dictionary<LineViewModel, Tuple<NodeViewModel, NodeViewModel>>();

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AddRemoveLineAction"/>.
        /// </summary>
        /// <param name="line">Добавляемая/удаляемая линия.</param>
        /// <param name="mainViewModel">Главная модель представления.</param>
        /// <param name="isAdding">Значение, указывающее на то, что выполняется ли добавление.</param>
        public AddRemoveLineAction(LineViewModel line, MainViewModel mainViewModel, bool isAdding) : base(isAdding)
        {
            this.line = line;
            this.mainViewModel = mainViewModel;

            this.nodes.Add(line, new Tuple<NodeViewModel, NodeViewModel>(line.LeftNode, line.RightNode));
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AddRemoveLineAction"/>.
        /// </summary>
        /// <param name="line">Линии.</param>
        /// <param name="mainViewModel">Главная модель представления.</param>
        /// <param name="isAdding">Значение, указывающее на то, что выполняется ли добавление.</param>
        public AddRemoveLineAction(List<LineViewModel> lines, MainViewModel mainViewModel, bool isAdding) : base(isAdding)
        {
            this.lines = lines;
            this.mainViewModel = mainViewModel;

            foreach (var line in lines)
                this.nodes.Add(line, new Tuple<NodeViewModel, NodeViewModel>(line.LeftNode, line.RightNode));
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Добавляет линию.
        /// </summary>
        /// <param name="line">Линия.</param>
        private void AddLine(LineViewModel line)
        {
            this.mainViewModel.AddRemoveLine(line, true);

            this.mainViewModel.AddRemoveNodes(line, this.nodes[line].Item1, this.nodes[line].Item2, true);
        }

        /// <summary>
        /// Удаляет линию.
        /// </summary>
        /// <param name="line">Линия.</param>
        private void RemoveLine(LineViewModel line)
        {
            this.mainViewModel.AddRemoveLine(line, false);

            this.mainViewModel.AddRemoveNodes(line, this.nodes[line].Item1, this.nodes[line].Item2, false);
        }

        #endregion
    }

    // Реализация AddRemoveObjectAction.
    internal sealed partial class AddRemoveLineAction
    {
        #region Защищенные переопределенные методы

        /// <summary>
        /// Добавляет объект.
        /// </summary>
        protected override void Add()
        {
            if (this.line == null)
                foreach (var line in this.lines)
                    this.AddLine(line);
            else
                this.AddLine(this.line);
        }

        /// <summary>
        /// Удаляет объект.
        /// </summary>
        protected override void Remove()
        {
            if (this.line == null)
                foreach (var line in this.lines)
                    this.RemoveLine(line);
            else
                this.RemoveLine(this.line);
        }

        #endregion

        #region Открытые переопределенные методы
        
        /// <summary>
        /// Возвращает строковое представление действия.
        /// </summary>
        /// <returns>Строковое представление действия.</returns>
        public override string GetString()
        {
            var sb = new StringBuilder();

            sb.Append("Action: ");
            sb.Append(nameof(AddRemoveLineAction));
            sb.Append(Environment.NewLine);

            if (this.line != null)
            {
                sb.Append("Line: ");
                sb.Append(this.line.Id);
            }
            else
            {
                sb.Append("Lines: ");
                sb.Append(this.lines[0].Id);
                for (int i = 1; i < this.lines.Count; i++)
                    sb.Append(", " + this.lines[i].Id);
            }

            return sb.ToString();
        }

        #endregion
    }
}