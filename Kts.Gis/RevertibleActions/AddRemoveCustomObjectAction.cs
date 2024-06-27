using Kts.Gis.ViewModels;
using System;
using System.Text;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие добавления/удаления кастомного объекта.
    /// </summary>
    internal sealed partial class AddRemoveCustomObjectAction : AddRemoveObjectAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Добавляемый/удаляемый объект.
        /// </summary>
        private readonly ICustomLayerObject customLayerObject;

        /// <summary>
        /// Главная модель представления.
        /// </summary>
        private readonly MainViewModel mainViewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AddRemoveCustomObjectAction"/>.
        /// </summary>
        /// <param name="label">Добавляемый/удаляемый объект.</param>
        /// <param name="mainViewModel">Главная модель представления.</param>
        /// <param name="isAdding">Значение, указывающее на то, что выполняется ли добавление.</param>
        public AddRemoveCustomObjectAction(ICustomLayerObject obj, MainViewModel mainViewModel, bool isAdding) : base(isAdding)
        {
            this.customLayerObject = obj;
            this.mainViewModel = mainViewModel;
        }

        #endregion
    }

    // Реализация AddRemoveObjectAction.
    internal sealed partial class AddRemoveCustomObjectAction
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

            this.customLayerObject.RegisterBinding();

            this.customLayerObject.RestoreLayer(this.mainViewModel.CustomLayersViewModel.Layers);

            this.mainViewModel.AddCustomLayerObject(this.customLayerObject);
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

            this.customLayerObject.IsPlaced = false;

            this.customLayerObject.UnregisterBinding();

            this.mainViewModel.RemoveCustomLayerObject(this.customLayerObject);
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
            sb.Append(nameof(AddRemoveCustomObjectAction));
            sb.Append(Environment.NewLine);
            sb.Append("CustomObjectType: ");
            sb.Append(this.customLayerObject.GetType());

            return sb.ToString();
        }

        #endregion
    }
}