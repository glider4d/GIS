using System;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель информации о топливе котельной.
    /// </summary>
    [Serializable]
    public sealed class FuelInfoModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="FuelInfoModel"/>.
        /// </summary>
        /// <param name="availableCount">Наличие на начало месяца.</param>
        /// <param name="monthLimit">Лимит (в месяц).</param>
        /// <param name="dayLimit">Лимит (в сутки).</param>
        /// <param name="incoming">Приход</param>
        /// <param name="consumption">Расход</param>
        /// <param name="endBalance">Остаток на конец периода.</param>
        /// <param name="moving">Значение перемещения.</param>
        /// <param name="provision">Обеспеченность.</param>
        public FuelInfoModel(double availableCount, double monthLimit, double dayLimit, double incoming, double consumption, double endBalance, double moving, string provision)
        {
            this.AvailableCount = availableCount;
            this.MonthLimit = monthLimit;
            this.DayLimit = dayLimit;
            this.Incoming = incoming;
            this.Consumption = consumption;
            this.EndBalance = endBalance;
            this.Moving = moving;
            this.Provision = this.FormatProvision(provision);

            this.HasDayBalance = false;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="FuelInfoModel"/>.
        /// </summary>
        /// <param name="availableCount">Наличие на начало месяца.</param>
        /// <param name="monthLimit">Лимит (в месяц).</param>
        /// <param name="dayLimit">Лимит (в сутки).</param>
        /// <param name="incoming">Приход</param>
        /// <param name="consumption">Расход</param>
        /// <param name="endBalance">Остаток на конец периода.</param>
        /// <param name="dayBalance">Остаток дней.</param>
        /// <param name="moving">Значение перемещения.</param>
        /// <param name="provision">Обеспеченность.</param>
        public FuelInfoModel(double availableCount, double monthLimit, double dayLimit, double incoming, double consumption, double endBalance, int dayBalance, double moving, string provision)
        {
            this.AvailableCount = availableCount;
            this.MonthLimit = monthLimit;
            this.DayLimit = dayLimit;
            this.Incoming = incoming;
            this.Consumption = consumption;
            this.EndBalance = endBalance;
            this.DayBalance = dayBalance;
            this.Moving = moving;
            this.Provision = this.FormatProvision(provision);

            this.HasDayBalance = true;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает наличие на начало месяца.
        /// </summary>
        public double AvailableCount
        {
            get;
        }

        /// <summary>
        /// Возвращает расход.
        /// </summary>
        public double Consumption
        {
            get;
        }

        /// <summary>
        /// Возвращает остаток дней.
        /// </summary>
        public int DayBalance
        {
            get;
        }

        /// <summary>
        /// Возвращает лимит (в сутки).
        /// </summary>
        public double DayLimit
        {
            get;
        }

        /// <summary>
        /// Возвращает остаток на конец периода.
        /// </summary>
        public double EndBalance
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеется ли остаток дней.
        /// </summary>
        public bool HasDayBalance
        {
            get;
        }

        /// <summary>
        /// Возвращает приход.
        /// </summary>
        public double Incoming
        {
            get;
        }

        /// <summary>
        /// Возвращает лимит (в месяц).
        /// </summary>
        public double MonthLimit
        {
            get;
        }

        /// <summary>
        /// Возвращает значение перемещения.
        /// </summary>
        public double Moving
        {
            get;
        }

        /// <summary>
        /// Возвращает обеспеченность.
        /// </summary>
        public string Provision
        {
            get;
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Форматирует значение обеспеченности.
        /// </summary>
        private string FormatProvision(string input)
        {
            DateTime result;

            if (DateTime.TryParse(input, out result))
                return result.ToShortDateString();
            
            return input;
        }

        #endregion
    }
}