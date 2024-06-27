using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления типа линии.
    /// </summary>
    internal sealed class LineTypeViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что выбран ли тип линии.
        /// </summary>
        private bool isSelected;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LineTypeViewModel"/>.
        /// </summary>
        /// <param name="type">Тип линии.</param>
        public LineTypeViewModel(ObjectType type)
        {
            this.Type = type;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что выбран ли тип линии.
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
        /// Возвращает тип линии.
        /// </summary>
        public ObjectType Type
        {
            get;
        }

        #endregion

        #region Открытые статические методы

        /// <summary>
        /// Возвращает список типов линий.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Список типов линий.</returns>
        public static List<LineTypeViewModel> GetLineTypes(IDataService dataService)
        {
            var result = new List<LineTypeViewModel>();

            foreach (var type in dataService.ObjectTypes.Where(x => x.ObjectKind == ObjectKind.Line))
                result.Add(new LineTypeViewModel(type));

            return result;
        }

        #endregion
    }
}