using Kts.Messaging;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления диалога формирования акта раздела границ.
    /// </summary>
    internal sealed class PartitionActViewModel : BaseViewModel
    {
        #region Закрытые неизменямые поля

        /// <summary>
        /// Хранитель слоев.
        /// </summary>
        private readonly ILayerHolder layerHolder;

        /// <summary>
        /// Сервис сообщений.
        /// </summary>
        private readonly IMessageService messageService;
        
        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса закрытия представления.
        /// </summary>
        public event EventHandler CloseRequested;

        /// <summary>
        /// Событие снятия скриншота с карты.
        /// </summary>
        public event EventHandler TakeMapScreenshot;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PartitionActViewModel"/>.
        /// </summary>
        /// <param name="layerHolder">Хранитель слоев.</param>
        /// <param name="messageService">Сервис сообщений.</param>
        public PartitionActViewModel(ILayerHolder layerHolder, IMessageService messageService)
        {
            this.layerHolder = layerHolder;
            this.messageService = messageService;

            this.GenerateCommand = new RelayCommand(this.ExecuteGenerate);
            this.SelectAreaCommand = new RelayCommand(this.ExecuteSelectArea);
            this.SelectPipesCommand = new RelayCommand(this.ExecuteSelectPipes);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает команду генерации акта.
        /// </summary>
        public RelayCommand GenerateCommand
        {
            get;
        }

        /// <summary>
        /// Список труб.
        /// </summary>
        private string pipes;

        /// <summary>
        /// Возвращает или задает список труб.
        /// </summary>
        public string Pipes
        {
            get
            {
                return this.pipes;
            }
            private set
            {
                this.pipes = value;

                this.NotifyPropertyChanged(nameof(this.Pipes));
            }
        }

        /// <summary>
        /// Область печати.
        /// </summary>
        private RenderTargetBitmap printArea;

        /// <summary>
        /// Возвращает область печати.
        /// </summary>
        public RenderTargetBitmap PrintArea
        {
            get
            {
                return this.printArea;
            }
            set
            {
                this.printArea = value;

                this.NotifyPropertyChanged(nameof(this.PrintArea));
            }
        }

        /// <summary>
        /// Возвращает команду выбора области.
        /// </summary>
        public RelayCommand SelectAreaCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду выбора труб.
        /// </summary>
        public RelayCommand SelectPipesCommand
        {
            get;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выполняет генерацию акта.
        /// </summary>
        private void ExecuteGenerate()
        {
            this.CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Выполняет выбор области.
        /// </summary>
        private void ExecuteSelectArea()
        {
            this.TakeMapScreenshot?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Выполняет выбор труб.
        /// </summary>
        private void ExecuteSelectPipes()
        {
            var pipes = this.layerHolder.GetSelectedObjects().Where(x => x.Type.ObjectKind == Models.ObjectKind.Line).Cast<LineViewModel>().ToList();

            if (pipes.Count == 0)
            {
                this.messageService.ShowMessage("Выберите хотя бы одну трубу", "Выбор труб", MessageType.Error);

                return;
            }

            // Собираем названия труб.
            string pipeList = "";
            foreach (var pipe in pipes)
                pipeList += "(" + pipe.Name + ") ";
            this.Pipes = pipeList;
        }

        #endregion
    }
}