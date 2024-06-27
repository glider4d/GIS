using Kts.Gis.Models;
using System;
using System.Collections.Generic;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет аргумент события запроса схемы населенного пункта.
    /// </summary>
    internal sealed class SchemaRequestedEventArgs : EventArgs
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SchemaRequestedEventArgs"/>.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        public SchemaRequestedEventArgs(CityViewModel city)
        {
            this.City = city;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает идентификаторы выбранных котельных, если они не все были выбраны.
        /// </summary>
        public List<Guid> BoilerIds
        {
            get;
        } = new List<Guid>();

        /// <summary>
        /// Возвращает населенный пункт.
        /// </summary>
        public CityViewModel City
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает схему населенного пункта.
        /// </summary>
        public SchemaModel Schema
        {
            get;
            set;
        }

        #endregion
    }
}