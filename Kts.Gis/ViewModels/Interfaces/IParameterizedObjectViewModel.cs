using Kts.Gis.Models;
using Kts.WpfUtilities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет интерфейс параметризованного объекта.
    /// </summary>
    internal interface IParameterizedObjectViewModel
    {
        #region Свойства
    
        /// <summary>
        /// Возвращает модель представления значений вычисляемых параметров объекта.
        /// </summary>
        ParameterValueSetViewModel CalcParameterValuesViewModel
        {
            get;
        }

        /// <summary>
        /// Возвращает набор значений измененных параметров объекта.
        /// </summary>
        ParameterValueSetModel ChangedParameterValues
        {
            get;
        }

        /// <summary>
        /// Возвращает модель представления значений общих параметров объекта.
        /// </summary>
        ParameterValueSetViewModel CommonParameterValuesViewModel
        {
            get;
        }

        /// <summary>
        /// Возвращает команду копирования параметров.
        /// </summary>
        RelayCommand CopyParametersCommand
        {
            get;
        }
        
        /// <summary>
        /// Возвращает модель представления значений параметров объекта.
        /// </summary>
        ParameterValueSetViewModel ParameterValuesViewModel
        {
            get;
        }

        /// <summary>
        /// Возвращает команду вставки параметров.
        /// </summary>
        RelayCommand PasteParametersCommand
        {
            get;
        }

        #endregion

        #region Методы

        /// <summary>
        /// Меняет значение измененного параметра.
        /// </summary>
        /// <param name="param">Параметр.</param>
        /// <param name="newValue">Новое значение.</param>
        void ChangeChangedValue(ParameterModel param, object newValue);

        /// <summary>
        /// Возвращает список параметров, содержащих ошибки в значениях.
        /// </summary>
        /// <returns>Список параметров.</returns>
        List<ParameterModel> GetErrors();

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеет ли заданный параметр измененное значение.
        /// </summary>
        /// <param name="param">Параметр.</param>
        /// <returns>true, если имеет, иначе - false.</returns>
        bool HasChangedValue(ParameterModel param);

        /// <summary>
        /// Выполняет загрузку значений вычисляемых параметров объекта.
        /// </summary>
        void LoadCalcParameterValues();

        /// <summary>
        /// Выполняет асинхронную загрузку значений вычисляемых параметров объекта.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Задача.</returns>
        Task LoadCalcParameterValuesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Выполняет асинхронную загрузку значений общих параметров объекта.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Задача.</returns>
        Task LoadCommonParameterValuesAsync(CancellationToken cancellationToken);
        
        /// <summary>
        /// Выполняет загрузку значений параметров объекта.
        /// </summary>
        void LoadParameterValues();
       
        /// <summary>
        /// Заполняет параметры, значениями по умолчанию
        /// </summary>
        void LoadParameterDefaultValues();

        /// <summary>
        /// Заполняет вычисляемые параметры, значениями по умолчанию
        /// </summary>
        void LoadCalcParameterDefaultValues();
        /// <summary>
        /// Выполняет асинхронную загрузку значений параметров объекта.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Задача.</returns>
        Task LoadParameterValuesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Уведомляет объект об изменении значения параметра.
        /// </summary>
        /// <param name="param">Параметр.</param>
        /// <param name="prevValue">Предыдущее значение.</param>
        /// <param name="newValue">Новое значение.</param>
        void NotifyParameterValueChanged(ParameterModel param, object prevValue, object newValue);

        /// <summary>
        /// Выполняет выгрузку значений вычисляемых параметров объекта.
        /// </summary>
        void UnloadCalcParameterValues();

        /// <summary>
        /// Выполняет выгрузку значений общих параметров объекта.
        /// </summary>
        void UnloadCommonParameterValues();

        /// <summary>
        /// Выполняет выгрузку значений параметров объекта.
        /// </summary>
        void UnloadParameterValues();

        #endregion
    }
}