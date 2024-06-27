using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfCustomMessageBox.Classes
{
    /// <summary>
    /// Представляет расширения для классов <see cref="ImageSource"/> и <see cref="string"/>.
    /// </summary>
    internal static class Extensions
    {
        #region Открытые статические методы

        /// <summary>
        /// Преобразовывает экземпляр класса <see cref="Icon"/> в экземпляр класса <see cref="ImageSource"/>.
        /// </summary>
        /// <param name="icon">Иконка.</param>
        /// <returns>Источник изображения.</returns>
        public static ImageSource ToImageSource(this Icon icon)
        {
            return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        /// <summary>
        /// Пробует добавить символ подчеркивания во входную строку.
        /// </summary>
        /// <param name="input">Входная строка.</param>
        /// <returns>Входная строка с символом подчеркивания.</returns>
        public static string TryAddKeyboardAccellerator(this string input)
        {
            string accellerator = "_";

            if (input.Contains(accellerator))
                return input;

            return accellerator + input;
        }

        #endregion
    }
}