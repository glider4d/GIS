using Kts.Gis.ViewModels;
using System;
using System.Linq;
using System.Text;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие добавления/удаления фигуры.
    /// </summary>
    internal sealed partial class AddRemoveFigureAction : AddRemoveObjectAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Добавляемая/удаляемая фигура.
        /// </summary>
        private readonly FigureViewModel figure;

        /// <summary>
        /// Главная модель представления.
        /// </summary>
        private readonly MainViewModel mainViewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AddRemoveFigureAction"/>.
        /// </summary>
        /// <param name="figure">Добавляемая/удаляемая фигура.</param>
        /// <param name="mainViewModel">Главная модель представления.</param>
        /// <param name="isAdding">Значение, указывающее на то, что выполняется ли добавление.</param>
        public AddRemoveFigureAction(FigureViewModel figure, MainViewModel mainViewModel, bool isAdding) : base(isAdding)
        {
            this.figure = figure;
            this.mainViewModel = mainViewModel;
        }

        #endregion
    }

    // Реализация AddRemoveObjectAction.
    internal sealed partial class AddRemoveFigureAction
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

            if (this.mainViewModel.NotPlacedObjectsGroup.Contains(this.figure))
                // Если в группе неразмещенных объектов имеется добавляемый объект, то удаляем его из нее.
                this.mainViewModel.NotPlacedObjectsGroup.Layers.First(x => x.Type == this.figure.Type).Remove(this.figure);

            this.mainViewModel.AddObject(this.figure);
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

            this.mainViewModel.RemoveObject(this.figure);

            if (this.figure.IsSaved)
                // Если у объекта есть идентификатор, то это означает то, что он существует в источнике данных, поэтому его следует добавить в группу неразмещенных объектов.
                this.mainViewModel.NotPlacedObjectsGroup.Layers.First(x => x.Type == this.figure.Type).Add(this.figure);
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
            sb.Append("Figure: ");
            sb.Append(this.figure.Id);

            return sb.ToString();
        }

        #endregion
    }
}