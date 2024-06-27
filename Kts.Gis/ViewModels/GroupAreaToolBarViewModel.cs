using Kts.Gis.RevertibleActions;
using Kts.History;
using System.Collections.Generic;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления панели инструментов группового редактирования.
    /// </summary>
    internal sealed partial class GroupAreaToolBarViewModel : ISetterIgnorer
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что зафиксированы ли длины линий.
        /// </summary>
        private bool areLinesFixed;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис истории изменений.
        /// </summary>
        private readonly HistoryService historyService;

        /// <summary>
        /// Редактируемые линии.
        /// </summary>
        private readonly List<LineViewModel> lines;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="GroupAreaToolBarViewModel"/>.
        /// </summary>
        /// <param name="lines">Редактируемые линии.</param>
        /// <param name="figureCount">Количество фигур.</param>
        /// <param name="lineCount">Количество линий.</param>
        /// <param name="historyService">Сервис истории изменений.</param>
        public GroupAreaToolBarViewModel(List<LineViewModel> lines, int figureCount, int lineCount, HistoryService historyService)
        {
            this.lines = lines;
            this.FigureCount = figureCount;
            this.LineCount = lineCount;
            this.historyService = historyService;

            // Определяем по большинству значение фиксированности длин линий.
            var fixedCount = 0;
            var unfixedCount = 0;
            foreach (var line in lines)
                if (line.IsLengthFixed)
                    fixedCount++;
                else
                    unfixedCount++;
            if (fixedCount >= unfixedCount)
                this.areLinesFixed = true;
            else
                this.areLinesFixed = false;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что зафиксированы ли длины линий.
        /// </summary>
        public bool AreLinesFixed
        {
            get
            {
                return this.areLinesFixed;
            }
            set
            {
#warning Тут заведомо есть ошибка, например, если у линий до изменения были разные значения фиксированности длины, то при откате действия откат будет произведен на общее значение
                // Запоминаем действие в истории изменений и выполняем его.
                var action = new SetPropertyAction(this, nameof(this.AreLinesFixed), this.AreLinesFixed, value);
                this.historyService.Add(new HistoryEntry(action, Target.Data, "изменение фиксированности протяженности группы линий"));
                action.Do();
            }
        }

        /// <summary>
        /// Возвращает количество фигур.
        /// </summary>
        public int FigureCount
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеются ли линии.
        /// </summary>
        public bool HasLines
        {
            get
            {
                return this.LineCount > 0;
            }
        }

        /// <summary>
        /// Возвращает количество линий.
        /// </summary>
        public int LineCount
        {
            get;
        }

        #endregion
    }

    // Реализация ISetterIgnorer.
    internal sealed partial class GroupAreaToolBarViewModel
    {
        #region Открытые методы

        /// <summary>
        /// Задает значение заданного свойства в обход его сеттера.
        /// </summary>
        /// <param name="propertyName">Название свойства.</param>
        /// <param name="value">Значение.</param>
        public void SetValue(string propertyName, object value)
        {
            if (propertyName == nameof(this.AreLinesFixed))
            {
                this.areLinesFixed = (bool)value;

                foreach (var line in this.lines)
                    line.SetValue(nameof(line.IsLengthFixed), this.AreLinesFixed);
            }
        }

        #endregion
    }
}