using Kts.Updater.Models;
using Kts.Updater.Services;
using Kts.Utilities;
using System.Collections.Generic;

namespace Kts.Updater.ViewModels
{
    /// <summary>
    /// Представляет базовую модель представления страницы.
    /// </summary>
    internal abstract partial class BasePageViewModel : BaseViewModel, IPageViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что выделена ли страница.
        /// </summary>
        private bool isSelected;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="BasePageViewModel"/>.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        public BasePageViewModel(SqlDataService dataService)
        {
            this.SqlDataService = dataService;
        }

        #endregion

        #region Защищенные свойства

        /// <summary>
        /// Возвращает сервис доступа к данным.
        /// </summary>
        protected SqlDataService SqlDataService
        {
            get;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает список объектов SQL.
        /// </summary>
        public AdvancedObservableCollection<SqlObjectViewModel> SqlObjects
        {
            get;
        } = new AdvancedObservableCollection<SqlObjectViewModel>();

        #endregion

        #region Открытые методы

        /// <summary>
        /// Возвращает список выбранных объектов SQL.
        /// </summary>
        /// <returns>Список объектов SQL.</returns>
        public List<SqlObjectModel> GetSelectedObjects()
        {
            return SqlObjectViewModel.GetSelectedObjects(this.SqlObjects);
        }

        #endregion
    }

    // Реализация IPageViewModel.
    internal abstract partial class BasePageViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что выделена ли страница.
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

        #endregion
    }
}