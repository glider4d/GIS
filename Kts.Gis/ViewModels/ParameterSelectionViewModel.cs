using Kts.Gis.Models;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления выбора параметров.
    /// </summary>
    internal sealed class ParameterSelectionViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что выбраны ли все параметры.
        /// </summary>
        private bool isAllChecked = true;

        #endregion

        #region Закрытые статические неизменяемые поля

        /// <summary>
        /// Последние выбранные параметры.
        /// </summary>
        private static readonly List<ParameterModel> lastSelectedParameters = new List<ParameterModel>();

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса закрытия представления.
        /// </summary>
        public event EventHandler CloseRequested;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ParameterSelectionViewModel"/>.
        /// </summary>
        /// <param name="parameters">Параметры.</param>
        public ParameterSelectionViewModel(List<ParameterModel> parameters)
        {
            if (lastSelectedParameters.Count == 0)
                foreach (var param in parameters)
                    this.Parameters.Add(new SelectableParameterViewModel(param));
            else
                foreach (var param in parameters)
                    this.Parameters.Add(new SelectableParameterViewModel(param, lastSelectedParameters.Contains(param)));

            this.CheckAllCommand = new RelayCommand(this.ExecuteCheckAll);
            this.PasteCommand = new RelayCommand(this.ExecutePaste);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает команду выбора всех объектов.
        /// </summary>
        public RelayCommand CheckAllCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает список параметров.
        /// </summary>
        public List<SelectableParameterViewModel> Parameters
        {
            get;
        } = new List<SelectableParameterViewModel>();

        /// <summary>
        /// Возвращает команду вставки.
        /// </summary>
        public RelayCommand PasteCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает список выбранных параметров.
        /// </summary>
        public List<ParameterModel> SelectedParameters
        {
            get;
        } = new List<ParameterModel>();

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выбирает все параметры.
        /// </summary>
        private void ExecuteCheckAll()
        {
            this.isAllChecked = !this.isAllChecked;

            foreach (var param in this.Parameters)
                param.IsSelected = this.isAllChecked;
        }

        /// <summary>
        /// Выполняет вставку.
        /// </summary>
        private void ExecutePaste()
        {
            foreach (var entry in this.Parameters)
                if (entry.IsSelected)
                    this.SelectedParameters.Add(entry.Parameter);

            lastSelectedParameters.Clear();
            lastSelectedParameters.AddRange(this.SelectedParameters);

            this.CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}