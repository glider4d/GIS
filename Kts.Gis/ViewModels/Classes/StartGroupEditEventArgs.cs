using System;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет аргумент события запуска редактирования группы объектов.
    /// </summary>
    internal sealed class StartGroupEditEventArgs : EventArgs
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что можно ли запустить редактирование группы объектов.
        /// </summary>
        public bool CanStart
        {
            get;
            set;
        }

        #endregion
    }
}