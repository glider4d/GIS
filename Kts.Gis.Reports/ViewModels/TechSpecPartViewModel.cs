using Kts.Utilities;
using System.Collections.Generic;

namespace Kts.Gis.Reports.ViewModels
{
    /// <summary>
    /// Представляет модель представления части отчета о технических характеристиках.
    /// </summary>
    public sealed class TechSpecPartViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что выбрана ли часть отчета о технических характеристиках.
        /// </summary>
        private bool isSelected;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TechSpecPartViewModel"/>.
        /// </summary>
        /// <param name="name">Название части отчета о технических характеристиках.</param>
        /// <param name="id">Идентификаторы частей отчета о технических характеристиках.</param>
        public TechSpecPartViewModel(string name, List<int> ids)
        {
            this.Name = name;
            this.Ids = ids;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает идентификаторы частей отчета о технических характеристиках.
        /// </summary>
        public List<int> Ids
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что выбрана ли часть отчета о технических характеристиках.
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
        /// Возвращает название части отчета о технических характеристиках.
        /// </summary>
        public string Name
        {
            get;
        }

        #endregion
    }
}