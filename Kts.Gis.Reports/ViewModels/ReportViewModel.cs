using Kts.WpfUtilities;
using System;
using System.Collections.Generic;

namespace Kts.Gis.Reports.ViewModels
{
    /// <summary>
    /// Представляет модель представления отчета.
    /// </summary>
    public sealed partial class ReportViewModel : ITreeItemViewModel
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Вид отчета.
        /// </summary>
        private readonly ReportKind reportKind;

        /// <summary>
        /// Сервис отчетов.
        /// </summary>
        private readonly ReportService reportService;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ReportViewModel"/>.
        /// </summary>
        /// <param name="name">Название отчета.</param>
        /// <param name="reportKind">Вид отчета.</param>
        /// <param name="reportService">Сервис отчетов.</param>
        public ReportViewModel(string name, ReportKind reportKind, ReportService reportService)
        {
            this.Name = name;
            this.reportKind = reportKind;
            this.reportService = reportService;

            this.GenerateCommand = new RelayCommand(this.ExecuteGenerate);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает команду генерации отчета.
        /// </summary>
        public RelayCommand GenerateCommand
        {
            get;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выполняет генерацию отчета.
        /// </summary>
        private void ExecuteGenerate()
        {
            switch (this.reportKind)
            {
                case ReportKind.AddedObjectInfo:
                    this.reportService.RequestAddedObjectInfoViewShow();

                    break;

                case ReportKind.Kts:
                    this.reportService.RequestKtsViewShow();

                    break;

                case ReportKind.TechSpec:
                    this.reportService.RequestTechSpecViewShow();

                    break;

                default:
                    throw new NotImplementedException("Не реализована работа с отчетом следующего вида: " + this.reportKind.ToString());
            }
        }

        #endregion
    }

    // Реализация ITreeItemViewModel.
    public sealed partial class ReportViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает дочерние объекты элемента дерева.
        /// </summary>
        public List<ITreeItemViewModel> Children
        {
            get;
        } = new List<ITreeItemViewModel>();

        /// <summary>
        /// Возвращает название элемента дерева.
        /// </summary>
        public string Name
        {
            get;
        }

        #endregion
    }
}