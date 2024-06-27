using System.Windows;
using System.Windows.Controls.Primitives;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет кнопку уменьшения/увеличения размера надписи.
    /// </summary>
    internal sealed class LabelResizeButton : RepeatButton
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Значение, указывающее на то, что увеличивает ли кнопка размер надписи.
        /// </summary>
        private readonly bool isUp;

        /// <summary>
        /// Надпись.
        /// </summary>
        private readonly SmartLabel label;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LabelResizeButton"/>.
        /// </summary>
        /// <param name="label">Надпись.</param>
        /// <param name="isUp">Значение, указывающее на то, что увеличивает ли кнопка размер надписи.</param>
        /// <param name="size">Размер надписи.</param>
        public LabelResizeButton(SmartLabel label, bool isUp, double size)
        {
            this.label = label;
            this.isUp = isUp;

            this.Height = size;
            this.Width = size;

            if (isUp)
            {
                this.Margin = new Thickness(-size * 2, -size, 0, 0);

                this.Tag = "+";
            }
            else
            {
                this.Margin = new Thickness(-size * 2, size, 0, 0);

                this.Tag = "-";
            }

            this.Delay = 500;
            this.Interval = 100;
        }

        #endregion

        #region Защищенные переопределенные методы

        /// <summary>
        /// Выполняет действия, связанные с нажатием по надписи.
        /// </summary>
        protected override void OnClick()
        {
            if (this.isUp)
                this.label.IncreaseFontSize();
            else
                this.label.DecreaseFontSize();

            this.label.Relocate(new Point(0, 0));
        }

        #endregion
    }
}