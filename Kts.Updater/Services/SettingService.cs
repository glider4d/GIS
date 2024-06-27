using Microsoft.Win32;
using System;

namespace Kts.Updater.Services
{
    /// <summary>
    /// Представляет сервис настроек.
    /// </summary>
    internal sealed class SettingService
    {
        #region Закрытые константы

        /// <summary>
        /// Путь к разделу с настройками.
        /// </summary>
        private const string path = "HKEY_CURRENT_USER\\Software\\Kts.Updater";

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает название последней использованной базы данных.
        /// </summary>
        public string LastDatabase
        {
            get
            {
                return Convert.ToString(this.ReadFromRegistry(nameof(this.LastDatabase)));
            }
            set
            {
                this.SaveToRegistry(nameof(this.LastDatabase), value, RegistryValueKind.String);
            }
        }

        /// <summary>
        /// Возвращает или задает адрес последнего использованного сервера.
        /// </summary>
        public string LastServer
        {
            get
            {
                return Convert.ToString(this.ReadFromRegistry(nameof(this.LastServer)));
            }
            set
            {
                this.SaveToRegistry(nameof(this.LastServer), value, RegistryValueKind.String);
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выполняет чтение значения заданного параметра из реестра.
        /// </summary>
        /// <param name="parameter">Параметр, значение которого нужно прочитать.</param>
        /// <returns>Значение.</returns>
        private object ReadFromRegistry(string parameter)
        {
            return Registry.GetValue(path, parameter, null);
        }

        /// <summary>
        /// Выполняет запись значения заданного параметра в реестр.
        /// </summary>
        /// <param name="parameter">Параметр, значение которого нужно записать.</param>
        /// <param name="value">Записываемое значение.</param>
        /// <param name="type">Тип записываемого значения.</param>
        private void SaveToRegistry(string parameter, object value, RegistryValueKind type)
        {
            Registry.SetValue(path, parameter, value, type);
        }

        #endregion
    }
}