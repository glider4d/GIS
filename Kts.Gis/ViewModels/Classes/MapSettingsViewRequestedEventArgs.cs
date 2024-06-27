using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет аргументы события запроса отображения представления настроек вида карты.
    /// </summary>
    internal sealed class MapSettingsViewRequestedEventArgs : EventArgs
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MapSettingsViewRequestedEventArgs"/>.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public MapSettingsViewRequestedEventArgs(MapSettingsViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает результат запроса.
        /// </summary>
        public bool Result
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает модель представления настроек вида карты.
        /// </summary>
        public MapSettingsViewModel ViewModel
        {
            get;
        }

        #endregion
    }
}