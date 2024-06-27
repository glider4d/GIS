using Kts.Gis.Models;
using System;

namespace Kts.Gis.Data
{
    /// <summary>
    /// Представляет интерфейс сервис доступа к данным надписей.
    /// </summary>
    public interface ILabelAccessService : IDeletableObjectAccessService<LabelModel>, IModifiableObjectAccessService<LabelModel>, IObjectAccessService<LabelModel>
    {
        #region Свойства

        /// <summary>
        /// Обновляет данные нового объекта в источнике данных.
        /// </summary>
        /// <param name="obj">Обновляемый объект.</param>
        /// <param name="schema">Схема.</param>
        /// <returns>Идентификатор нового объекта.</returns>
        Guid UpdateNewObject(LabelModel obj, SchemaModel schema);
        Guid UpdateNewObjectParamValuesFromLocal(int indexForSerialized);

        #endregion
    }
}