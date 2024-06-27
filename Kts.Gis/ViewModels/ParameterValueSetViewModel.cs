using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Gis.Services;
using Kts.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления набора значений параметров.
    /// </summary>
    [Serializable]
    internal sealed class ParameterValueSetViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Отношения моделей и моделей представлений параметров.
        /// </summary>
        private Dictionary<ParameterModel, ParameterViewModel> paramRelations = new Dictionary<ParameterModel, ParameterViewModel>();

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Значение, указывающее на то, что являются ли значения параметров значениями только для чтения.
        /// </summary>
        private readonly bool isReadOnly;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ParameterValueSetViewModel"/>.
        /// </summary>
        /// <param name="parameterizedObject">Параметризованный объект.</param>
        /// <param name="parameterValueSet">Набор значений параметров.</param>
        /// <param name="isReadOnly">Значение, указывающее на то, что являются ли значения параметров значениями только для чтения.</param>
        /// <param name="canBeColored">Значение, указывающее на то, что могут ли параметры быть раскрашенными.</param>
        /// <param name="cityId">Идентификатор населенного пункта, который возможно будет использоваться при обновлении таблиц.</param>
        /// <param name="layerHolder">Хранитель слоев.</param>
        /// <param name="accessService">Сервис доступа к функциям приложения.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public ParameterValueSetViewModel(IParameterizedObjectViewModel parameterizedObject, ParameterValueSetModel parameterValueSet, bool isReadOnly, bool canBeColored, int cityId, ILayerHolder layerHolder, AccessService accessService, IDataService dataService, IMessageService messageService)
        {
            this.ParameterizedObject = parameterizedObject;
            this.ParameterValueSet = parameterValueSet;

            // Тут нужно учесть права пользователя на возможность редактирования параметров объектов определенного типа.
            var type = (parameterizedObject as ITypedObjectViewModel).Type;
            if (accessService.IsTypePermitted(type.TypeId))
                this.isReadOnly = isReadOnly;
            else
                this.isReadOnly = true;

            // Также, если объект не активен, то его в любом случае нельзя редактировать.
            if (parameterizedObject is IObjectViewModel && !(parameterizedObject as IObjectViewModel).IsActive)
                this.isReadOnly = true;
                
            for (int i = 0; i < this.ParameterValueSet.ParameterValues.Count; i++)
            {
                var paramValue = this.ParameterValueSet.ParameterValues.ElementAt(i);

                ParameterViewModel paramViewModel = null;

                if (paramValue.Key.Format.IsArray)
                {
                    // Делим значения параметра на части.
                    var div = new string[1]
                    {
                        "@@"
                    };
                    var parts = paramValue.Value.ToString().Split(div, StringSplitOptions.RemoveEmptyEntries);
                    var partDiv = new string[1]
                    {
                        "~~"
                    };

                    if (type.ParameterBonds.ContainsKey(paramValue.Key))
                    {
                        var parentParam = this.paramRelations[type.ParameterBonds[paramValue.Key]];

                        foreach (var entry in parts)
                        {
                            var temp = entry.Split(partDiv, StringSplitOptions.RemoveEmptyEntries);

                            paramViewModel = new ParameterViewModel(paramValue.Key, this, temp[0], temp[1].Replace('.', ','), parentParam.DepthLevel + 1, this.isReadOnly, canBeColored, cityId, type, layerHolder, accessService, dataService, messageService);

                            this.paramRelations[type.ParameterBonds[paramValue.Key]].Children.Add(paramViewModel);
                        }
                    }
                    else
                        foreach (var entry in parts)
                        {
                            var temp = entry.Split(partDiv, StringSplitOptions.RemoveEmptyEntries);

                            paramViewModel = new ParameterViewModel(paramValue.Key, this, temp[0], temp[1].Replace('.', ','), 0, this.isReadOnly, canBeColored, cityId, type, layerHolder, accessService, dataService, messageService);

                            this.Parameters.Add(paramViewModel);
                        }
                }
                else
                    if (type.ParameterBonds.ContainsKey(paramValue.Key))
                    {
                        var parentParam = this.paramRelations[type.ParameterBonds[paramValue.Key]];

                        paramViewModel = new ParameterViewModel(paramValue.Key, this, paramValue.Value, parentParam.DepthLevel + 1, this.isReadOnly, canBeColored, cityId, type, layerHolder, accessService, dataService, messageService);

                        this.paramRelations[type.ParameterBonds[paramValue.Key]].Children.Add(paramViewModel);
                    }
                    else
                    {
                        paramViewModel = new ParameterViewModel(paramValue.Key, this, paramValue.Value, 0, this.isReadOnly, canBeColored, cityId, type, layerHolder, accessService, dataService, messageService);

                        this.Parameters.Add(paramViewModel);
                    }

                this.paramRelations.Add(paramValue.Key, paramViewModel);
            }
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает параметризованный объект.
        /// </summary>
        public IParameterizedObjectViewModel ParameterizedObject
        {
            get;
        }

        /// <summary>
        /// Возвращает набор значений параметров.
        /// </summary>
        public ParameterValueSetModel ParameterValueSet
        {
            get;
        }

        /// <summary>
        /// Возвращает параметры.
        /// </summary>
        public List<ParameterViewModel> Parameters
        {
            get;
        } = new List<ParameterViewModel>();

        #endregion

        #region Открытые методы

        /// <summary>
        /// Возвращает параметр по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор параметра.</param>
        /// <returns>Параметр.</returns>
        public ParameterViewModel GetParameter(int id)
        {
            return this.paramRelations.First(x => x.Key.Id == id).Value;
        }

        /// <summary>
        /// Обновляет состояния параметров.
        /// </summary>
        public void UpdateParameters()
        {
            foreach (var param in this.paramRelations.Values)
                param.Refresh();
        }

        #endregion
    }
}