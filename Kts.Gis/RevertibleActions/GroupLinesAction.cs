using Kts.Gis.ViewModels;
using Kts.History;
using System.Collections.Generic;
using System.Text;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие группировки линий.
    /// </summary>
    internal sealed partial class GroupLinesAction : IRevertibleAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Связи линий.
        /// </summary>
        private readonly Dictionary<LineViewModel, LineViewModel> bonds = new Dictionary<LineViewModel, LineViewModel>();

        /// <summary>
        /// Линии.
        /// </summary>
        private readonly List<LineViewModel> lines;

        /// <summary>
        /// Главная линия, с которой будут сгруппированы заданные линии.
        /// </summary>
        private readonly LineViewModel mainLine;

        /// <summary>
        /// Новые связи линий.
        /// </summary>
        private readonly Dictionary<LineViewModel, LineViewModel> newBonds;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="GroupLinesAction"/>.
        /// </summary>
        /// <param name="lines">Линии.</param>
        /// <param name="bonds">Связи линий.</param>
        public GroupLinesAction(List<LineViewModel> lines, Dictionary<LineViewModel, LineViewModel> bonds)
        {
            this.lines = lines;
            this.newBonds = bonds;

            // Запоминаем, с кем на данный момент сгруппированы линии.
            foreach (var line in lines)
                this.bonds.Add(line, line.GroupedLine);
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="GroupLinesAction"/>.
        /// </summary>
        /// <param name="lines">Линии.</param>
        /// <param name="mainLine">Главная линия, с которой будут сгруппированы заданные линии.</param>
        public GroupLinesAction(List<LineViewModel> lines, LineViewModel mainLine)
        {
            this.lines = lines;
            this.mainLine = mainLine;

            // Запоминаем, с кем на данный момент сгруппированы линии.
            foreach (var line in lines)
                this.bonds.Add(line, line.GroupedLine);
        }

        #endregion
    }

    // Реализация IRevertibleAction.
    internal sealed partial class GroupLinesAction
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        public void Do()
        {
            if (this.mainLine == null)
                foreach (var line in this.lines)
                    line.GroupWith(this.newBonds[line]);
            else
                foreach (var line in this.lines)
                    line.GroupWith(this.mainLine);
        }

        /// <summary>
        /// Возвращает строковое представление действия.
        /// </summary>
        /// <returns>Строковое представление действия.</returns>
        public string GetString()
        {
            var sb = new StringBuilder();

            sb.Append("Action: ");
            sb.Append(nameof(GroupLinesAction));

            return sb.ToString();
        }

        /// <summary>
        /// Выполняет действие, обратное прямому действию.
        /// </summary>
        public void Revert()
        {
            foreach (var line in this.lines)
                line.GroupWith(this.bonds[line]);
        }

        #endregion
    }
}