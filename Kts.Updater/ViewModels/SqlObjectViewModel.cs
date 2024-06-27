using Kts.Updater.Models;
using Kts.Updater.Services;
using Kts.Utilities;
using System;
using System.Collections.Generic;

namespace Kts.Updater.ViewModels
{
    /// <summary>
    /// Представляет модель представления объекта SQL.
    /// </summary>
    internal class SqlObjectViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что выбран ли объект SQL.
        /// </summary>
        private bool isSelected;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Модель объекта SQL.
        /// </summary>
        private readonly SqlObjectModel model;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SqlObjectViewModel"/>.
        /// </summary>
        /// <param name="model">Модель объекта SQL.</param>
        public SqlObjectViewModel(SqlObjectModel model)
        {
            this.model = model;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что выбран ли объект SQL.
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
        /// Возвращает дату изменения объекта SQL.
        /// </summary>
        public DateTime Modified
        {
            get
            {
                return this.model.Modified;
            }
        }
        
        /// <summary>
        /// Возвращает название объекта SQL.
        /// </summary>
        public string Name
        {
            get
            {
                return this.model.Name;
            }
        }

        /// <summary>
        /// Возвращает название схемы.
        /// </summary>
        public string Schema
        {
            get
            {
                return this.model.Schema;
            }
        }

        #endregion

        #region Открытые статические методы

        /// <summary>
        /// Возвращает справочники.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Список справочников.</returns>
        public static List<SqlObjectViewModel> GetCatalogs(SqlDataService dataService)
        {
            var result = new List<SqlObjectViewModel>();

            foreach (var obj in dataService.GetCatalogs())
                result.Add(new SqlObjectViewModel(obj));

            return result;
        }

        /// <summary>
        /// Возвращает скалярные функции SQL.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Список скалярных функций SQL.</returns>
        public static List<SqlObjectViewModel> GetScalarFunctions(SqlDataService dataService)
        {
            var result = new List<SqlObjectViewModel>();

            foreach (var obj in dataService.GetScalarFunctions())
                result.Add(new SqlObjectViewModel(obj));

            return result;
        }

        /// <summary>
        /// Возвращает список моделей выбранных объектов SQL.
        /// </summary>
        /// <param name="objects">Объекты SQL.</param>
        /// <returns>Список моделей объектов SQL.</returns>
        public static List<SqlObjectModel> GetSelectedObjects(IEnumerable<SqlObjectViewModel> objects)
        {
            var result = new List<SqlObjectModel>();

            foreach (var obj in objects)
                if (obj.IsSelected)
                    result.Add(obj.model);

            return result;
        }

        /// <summary>
        /// Возвращает хранимые процедуры SQL.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Список хранимых процедур SQL.</returns>
        public static List<SqlObjectViewModel> GetStoredProcedures(SqlDataService dataService)
        {
            var result = new List<SqlObjectViewModel>();

            foreach (var obj in dataService.GetStoredProcedures())
                result.Add(new SqlObjectViewModel(obj));

            return result;
        }

        /// <summary>
        /// Возвращает табличные функции SQL.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Список табличных функций SQL.</returns>
        public static List<SqlObjectViewModel> GetTableFunctions(SqlDataService dataService)
        {
            var result = new List<SqlObjectViewModel>();

            foreach (var obj in dataService.GetTableFunctions())
                result.Add(new SqlObjectViewModel(obj));

            return result;
        }

        /// <summary>
        /// Возвращает таблицы SQL.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Список таблиц SQL.</returns>
        public static List<SqlObjectViewModel> GetTables(SqlDataService dataService)
        {
            var result = new List<SqlObjectViewModel>();

            foreach (var obj in dataService.GetTables())
                result.Add(new SqlObjectViewModel(obj));

            return result;
        }

        /// <summary>
        /// Возвращает представления SQL.
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns>Список представлений SQL.</returns>
        public static List<SqlObjectViewModel> GetViews(SqlDataService dataService)
        {
            var result = new List<SqlObjectViewModel>();

            foreach (var obj in dataService.GetViews())
                result.Add(new SqlObjectViewModel(obj));

            return result;
        }

        #endregion
    }
}