using Kts.Gis.ViewModels;
using Kts.History;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие перемещения объектов.
    /// </summary>
    internal sealed partial class MoveObjectsAction : IRevertibleAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Дельта.
        /// </summary>
        private readonly Point delta;

        /// <summary>
        /// Главная модель представления.
        /// </summary>
        private readonly MainViewModel mainViewModel;

        /// <summary>
        /// Объекты.
        /// </summary>
        private readonly List<IMapObjectViewModel> objects;

        /// <summary>
        /// Обратная дельта.
        /// </summary>
        private readonly Point revertDelta;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MoveObjectsAction"/>.
        /// </summary>
        /// <param name="objects">Объекты.</param>
        /// <param name="delta">Дельта.</param>
        /// <param name="viewModel">Главная модель представления.</param>
        public MoveObjectsAction(List<IMapObjectViewModel> objects, Point delta, MainViewModel viewModel)
        {
            this.objects = objects;
            this.delta = delta;
            this.mainViewModel = viewModel;

            this.revertDelta = new Point(-this.delta.X, -this.delta.Y);
        }

        #endregion
    }

    // Реализация IRevertibleAction.
    internal sealed partial class MoveObjectsAction
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        public void Do()
        {
            foreach (var obj in this.objects)
                obj.Shift(this.delta);

            this.mainViewModel.RedrawOutlines();
        }

        /// <summary>
        /// Возвращает строковое представление действия.
        /// </summary>
        /// <returns>Строковое представление действия.</returns>
        public string GetString()
        {
            var sb = new StringBuilder();

            sb.Append("Action: ");
            sb.Append(nameof(MoveObjectsAction));

            return sb.ToString();
        }

        /// <summary>
        /// Выполняет действие, обратное прямому действию.
        /// </summary>
        public void Revert()
        {
            foreach (var obj in this.objects)
                obj.Shift(this.revertDelta);

            this.mainViewModel.RedrawOutlines();
        }

        #endregion
    }
}