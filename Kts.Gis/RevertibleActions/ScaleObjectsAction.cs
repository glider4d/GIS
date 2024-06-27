using Kts.Gis.ViewModels;
using Kts.History;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие масштабирования объектов.
    /// </summary>
    internal sealed partial class ScaleObjectsAction : IRevertibleAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Главная модель представления.
        /// </summary>
        private readonly MainViewModel mainViewModel;

        /// <summary>
        /// Объекты.
        /// </summary>
        private readonly List<IMapObjectViewModel> objects;

        /// <summary>
        /// Точка, относительно которой нужно смасштабировать объекты.
        /// </summary>
        private readonly Point origin;

        /// <summary>
        /// Масштаб.
        /// </summary>
        private readonly double scale;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ScaleObjectsAction"/>.
        /// </summary>
        /// <param name="objects">Объекты.</param>
        /// <param name="scale">Масштаб.</param>
        /// <param name="origin">Точка, относительно которой нужно смасштабировать объекты.</param>
        /// <param name="viewModel">Главная модель представления.</param>
        public ScaleObjectsAction(List<IMapObjectViewModel> objects, double scale, Point origin, MainViewModel viewModel)
        {
            this.objects = objects;
            this.scale = scale;
            this.origin = origin;
            this.mainViewModel = viewModel;
        }

        #endregion
    }

    // Реализация IRevertibleAction.
    internal sealed partial class ScaleObjectsAction
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        public void Do()
        {
            foreach (var obj in this.objects)
                obj.Scale(this.scale, this.origin);

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
            sb.Append(nameof(ScaleObjectsAction));

            return sb.ToString();
        }

        /// <summary>
        /// Выполняет действие, обратное прямому действию.
        /// </summary>
        public void Revert()
        {
            foreach (var obj in this.objects)
                obj.Scale(1 / this.scale, this.origin);

            this.mainViewModel.RedrawOutlines();
        }

        #endregion
    }
}