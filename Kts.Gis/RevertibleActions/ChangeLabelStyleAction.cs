using Kts.Gis.ViewModels;
using Kts.History;
using System;
using System.Text;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие изменения стиля надписи.
    /// </summary>
    internal sealed partial class ChangeLabelStyleAction : IRevertibleAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Угол поворота надписи.
        /// </summary>
        private readonly double angle;

        /// <summary>
        /// Содержимое надписи.
        /// </summary>
        private readonly string content;

        /// <summary>
        /// Значение, указывающее на то, что является ли шрифт надписи полужирным.
        /// </summary>
        private readonly bool isBold;

        /// <summary>
        /// Значение, указывающее на то, что является ли шрифт надписи курсивным.
        /// </summary>
        private readonly bool isItalic;

        /// <summary>
        /// Значение, указывающее на то, что является ли шрифт надписи подчеркнутым.
        /// </summary>
        private readonly bool isUnderline;

        /// <summary>
        /// Надпись.
        /// </summary>
        private readonly LabelViewModel label;

        /// <summary>
        /// Старый угол поворота надписи.
        /// </summary>
        private readonly double oldAngle;

        /// <summary>
        /// Старое содержимое надписи.
        /// </summary>
        private readonly string oldContent;

        /// <summary>
        /// Старое значение, указывающее на то, что является ли шрифт надписи полужирным.
        /// </summary>
        private readonly bool oldIsBold;

        /// <summary>
        /// Старое значение, указывающее на то, что является ли шрифт надписи курсивным.
        /// </summary>
        private readonly bool oldIsItalic;

        /// <summary>
        /// Старое значение, указывающее на то, что является ли шрифт надписи подчеркнутым.
        /// </summary>
        private readonly bool oldIsUnderline;

        /// <summary>
        /// Старый размер надписи.
        /// </summary>
        private readonly int oldSize;

        /// <summary>
        /// Размер надписи.
        /// </summary>
        private readonly int size;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ChangeLabelStyleAction"/>.
        /// </summary>
        /// <param name="label">Надпись.</param>
        /// <param name="content">Содержимое надписи.</param>
        /// <param name="size">Размер надписи.</param>
        /// <param name="isBold">Значение, указывающее на то, что является ли шрифт надписи полужирным.</param>
        /// <param name="isItalic">Значение, указывающее на то, что является ли шрифт надписи курсивным.</param>
        /// <param name="isUnderline">Значение, указывающее на то, что является ли шрифт надписи подчеркнутым.</param>
        /// <param name="angle">Угол поворота надписи.</param>
        public ChangeLabelStyleAction(LabelViewModel label, string content, int size, bool isBold, bool isItalic, bool isUnderline, double angle)
        {
            this.label = label;
            this.content = content;
            this.size = size;
            this.isBold = isBold;
            this.isItalic = isItalic;
            this.isUnderline = isUnderline;
            this.angle = angle;

            // Получаем старые значения свойств надписи.
            this.oldContent = label.Content;
            this.oldSize = label.Size;
            this.oldIsBold = label.IsBold;
            this.oldIsItalic = label.IsItalic;
            this.oldIsUnderline = label.IsUnderline;
            this.oldAngle = label.Angle;
        }

        #endregion
    }

    // Реализация IRevertibleAction.
    internal sealed partial class ChangeLabelStyleAction
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        public void Do()
        {
            this.label.SetValue(nameof(LabelViewModel.Content), this.content);
            this.label.SetValue(nameof(LabelViewModel.Size), this.size);

            this.label.IsBold = this.isBold;
            this.label.IsItalic = this.isItalic;
            this.label.IsUnderline = this.isUnderline;

            if (this.label.Angle != this.angle)
                this.label.SetValue(nameof(LabelViewModel.Angle), this.angle);

            this.label.IsModified = true;
        }

        /// <summary>
        /// Возвращает строковое представление действия.
        /// </summary>
        /// <returns>Строковое представление действия.</returns>
        public string GetString()
        {
            var sb = new StringBuilder();

            sb.Append("Action: ");
            sb.Append(nameof(ChangeLabelStyleAction));
            sb.Append(Environment.NewLine);
            sb.Append("Label: ");
            sb.Append(this.label.Id);
            sb.Append(Environment.NewLine);
            sb.Append("Content: ");
            sb.Append(this.oldContent);
            sb.Append(" -> ");
            sb.Append(this.content);
            sb.Append(Environment.NewLine);
            sb.Append("Size: ");
            sb.Append(this.oldSize);
            sb.Append(" -> ");
            sb.Append(this.size);
            sb.Append(Environment.NewLine);
            sb.Append("IsBold: ");
            sb.Append(this.oldIsBold);
            sb.Append(" -> ");
            sb.Append(this.isBold);
            sb.Append(Environment.NewLine);
            sb.Append("IsItalic: ");
            sb.Append(this.oldIsItalic);
            sb.Append(" -> ");
            sb.Append(this.isItalic);
            sb.Append(Environment.NewLine);
            sb.Append("IsUnderline: ");
            sb.Append(this.oldIsUnderline);
            sb.Append(" -> ");
            sb.Append(this.isUnderline);
            sb.Append(Environment.NewLine);
            sb.Append("Angle: ");
            sb.Append(this.oldAngle);
            sb.Append(" -> ");
            sb.Append(this.angle);

            return sb.ToString();
        }

        /// <summary>
        /// Выполняет действие, обратное прямому действию.
        /// </summary>
        public void Revert()
        {
            this.label.SetValue(nameof(LabelViewModel.Content), this.oldContent);
            this.label.SetValue(nameof(LabelViewModel.Size), this.oldSize);

            this.label.IsBold = this.oldIsBold;
            this.label.IsItalic = this.oldIsItalic;
            this.label.IsUnderline = this.oldIsUnderline;

            if (this.label.Angle != this.oldAngle)
                this.label.SetValue(nameof(LabelViewModel.Angle), this.oldAngle);

            this.label.IsModified = true;
        }

        #endregion
    }
}