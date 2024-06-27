using Kts.Gis.Models;
using Kts.Gis.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления именованного объекта.
    /// </summary>
    [Serializable]
    internal sealed partial class NamedObjectViewModel : INamedObjectViewModel
    {
        #region Закрытые константы

        /// <summary>
        /// Текст пустой части названия.
        /// </summary>
        private const string emptyNamePart = "-";

        /// <summary>
        /// Разделитель названия объекта.
        /// </summary>
        private const string nameDivider = "; ";

        #endregion

        #region Закрытые поля

        /// <summary>
        /// Название объекта.
        /// </summary>
        private string name;

        /// <summary>
        /// Части названия объекта. Ключом является параметр, участвующий в составлении названия объекта, а значением - значение этого параметра.
        /// </summary>
        private Dictionary<ParameterModel, string> nameParts = new Dictionary<ParameterModel, string>(new ParameterModelEqualityComparer());

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис привязки представлений карты с моделями представлений.
        /// </summary>
        private readonly IMapBindingService mapBindingService;

        /// <summary>
        /// Значение, указывающее на то, что следует ли уведомлять представление об изменении названия объекта.
        /// </summary>
        private readonly bool notifyNameChange;

        /// <summary>
        /// Объект.
        /// </summary>
        private readonly IObjectViewModel obj;

        /// <summary>
        /// Параметризованный объект.
        /// </summary>
        private readonly IParameterizedObjectViewModel paramObj;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NamedObjectViewModel"/>.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="rawName">Необработанное название объекта.</param>
        /// <param name="notifyNameChange">Значение, указывающее на то, что следует ли уведомлять представление об изменении названия объекта.</param>
        /// <param name="mapBindingService">Сервис привязки представлений карты с моделями представлений.</param>
        public NamedObjectViewModel(IObjectViewModel obj, string rawName, bool notifyNameChange, IMapBindingService mapBindingService)
        {
            this.obj = obj;
            this.RawName = rawName;
            this.notifyNameChange = notifyNameChange;
            this.mapBindingService = mapBindingService;

            this.paramObj = obj as IParameterizedObjectViewModel;

            this.ProcessRawName(rawName);

            this.UpdateName();
        }

        #endregion
        
        #region Открытые методы

        /// <summary>
        /// Обрабатывает необработанное название объекта.
        /// </summary>
        /// <param name="rawName">Необработанное название объекта.</param>
        public void ProcessRawName(string rawName)
        {
            // Получаем параметры, участвующие в составлении названия объекта.
            var parameters = this.obj.Type.CaptionParameters;

            var div = new string[1]
            {
                "@@"
            };

            // Инициализируем части названия объекта.
            this.nameParts.Clear();
            foreach (var param in parameters)
                this.nameParts.Add(param, "");

            if (!string.IsNullOrEmpty(rawName))
            {
                var parts = rawName.Split(div, StringSplitOptions.RemoveEmptyEntries);

                var partDiv = new string[1]
                {
                    "~~"
                };

                foreach (var part in parts)
                {
                    var temp = part.Split(partDiv, StringSplitOptions.None);

                    var param = parameters.FirstOrDefault(x => x.Id == Convert.ToInt32(temp[0]));

                    if (param != null && this.nameParts.ContainsKey(param))
                        this.nameParts[param] = temp[1];
                    else
                    {
                        // Если встретится хоть одна ошибка, то очищаем части названия объекта и выходим из цикла, чтобы при обновлении имени мы смогли увидеть это.
                        this.nameParts.Clear();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Обновляет название объекта.
        /// </summary>
        public void UpdateName()
        {
            if (this.paramObj == null)
                return;

            string s = "";

            bool isFirst = true;

            foreach (var part in this.nameParts)
            {
                var param = part.Key;

                if (isFirst)
                {
                    if (param.HasPredefinedValues && !string.IsNullOrEmpty(part.Value))
                        s = param.GetTableEntry(part.Value).Value;
                    else
                        if (param.Alias == Alias.LineLength)
#warning Тут типо хитрый ход для маркировки протяженности трубы в названии. Молимся что никто не будет использовать следующую последовательность символов.
                        // Оставляем лазейку для подстановки длины трубы.
                        s = "!@#$%Alias.LineLength!@#$%";
                    else
                        s = part.Value;

                    isFirst = false;
                }
                else
#warning Попросили убрать пустые записи из названий объектов
                    //if (string.IsNullOrEmpty(part.Value))
                    //    s += nameDivider + emptyNamePart;
                    //else
                    if (!string.IsNullOrEmpty(part.Value))
                        if (param.HasPredefinedValues)
                            s += nameDivider + param.GetTableEntry(part.Value).Value;
                        else
                            if (param.Alias == Alias.LineLength)
#warning Тут типо хитрый ход для маркировки протяженности трубы в названии. Молимся что никто не будет использовать следующую последовательность символов.
                                // Оставляем лазейку для подстановки длины трубы.
                                s += nameDivider + "!@#$%Alias.LineLength!@#$%";
                            else
                                s += nameDivider + part.Value;
            }

            if (this.nameParts.Count == 0)
                s = "Ошибка";

            this.Name = s;
        }

        /// <summary>
        /// Обновляет название объекта.
        /// </summary>
        /// <param name="param">Измененный параметр.</param>
        /// <param name="value">Значение измененного параметра.</param>
        public void UpdateName(ParameterModel param, string value)
        {
            this.nameParts[param] = value;

            this.UpdateName();
        }

        #endregion
    }

    // Реализация INamedObjectViewModel.
    internal sealed partial class NamedObjectViewModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает название объекта.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
            private set
            {
                this.name = value;

                this.obj.OnPropertyChanged(nameof(this.Name));

                if (this.notifyNameChange)
                    this.mapBindingService.SetMapObjectViewValue(this.obj as IMapObjectViewModel, nameof(this.Name), value);
            }
        }

        /// <summary>
        /// Возвращает необработанное название объекта, полученное из источника данных.
        /// </summary>
        public string RawName
        {
            get;
        }

        #endregion
    }
}