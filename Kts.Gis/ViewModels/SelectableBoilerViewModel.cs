using Kts.Utilities;
using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления выбираемой котельной.
    /// </summary>
    internal sealed class SelectableBoilerViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что выбрана ли котельная.
        /// </summary>
        private bool isSelected;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SelectableBoilerViewModel"/>.
        /// </summary>
        /// <param name="id">Идентификатор котельной.</param>
        /// <param name="name">Название котельной.</param>
        public SelectableBoilerViewModel(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает идентификатор котельной.
        /// </summary>
        public Guid Id
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что выбрана ли котельная.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                if (this.IsSelected != value)
                {
                    this.isSelected = value;

                    this.NotifyPropertyChanged(nameof(this.IsSelected));
                }
            }
        }

        /// <summary>
        /// Возвращает название котельной.
        /// </summary>
        public string Name
        {
            get;
        }

        #endregion
    }
}