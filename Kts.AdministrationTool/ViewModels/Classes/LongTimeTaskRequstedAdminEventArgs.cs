using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kts.AdministrationTool.ViewModels.Classes
{
    public class LongTimeTaskRequstedAdminEventArgs : EventArgs
    {
        /// <summary>
        /// Представляет аргумент события запроса выполнения долгодлительной задачи.
        /// </summary>


        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземепляр класса <see cref="LongTimeTaskRequestedEventArgs"/>.
        /// </summary>
        /// <param name="waitViewModel">Модель представления ожидания окончания выполнения какой-либо задачи.</param>
        public LongTimeTaskRequstedAdminEventArgs(WaitScreenViewModel waitViewModel)
        {
            this.WaitViewModel = waitViewModel;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает модель представления ожидания окончания выполнения какой-либо задачи.
        /// </summary>
        public WaitScreenViewModel WaitViewModel
        {
            get;
        }

        #endregion

    }
}
