using Kts.Gis.ViewModels;
using Kts.History;
using System;
using System.Text;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие изменения кастомного слоя.
    /// </summary>
    internal sealed partial class ChangeCustomLayerAction : IRevertibleAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Новый кастомный слой.
        /// </summary>
        private readonly CustomLayerViewModel newLayer;

        /// <summary>
        /// Кастомный объект, у которой меняется кастомный слой.
        /// </summary>
        private readonly ICustomLayerObject obj;

        /// <summary>
        /// Старый кастомный слой.
        /// </summary>
        private readonly CustomLayerViewModel oldLayer;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ChangeCustomLayerAction"/>.
        /// </summary>
        /// <param name="obj">Кастомный объект, у которой меняется кастомный слой.</param>
        /// <param name="oldLayer">Старый кастомный слой.</param>
        /// <param name="newLayer">Новый кастомный слой.</param>
        public ChangeCustomLayerAction(ICustomLayerObject obj, CustomLayerViewModel oldLayer, CustomLayerViewModel newLayer)
        {
            this.obj = obj;
            this.oldLayer = oldLayer;
            this.newLayer = newLayer;
        }

        #endregion
    }

    // Реализация IRevertibleAction.
    internal sealed partial class ChangeCustomLayerAction
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        public void Do()
        {
            this.obj.CustomLayer = this.newLayer;
        }

        /// <summary>
        /// Возвращает строковое представление действия.
        /// </summary>
        /// <returns>Строковое представление действия.</returns>
        public string GetString()
        {
            var sb = new StringBuilder();

            sb.Append("Action: ");
            sb.Append(nameof(ChangeCustomLayerAction));
            sb.Append(Environment.NewLine);
            sb.Append("CustomObjectType: ");
            sb.Append(this.obj.GetType());
            sb.Append(Environment.NewLine);
            sb.Append("Layer: ");
            sb.Append(this.oldLayer != null ? this.oldLayer.Id : Guid.Empty);
            sb.Append(" -> ");
            sb.Append(this.newLayer != null ? this.newLayer.Id : Guid.Empty);

            return sb.ToString();
        }

        /// <summary>
        /// Выполняет действие, обратное прямому действию.
        /// </summary>
        public void Revert()
        {
            this.obj.CustomLayer = this.oldLayer;
        }

        #endregion
    }
}