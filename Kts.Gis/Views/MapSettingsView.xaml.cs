using Kts.Gis.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представление настроек карты.
    /// </summary>
    internal sealed partial class MapSettingsView : Window
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Модель представления.
        /// </summary>
        private readonly MapSettingsViewModel viewModel;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MapSettingsView"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public MapSettingsView(MapSettingsViewModel viewModel)
        {
            this.InitializeComponent();

            this.DataContext = viewModel;

            this.viewModel = viewModel;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="ButtonBase.Click"/> кнопки "ОК".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragCompleted"/> слайдера.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void FigureLabelDefaultSizeSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            this.viewModel.FigureLabelDefaultSize = Convert.ToInt32((sender as Slider).Value);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragCompleted"/> слайдера.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void FigurePlanningOffsetSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            this.viewModel.FigurePlanningOffset = (sender as Slider).Value;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragCompleted"/> слайдера.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void FigureThicknessSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            this.viewModel.FigureThickness = (sender as Slider).Value;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragCompleted"/> слайдера.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void IndependentLabelDefaultSizeSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            this.viewModel.IndependentLabelDefaultSize = Convert.ToInt32((sender as Slider).Value);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragCompleted"/> слайдера.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void LineDisabledOffsetSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            this.viewModel.LineDisabledOffset = (sender as Slider).Value;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragCompleted"/> слайдера.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void LineLabelDefaultSizeSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            this.viewModel.LineLabelDefaultSize = Convert.ToInt32((sender as Slider).Value);
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragCompleted"/> слайдера.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void LinePlanningOffsetSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            this.viewModel.LinePlanningOffset = (sender as Slider).Value;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="Thumb.DragCompleted"/> слайдера.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void LineThicknessSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            this.viewModel.LineThickness = (sender as Slider).Value;
        }

        #endregion
    }
}