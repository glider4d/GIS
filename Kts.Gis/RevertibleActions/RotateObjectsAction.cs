using Kts.Gis.ViewModels;
using Kts.History;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие поворота объектов.
    /// </summary>
    internal sealed partial class RotateObjectsAction : IRevertibleAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Угол поворота.
        /// </summary>
        private readonly double angle;

        /// <summary>
        /// Главная модель представления.
        /// </summary>
        private readonly MainViewModel mainViewModel;

        /// <summary>
        /// Объекты.
        /// </summary>
        private readonly List<IMapObjectViewModel> objects;

        /// <summary>
        /// Точка, относительно которой нужно повернуть объекты.
        /// </summary>
        private readonly Point origin;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RotateObjectsAction"/>.
        /// </summary>
        /// <param name="objects">Объекты.</param>
        /// <param name="angle">Угол поворота.</param>
        /// <param name="origin">Точка, относительно которой нужно повернуть объекты.</param>
        /// <param name="viewModel">Главная модель представления.</param>
        public RotateObjectsAction(List<IMapObjectViewModel> objects, double angle, Point origin, MainViewModel viewModel)
        {
            this.objects = objects;
            this.angle = angle;
            this.origin = origin;
            this.mainViewModel = viewModel;
        }

        #endregion
    }

    // Реализация IRevertibleAction.
    internal sealed partial class RotateObjectsAction
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        public void Do()
        {
            foreach (var obj in this.objects)
                obj.Rotate(this.angle, this.origin);

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
            sb.Append(nameof(RotateObjectsAction));

            return sb.ToString();
        }

        /// <summary>
        /// Выполняет действие, обратное прямому действию.
        /// </summary>
        public void Revert()
        {
            foreach (var obj in this.objects)
                obj.Rotate(-this.angle, this.origin);

            this.mainViewModel.RedrawOutlines();
        }

        #endregion
    }
}