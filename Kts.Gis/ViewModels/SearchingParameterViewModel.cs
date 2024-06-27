using Kts.Gis.Models;
using Kts.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления параметра, используемого при поиске.
    /// </summary>
    internal class SearchingParameterViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что выбран ли данный параметр.
        /// </summary>
        private bool isSelected;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Параметр.
        /// </summary>
        private readonly ParameterModel parameter;

        /// <summary>
        /// Тип объекта, которому принадлежит параметр.
        /// </summary>
        private readonly ObjectType type;

        #endregion

        #region Закрытые статические поля

        /// <summary>
        /// Список булевых значений.
        /// </summary>
        private static List<TableEntryModel> boolList = new List<TableEntryModel>()
        {
            new TableEntryModel(0, "Нет"),
            new TableEntryModel(1, "Да")
        };

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SearchingParameterViewModel"/>.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="type">Тип объекта, которому принадлежит параметр.</param>
        public SearchingParameterViewModel(ParameterModel parameter, ObjectType type)
        {
            this.parameter = parameter;
            this.type = type;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает дочерние параметры параметра.
        /// </summary>
        public List<SearchingParameterViewModel> Children
        {
            get;
        } = new List<SearchingParameterViewModel>();

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеет ли параметр предопределенные значения.
        /// </summary>
        public bool HasPredefinedValues
        {
            get
            {
                return this.parameter.HasPredefinedValues || this.parameter.Format.IsBoolean;
            }
        }

        /// <summary>
        /// Возвращает идентификатор параметра.
        /// </summary>
        public int Id
        {
            get
            {
                return this.parameter.Id;
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли значение параметра датой.
        /// </summary>
        public bool IsDate
        {
            get
            {
                return this.parameter.Format.IsShortDateTime;
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли значение параметра числовым.
        /// </summary>
        public bool IsNumeric
        {
            get
            {
                return this.parameter.Format.IsNumeric;
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли искать по данному параметру.
        /// </summary>
        public bool IsSearchable
        {
            get
            {
                if (this.parameter.Format.IsCalculate && !this.parameter.Format.IsGroup)
                    return true;

                return !this.type.IsParameterReadonly(this.parameter);
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что выбран ли данный параметр.
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
        /// Возвращает название параметра.
        /// </summary>
        public string Name
        {
            get
            {
                return this.parameter.Name;
            }
        }

        /// <summary>
        /// Возвращает предопределенные значения.
        /// </summary>
        public List<TableEntryModel> PredefinedValues
        {
            get
            {
                if (!this.HasPredefinedValues)
                    return null;

                if (this.parameter.Format.IsBoolean)
                    return boolList;
                    
                return this.parameter.GetAllPredefinedValues().OrderBy(x => x.Value, new NaturalSortComparer<string>()).ToList();
            }
        }

        #endregion
    }
}