using Kts.Gis.ViewModels;
using Kts.History;
using System;
using System.Text;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие добавления/удаления кастомного слоя.
    /// </summary>
    internal sealed partial class AddRemoveCustomLayerAction : IRevertibleAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Кастомный слой.
        /// </summary>
        private readonly CustomLayerViewModel customLayer;

        /// <summary>
        /// Кастомные слои.
        /// </summary>
        private readonly CustomLayersViewModel customLayers;

        /// <summary>
        /// Значение, указывающее на то, что выполняется ли добавление.
        /// </summary>
        private readonly bool isAdding;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AddRemoveCustomLayerAction"/>.
        /// </summary>
        /// <param name="customLayer">Кастомный слой.</param>
        /// <param name="customLayers">Кастомные слои.</param>
        /// <param name="isAdding">Значение, указывающее на то, что выполняется ли добавление.</param>
        public AddRemoveCustomLayerAction(CustomLayerViewModel customLayer, CustomLayersViewModel customLayers, bool isAdding)
        {
            this.customLayer = customLayer;
            this.customLayers = customLayers;
            this.isAdding = isAdding;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Добавляет кастомный слой.
        /// </summary>
        private void Add()
        {
            this.customLayers.DeletedLayers.Remove(this.customLayer);

            this.customLayers.Layers.Add(this.customLayer);

            this.customLayers.RefreshLayers();
        }

        /// <summary>
        /// Удаляет кастомный слой.
        /// </summary>
        private void Remove()
        {
            if (this.customLayer.IsVisible)
                this.customLayer.IsVisible = false;

            this.customLayers.DeletedLayers.Add(this.customLayer);

            this.customLayers.Layers.Remove(this.customLayer);

            this.customLayers.RefreshLayers();
        }

        #endregion
    }

    // Реализация IRevertibleAction.
    internal sealed partial class AddRemoveCustomLayerAction
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        public void Do()
        {
            if (this.isAdding)
                this.Add();
            else
                this.Remove();
        }

        /// <summary>
        /// Возвращает строковое представление действия.
        /// </summary>
        /// <returns>Строковое представление действия.</returns>
        public string GetString()
        {
            var sb = new StringBuilder();

            sb.Append("Action: ");
            sb.Append(nameof(AddRemoveFigureAction));
            sb.Append(Environment.NewLine);
            sb.Append("CustomLayerId: ");
            sb.Append(this.customLayer.Id);
            sb.Append(Environment.NewLine);
            sb.Append("CustomLayerName: ");
            sb.Append(this.customLayer.Name);

            return sb.ToString();
        }

        /// <summary>
        /// Выполняет действие, обратное прямому действию.
        /// </summary>
        public void Revert()
        {
            if (this.isAdding)
                this.Remove();
            else
                this.Add();
        }

        #endregion
    }
}