using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Kts.Utilities
{
    /// <summary>
    /// Представляет натуральный сортировщик.
    /// </summary>
    /// <typeparam name="T">Тип сортируемых данных.</typeparam>
    public sealed partial class NaturalSortComparer<T> : IComparer<string>, IDisposable
    {
        #region Закрытые поля
    
        /// <summary>
        /// Значение, указывающее на то, что высвобождены ли ресурсы.
        /// </summary>
        private bool isDisposed;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Значение, указывающее на то, что происходит ли сортировка по возрастанию.
        /// </summary>
        private readonly bool isAscending;

        /// <summary>
        /// ???.
        /// </summary>
        private readonly Dictionary<string, string[]> table = new Dictionary<string, string[]>();

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NaturalSortComparer{T}"/>.
        /// </summary>
        /// <param name="inAscendingOrder">Значение, указывающее на то, что происходит ли сортировка по возрастанию.</param>
        public NaturalSortComparer(bool isAscending = true)
        {
            this.isAscending = isAscending;
        }

        #endregion

        #region Деструкторы

        /// <summary>
        /// Финализирует экземпляр класса <see cref="NaturalSortComparer{T}"/>.
        /// </summary>
        ~NaturalSortComparer()
        {
            this.Dispose(false);
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Высвобождает ресурсы.
        /// </summary>
        /// <param name="isDisposing">Значение, указывающее на то, что нужно ли высвободить ресурсы.</param>
        private void Dispose(bool isDisposing)
        {
            if (!this.isDisposed)
            {
                if (isDisposing)
                    this.table.Clear();

                this.isDisposed = true;
            }
        }

        /// <summary>
        /// Выполняет сравнение двух строк.
        /// </summary>
        /// <param name="left">Первая строка.</param>
        /// <param name="right">Вторая строка.</param>
        /// <returns>-1, если текущий объект меньше заданного, 1, если наоборот и 0, если они равны.</returns>
        private int PartCompare(string left, string right)
        {
            int x, y;

            if (!int.TryParse(left, out x))
                return left.CompareTo(right);

            if (!int.TryParse(right, out y))
                return left.CompareTo(right);

            return x.CompareTo(y);
        }

        #endregion
    }

    // Реализация IComparer.
    public sealed partial class NaturalSortComparer<T>
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет сравнение двух строк.
        /// </summary>
        /// <param name="x">Первая строка.</param>
        /// <param name="y">Вторая строка.</param>
        /// <returns>-1, если текущий объект меньше заданного, 1, если наоборот и 0, если они равны.</returns>
        public int Compare(string x, string y)
        {
            if (x == y)
                return 0;

            string[] x1, y1;

            if (!table.TryGetValue(x, out x1))
            {
                x1 = Regex.Split(x.Replace(" ", ""), "([0-9]+)");

                table.Add(x, x1);
            }

            if (!table.TryGetValue(y, out y1))
            {
                y1 = Regex.Split(y.Replace(" ", ""), "([0-9]+)");

                table.Add(y, y1);
            }

            int returnVal;

            for (int i = 0; i < x1.Length && i < y1.Length; i++)
                if (x1[i] != y1[i])
                {
                    returnVal = this.PartCompare(x1[i], y1[i]);

                    return this.isAscending ? returnVal : -returnVal;
                }

            if (y1.Length > x1.Length)
                returnVal = 1;
            else
                if (x1.Length > y1.Length)
                    returnVal = -1;
                else
                    returnVal = 0;

            return this.isAscending ? returnVal : -returnVal;
        }

        #endregion
    }

    // Реализация IDisposable.
    public sealed partial class NaturalSortComparer<T>
    {
        #region Открытые методы

        /// <summary>
        /// Высвобождает ресурсы.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}