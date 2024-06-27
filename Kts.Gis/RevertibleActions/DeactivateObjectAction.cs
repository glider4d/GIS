using Kts.Gis.Models;
using Kts.Gis.ViewModels;
using Kts.History;
using System;
using System.Linq;
using System.Text;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие деактивации объекта.
    /// </summary>
    internal sealed partial class DeactivateObjectAction : IRevertibleAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Объект.
        /// </summary>
        private readonly IObjectViewModel obj;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DeactivateObjectAction"/>.
        /// </summary>
        /// <param name="obj">Объект.</param>
        public DeactivateObjectAction(IObjectViewModel obj)
        {
            this.obj = obj;

            if (!(obj is IParameterizedObjectViewModel))
                throw new ArgumentException("Заданный объект должен реализовывать интерфейс IParameterizedObjectViewModel");
        }

        #endregion
    }

    // Реализация IRevertibleAction.
    internal sealed partial class DeactivateObjectAction
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        public void Do()
        {
            (this.obj as IParameterizedObjectViewModel).ChangeChangedValue(this.obj.Type.Parameters.First(x => x.Alias == Alias.IsActive), !this.obj.IsActive);
        }

        /// <summary>
        /// Возвращает строковое представление действия.
        /// </summary>
        /// <returns>Строковое представление действия.</returns>
        public string GetString()
        {
            var sb = new StringBuilder();

            sb.Append("Action: ");
            sb.Append(nameof(DeactivateObjectAction));
            sb.Append(Environment.NewLine);
            sb.Append("Object: ");
            sb.Append(this.obj.Id);

            return sb.ToString();
        }

        /// <summary>
        /// Выполняет действие, обратное прямому действию.
        /// </summary>
        public void Revert()
        {
            (this.obj as IParameterizedObjectViewModel).ChangeChangedValue(this.obj.Type.Parameters.First(x => x.Alias == Alias.IsActive), !this.obj.IsActive);
        }

        #endregion
    }
}