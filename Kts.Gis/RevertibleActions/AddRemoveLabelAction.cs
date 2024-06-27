using Kts.Gis.ViewModels;
using System;
using System.Text;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие добавления/удаления надписи.
    /// </summary>
    internal sealed partial class AddRemoveLabelAction : AddRemoveObjectAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Добавляемая/удаляемая надпись.
        /// </summary>
        private readonly LabelViewModel label;

        /// <summary>
        /// Главная модель представления.
        /// </summary>
        private readonly MainViewModel mainViewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AddRemoveLabelAction"/>.
        /// </summary>
        /// <param name="label">Добавляемая/удаляемая надпись.</param>
        /// <param name="mainViewModel">Главная модель представления.</param>
        /// <param name="isAdding">Значение, указывающее на то, что выполняется ли добавление.</param>
        public AddRemoveLabelAction(LabelViewModel label, MainViewModel mainViewModel, bool isAdding) : base(isAdding)
        {
            this.label = label;
            this.mainViewModel = mainViewModel;
        }

        #endregion
    }

    // Реализация AddRemoveObjectAction.
    internal sealed partial class AddRemoveLabelAction
    {
        #region Защищенные переопределенные методы

        /// <summary>
        /// Добавляет объект.
        /// </summary>
        protected override void Add()
        {
            // Убираем со всего выбор.
            this.mainViewModel.DeselectAll();

            // Очищаем группу выбранных объектов.
            this.mainViewModel.ClearSelectedGroup();

            this.label.RegisterBinding();

            this.label.RestoreLayer(this.mainViewModel.CustomLayersViewModel.Layers);

            this.mainViewModel.AddLabel(this.label);
        }

        /// <summary>
        /// Удаляет объект.
        /// </summary>
        protected override void Remove()
        {
            // Убираем со всего выбор.
            this.mainViewModel.DeselectAll();

            // Очищаем группу выбранных объектов.
            this.mainViewModel.ClearSelectedGroup();

            this.label.IsPlaced = false;

            this.label.UnregisterBinding();

            this.mainViewModel.RemoveLabel(this.label);
        }

        #endregion

        #region Открытые переопределенные методы

        /// <summary>
        /// Возвращает строковое представление действия.
        /// </summary>
        /// <returns>Строковое представление действия.</returns>
        public override string GetString()
        {
            var sb = new StringBuilder();

            sb.Append("Action: ");
            sb.Append(nameof(AddRemoveLabelAction));
            sb.Append(Environment.NewLine);
            sb.Append("Label: ");
            sb.Append(this.label.Id);

            return sb.ToString();
        }

        #endregion
    }
}