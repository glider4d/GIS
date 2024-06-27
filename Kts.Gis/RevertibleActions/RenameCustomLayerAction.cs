using Kts.Gis.ViewModels;
using Kts.History;
using System;
using System.Text;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие переименования кастомного слоя.
    /// </summary>
    internal sealed partial class RenameCustomLayerAction : IRevertibleAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Кастомный слой.
        /// </summary>
        private readonly CustomLayerViewModel layer;

        /// <summary>
        /// Новое название.
        /// </summary>
        private readonly string newName;

        /// <summary>
        /// Старое название.
        /// </summary>
        private readonly string oldName;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RenameCustomLayerAction"/>.
        /// </summary>
        /// <param name="layer">Кастомный слой.</param>
        /// <param name="oldValue">Старое название.</param>
        /// <param name="newValue">Новое название.</param>
        public RenameCustomLayerAction(CustomLayerViewModel layer, string oldName, string newName)
        {
            this.layer = layer;
            this.oldName = oldName;
            this.newName = newName;
        }

        #endregion
    }

    // Реализация IRevertibleAction.
    internal sealed partial class RenameCustomLayerAction
    {
        #region Открытые виртуальные методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        public void Do()
        {
            this.layer.Name = this.newName;
        }

        /// <summary>
        /// Возвращает строковое представление действия.
        /// </summary>
        /// <returns>Строковое представление действия.</returns>
        public string GetString()
        {
            var sb = new StringBuilder();

            sb.Append("Action: ");
            sb.Append(nameof(RenameCustomLayerAction));
            sb.Append(Environment.NewLine);
            sb.Append("Layer: ");
            sb.Append(this.layer.Id);
            sb.Append(Environment.NewLine);
            sb.Append("Name: ");
            sb.Append(this.oldName);
            sb.Append(" -> ");
            sb.Append(this.newName);

            return sb.ToString();
        }

        /// <summary>
        /// Выполняет действие, обратное прямому действию.
        /// </summary>
        public void Revert()
        {
            this.layer.Name = this.oldName;
        }

        #endregion
    }
}