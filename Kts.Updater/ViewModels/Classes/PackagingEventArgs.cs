using System;

namespace Kts.Updater.ViewModels
{
    /// <summary>
    /// Представляет аргументы события создания пакета обновления.
    /// </summary>
    internal sealed class PackagingEventArgs : EventArgs
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PackagingEventArgs"/>.
        /// </summary>
        /// <param name="package">Создаваемый пакет обновления.</param>
        public PackagingEventArgs(PackageViewModel package)
        {
            this.Package = package;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что создан ли пакет обновления.
        /// </summary>
        public bool IsCreated
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает создаваемый пакет обновления.
        /// </summary>
        public PackageViewModel Package
        {
            get;
        }

        #endregion
    }
}