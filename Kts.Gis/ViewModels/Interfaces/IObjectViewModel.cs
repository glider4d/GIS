using Kts.Gis.Services;
using System;
using System.ComponentModel;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет интерфейс модели представления объекта.
    /// </summary>
    
    internal interface IObjectViewModel : INotifyPropertyChanged, ISavableObjectViewModel, ITypedObjectViewModel
    {
        #region Свойства

        /// <summary>
        /// Возвращает сервис доступа к функциям приложения.
        /// </summary>
        AccessService AccessService
        {
            get;
        }

        /// <summary>
        /// Возвращает идентификатор населенного пункта, в котором находится объект.
        /// </summary>
        int CityId
        {
            get;
        }

        /// <summary>
        /// Возвращает идентификатор объекта.
        /// </summary>
        Guid Id
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что является ли объект активным.
        /// </summary>
        bool IsActive
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что инициализирован ли объект.
        /// </summary>
        bool IsInitialized
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли объект планируемым.
        /// </summary>
        bool IsPlanning
        {
            get;
        }

        /// <summary>
        /// Возвращает идентификатор родителя объекта.
        /// </summary>
        Guid? ParentId
        {
            get;
        }

        #endregion

        #region Методы

        /// <summary>
        /// Начинает сохранение объекта в источнике данных.
        /// </summary>
        void BeginSave();

        /// <summary>
        /// Завершает сохранение объекта в источнике данных.
        /// </summary>
        void EndSave();

        /// <summary>
        /// Вызывается при изменении свойства.
        /// </summary>
        /// <param name="propertyName">Название измененного свойства.</param>
        void OnPropertyChanged(string propertyName);

        /// <summary>
        /// Отменяет сохранение объекта в источнике данных.
        /// </summary>
        void RevertSave();

        #endregion
    }
}