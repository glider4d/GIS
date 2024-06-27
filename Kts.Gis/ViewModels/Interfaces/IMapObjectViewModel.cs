using System.Windows;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет интерфейс модели представления объекта карты.
    /// </summary>
    internal interface IMapObjectViewModel
    {
        #region Свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что размещен ли объект на карте.
        /// </summary>
        bool IsPlaced
        {
            get;
            set;
        }

        #endregion

        #region Методы

        /// <summary>
        /// Регистрирует связь с представлением.
        /// </summary>
        void RegisterBinding();

        /// <summary>
        /// Вращает объект карты на заданный угол.
        /// </summary>
        /// <param name="angle">Угол.</param>
        /// <param name="origin">Точка, относительно которой выполняется поворот.</param>
        void Rotate(double angle, Point origin);

        /// <summary>
        /// Задает новый масштаб объекта карты.
        /// </summary>
        /// <param name="angle">Масштаб.</param>
        /// <param name="origin">Точка, относительно которой выполняется масштабирование.</param>
        void Scale(double scale, Point origin);

        /// <summary>
        /// Сдвигает объект карты на заданную дельту.
        /// </summary>
        /// <param name="delta">Дельта.</param>
        void Shift(Point delta);

        /// <summary>
        /// Убирает связь с представлением.
        /// </summary>
        void UnregisterBinding();

        #endregion
    }
}