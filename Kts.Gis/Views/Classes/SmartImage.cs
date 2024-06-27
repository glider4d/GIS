using System.Windows;
using System.Windows.Controls;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет умное изображение, уведомляющее об изменении своего источника.
    /// </summary>
    internal sealed class SmartImage : Image
    {
        #region Открытые статические неизменяемые события

        /// <summary>
        /// Событие изменения источника изображения.
        /// </summary>
        public static readonly RoutedEvent SourceChangedEvent = EventManager.RegisterRoutedEvent("SourceChanged", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(SmartImage));

        #endregion

        #region Статические конструкторы

        /// <summary>
        /// Инициализирует статические члены класса <see cref="SmartImage"/>.
        /// </summary>
        static SmartImage()
        {
            SourceProperty.OverrideMetadata(typeof(SmartImage), new FrameworkPropertyMetadata(SourcePropertyChanged));
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Добавляет или удаляет обработчика события <see cref="SourceChangedEvent"/>.
        /// </summary>
        public event RoutedEventHandler SourceChanged
        {
            add
            {
                AddHandler(SourceChangedEvent, value);
            }
            remove
            {
                RemoveHandler(SourceChangedEvent, value);
            }
        }

        #endregion

        #region Статические обработчики событий

        /// <summary>
        /// Обрабатывает изменение свойства <see cref="SourceProperty"/>.
        /// </summary>
        /// <param name="obj">Источник.</param>
        /// <param name="e">Аргументы.</param>
        private static void SourcePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var image = source as SmartImage;

            if (image != null)
                image.RaiseEvent(new RoutedEventArgs(SourceChangedEvent));
        }

        #endregion
    }
}