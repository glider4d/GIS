using Kts.Utilities;
using Kts.WpfUtilities;
using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления выбора дочернего объекта.
    /// </summary>
    [Serializable]
    internal sealed class SelectChildViewModel : BaseViewModel
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Дочерний объект.
        /// </summary>
        private readonly IObjectViewModel child;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SelectChildViewModel"/>.
        /// </summary>
        /// <param name="child">Дочерний объект.</param>
        public SelectChildViewModel(IObjectViewModel child)
        {
            this.child = child;

            this.SelectChildCommand = new RelayCommand(this.ExecuteSelectChild);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает название дочернего объекта.
        /// </summary>
        public string Name
        {
            get
            {
                var name = ((INamedObjectViewModel)this.child).Name;

                if (string.IsNullOrEmpty(name))
                    return "-";
                else
                    return name;
            }
        }

        /// <summary>
        /// Возвращает команду выбора дочернего объекта.
        /// </summary>
        public RelayCommand SelectChildCommand
        {
            get;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выполняет выбор дочернего объекта.
        /// </summary>
        private void ExecuteSelectChild()
        {
            ((ISelectableObjectViewModel)this.child).IsSelected = true;
        }

        #endregion
    }
}