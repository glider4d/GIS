using Kts.Gis.Models;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Kts.Gis.Substrates
{
    /// <summary>
    /// Представляет сервис подложек.
    /// </summary>
    public sealed class SubstrateService
    {
        #region Закрытые константы

        /// <summary>
        /// Название папки с кешированными файлами-изображениями подложек.
        /// </summary>
        private const string substrateFolderName = "Substrates";
        private const string thumbnailsFolderName = "Thumbnails";

        #endregion

        #region Закрытые поля

        /// <summary>
        /// Словарь кешированных подложек, где ключом выступает идентификатор населенного пункта, которому принадлежит подложка, а значением - сама подложка.
        /// </summary>
        private Dictionary<int, SubstrateModel> substrates = new Dictionary<int, SubstrateModel>();

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Полный путь к файлу с данными кешированных подложек.
        /// </summary>
        private readonly string fileName;

        /// <summary>
        /// Полный путь к папке с кешированными файлами-изображениями подложек.
        /// </summary>
        private readonly string folderName;

        private readonly string folderThumbnailsName;


        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SubstrateService"/>.
        /// </summary>
        /// <param name="folderName">Полный путь к папке с файлом данных кешированных подложек.</param>
        /// <param name="fileName">Название файла с данными кешированных подложек.</param>
        public SubstrateService(string folderName, string fileName)
        {
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);

            this.fileName = folderName + "\\" + fileName;
            this.folderName = folderName + "\\" + substrateFolderName;
            this.folderThumbnailsName = folderName + "\\" + thumbnailsFolderName;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Добавляет подложку в кеш.
        /// </summary>
        /// <param name="substrate">Подложка.</param>
        public void AddSubstrate(SubstrateModel substrate)
        {
            if (this.substrates.ContainsKey(substrate.City.Id))
                return;

            this.substrates.Add(substrate.City.Id, substrate);
        }


        public bool CasheThumbnails(SubstrateModel substrate, string fullFileName, Stream fileStream, out string newFileFullName)
        {
            bool result = false;
            newFileFullName = "";
            try
            {
                if (!Directory.Exists(this.folderThumbnailsName))
                    Directory.CreateDirectory(this.folderThumbnailsName);

                string fileName = Path.GetFileName(fullFileName);

                string newFileName = this.folderThumbnailsName + "\\" + fileName;

                if (File.Exists(newFileName))
                {
                    File.Delete(newFileName);
                }

                var newFileStream = File.Create(newFileName);

                fileStream.CopyTo(newFileStream);
                newFileStream.Close();

                newFileFullName = newFileName;

                result = true;
            }
            catch
            {

            }

            return result;
        }


        public string GetThumbnailFile(int id)
        {
            string fileName = "";

            try
            {

                fileName = Directory.GetFiles(folderThumbnailsName).FirstOrDefault(x =>
                {
                    var idFromFolder = Convert.ToInt32(Path.GetFileNameWithoutExtension(x));
                    return id == idFromFolder;
                }
                );
            }
            catch 
            {

            }
            return fileName;
        }
        public Stream GetStreamThumbnail(int id)
        {
            Stream result = null;
            string fileName = "";
            try
            {

                fileName = Directory.GetFiles(folderThumbnailsName).FirstOrDefault(x =>
                {
                    var idFromFolder = Convert.ToInt32(Path.GetFileNameWithoutExtension(x));
                    return id == idFromFolder;
                }
                );

                if (fileName != null && fileName.Length > 0)
                {
                    MemoryStream stream = new MemoryStream();
                    var bytes = File.ReadAllBytes(fileName);
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Position = 0;
                    result = stream;
                }
            }
            catch
            {

            }
            return result;
        }


        private Dictionary<string, Stream> UploadFile(string file)
        {
            MemoryStream stream = new MemoryStream();
            Dictionary<string, Stream> result = null;
            try
            {
                var bytes = File.ReadAllBytes(file);
                stream.Write(bytes, 0, bytes.Length);
                stream.Position = 0;
                result = (new Dictionary<string, Stream>());
                result.Add(file, stream);
            }
            catch
            {

            }
            return result;
        }

        public Dictionary<string, List<Dictionary<string, Stream>>> GetImagesFilesSubscrabes(SubstrateModel substrate, string sourceFolderName)
        {
            Dictionary<string, List<Dictionary<string, Stream>>> filesStreamResult = new Dictionary<string, List<Dictionary<string, Stream>>>();
            try
            {
                if (!Directory.Exists(this.folderName))
                    // Если папка кешированных файлов-изображений не существует, то создаем ее.
                    Directory.CreateDirectory(this.folderName);
                foreach (var directory in Directory.GetDirectories(sourceFolderName).Where(x =>
                {
                    var directoryName = Path.GetFileName(x);

                    return directoryName == substrate.City.Id.ToString() || directoryName.StartsWith(substrate.City.Id.ToString() + ".");
                }))
                {
                    var directoryName = this.folderName + "\\" + Path.GetFileName(directory);
                    //result.Add(directoryName);
                    List<Dictionary<string, Stream>> fileStream = new List<Dictionary<string, Stream>>();
                    foreach(var file in Directory.GetFiles(directory))
                    {
                        var resultUploadFile = UploadFile(file);
                        if ( resultUploadFile != null)
                            fileStream.Add(resultUploadFile);
                        
                        //filesStream.a
                    }
                    if (fileStream != null && fileStream.Count > 0)
                    {
                        filesStreamResult.Add(Path.GetFileName(directory), fileStream);
                        ///////////////break;
                    }


                    //foreach (var file in Directory.GetFiles(directory))
                    //  ;//запихнуть в fileString// File.Copy(file, directoryName + "\\" + Path.GetFileName(file), true);
                }
            }
            catch
            {

            }
            return filesStreamResult;
        }

        //Получить список файлов подлжоки
        public List<string> GetImagesFiles(SubstrateModel substrate, string sourceFolderName)
        {
            List<string> result = new List<string>();
            try
            {
                if (!Directory.Exists(this.folderName))
                    // Если папка кешированных файлов-изображений не существует, то создаем ее.
                    Directory.CreateDirectory(this.folderName);
                foreach (var directory in Directory.GetDirectories(sourceFolderName).Where(x =>
                {
                    var directoryName = Path.GetFileName(x);

                    return directoryName == substrate.City.Id.ToString() || directoryName.StartsWith(substrate.City.Id.ToString() + ".");
                }))
                {
                    var directoryName = this.folderName + "\\" + Path.GetFileName(directory);
                    result.Add(directoryName);
                    //foreach (var file in Directory.GetFiles(directory))
                    //  ;//запихнуть в fileString// File.Copy(file, directoryName + "\\" + Path.GetFileName(file), true);
                }
            }
            catch
            {
                //return result;
            }
            return result;

        }

        /// <summary>
        /// Кеширует файлы-изображения подложки.
        /// </summary>
        /// <param name="substrate">Подложка.</param>
        /// <param name="sourceFolderName">Название папки, содержащей папки с подложками.</param>
        /// <returns>Возвращает значение, указывающее на то, что выполнено ли кеширование файлов-изображений.</returns>
        public bool CacheImages(SubstrateModel substrate, string sourceFolderName)
        {
            try
            {
                if (!Directory.Exists(this.folderName))
                    // Если папка кешированных файлов-изображений не существует, то создаем ее.
                    Directory.CreateDirectory(this.folderName);

                foreach (var directory in Directory.GetDirectories(sourceFolderName).Where(x =>
                {
                    var directoryName = Path.GetFileName(x);

                    return directoryName == substrate.City.Id.ToString() || directoryName.StartsWith(substrate.City.Id.ToString() + ".");
                }))
                {
                    var directoryName = this.folderName + "\\" + Path.GetFileName(directory);

                    if (Directory.Exists(directoryName))
                    {
                        foreach (var fileName in Directory.GetFiles(directoryName))
                            File.Delete(fileName);

                        Directory.Delete(directoryName);
                    }

                    Directory.CreateDirectory(directoryName);

                    foreach (var file in Directory.GetFiles(directory))
                        File.Copy(file, directoryName + "\\" + Path.GetFileName(file), true);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Удаляет все кешированные файлы-изображения подложек.
        /// </summary>
        /// <returns>Возвращает значение, указывающее на то, что удалены ли кешированные файлы-изображения.</returns>
        public bool DeleteCachedImages()
        {
            var hasErrors = false;

            if (!Directory.Exists(this.folderName))
                // Если папка кешированных файлов-изображений не существует, то создаем ее.
                Directory.CreateDirectory(this.folderName);

            foreach (var directory in Directory.GetDirectories(this.folderName))
            {
                try
                {
                    if (Directory.Exists(directory))
                        Directory.Delete(directory, true);
                }
                catch
                {
                    hasErrors = true;
                }
            }

            return !hasErrors;
        }

        /// <summary>
        /// Удаляет кешированные файлы-изображения подложки заданного населенного пункта.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <returns>Возвращает значение, указывающее на то, что удалены ли кешированные файлы-изображения.</returns>
        public bool DeleteCachedImages(TerritorialEntityModel city)
        {
            var hasErrors = false;

            if (!Directory.Exists(this.folderName))
                // Если папка кешированных файлов-изображений не существует, то создаем ее.
                Directory.CreateDirectory(this.folderName);

            foreach (var directory in Directory.GetDirectories(this.folderName).Where(x =>
            {
                var directoryName = Path.GetFileName(x);

                return directoryName == city.Id.ToString() || directoryName.StartsWith(city.Id.ToString() + ".");
            }))
            {
                try
                {
                    if (Directory.Exists(directory))
                        Directory.Delete(directory, true);
                }
                catch
                {
                    hasErrors = true;
                }
            }

            return !hasErrors;
        }

        /// <summary>
        /// Сбрасывает данные кешированных подложек и удаляет соответствующий файл.
        /// </summary>
        /// <returns>true, если удалось выполнить сброс, иначе - false.</returns>
        public bool FullReset()
        {
            try
            {
                // Удаляем файл с данными о подложках.
                File.Delete(this.fileName);

                // И все кешированные файлы-изображения подложек.
                if (!this.DeleteCachedImages())
                    return false;
            }
            catch
            {
                return false;
            }
            
            this.Reset();

            return true;
        }

        /// <summary>
        /// Возвращает названия кешированных файлов-изображений заданного населенного пункта.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <returns>Названия кешированных файлов-изображений.</returns>
        public string[][] GetCachedImageFileNames(TerritorialEntityModel city)
        {
            if (!this.substrates.ContainsKey(city.Id))
                return null;

            var directories = Directory.GetDirectories(this.folderName).Where(x =>
            {
                var directoryName = Path.GetFileName(x);

                return directoryName == city.Id.ToString() || directoryName.StartsWith(city.Id.ToString() + ".");
            }).ToArray();

            var result = new string[directories.Length][];

            for (int i = 0; i < directories.Length; i++)
                result[i] = Directory.GetFiles(directories[i]);

            return result;
        }

        /// <summary>
        /// Возвращает размерность кешированной подложки заданного населенного пункта.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <returns>Размерность кешированной подложки.</returns>
        public Size GetSubstrateDimension(TerritorialEntityModel city)
        {
            if (!this.substrates.ContainsKey(city.Id))
                return Size.Empty;

            return new Size(this.substrates[city.Id].ColumnCount, this.substrates[city.Id].RowCount);
        }

        /// <summary>
        /// Возвращает размер кешированной подложки заданного населенного пункта.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <returns>Размер кешированной подложки.</returns>
        public Size GetSubstrateSize(TerritorialEntityModel city)
        {
            if (!this.substrates.ContainsKey(city.Id))
                return Size.Empty;

            return new Size(this.substrates[city.Id].Width, this.substrates[city.Id].Height);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеет ли подложка заданного населенного пункта изображение.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        /// <returns>Значение, указывающее на то, что имеет ли подложка заданного населенного пункта изображение.</returns>
        public bool HasImageSource(TerritorialEntityModel city)
        {
            if (!this.substrates.ContainsKey(city.Id))
                return false;

            return this.substrates[city.Id].HasImageSource;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что новее ли заданная подложка чем ее кешированная версия.
        /// </summary>
        /// <param name="substrate">Подложка.</param>
        /// <returns>Значение, указывающее на то, что новее ли заданная подложка чем ее кешированная версия.</returns>
        public bool IsSubstrateNewer(SubstrateModel substrate)
        {
            if (!this.substrates.ContainsKey(substrate.City.Id))
                // Если нет кешированной версии подложки, то возвращаем true.
                return true;

            return substrate.LastUpdate.CompareTo(this.substrates[substrate.City.Id].LastUpdate) > 0;
        }

        /// <summary>
        /// Загружает данные кешированных подложек из файла.
        /// </summary>
        /// <returns>Значение, указывающее на то, что загружены ли данные кешированных подложек.</returns>
        public bool Load()
        {
            try
            {
                using (var stream = File.Open(this.fileName, FileMode.OpenOrCreate))
                    if (stream.Length > 0)
                    {
                        var formatter = new BinaryFormatter();

                        this.substrates = formatter.Deserialize(stream) as Dictionary<int, SubstrateModel>;
                    }
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Удаляет подложку из кеша.
        /// </summary>
        /// <param name="city">Населенный пункт.</param>
        public void RemoveSubstrate(TerritorialEntityModel city)
        {
            this.substrates.Remove(city.Id);
        }

        /// <summary>
        /// Сбрасывает данные кешированных подложек.
        /// </summary>
        public void Reset()
        {
            this.substrates = new Dictionary<int, SubstrateModel>();
        }

        /// <summary>
        /// Сохраняет данные кешированных подложек.
        /// </summary>
        /// <returns>Значение, указывающее на то, что сохранены ли данные кешированных подложек.</returns>
        public bool Save()
        {
            try
            {
                using (var stream = File.Open(this.fileName, FileMode.OpenOrCreate))
                {
                    var formatter = new BinaryFormatter();

                    formatter.Serialize(stream, this.substrates);
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