using Kts.Gis.Models;
using Kts.Gis.ViewModels;
using Kts.History;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие вставки значений группы параметров.
    /// </summary>
    internal sealed partial class PasteParametersAction : IRevertibleAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Новые значения параметров.
        /// </summary>
        private readonly Dictionary<ParameterModel, object> newValues;

        /// <summary>
        /// Предыдущие значения параметров.
        /// </summary>
        private readonly Dictionary<ParameterModel, object> oldValues;

        /// <summary>
        /// Цель изменений.
        /// </summary>
        private readonly IParameterizedObjectViewModel target;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PasteParametersAction"/>.
        /// </summary>
        /// <param name="target">Цель изменений.</param>
        /// <param name="oldValues">Предыдущие значения параметров.</param>
        /// <param name="newValues">Новые значения параметров.</param>
        public PasteParametersAction(IParameterizedObjectViewModel target, Dictionary<ParameterModel, object> oldValues, Dictionary<ParameterModel, object> newValues)
        {
            this.target = target;
            this.oldValues = oldValues;
            this.newValues = newValues;
        }

        #endregion
    }

    // Реализация IRevertibleAction.
    internal sealed partial class PasteParametersAction
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        public void Do()
        {
            foreach (var entry in this.newValues)
                this.target.ChangeChangedValue(entry.Key, entry.Value);
        }

        /// <summary>
        /// Возвращает строковое представление действия.
        /// </summary>
        /// <returns>Строковое представление действия.</returns>
        public string GetString()
        {
            var sb = new StringBuilder();

            // Получаем идентификаторы параметров.
            var ids = this.oldValues.Keys.ToList();
            var parameters = ids[0].Id.ToString();
            for (int i = 1; i < ids.Count; i++)
                parameters += ", " + ids[i].Id.ToString();

            // Получаем старые значения параметров.
            var list = this.oldValues.Values.ToList();
            var oldValues = list[0].ToString();
            for (int i = 1; i < list.Count; i++)
                oldValues += ", " + list[i].ToString();

            // Получаем новые значения параметров.
            list = this.newValues.Values.ToList();
            var newValues = list[0].ToString();
            for (int i = 1; i < list.Count; i++)
                newValues += ", " + list[i].ToString();

            sb.Append("Action: ");
            sb.Append(nameof(PasteParametersAction));
            sb.Append(Environment.NewLine);
            sb.Append("Target: ");
            if (this.target is IObjectViewModel)
                sb.Append((this.target as IObjectViewModel).Id);
            else
                sb.Append(this.target.GetType());
            sb.Append(Environment.NewLine);
            sb.Append("Parameters: ");
            sb.Append(parameters);
            sb.Append(Environment.NewLine);
            sb.Append("OldValues: ");
            sb.Append(oldValues);
            sb.Append(Environment.NewLine);
            sb.Append("NewValues: ");
            sb.Append(newValues);

            return sb.ToString();
        }

        /// <summary>
        /// Выполняет действие, обратное прямому действию.
        /// </summary>
        public void Revert()
        {
            foreach (var entry in this.oldValues)
                this.target.ChangeChangedValue(entry.Key, entry.Value);
        }

        #endregion
    }
}