using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Kts.Settings
{
    /// <summary>
    /// Представляет сервис настроек, хранящихся в бинарном файле.
    /// </summary>
    public sealed partial class BinarySettingService : ISettingService
    {
        #region Закрытые поля

        /// <summary>
        /// Зарегистрированные настройки.
        /// </summary>
        private Dictionary<string, Setting> registeredSettings = new Dictionary<string, Setting>();

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Полный путь к файлу с настройками.
        /// </summary>
        private readonly string fileName;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="BinarySettingService"/>.
        /// </summary>
        /// <param name="folderName">Полный путь к папке с файлом настроек.</param>
        /// <param name="fileName">Название файла с настройками.</param>
        public BinarySettingService(string folderName, string fileName)
        {
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);

            this.fileName = folderName + "\\" + fileName;
        }

        #endregion
    }

    // Реализация ISettingService.
    public sealed partial class BinarySettingService
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает словарь настроек, где ключом является название настройки, а значением - ее значение.
        /// </summary>
        public Dictionary<string, object> Settings
        {
            get;
        } = new Dictionary<string, object>();

        #endregion

        #region Открытые методы

        /// <summary>
        /// Сбрасывает настройки и удаляет соответствующий файл.
        /// </summary>
        /// <returns>true, если удалось выполнить сброс, иначе - false.</returns>
        public bool FullReset()
        {
            try
            {
                // Удаляем файл с настройками.
                File.Delete(this.fileName);
            }
            catch
            {
                return false;
            }

            this.Reset();

            return true;
        }

        /// <summary>
        /// Загружает настройки.
        /// </summary>
        /// <returns>Возвращает значение, указывающее на то, что загружены ли настройки.</returns>
        public bool Load()
        {
            try
            {
                using (var stream = File.Open(this.fileName, FileMode.OpenOrCreate))
                    if (stream.Length > 0)
                    {
                        var formatter = new BinaryFormatter();

                        var temp = formatter.Deserialize(stream) as Dictionary<string, object>;

                        // Запоминаем только зарегистрированные настройки.
                        this.Settings.Clear();
                        foreach (var setting in this.registeredSettings.Values)
                            if (temp.ContainsKey(setting.Name) && temp[setting.Name].GetType() == setting.Type)
                                // Если в файле сохранена текущая настройка и тип ее значения совпадает с типом значения зарегистрированной настройки, то запоминаем ее.
                                this.Settings.Add(setting.Name, temp[setting.Name]);
                            else
                                // Иначе берем значение зарегистрированной настройки по умолчанию.
                                this.Settings.Add(setting.Name, setting.DefaultValue);
                    }
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Регистрирует настройку.
        /// </summary>
        /// <param name="setting">Настройка.</param>
        public void RegisterSetting(Setting setting)
        {
            if (this.registeredSettings.ContainsKey(setting.Name))
                throw new ArgumentException(nameof(setting));

            this.registeredSettings.Add(setting.Name, setting);
        }

        /// <summary>
        /// Сбрасывает настройки.
        /// </summary>
        public void Reset()
        {
            this.Settings.Clear();

            foreach (var setting in this.registeredSettings.Values)
                this.Settings.Add(setting.Name, setting.DefaultValue);
        }

        /// <summary>
        /// Сохраняет настройки.
        /// </summary>
        /// <returns>Возвращает значение, указывающее на то, что сохранены ли настройки.</returns>
        public bool Save()
        {
            try
            {
                using (var stream = File.Open(this.fileName, FileMode.OpenOrCreate))
                {
                    var formatter = new BinaryFormatter();

                    formatter.Serialize(stream, this.Settings);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}