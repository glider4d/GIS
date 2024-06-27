using System;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель визуального региона.
    /// </summary>
    [Serializable]
    public sealed class VisualRegionModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса 
        /// </summary>
        /// <param name="id">Идентификатор визуального региона.</param>
        /// <param name="name">Название визуального региона.</param>
        /// <param name="path">Путь, из которого состоит фигура визуального региона.</param>
        /// <param name="transform">Трансформация фигуры визуального региона.</param>
        public VisualRegionModel(int id, string name, string path, string transform)
        {
            this.Id = id;
            this.Name = name;
            this.Path = path;
            this.Transform = transform;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает идентификатор визуального региона.
        /// </summary>
        public int Id
        {
            get;
        }

        /// <summary>
        /// Возвращает название визуального региона.
        /// </summary>
        public string Name
        {
            get;
        }

        /// <summary>
        /// Возвращает путь, из которого состоит фигура визуального региона.
        /// </summary>
        public string Path
        {
            get;
        }

        /// <summary>
        /// Возвращает трансформацию фигуры визуального региона.
        /// </summary>
        public string Transform
        {
            get;
        }

        #endregion
    }
}