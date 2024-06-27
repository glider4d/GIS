using Kts.Gis.Services;
using Kts.Gis.ViewModels;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет умную, независимую от других объектов надпись.
    /// </summary>
    internal sealed partial class SmartIndependentLabel : SmartLabel, IEditableObject
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис привязки представлений карты с моделями представлений.
        /// </summary>
        private readonly IMapBindingService mapBindingService;

        /// <summary>
        /// Модель представления.
        /// </summary>
        private readonly LabelViewModel viewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SmartIndependentLabel"/>.
        /// </summary>
        /// <param name="content">Содержимое надписи.</param>
        /// <param name="angle">Угол поворота надписи.</param>
        /// <param name="position">Положение надписи.</param>
        /// <param name="size">Размер надписи.</param>
        /// <param name="isBold">Значение, указывающее на то, что является ли шрифт надписи полужирным.</param>
        /// <param name="isItalic">Значение, указывающее на то, что является ли шрифт надписи курсивным.</param>
        /// <param name="isUnderline">Значение, указывающее на то, что является ли шрифт надписи подчеркнутым.</param>
        /// <param name="viewModel">Модель представления надписи.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        public SmartIndependentLabel(string content, double angle, Point position, int size, bool isBold, bool isItalic, bool isUnderline, LabelViewModel viewModel, IMapBindingService mapBindingService) : base(angle, position, size, mapBindingService.MapSettingService.IndependentLabelDefaultSize, false)
        {
            this.Text = content;
            this.IsBold = isBold;
            this.IsItalic = isItalic;
            this.IsUnderline = isUnderline;
            this.viewModel = viewModel;
            this.mapBindingService = mapBindingService;

            this.Loaded += this.SmartIndependentLabel_Loaded;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает начальный отступ положения надписи.
        /// </summary>
        public Point InitialOffset
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли шрифт надписи полужирным.
        /// </summary>
        public bool IsBold
        {
            get
            {
                return this.FontWeight == FontWeights.Bold;
            }
            set
            {
                this.FontWeight = value ? FontWeights.Bold : FontWeights.Normal;
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли шрифт надписи курсивным.
        /// </summary>
        public bool IsItalic
        {
            get
            {
                return this.FontStyle == FontStyles.Italic;
            }
            set
            {
                this.FontStyle = value ? FontStyles.Italic : FontStyles.Normal;
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли шрифт надписи подчеркнутым.
        /// </summary>
        public bool IsUnderline
        {
            get
            {
                return this.TextDecorations == System.Windows.TextDecorations.Underline;
            }
            set
            {
                this.TextDecorations = value ? System.Windows.TextDecorations.Underline : null;
            }
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="ContextMenu.Closed"/> контекстного меню.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            if (this.ContextMenu != null)
            {
                this.ContextMenu.Closed -= this.ContextMenu_Closed;

                this.ContextMenu = null;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="FrameworkElement.Loaded"/> надписи.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void SmartIndependentLabel_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= this.SmartIndependentLabel_Loaded;

            if (!this.viewModel.WasPlaced)
            {
                this.viewModel.WasPlaced = true;

                // Если надпись новая, то смещаем положение надписи, чтобы курсор мыши находился в центре надписи.
                var desiredSize = this.MeasureString(this.Text);
                this.InitialOffset = new Point(-desiredSize.Width / 2, -desiredSize.Height / 2);
                this.mapBindingService.SetMapObjectViewModelValue(this, nameof(this.InitialOffset), this.InitialOffset);
            }
        }
        
        #endregion

        #region Закрытые методы

        /// <summary>
        /// Высчитывает размер строки.
        /// </summary>
        /// <param name="candidate">Строка.</param>
        /// <returns>Размер строки.</returns>
        private Size MeasureString(string candidate)
        {
            var formattedText = new FormattedText(candidate, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new Typeface(this.FontFamily, this.FontStyle, this.FontWeight, this.FontStretch), this.FontSize, Brushes.Black);

            return new Size(formattedText.Width, formattedText.Height);
        }

        #endregion
    }

    // Реализация SmartLabel.
    internal sealed partial class SmartIndependentLabel
    {
        #region Открытые переопределенные свойства

        /// <summary>
        /// Возвращает размер надписи по умолчанию.
        /// </summary>
        public override int DefaultSize
        {
            get
            {
                return this.mapBindingService.MapSettingService.IndependentLabelDefaultSize;
            }
        }

        /// <summary>
        /// Возвращает максимальный размер надписи.
        /// </summary>
        public override int MaxSize
        {
            get
            {
#warning Это надо будет брать из настроек вида карты
                return this.DefaultSize * 10;
            }
        }

        /// <summary>
        /// Возвращает минимальный размер надписи.
        /// </summary>
        public override int MinSize
        {
            get
            {
#warning Это надо будет брать из настроек вида карты
                return this.DefaultSize / 10;
            }
        }

        /// <summary>
        /// Возвращает значение, которое будет отниматься/прибавляться к размеру надписи.
        /// </summary>
        public override int SizeDelta
        {
            get
            {
#warning Это надо будет брать из настроек вида карты
                return 5;
            }
        }

        #endregion

        #region Защищенные переопределенные методы

        /// <summary>
        /// Выполняется при требовании контекстного меню.
        /// </summary>
        protected override void OnContextMenuRequested()
        {
            (this.viewModel as IEditableObjectViewModel).IsEditing = true;

            this.ContextMenu = Application.Current.Resources["LabelEditContextMenu"] as ContextMenu;

            // Задаем контекст данных, предварительно обнуляя его.
            this.ContextMenu.DataContext = null;
            this.ContextMenu.DataContext = this.viewModel;

            this.ContextMenu.Closed += this.ContextMenu_Closed;

            this.ContextMenu.IsOpen = true;
        }

        /// <summary>
        /// Выполняется при нажатии левой кнопки мыши по надписи.
        /// </summary>
        protected override void OnMouseClick()
        {
            (this.viewModel as IEditableObjectViewModel).IsEditing = true;
        }

        /// <summary>
        /// Выполняется при двойном нажатии левой кнопки мыши по надписи.
        /// </summary>
        protected override void OnMouseDoubleClick()
        {
            var view = new LabelEditView(this.viewModel, this.GetSizes())
            {
                Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive)
            };

            view.ShowDialog();
        }

        /// <summary>
        /// Выполняет действия, связанные с изменением положения надписи.
        /// </summary>
        /// <param name="prevPosition">Предыдущее положение надписи.</param>
        protected override void OnPositionChanged(Point prevPosition)
        {
            if (Math.Abs(prevPosition.X - this.LeftTopCorner.X) >= 1 || Math.Abs(prevPosition.Y - this.LeftTopCorner.Y) >= 1)
                this.mapBindingService.SetMapObjectViewModelValue(this, nameof(this.LeftTopCorner), this.LeftTopCorner);
        }

        /// <summary>
        /// Выполняет действия, связанные с изменением размера надписи.
        /// </summary>
        protected override void OnSizeChanged()
        {
            this.mapBindingService.SetMapObjectViewModelValue(this, nameof(this.Size), this.Size);
        }

        #endregion

        #region Открытые переопределенные методы

        /// <summary>
        /// Выполняет действия, связанные с изменением угла поворота надписи.
        /// </summary>
        /// <param name="prevAngle">Предыдущий угол поворота надписи.</param>
        public override void OnAngleChanged(double prevAngle)
        {
            if (Math.Abs(this.Angle - prevAngle) > 1)
                this.mapBindingService.SetMapObjectViewModelValue(this, nameof(this.Angle), this.Angle);
        }

        /// <summary>
        /// Переопределяет положение надписи.
        /// </summary>
        /// <param name="positionDelta">Разница между текущим и предыдущим положением фигуры.</param>
        public override void Relocate(Point positionDelta)
        {
            // Ничего не делаем.
        }

        #endregion
    }

    // Реализация IEditableObject.
    internal sealed partial class SmartIndependentLabel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что редактируется ли объект.
        /// </summary>
        public bool IsEditing
        {
            get;
            set;
        }

        #endregion
    }
}