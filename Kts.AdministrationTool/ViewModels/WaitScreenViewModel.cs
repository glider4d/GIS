using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kts.AdministrationTool.ViewModels
{
    public class WaitScreenViewModel
    {
        #region Открытые события

        /// <summary>
        /// Событие запроса закрытия представления.
        /// </summary>
        public event EventHandler CloseRequested;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="WaitViewModel"/>.
        /// </summary>
        /// <param name="title">Заголовок представления.</param>
        /// <param name="content">Содержимое представления.</param>
        /// <param name="action">Действие.</param>
        public WaitScreenViewModel(string title, string content, Func<Task> action)
        {
            this.Title = title;
            this.Content = content;
            this.Action = action;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="WaitViewModel"/>.
        /// </summary>
        /// <param name="title">Заголовок представления.</param>
        /// <param name="content">Содержимое представления.</param>
        /// <param name="func">Функция.</param>
        public WaitScreenViewModel(string title, string content, Func<Task<bool>> func)
        {
            this.Title = title;
            this.Content = content;
            this.Func = func;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает действие.
        /// </summary>
        public Func<Task> Action
        {
            get;
        }

        /// <summary>
        /// Возвращает содержимое представления.
        /// </summary>
        public string Content
        {
            get;
        }

        /// <summary>
        /// Возвращает функцию.
        /// </summary>
        public Func<Task<bool>> Func
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает результат функции.
        /// </summary>
        public bool Result
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает заголовок представления.
        /// </summary>
        public string Title
        {
            get;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Начинает асинхронное выполнение поставленной задачи.
        /// </summary>
        public async Task StartActionAsync()
        {
            if (this.Action == null)
                throw new NullReferenceException(nameof(this.Action));

            await this.Action();

            this.CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Начинает асинхронное выполнение поставленной функции.
        /// </summary>
        public async Task StartFuncAsync()
        {
            if (this.Func == null)
                throw new NullReferenceException(nameof(this.Func));

            this.Result = await this.Func();

            this.CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}

