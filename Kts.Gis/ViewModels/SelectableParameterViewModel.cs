using Kts.Gis.Models;
using Kts.Utilities;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления выбираемого параметра.
    /// </summary>
    internal sealed class SelectableParameterViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что выбран ли параметр.
        /// </summary>
        private bool isSelected;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SelectableParameterViewModel"/>.
        /// </summary>
        /// <param name="param">Параметр.</param>
        /// <param name="isSelected">Значение, указывающее на то, что выбран ли параметр.</param>
        public SelectableParameterViewModel(ParameterModel param, bool isSelected = true)
        {
            this.Parameter = param;
            this.IsSelected = isSelected;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что выбран ли параметр.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                this.isSelected = value;

                this.NotifyPropertyChanged(nameof(this.IsSelected));
            }
        }

        /// <summary>
        /// Возвращает название параметра.
        /// </summary>
        public string Name
        {
            get
            {
                return this.Parameter.Name;
            }
        }

        /// <summary>
        /// Возвращает параметр.
        /// </summary>
        public ParameterModel Parameter
        {
            get;
        }

        #endregion
    }
}