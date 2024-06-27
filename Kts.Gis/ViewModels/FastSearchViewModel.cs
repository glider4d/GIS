using Kts.Gis.Models;
using Kts.Utilities;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления быстрого поиска.
    /// </summary>
    internal sealed class FastSearchViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Сигнализитор токена отмены поиска.
        /// </summary>
        private CancellationTokenSource cts;

        /// <summary>
        /// Значение, указывающее на то, что есть ли результат поиска.
        /// </summary>
        private bool hasResult;

        /// <summary>
        /// Значение, указывающее на то, что доступен ли быстрый поиск.
        /// </summary>
        private bool isEnabled;

        /// <summary>
        /// Значение, указывающее на то, что идет ли поиск.
        /// </summary>
        private bool isSearching;

        /// <summary>
        /// Поисковое значение.
        /// </summary>
        private string searchValue;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Хранитель слоев.
        /// </summary>
        private readonly ILayerHolder layerHolder;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="FastSearchViewModel"/>.
        /// </summary>
        /// <param name="layerHolder">Хранитель слоев.</param>
        public FastSearchViewModel(ILayerHolder layerHolder)
        {
            this.layerHolder = layerHolder;

            this.ClearCommand = new RelayCommand(this.ExecuteClear, this.CanExecuteClear);

            this.PropertyChanged += this.FastSearchViewModel_PropertyChanged;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает команду очистки поискового значения.
        /// </summary>
        public RelayCommand ClearCommand
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что есть ли результат поиска.
        /// </summary>
        public bool HasResult
        {
            get
            {
                return this.hasResult;
            }
            private set
            {
                this.hasResult = value;

                this.NotifyPropertyChanged(nameof(this.HasResult));
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что доступен ли быстрый поиск.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return this.isEnabled;
            }
            set
            {
                this.isEnabled = value;

                this.SearchValue = "";

                this.NotifyPropertyChanged(nameof(this.IsEnabled));
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что идет ли поиск.
        /// </summary>
        public bool IsSearching
        {
            get
            {
                return this.isSearching;
            }
            private set
            {
                this.isSearching = value;

                this.NotifyPropertyChanged(nameof(this.IsSearching));
            }
        }

        /// <summary>
        /// Возвращает или задает результат поиска.
        /// </summary>
        public AdvancedObservableCollection<Tuple<Guid, string>> SearchResult
        {
            get;
        } = new AdvancedObservableCollection<Tuple<Guid, string>>();

        /// <summary>
        /// Возвращает или задает поисковое значение.
        /// </summary>
        public string SearchValue
        {
            get
            {
                return this.searchValue;
            }
            set
            {
                this.searchValue = value;

                this.NotifyPropertyChanged(nameof(this.SearchValue));

                this.ClearCommand.RaiseCanExecuteChanged();

                this.HasResult = !string.IsNullOrEmpty(value);
            }
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="INotifyPropertyChanged.PropertyChanged"/> группы слоев.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private async void FastSearchViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.SearchValue))
            {
                var filter = this.SearchValue;

                if (this.cts != null)
                    this.cts.Cancel();

                this.SearchResult.Clear();

                if (string.IsNullOrEmpty(filter))
                    this.IsSearching = false;
                else
                {
                    // Создаем новый сигнализатор.
                    var cts = new CancellationTokenSource();
                    this.cts = cts;

                    this.IsSearching = true;

                    try
                    {
                        var objects = await this.FindObjects(filter, cts.Token);

                        if (!cts.IsCancellationRequested)
                        {
                            this.SearchResult.AddRange(objects);
                            
                            this.IsSearching = false;
                        }
                    }
                    catch
                    {
                        if (!cts.IsCancellationRequested)
                            this.IsSearching = false;
                    }
                }
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли выполнить очистку поискового значения.
        /// </summary>
        /// <returns>true, если можно выполнить очистку поискового значения, иначе - false.</returns>
        private bool CanExecuteClear()
        {
            return !string.IsNullOrEmpty(this.SearchValue);
        }

        /// <summary>
        /// Выполняет очистку поискового значения.
        /// </summary>
        private void ExecuteClear()
        {
            this.SearchValue = "";
        }

        /// <summary>
        /// Асинхронно выполняет поиск объектов по заданному значению.
        /// </summary>
        /// <param name="searchValue">Поисковое значение.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список тюплов, где первый элемент - это идентификатор найденного объекта, а второй - его название.</returns>
        private Task<List<Tuple<Guid, string>>> FindObjects(string searchValue, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                var result = new List<Tuple<Guid, string>>();

                var layers = this.layerHolder.GetLayers(ObjectKind.Figure);

                foreach (var layer in layers)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return new List<Tuple<Guid, string>>();

                    var copy = new List<IObjectViewModel>(layer.Objects);

                    var conditions = searchValue.Trim().Replace("  ", " ").Split(new string[1]
                    {
                        " "
                    }, StringSplitOptions.RemoveEmptyEntries);

                    var ok = true;

                    foreach (INamedObjectViewModel obj in copy)
                    {
                        ok = true;

                        foreach (var condition in conditions)
                            if (!obj.Name.ToLower().Contains(condition.ToLower()))
                            {
                                ok = false;

                                break;
                            }

                        if (ok)
                            result.Add(new Tuple<Guid, string>((obj as IObjectViewModel).Id, obj.Name));

                        if (cancellationToken.IsCancellationRequested)
                            return new List<Tuple<Guid, string>>();
                    }
                }

                return new List<Tuple<Guid, string>>(result.OrderBy(x => x.Item2));
            });
        }

        #endregion
    }
}