using Kts.Gis.Models;
using Kts.Gis.ViewModels;
using System;
using System.Text;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие изменения значения параметра линии.
    /// </summary>
    internal sealed partial class ChangeLineParameterAction : ChangeParameterAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Линия.
        /// </summary>
        private readonly LineViewModel line;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ChangeLineParameterAction"/>.
        /// </summary>
        /// <param name="line">Линия.</param>
        /// <param name="param">Параметр.</param>
        /// <param name="oldValue">Старое значение.</param>
        /// <param name="newValue">Новое значение.</param>
        public ChangeLineParameterAction(LineViewModel line, ParameterModel param, object oldValue, object newValue) : base(line, param, oldValue, newValue)
        {
            this.line = line;
        }

        #endregion
    }

    // Реализация ChangeParameterAction.
    internal sealed partial class ChangeLineParameterAction
    {
        #region Открытые переопределенные методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        public override void Do()
        {
            base.Do();
            
            // Если изменена длина линии, то меняем ее всем остальным линиям.
            if (this.param.Alias == Alias.LineLength)
                foreach (var line in this.line.GetGroupedLines())
                    line.ChangeChangedValue(this.param, this.newValue);
        }

        /// <summary>
        /// Возвращает строковое представление действия.
        /// </summary>
        /// <returns>Строковое представление действия.</returns>
        public override string GetString()
        {
            var sb = new StringBuilder();

            sb.Append("Action: ");
            sb.Append(nameof(ChangeLineParameterAction));
            sb.Append(Environment.NewLine);
            sb.Append("Line: ");
            sb.Append(this.line.Id);
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
        public override void Revert()
        {
            base.Revert();

            // Если изменена длина линии, то меняем ее всем остальным линиям.
            if (this.param.Alias == Alias.LineLength)
                foreach (var line in this.line.GetGroupedLines())
                    line.ChangeChangedValue(this.param, this.oldValue);
        }

        #endregion
    }
}