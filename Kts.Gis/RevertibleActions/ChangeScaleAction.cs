using Kts.Gis.Models;
using Kts.Gis.ViewModels;
using Kts.History;
using System;
using System.Text;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие изменения масштаба линий.
    /// </summary>
    internal sealed partial class ChangeScaleAction : IRevertibleAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Тип линий.
        /// </summary>
        private readonly ObjectType lineType;

        /// <summary>
        /// Главная модель представления.
        /// </summary>
        private readonly MainViewModel mainViewModel;

        /// <summary>
        /// Предыдущий масштаб линий.
        /// </summary>
        private readonly double oldScale;

        /// <summary>
        /// Опция обновления длин линий.
        /// </summary>
        private readonly LengthUpdateOption option;

        /// <summary>
        /// Масштаб линий.
        /// </summary>
        private readonly double scale;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ChangeScaleAction"/>.
        /// </summary>
        /// <param name="oldScale">Предыдущий масштаб линий.</param>
        /// <param name="scale">Масштаб линий.</param>
        /// <param name="option">Опция обновления длин линий.</param>
        /// <param name="mainViewModel">Главная модель представления.</param>
        /// <param name="lineType">Тип линий, для которых нужно изменить масштаб. null, если для всех.</param>
        public ChangeScaleAction(double oldScale, double scale, LengthUpdateOption option, MainViewModel mainViewModel, ObjectType lineType)
        {
            this.oldScale = oldScale;
            this.scale = scale;
            this.option = option;
            this.mainViewModel = mainViewModel;
            this.lineType = lineType;
        }

        #endregion
    }

    // Реализация IRevertibleAction.
    internal sealed partial class ChangeScaleAction
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        public void Do()
        {
            this.mainViewModel.MapViewModel.Scale = this.scale;
            this.mainViewModel.MapViewModel.ScaleLineType = this.lineType;
            this.mainViewModel.MapViewModel.ScaleMode = this.option;

            this.mainViewModel.RecalculateLength();
        }

        /// <summary>
        /// Возвращает строковое представление действия.
        /// </summary>
        /// <returns>Строковое представление действия.</returns>
        public string GetString()
        {
            var sb = new StringBuilder();

            sb.Append("Action: ");
            sb.Append(nameof(ChangeScaleAction));
            sb.Append(Environment.NewLine);
            sb.Append("LengthUpdateOption: ");
            sb.Append(this.option);
            sb.Append(Environment.NewLine);
            sb.Append("Scale: ");
            sb.Append(this.oldScale);
            sb.Append(" -> ");
            sb.Append(this.scale);

            return sb.ToString();
        }

        /// <summary>
        /// Выполняет действие, обратное прямому действию.
        /// </summary>
        public void Revert()
        {
            this.mainViewModel.MapViewModel.Scale = this.oldScale;
            this.mainViewModel.MapViewModel.ScaleLineType = this.lineType;
            this.mainViewModel.MapViewModel.ScaleMode = this.option;

            this.mainViewModel.RecalculateLength();
        }

        #endregion
    }
}