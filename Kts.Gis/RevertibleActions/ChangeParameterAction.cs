using Kts.Gis.Models;
using Kts.Gis.ViewModels;
using Kts.History;
using System;
using System.Text;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие изменения значения параметра.
    /// </summary>
    internal partial class ChangeParameterAction : IRevertibleAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Цель изменений.
        /// </summary>
        private readonly IParameterizedObjectViewModel target;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ChangeParameterAction"/>.
        /// </summary>
        /// <param name="target">Цель изменений.</param>
        /// <param name="param">Параметр.</param>
        /// <param name="oldValue">Старое значение.</param>
        /// <param name="newValue">Новое значение.</param>
        public ChangeParameterAction(IParameterizedObjectViewModel target, ParameterModel param, object oldValue, object newValue)
        {
            this.target = target;
            this.param = param;
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        #endregion

        #region Защищенные неизменяемые поля

        /// <summary>
        /// Новое значение.
        /// </summary>
        protected readonly object newValue;

        /// <summary>
        /// Старое значение.
        /// </summary>
        protected readonly object oldValue;

        /// <summary>
        /// Параметр.
        /// </summary>
        protected readonly ParameterModel param;

        #endregion
    }

    // Реализация IRevertibleAction.
    internal partial class ChangeParameterAction
    {
        #region Открытые виртуальные методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        public virtual void Do()
        {
            this.target.ChangeChangedValue(this.param, this.newValue);
        }

        /// <summary>
        /// Возвращает строковое представление действия.
        /// </summary>
        /// <returns>Строковое представление действия.</returns>
        public virtual string GetString()
        {
            var sb = new StringBuilder();

            sb.Append("Action: ");
            sb.Append(nameof(ChangeParameterAction));
            sb.Append(Environment.NewLine);
            sb.Append("Target: ");
            if (this.target is IObjectViewModel)
                sb.Append((this.target as IObjectViewModel).Id);
            else
                sb.Append(this.target.GetType());
            sb.Append(Environment.NewLine);
            sb.Append("Parameter: ");
            sb.Append(this.param.Id);
            sb.Append(Environment.NewLine);
            sb.Append("Value: ");
            sb.Append(this.oldValue);
            sb.Append(" -> ");
            sb.Append(this.newValue);

            return sb.ToString();
        }

        /// <summary>
        /// Выполняет действие, обратное прямому действию.
        /// </summary>
        public virtual void Revert()
        {
            this.target.ChangeChangedValue(this.param, this.oldValue);
        }

        #endregion
    }
}