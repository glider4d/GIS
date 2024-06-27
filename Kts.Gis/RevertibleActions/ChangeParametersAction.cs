using Kts.Gis.Models;
using Kts.Gis.ViewModels;
using Kts.History;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие изменения значений параметра нескольких объектов.
    /// </summary>
    internal sealed partial class ChangeParametersAction : IRevertibleAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Слой объектов.
        /// </summary>
        private readonly LayerViewModel layer;

        /// <summary>
        /// Новое значение параметра.
        /// </summary>
        private readonly object newValue;

        /// <summary>
        /// Старые значения параметра объектов.
        /// </summary>
        private readonly Dictionary<IObjectViewModel, object> oldValues;

        /// <summary>
        /// Параметр.
        /// </summary>
        private readonly ParameterModel param;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ChangeParametersAction"/>.
        /// </summary>
        /// <param name="layer">Слой объектов.</param>
        /// <param name="param">Параметр.</param>
        /// <param name="newValue">Новое значение параметра.</param>
        /// <param name="oldValues">Старые значения параметра объектов.</param>
        public ChangeParametersAction(LayerViewModel layer, ParameterModel param, object newValue, Dictionary<IObjectViewModel, object> oldValues)
        {
            this.layer = layer;
            this.param = param;
            this.newValue = newValue;
            this.oldValues = oldValues;
        }

        #endregion
    }

    // Реализация IRevertibleAction.
    internal sealed partial class ChangeParametersAction
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        public void Do()
        {
            // Значение, указывающее на то, что следует ли обновить представление параметров.
            bool isRefreshNeeded = false;

            foreach (var obj in this.layer.Objects)
                if (this.oldValues.ContainsKey(obj))
                {
                    // Если в текущем слое содержится хотя бы один объект из заданного словаря, то это означает то, что нужно обновить представление параметров.
                    isRefreshNeeded = true;

                    break;
                }

            foreach (var entry in this.oldValues)
                (entry.Key as IParameterizedObjectViewModel).ChangeChangedValue(this.param, this.newValue);

            if (isRefreshNeeded && this.layer.IsSelected)
                this.layer.UpdateParameters();
        }

        /// <summary>
        /// Возвращает строковое представление действия.
        /// </summary>
        /// <returns>Строковое представление действия.</returns>
        public string GetString()
        {
            var sb = new StringBuilder();

            sb.Append("Action: ");
            sb.Append(nameof(ChangeParametersAction));
            sb.Append(Environment.NewLine);
            sb.Append("Layer: ");
            sb.Append(this.layer.Name);
            sb.Append(Environment.NewLine);
            sb.Append("Parameter: ");
            sb.Append(this.param.Id);
            sb.Append(Environment.NewLine);
            sb.Append("NewValue: ");
            sb.Append(this.newValue);

            return sb.ToString();
        }

        /// <summary>
        /// Выполняет действие, обратное прямому действию.
        /// </summary>
        public void Revert()
        {
            // Значение, указывающее на то, что следует ли обновить представление параметров.
            bool isRefreshNeeded = false;

            foreach (var obj in this.layer.Objects)
                if (this.oldValues.ContainsKey(obj))
                {
                    // Если в текущем слое содержится хотя бы один объект из заданного словаря, то это означает то, что нужно обновить представление параметров.
                    isRefreshNeeded = true;

                    break;
                }

            foreach (var entry in this.oldValues)
                (entry.Key as IParameterizedObjectViewModel).ChangeChangedValue(this.param, entry.Value);

            if (isRefreshNeeded && this.layer.IsSelected)
                this.layer.UpdateParameters();
        }

        #endregion
    }
}