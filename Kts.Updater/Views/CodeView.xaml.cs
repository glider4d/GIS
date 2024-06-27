using Kts.Updater.ViewModels;
using System.Windows;

namespace Kts.Updater.Views
{
    /// <summary>
    /// Представляет представление редактирования кода.
    /// </summary>
    internal sealed partial class CodeView : Window
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CodeView"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public CodeView(CodeEditorViewModel viewModel)
        {
            this.InitializeComponent();

            this.DataContext = viewModel;
        }

        #endregion
    }
}