using System.Collections.Generic;

namespace Kts.Settings
{
    /// <summary>
    /// Представляет интерфейс сервиса настроек.
    /// </summary>
    public interface ISettingService
    {
        #region Свойства

        /// <summary>
        /// Возвращает словарь настроек, где ключом является название настройки, а значением - ее значение.
        /// </summary>
        Dictionary<string, object> Settings
        {
            get;
        }

        #endregion

        #region Методы

        /// <summary>
        /// Сбрасывает настройки и удаляет соответствующий файл.
        /// </summary>
        /// <returns>true, если удалось выполнить сброс, иначе - false.</returns>
        bool FullReset();

        /// <summary>
        /// Загружает настройки.
        /// </summary>
        /// <returns>Возвращает значение, указывающее на то, что загружены ли настройки.</returns>
        bool Load();

        /// <summary>
        /// Регистрирует настройку.
        /// </summary>
        /// <param name="setting">Настройка.</param>
        void RegisterSetting(Setting setting);

        /// <summary>
        /// Сбрасывает настройки.
        /// </summary>
        void Reset();

        /// <summary>
        /// Сохраняет настройки.
        /// </summary>
        /// <returns>Возвращает значение, указывающее на то, что сохранены ли настройки.</returns>
        bool Save();

        #endregion
    }
}