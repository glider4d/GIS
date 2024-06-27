using Kts.Gis.Models;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления добавления дочернего объекта объекту.
    /// </summary>
    [Serializable]
    internal sealed class AddChildViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Тип дочернего объекта.
        /// </summary>
        private ObjectType childType;

        /// <summary>
        /// Значение, указывающее на то, что является ли добавляемый дочерний объект значком.
        /// </summary>
        private bool isBadge;

        /// <summary>
        /// Родительский объект.
        /// </summary>
        private IObjectViewModel parent;

        #endregion
        
        #region Открытые события

        /// <summary>
        /// Событие запроса добавления объекта, представляемого значком на карте.
        /// </summary>
        public event EventHandler<AddBadgeRequestedEventArgs> AddBadgeRequested;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AddChildViewModel"/>.
        /// </summary>
        /// <param name="parent">Родительский объект.</param>
        /// <param name="childType">Тип дочернего объекта.</param>
        /// <param name="isBadge">Значение, указывающее на то, что является ли добавляемый дочерний объект значком.</param>
        public AddChildViewModel(IObjectViewModel parent, ObjectType childType, bool isBadge)
        {
            this.parent = parent;
            this.childType = childType;
            this.isBadge = isBadge;

            this.AddChildCommand = new RelayCommand(this.ExecuteAddChild, this.CanExecuteAddChild);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает команду добавления дочернего объекта объекту.
        /// </summary>
        public RelayCommand AddChildCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает название типа дочернего объекта.
        /// </summary>
        public string Name
        {
            get
            {
                return this.childType.SingularName;
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить добавление дочернего объекта объекту.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteAddChild()
        {
            return this.parent.AccessService.IsTypePermitted(this.parent.Type.TypeId) && this.parent.IsActive;
        }

        /// <summary>
        /// Выполняет добавление дочернего объекта объекту.
        /// </summary>
        private void ExecuteAddChild()
        {
            if (!this.isBadge)
                (this.parent as IContainerObjectViewModel).AddChild(this.childType);
            else
                if (this.AddBadgeRequested != null)
                {
                    var eventArgs = new AddBadgeRequestedEventArgs(this.parent as LineViewModel);

                    this.AddBadgeRequested(this, eventArgs);

                    if (eventArgs.Result != null)
                        (this.parent as LineViewModel).AddChild(this.childType, eventArgs.Result.Value);
                }
        }

        #endregion
    }
}