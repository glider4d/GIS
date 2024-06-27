using Kts.Gis.ViewModels;
using System.Windows.Controls;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет автоматическую легенду.
    /// </summary>
    internal sealed class AutoLegend : Control
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AutoLegend"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public AutoLegend(AutoLegendViewModel viewModel)
        {
            this.DataContext = viewModel;
        }

        #endregion
    }
}