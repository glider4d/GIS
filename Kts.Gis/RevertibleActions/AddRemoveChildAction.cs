using Kts.Gis.ViewModels;
using System;
using System.Linq;
using System.Text;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие добавления/удаления потомка из его родительского объекта.
    /// </summary>
    internal sealed partial class AddRemoveChildAction : AddRemoveObjectAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Объект-потомок.
        /// </summary>
        private readonly IObjectViewModel child;

        /// <summary>
        /// Объект-родитель.
        /// </summary>
        private readonly ContainerObjectViewModel container;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AddRemoveChildAction"/>.
        /// </summary>
        /// <param name="container">Объект-родитель.</param>
        /// <param name="child">Объект-потомок.</param>
        /// <param name="isAdding">Значение, указывающее на то, что выполняется ли добавление.</param>
        public AddRemoveChildAction(ContainerObjectViewModel container, IObjectViewModel child, bool isAdding) : base(isAdding)
        {
            this.container = container;
            this.child = child;
        }

        #endregion
    }

    // Реализация AddRemoveObjectAction.
    internal sealed partial class AddRemoveChildAction
    {
        #region Защищенные переопределенные методы

        /// <summary>
        /// Добавляет объект.
        /// </summary>
        protected override void Add()
        {
            if (!this.container.HasChildrenLayer(this.child.Type))
                this.container.AddChildrenLayer(this.child.Type);

            this.container.ChildrenLayers.First(x => x.Type == this.child.Type).Add(this.child);

            this.child.IsInitialized = true;

            // Если объект является объектом карты, то отображаем его.
            var mapObject = this.child as IMapObjectViewModel;
            if (mapObject != null)
                mapObject.IsPlaced = true;

            (this.child as ISelectableObjectViewModel).IsSelected = true;
        }

        /// <summary>
        /// Удаляет объект.
        /// </summary>
        protected override void Remove()
        {
            this.child.IsInitialized = true;

            // Если объект является объектом карты, то скрываем его.
            var mapObject = this.child as IMapObjectViewModel;
            if (mapObject != null)
                mapObject.IsPlaced = false;

            var layer = this.container.ChildrenLayers.First(x => x.Type == this.child.Type);

            layer.Remove(this.child);

            if (layer.ObjectCount == 0)
                this.container.RemoveChildrenLayer(this.child.Type);
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
            sb.Append(nameof(AddRemoveFigureAction));
            sb.Append(Environment.NewLine);
            sb.Append("Container: ");
            sb.Append(this.container.ObjectId);
            sb.Append(Environment.NewLine);
            sb.Append("Child: ");
            sb.Append(this.child.Id);

            return sb.ToString();
        }

        #endregion
    }
}