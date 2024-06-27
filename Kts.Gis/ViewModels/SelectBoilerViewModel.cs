using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления выбора котельной.
    /// </summary>
    internal sealed class SelectBoilerViewModel
    {
        #region Открытые события

        /// <summary>
        /// Событие запроса закрытия представления.
        /// </summary>
        public event EventHandler CloseRequested;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SelectBoilerViewModel"/>.
        /// </summary>
        /// <param name="boilers">Котельные.</param>
        public SelectBoilerViewModel(List<SelectableBoilerViewModel> boilers)
        {
            this.OKCommand = new RelayCommand(this.ExecuteOK);

            this.Boilers = boilers;

            this.SelectedBoiler = this.Boilers.First();
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает список котельных.
        /// </summary>
        public List<SelectableBoilerViewModel> Boilers
        {
            get;
        } = new List<SelectableBoilerViewModel>();

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что выбрана ли котельная.
        /// </summary>
        public bool IsBoilerSelected
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает команду ОК.
        /// </summary>
        public RelayCommand OKCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает выбранную котельную.
        /// </summary>
        public SelectableBoilerViewModel SelectedBoiler
        {
            get;
            set;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выполняет ОК.
        /// </summary>
        private void ExecuteOK()
        {
            this.IsBoilerSelected = true;

            this.CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}