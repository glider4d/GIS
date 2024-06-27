using Kts.Gis.ViewModels;
using Kts.History;
using System;
using System.Text;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие вставки объекта.
    /// </summary>
    internal sealed partial class PasteObjectAction : IRevertibleAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Действие добавления/удаления объекта.
        /// </summary>
        private readonly AddRemoveObjectAction action;

        /// <summary>
        /// Вставляемый объект.
        /// </summary>
        private readonly ICopyableObjectViewModel pastingObject;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PasteObjectAction"/>.
        /// </summary>
        /// <param name="pastingObject">Вставляемый объект.</param>
        /// <param name="mainViewModel">Главная модель представления.</param>
        public PasteObjectAction(ICopyableObjectViewModel pastingObject, MainViewModel mainViewModel)
        {
            this.pastingObject = pastingObject;

            if (pastingObject is FigureViewModel)
                this.action = new AddRemoveFigureAction(pastingObject as FigureViewModel, mainViewModel, true);
            else
                if (pastingObject is LineViewModel)
                    this.action = new AddRemoveLineAction(pastingObject as LineViewModel, mainViewModel, true);
                else
                    throw new NotImplementedException("Не реализована работа со следующим типом вставляемых объектов: " + pastingObject.GetType().ToString());
        }

        #endregion
    }

    // Реализация IRevertibleAction.
    internal sealed partial class PasteObjectAction
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        public void Do()
        {
            this.action.Do();
        }

        /// <summary>
        /// Возвращает строковое представление действия.
        /// </summary>
        /// <returns>Строковое представление действия.</returns>
        public string GetString()
        {
            var sb = new StringBuilder();

            sb.Append("Action: ");
            sb.Append(nameof(PasteObjectAction));
            sb.Append(Environment.NewLine);
            sb.Append("Object: ");
            sb.Append(this.pastingObject.ToString());

            return sb.ToString();
        }

        /// <summary>
        /// Выполняет действие, обратное прямому действию.
        /// </summary>
        public void Revert()
        {
            this.action.Revert();
        }

        #endregion
    }
}