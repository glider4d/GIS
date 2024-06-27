using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления настройки надписей.
    /// </summary>
    internal sealed class CaptionManageViewModel : BaseViewModel
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Идентификатор населенного пункта.
        /// </summary>
        private readonly int cityId;

        /// <summary>
        /// Сервис данных.
        /// </summary>
        private readonly IDataService dataService;

        /// <summary>
        /// Результат выбора параметров.
        /// </summary>
        private readonly Dictionary<ObjectType, List<ParameterModel>> result = new Dictionary<ObjectType, List<ParameterModel>>();

        #endregion

        #region Открытые события

        /// <summary>
        /// Событие запроса закрытия.
        /// </summary>
        public event EventHandler CloseRequested;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CaptionManageViewModel"/>.
        /// </summary>
        /// <param name="cityId">Идентификатор населенного пункта.</param>
        /// <param name="dataService">Сервис данных.</param>
        public CaptionManageViewModel(int cityId, IDataService dataService)
        {
            this.cityId = cityId;
            this.dataService = dataService;

            this.AddCommand = new RelayCommand(this.ExecuteAdd, this.CanExecuteAdd);
            this.MoveDownCommand = new RelayCommand(this.ExecuteMoveDown, this.CanExecuteMoveDown);
            this.MoveUpCommand = new RelayCommand(this.ExecuteMoveUp, this.CanExecuteMoveUp);
            this.RemoveCommand = new RelayCommand(this.ExecuteRemove, this.CanExecuteRemove);
            this.SaveCommand = new RelayCommand(this.ExecuteSave);

            this.Types.AddRange(this.dataService.ObjectTypes.Where(x => x.ObjectKind == ObjectKind.Figure || x.ObjectKind == ObjectKind.Line || x.ObjectKind == ObjectKind.NonVisualObject).OrderBy(x => x.Name));

            // Заполняем по всем типам результат выбора параметров.
            foreach (var type in this.Types)
            {
                this.result.Add(type, new List<ParameterModel>());

                foreach (var param in type.CaptionParameters)
                    this.result[type].Add(param);
            }
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает команду добавления параметра в отфильтрованные.
        /// </summary>
        public RelayCommand AddCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает отфильтрованные параметры выбранного типа.
        /// </summary>
        public AdvancedObservableCollection<ParameterModel> Filtered
        {
            get;
        } = new AdvancedObservableCollection<ParameterModel>();

        /// <summary>
        /// Значение, указывающее на то, что есть ли параметр, выбранный слева.
        /// </summary>
        private bool hasSelectedLeft;

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что есть ли параметр, выбранный слева.
        /// </summary>
        public bool HasSelectedLeft
        {
            get
            {
                return this.hasSelectedLeft;
            }
            set
            {
                this.hasSelectedLeft = value;

                this.AddCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Значение, указывающее на то, что есть ли параметр, выбранный справа.
        /// </summary>
        private bool hasSelectedRight;

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что есть ли параметр, выбранный справа.
        /// </summary>
        public bool HasSelectedRight
        {
            get
            {
                return this.hasSelectedRight;
            }
            set
            {
                this.hasSelectedRight = value;

                this.MoveUpCommand.RaiseCanExecuteChanged();
                this.MoveDownCommand.RaiseCanExecuteChanged();
                this.RemoveCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Возвращает команду перемещения вниз.
        /// </summary>
        public RelayCommand MoveDownCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду перемещения наверх.
        /// </summary>
        public RelayCommand MoveUpCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает параметры выбранного типа.
        /// </summary>
        public AdvancedObservableCollection<ParameterModel> Parameters
        {
            get;
        } = new AdvancedObservableCollection<ParameterModel>();

        /// <summary>
        /// Возвращает команду удаления параметра.
        /// </summary>
        public RelayCommand RemoveCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает команду сохранения.
        /// </summary>
        public RelayCommand SaveCommand
        {
            get;
        }

        /// <summary>
        /// Параметр выбранный, слева.
        /// </summary>
        private ParameterModel selectedLeft;

        /// <summary>
        /// Возвращает или задает параметр выбранный, слева.
        /// </summary>
        public ParameterModel SelectedLeft
        {
            get
            {
                return this.selectedLeft;
            }
            set
            {
                this.selectedLeft = value;

                this.HasSelectedLeft = value != null;

                this.NotifyPropertyChanged(nameof(this.SelectedLeft));
            }
        }

        /// <summary>
        /// Параметр выбранный, справа.
        /// </summary>
        private ParameterModel selectedRight;

        /// <summary>
        /// Возвращает или задает параметр выбранный, справа.
        /// </summary>
        public ParameterModel SelectedRight
        {
            get
            {
                return this.selectedRight;
            }
            set
            {
                this.selectedRight = value;

                this.HasSelectedRight = value != null;

                this.NotifyPropertyChanged(nameof(this.SelectedRight));
            }
        }

        /// <summary>
        /// Выбранный тип.
        /// </summary>
        private ObjectType selectedType;

        /// <summary>
        /// Возвращает или задает выбранный тип.
        /// </summary>
        public ObjectType SelectedType
        {
            get
            {
                return this.selectedType;
            }
            set
            {
                this.selectedType = value;

                this.Parameters.Clear();
                this.Parameters.AddRange(value.Parameters.Where(x => x.CanBeCopied).OrderBy(x => x.Name));

                this.Filtered.Clear();
                this.Filtered.AddRange(this.result[value]);

                this.SelectedLeft = null;
                this.SelectedRight = null;
            }
        }

        /// <summary>
        /// Возвращает типы.
        /// </summary>
        public List<ObjectType> Types
        {
            get;
        } = new List<ObjectType>();

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает true, если можно выполнить команду добавления параметра в отфильтрованные, иначе - false.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteAdd()
        {
            return this.HasSelectedLeft && !this.Filtered.Contains(this.SelectedLeft);
        }

        /// <summary>
        /// Возвращает true, если можно выполнить команду перемещения вниз, иначе - false.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteMoveDown()
        {
            return this.HasSelectedRight && this.Filtered.IndexOf(this.SelectedRight) < this.Filtered.Count - 1;
        }

        /// <summary>
        /// Возвращает true, если можно выполнить команду перемещения вверх, иначе - false.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteMoveUp()
        {
            return this.HasSelectedRight && this.Filtered.IndexOf(this.SelectedRight) > 0;
        }

        /// <summary>
        /// Возвращает true, если можно выполнить команду удаления параметра, иначе - false.
        /// </summary>
        /// <returns>true, если можно, иначе - false.</returns>
        private bool CanExecuteRemove()
        {
            return this.HasSelectedRight;
        }

        /// <summary>
        /// Выполняет добавление выбранного параметра в отфильтрованные.
        /// </summary>
        private void ExecuteAdd()
        {
            this.Filtered.Add(this.SelectedLeft);

            // Запоминаем это изменение в словаре.
            this.result[this.SelectedType].Add(this.SelectedLeft);

            this.SelectedLeft = this.SelectedLeft;
        }

        /// <summary>
        /// Выполняет перемещение вниз.
        /// </summary>
        private void ExecuteMoveDown()
        {
            var param = this.SelectedRight;

            var index = this.Filtered.IndexOf(this.SelectedRight);

            this.Filtered.RemoveAt(index);
            this.result[this.SelectedType].RemoveAt(index);

            this.Filtered.Insert(index + 1, param);
            this.result[this.SelectedType].Insert(index + 1, param);

            this.SelectedRight = param;
        }

        /// <summary>
        /// Выполняет перемещение вверх.
        /// </summary>
        private void ExecuteMoveUp()
        {
            var param = this.SelectedRight;

            var index = this.Filtered.IndexOf(this.SelectedRight);

            this.Filtered.RemoveAt(index);
            this.result[this.SelectedType].RemoveAt(index);

            this.Filtered.Insert(index - 1, param);
            this.result[this.SelectedType].Insert(index - 1, param);

            this.SelectedRight = param;
        }

        /// <summary>
        /// Выполняет удаление выбранного параметра.
        /// </summary>
        private void ExecuteRemove()
        {
            var index = this.Filtered.IndexOf(this.SelectedRight);

            this.Filtered.RemoveAt(index);
            this.result[this.SelectedType].RemoveAt(index);

            this.SelectedRight = null;
        }

        /// <summary>
        /// Выполняет сохранение.
        /// </summary>
        private void ExecuteSave()
        {
            this.dataService.MapAccessService.UpdateCaptions(this.cityId, this.result);

            this.CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}