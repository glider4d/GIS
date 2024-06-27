using System.Windows;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет представлении информации об обновлении.
    /// </summary>
    internal sealed partial class UpdateInfoView : Window
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="UpdateInfoView"/>.
        /// </summary>
        public UpdateInfoView()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Открытые статические свойства

        /// <summary>
        /// Возвращает последнюю версию информации об обновлении.
        /// </summary>
        public static long LatestVersion
        {
            get
            {
                return 117;
            }
        }

        #endregion
    }
}