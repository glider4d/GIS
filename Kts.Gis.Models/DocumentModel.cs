using System;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель документа.
    /// </summary>
    [Serializable]
    public sealed class DocumentModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DocumentModel"/>.
        /// </summary>
        /// <param name="id">Идентификатор документа.</param>
        /// <param name="objectId">Идентификатор объекта, которому принадлежит документ.</param>
        /// <param name="name">Название документа.</param>
        /// <param name="extension">Расширение документа.</param>
        /// <param name="type">Тип документа.</param>
        public DocumentModel(Guid id, Guid objectId, string name, string extension, DocumentType type)
        {
            this.Id = id;
            this.ObjectId = objectId;
            this.Name = name;
            this.Extension = extension;
            this.Type = type;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает расширение документа.
        /// </summary>
        public string Extension
        {
            get;
        }
        
        /// <summary>
        /// Возвращает или задает идентификатор документа.
        /// </summary>
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что сохранен ли документ.
        /// </summary>
        public bool IsSaved
        {
            get
            {
                return this.Id != DefaultId;
            }
        }

        /// <summary>
        /// Возвращает название документа.
        /// </summary>
        public string Name
        {
            get;
        }

        /// <summary>
        /// Возвращает идентификатор объекта, которому принадлежит документ.
        /// </summary>
        public Guid ObjectId
        {
            get;
        }

        /// <summary>
        /// Возвращает тип документа.
        /// </summary>
        public DocumentType Type
        {
            get;
        }

        #endregion

        #region Открытые статические свойства

        /// <summary>
        /// Возвращает идентификатор по умолчанию.
        /// </summary>
        public static Guid DefaultId
        {
            get
            {
                return Guid.Empty;
            }
        }

        #endregion
    }
}