using System.Collections.Generic;
using System;

namespace Kts.Gis.Services
{
    /// <summary>
    /// Представляет сервис доступа к функциям приложения.
    /// </summary>
    [Serializable]
    internal sealed class AccessService
    {
        #region Закрытые поля

        /// <summary>
        /// Список идентификаторов типов объектов, параметры которых пользователь может редактировать.
        /// </summary>
        private List<int> permittedTypes;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AccessService"/>.
        /// </summary>
        /// <param name="permittedRegions">Список идентификаторов регионов, которых пользователь может открывать.</param>
        /// <param name="permittedTypes">Список идентификаторов типов объектов, параметры которых пользователь может редактировать.</param>
        /// <param name="isAdmin">Значение, указывающее на то, что имеет ли пользователь администраторские права.</param>
        /// <param name="roleName">Название пользовательской роли.</param>
        /// <param name="canDraw">Значение, указывающее на то, что может ли пользователь рисовать в фактической схеме.</param>
        /// <param name="canDrawIS">Значение, указывающее на то, что может ли пользователь рисовать в инвестиционной схеме.</param>
        public AccessService(List<int> permittedRegions, List<int> permittedTypes, bool isAdmin, string roleName, bool canDraw, bool canDrawIS)
        {
            this.PermittedRegions = permittedRegions;
            this.permittedTypes = permittedTypes;
            this.IsAdmin = isAdmin;
            this.RoleName = roleName;
            this.CanDraw = canDraw;
            this.CanDrawIS = canDrawIS;
        }

        #endregion

        #region Открытые свойства
                
        /// <summary>
        /// Возвращает значение, указывающее на то, что может ли пользователь рисовать в фактической схеме.
        /// </summary>
        public bool CanDraw
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что может ли пользователь рисовать в инвестиционной схеме.
        /// </summary>
        public bool CanDrawIS
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что может ли пользователь изменять документы.
        /// </summary>
        public bool CanModifyDocuments
        {
            get
            {
                return this.CanDraw;
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что может ли пользователь видеть идентификатор населенного пункта.
        /// </summary>
        public bool CanViewCityId
        {
            get
            {
                return this.IsAdmin;
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что может ли пользователь видеть параметр, представляющий идентификатор объекта.
        /// </summary>
        public bool CanViewIdParameter
        {
            get
            {
                return this.IsAdmin;
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что может ли пользователь видеть идентификатор узла.
        /// </summary>
        public bool CanViewNodeId
        {
            get
            {
                return this.IsAdmin;
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что может ли пользователь видеть идентификатор параметра.
        /// </summary>
        public bool CanViewParameterId
        {
            get
            {
                return this.IsAdmin;
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеет ли пользователь администраторские права.
        /// </summary>
        public bool IsAdmin
        {
            get;
        }

        /// <summary>
        /// Возвращает список идентификаторов регионов, которых пользователь может открывать.
        /// </summary>
        public List<int> PermittedRegions
        {
            get;
        }

        /// <summary>
        /// Возвращает название пользовательской роли.
        /// </summary>
        public string RoleName
        {
            get;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Возвращает значение, указывающее на то, что может ли пользователь изменить настройки надписей карты.
        /// </summary>
        /// <param name="isActual">Значение, указывающее на то, что является ли схема актуальной.</param>
        /// <returns>true, если может, иначе - false.</returns>
        public bool CanChangeMapCaptions(bool isActual)
        {
            if (isActual)
                return this.CanDraw;

            return false;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что может ли пользователь изменить настройки карты.
        /// </summary>
        /// <param name="isActual">Значение, указывающее на то, что является ли схема актуальной.</param>
        /// <returns>true, если может, иначе - false.</returns>
        public bool CanChangeMapSettings(bool isActual)
        {
            if (isActual)
                return this.CanDraw;

            return false;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что может ли пользователь проверить интеграцию с базовыми программами.
        /// </summary>
        /// <param name="isActual">Значение, указывающее на то, что является ли схема актуальной.</param>
        /// <returns>true, если может, иначе - false.</returns>
        public bool CanCheckBaseProgram(bool isActual)
        {
            return true;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что может ли пользователь проверить наличие ошибок.
        /// </summary>
        /// <param name="isActual">Значение, указывающее на то, что является ли схема актуальной.</param>
        /// <returns>true, если может, иначе - false.</returns>
        public bool CanCheckErrors(bool isActual)
        {
            if (isActual)
                return this.CanDraw;

            return false;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что может ли пользователь уменьшить размер надписи.
        /// </summary>
        /// <param name="isActual">Значение, указывающее на то, что является ли схема актуальной.</param>
        /// <returns>true, если может, иначе - false.</returns>
        public bool CanDecreaseLabelSize(bool isActual)
        {
            if (isActual)
                return this.CanDraw;

            return this.CanDrawIS;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что может ли пользователь удалить надпись.
        /// </summary>
        /// <param name="isActual">Значение, указывающее на то, что является ли схема актуальной.</param>
        /// <returns>true, если может, иначе - false.</returns>
        public bool CanDeleteLabel(bool isActual)
        {
            if (isActual)
                return this.CanDraw;

            return this.CanDrawIS;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что может ли пользователь искать углы поворота.
        /// </summary>
        /// <param name="isActual">Значение, указывающее на то, что является ли схема актуальной.</param>
        /// <returns>true, если может, иначе - false.</returns>
        public bool CanFindBendNodes(bool isActual)
        {
            if (isActual)
                return this.CanDraw;

            return false;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что может ли пользователь искать свободные узлы.
        /// </summary>
        /// <param name="isActual">Значение, указывающее на то, что является ли схема актуальной.</param>
        /// <returns>true, если может, иначе - false.</returns>
        public bool CanFindFreeNodes(bool isActual)
        {
            if (isActual)
                return this.CanDraw;

            return false;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что может ли пользователь увеличить размер надписи.
        /// </summary>
        /// <param name="isActual">Значение, указывающее на то, что является ли схема актуальной.</param>
        /// <returns>true, если может, иначе - false.</returns>
        public bool CanIncreaseLabelSize(bool isActual)
        {
            if (isActual)
                return this.CanDraw;

            return this.CanDrawIS;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что может ли пользователь переприсоединять узлы.
        /// </summary>
        /// <param name="isActual">Значение, указывающее на то, что является ли схема актуальной.</param>
        /// <returns>true, если может, иначе - false.</returns>
        public bool CanReattachNodes(bool isActual)
        {
            if (isActual)
                return this.CanDraw;

            return false;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что может ли пользователь сбросить настройки надписи.
        /// </summary>
        /// <param name="isActual">Значение, указывающее на то, что является ли схема актуальной.</param>
        /// <returns>true, если может, иначе - false.</returns>
        public bool CanResetLabel(bool isActual)
        {
            if (isActual)
                return this.CanDraw;

            return this.CanDrawIS;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что разрешена ли пользователю работа с заданным регионом.
        /// </summary>
        /// <param name="id">Идентификатор региона.</param>
        /// <returns>true, если разрешена, иначе - false.</returns>
        public bool IsRegionPermitted(int id)
        {
            return this.PermittedRegions.Contains(id);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что разрешена ли пользователю работа с заданным типом объекта.
        /// </summary>
        /// <param name="id">Идентификатор типа объекта.</param>
        /// <returns>true, если разрешена, иначе - false.</returns>
        public bool IsTypePermitted(int id)
        {
            return this.permittedTypes.Contains(id);
        }

        #endregion
    }
}