using Kts.Utilities;
using System.Collections.Generic;
using System.ComponentModel;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления фильтра слоев.
    /// </summary>
    internal sealed class FilterViewModel : BaseViewModel
    {
        #region Закрытые константы

        /// <summary>
        /// Окончание заголовка фильтра при непримененном фильтре.
        /// </summary>
        private const string isOffSuffix = ": выкл.";

        /// <summary>
        /// Окончание заголовка фильтра при примененном фильтре.
        /// </summary>
        private const string isOnSuffix = ": вкл.";

        #endregion

        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что применен ли фильтр.
        /// </summary>
        private bool isOn;

        /// <summary>
        /// Заголовок фильтра.
        /// </summary>
        private string title;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Название фильтра.
        /// </summary>
        private readonly string name;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="FilterViewModel"/>.
        /// </summary>
        /// <param name="layers">Слои.</param>
        /// <param name="name">Название фильтра.</param>
        public FilterViewModel(IEnumerable<LayerViewModel> layers, string name)
        {
            this.Layers = layers;
            this.name = name;

            this.IsOn = false;

            // Подписываемся на события изменения свойств слоев, чтобы определять, применен ли фильтр или нет.
            foreach (var layer in layers)
                layer.PropertyChanged += this.Layer_PropertyChanged;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что применен ли фильтр.
        /// </summary>
        public bool IsOn
        {
            get
            {
                return this.isOn;
            }
            private set
            {
                this.isOn = value;

                // Задаем заголовок фильтра.
                this.Title = this.name + (value ? isOnSuffix : isOffSuffix);

                this.NotifyPropertyChanged(nameof(this.IsOn));
            }
        }

        /// <summary>
        /// Возвращает слои.
        /// </summary>
        public IEnumerable<LayerViewModel> Layers
        {
            get;
        }

        /// <summary>
        /// Возвращает или задает заголовок фильтра.
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }
            private set
            {
                this.title = value;

                this.NotifyPropertyChanged(nameof(this.Title));
            }
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="INotifyPropertyChanged.PropertyChanged"/> слоя.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Layer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LayerViewModel.IsChanged))
            {
                bool isChanged = false;

                foreach (var layer in this.Layers)
                    if (layer.IsChanged)
                    {
                        // Если изменилась видимость или прозрачность слоя, то это означает то, что фильтр применен.
                        isChanged = true;

                        break;
                    }

                this.IsOn = isChanged;
            }
        }

        #endregion
    }
}