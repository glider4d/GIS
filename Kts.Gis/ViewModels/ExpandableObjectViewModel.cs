using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления раскрываемого объекта.
    /// </summary>
    [Serializable]
    internal sealed class ExpandableObjectViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что раскрыт ли объект.
        /// </summary>
        private bool isExpanded;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Объект.
        /// </summary>
        private readonly IObjectViewModel obj;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ExpandableObjectViewModel"/>.
        /// </summary>
        /// <param name="obj">Объект, который должен реализовывать следующие интерфейсы: <see cref="IContainerObjectViewModel"/>, <see cref="IExpandableObjectViewModel"/> и <see cref="IObjectViewModel"/>.</param>
        public ExpandableObjectViewModel(IObjectViewModel obj)
        {
            this.obj = obj;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что раскрыт ли объект.
        /// </summary>
        /// <returns>Значение, указывающее на то, что раскрыт ли объект.</returns>
        public bool GetIsExpanded()
        {
            return this.isExpanded;
        }

        /// <summary>
        /// Задает значение, указывающее на то, что раскрыт ли объект.
        /// </summary>
        /// <param name="value">Значение, указывающее на то, что раскрыт ли объект.</param>
        public void SetIsExpanded(bool value)
        {
            this.isExpanded = value;

            var container = this.obj as IContainerObjectViewModel;

            if (value && !container.AreChildrenLoaded)
                // Загружаем дочерние объекты объекта.
                container.LoadChildren();

            this.obj.OnPropertyChanged(nameof(IExpandableObjectViewModel.IsExpanded));
        }

        #endregion
    }
}