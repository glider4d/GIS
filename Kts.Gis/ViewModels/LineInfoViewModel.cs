using Kts.Utilities;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления информации о рисуемой линии.
    /// </summary>
    internal sealed class LineInfoViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, что видна ли информация о рисуемой линии.
        /// </summary>
        private bool isVisible;

        /// <summary>
        /// Координата информации по X.
        /// </summary>
        private double left;

        /// <summary>
        /// Общая протяженность линии.
        /// </summary>
        private double length;

        /// <summary>
        /// Протяженность участка линии.
        /// </summary>
        private double segmentLength;

        /// <summary>
        /// Координата информации по Y.
        /// </summary>
        private double top;

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что видна ли информация о рисуемой линии.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                return this.isVisible;
            }
            set
            {
                this.isVisible = value;

                this.NotifyPropertyChanged(nameof(this.IsVisible));
            }
        }

        /// <summary>
        /// Возвращает или задает координату информации по X.
        /// </summary>
        public double Left
        {
            get
            {
                return this.left;
            }
            set
            {
                this.left = value;

                this.NotifyPropertyChanged(nameof(this.Left));
            }
        }

        /// <summary>
        /// Возвращает или задает общую протяженность линии.
        /// </summary>
        public double Length
        {
            get
            {
                return this.length;
            }
            set
            {
                this.length = value;

                this.NotifyPropertyChanged(nameof(this.Length));
            }
        }

        /// <summary>
        /// Возвращает или задает протяженность участка линии.
        /// </summary>
        public double SegmentLength
        {
            get
            {
                return this.segmentLength;
            }
            set
            {
                this.segmentLength = value;

                this.NotifyPropertyChanged(nameof(this.SegmentLength));
            }
        }

        /// <summary>
        /// Возвращает или задает координату информации по Y.
        /// </summary>
        public double Top
        {
            get
            {
                return this.top;
            }
            set
            {
                this.top = value;

                this.NotifyPropertyChanged(nameof(this.Top));
            }
        }

        #endregion
    }
}