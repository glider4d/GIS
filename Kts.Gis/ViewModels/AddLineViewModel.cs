using Kts.Gis.Models;
using Kts.WpfUtilities;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления добавления линии к линии.
    /// </summary>
    internal sealed class AddLineViewModel
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Линия, к которой будут добавляться линии.
        /// </summary>
        private LineViewModel line;

        /// <summary>
        /// Тип добавляемой линии.
        /// </summary>
        private ObjectType lineType;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AddLineViewModel"/>.
        /// </summary>
        /// <param name="line">Линия, к которой будут добавляться линии.</param>
        /// <param name="lineType">Тип добавляемой линии.</param>
        public AddLineViewModel(LineViewModel line, ObjectType lineType)
        {
            this.line = line;
            this.lineType = lineType;

            this.AddLineCommand = new RelayCommand(this.ExecuteAddLine);
        }

        #endregion

        #region Открытые свойства
        
        /// <summary>
        /// Возвращает команду добавления линии к линии.
        /// </summary>
        public RelayCommand AddLineCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает название типа добавляемой линии.
        /// </summary>
        public string Name
        {
            get
            {
                return this.lineType.SingularName;
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выполняет добавление линии к линии.
        /// </summary>
        private void ExecuteAddLine()
        {
            this.line.AddLine(this.lineType);
        }
        
        #endregion
    }
}