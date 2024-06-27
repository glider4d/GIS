using Kts.Gis.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет менеджер буфера обмена.
    /// </summary>
    internal sealed class ClipboardManager
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сохраненные значения параметров.
        /// </summary>
        private readonly Dictionary<ParameterModel, object> storedParameters = new Dictionary<ParameterModel, object>();

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса выбора параметров.
        /// </summary>
        public event EventHandler<ParameterSelectRequestedEventArgs> ParameterSelectRequested;

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеются ли сохраненные значения параметров.
        /// </summary>
        public bool HasStoredParameters
        {
            get
            {
                return this.storedParameters.Count > 0;
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Возвращает список параметров, выбранных для вставки.
        /// </summary>
        /// <param name="type">Тип объекта.</param>
        /// <returns>Список параметров.</returns>
        public List<ParameterModel> GetSelectedParameters(ObjectType type)
        {
            var eventArgs = new ParameterSelectRequestedEventArgs(this.storedParameters.Keys.ToList().Where(x => type.HasParameter(x)).ToList());

            this.ParameterSelectRequested?.Invoke(this, eventArgs);

            return eventArgs.SelectedParameters;
        }

        /// <summary>
        /// Возвращает значения указанных параметров из внутреннего хранилища.
        /// </summary>
        /// <param name="parameters">Параметры, значения которых нужно вернуть.</param>
        /// <returns>Словарь значений указанных параметров.</returns>
        public Dictionary<ParameterModel, object> RetrieveParameters(List<ParameterModel> parameters)
        {
            var result = new Dictionary<ParameterModel, object>();

            foreach (var param in parameters)
                result.Add(param, this.storedParameters[param]);

            return result;
        }

        /// <summary>
        /// Сохраняет заданные значения параметров во внутреннем хранилище.
        /// </summary>
        /// <param name="values">Значения параметров.</param>
        public void StoreParameters(Dictionary<ParameterModel, object> values)
        {
            this.storedParameters.Clear();

            foreach (var entry in values)
                this.storedParameters.Add(entry.Key, entry.Value);
        }

        #endregion
    }
}