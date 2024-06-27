using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kts.Messaging
{
    /// <summary>
    /// Представляет модель представления ввода значения.
    /// </summary>
    public sealed class ValueInputViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Выбранная дополнительная опция.
        /// </summary>
        private OptionModel selectedAdditionalOption;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        private readonly IMessageService messageService;

        /// <summary>
        /// Тип вводимого значения.
        /// </summary>
        private readonly Type valueType;

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса закрытия представления.
        /// </summary>
        public event EventHandler CloseRequested;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ValueInputViewModel"/>.
        /// </summary>
        /// <param name="content">Содержимое диалога.</param>
        /// <param name="caption">Заголовок диалога.</param>
        /// <param name="valueType">Тип вводимого значения.</param>
        /// <param name="initialValue">Начальное значение.</param>
        /// <param name="options">Дополнительные опции.</param>
        /// <param name="additionalOptions">Дополнительные опции, представляемые списком выбора.</param>
        /// <param name="warning">Предупреждение.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public ValueInputViewModel(string content, string caption, Type valueType, object initialValue, List<OptionModel> options, List<OptionModel> additionalOptions, string additionalOptionsText, string warning, IMessageService messageService) : this(content, caption, valueType, initialValue, options, warning, messageService)
        {
            this.AdditionalOptions = additionalOptions;
            this.AdditionalOptionsText = additionalOptionsText;

            var selected = this.AdditionalOptions.FirstOrDefault(x => x.IsSelected);

            if (selected != null)
                this.SelectedAdditionalOption = selected;
            else
                this.SelectedAdditionalOption = this.AdditionalOptions.FirstOrDefault();
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ValueInputViewModel"/>.
        /// </summary>
        /// <param name="content">Содержимое диалога.</param>
        /// <param name="caption">Заголовок диалога.</param>
        /// <param name="valueType">Тип вводимого значения.</param>
        /// <param name="initialValue">Начальное значение.</param>
        /// <param name="options">Дополнительные опции.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public ValueInputViewModel(string content, string caption, Type valueType, object initialValue, List<OptionModel> options, IMessageService messageService)
        {
            this.Content = content;
            this.Caption = caption;
            this.valueType = valueType;
            this.Value = initialValue != null ? initialValue.ToString() : "";
            this.Options = options;
            this.messageService = messageService;

            this.CheckValueCommand = new RelayCommand(this.ExecuteCheckValue);
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ValueInputViewModel"/>.
        /// </summary>
        /// <param name="content">Содержимое диалога.</param>
        /// <param name="caption">Заголовок диалога.</param>
        /// <param name="valueType">Тип вводимого значения.</param>
        /// <param name="initialValue">Начальное значение.</param>
        /// <param name="options">Дополнительные опции.</param>
        /// <param name="warning">Предупреждение.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public ValueInputViewModel(string content, string caption, Type valueType, object initialValue, List<OptionModel> options, string warning, IMessageService messageService) : this(content, caption, valueType, initialValue, options, messageService)
        {
            this.Warning = warning;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает дополнительные опции, представляемые списком выбора.
        /// </summary>
        public List<OptionModel> AdditionalOptions
        {
            get;
        }

        /// <summary>
        /// Возвращает текст, описывающий дополнительные опции, представляемые списком выбора.
        /// </summary>
        public string AdditionalOptionsText
        {
            get;
        }

        /// <summary>
        /// Возвращает заголовок диалога.
        /// </summary>
        public string Caption
        {
            get;
        }

        /// <summary>
        /// Возвращает команду проверки правильности ввода значения.
        /// </summary>
        public RelayCommand CheckValueCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает содержимое диалога.
        /// </summary>
        public string Content
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что есть ли дополнительные опции, представляемые списком выбора.
        /// </summary>
        public bool HasAdditionalOptions
        {
            get
            {
                return this.AdditionalOptions != null && this.AdditionalOptions.Count > 0;
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что есть ли предупреждение.
        /// </summary>
        public bool HasWarning
        {
            get
            {
                return !string.IsNullOrEmpty(this.Warning);
            }
        }

        /// <summary>
        /// Возвращает дополнительные опции диалога.
        /// </summary>
        public List<OptionModel> Options
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает результат ввода.
        /// </summary>
        public object Result
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает выбранную дополнительную опцию.
        /// </summary>
        public OptionModel SelectedAdditionalOption
        {
            get
            {
                return this.selectedAdditionalOption;
            }
            set
            {
                if (this.SelectedAdditionalOption != value)
                {
                    if (this.SelectedAdditionalOption != null)
                        this.SelectedAdditionalOption.IsSelected = false;

                    this.selectedAdditionalOption = value;

                    if (value != null)
                        this.SelectedAdditionalOption.IsSelected = true;
                }
            }
        }

        /// <summary>
        /// Возвращает или задает вводимое значение.
        /// </summary>
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает предупреждение.
        /// </summary>
        public string Warning
        {
            get;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выполняет проверку правильности ввода значения.
        /// </summary>
        private void ExecuteCheckValue()
        {
            try
            {
                this.Result = Convert.ChangeType(this.Value, this.valueType);

                // Уведомляем о том, что нужно закрыть представление.
                this.CloseRequested?.Invoke(this, EventArgs.Empty);
            }
            catch
            {
                this.Result = null;

                this.messageService.ShowMessage("Вы ввели неверное значение", this.Caption, MessageType.Error);
            }
        }

        #endregion
    }
}