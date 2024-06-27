using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет тип объекта.
    /// </summary>
    [Serializable]
    public sealed class ObjectType
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Словарь, используемый для определения видимости параметров в зависимости от значений другого параметра.
        /// </summary>
        private readonly Dictionary<ParameterModel, Tuple<ParameterModel, List<int>>> paramVisibilities = new Dictionary<ParameterModel, Tuple<ParameterModel, List<int>>>();

        #endregion

        #region Закрытые статические неизменяемые поля

        /// <summary>
        /// Сравниватель по идентификатору типа объекта.
        /// </summary>
        private static readonly IEqualityComparer<int> typeIdComparer = EqualityComparer<int>.Default;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ObjectType"/>.
        /// </summary>
        /// <param name="typeId">Идентификатор типа объекта.</param>
        /// <param name="name">Название типа.</param>
        /// <param name="singularName">Название типа в единственном числе.</param>
        /// <param name="objectKind">Вид объектов данного типа.</param>
        /// <param name="color">Цвет объектов данного типа.</param>
        /// <param name="inactiveColor">Цвет неактивных объектов данного типа.</param>
        /// <param name="canBeChanged">Значение, указывающее на то, что можно ли изменить тип объекта на другой тип.</param>
        public ObjectType(int typeId, string name, string singularName, ObjectKind objectKind, Color color, Color inactiveColor, bool canBeChanged)
        {
            this.TypeId = typeId;
            this.Name = name;
            this.SingularName = singularName;
            this.ObjectKind = objectKind;
            this.Color = color;
            this.InactiveColor = inactiveColor;
            this.CanBeChanged = canBeChanged;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает значение, указывающее на то, что можно ли изменить тип объекта на другой тип.
        /// </summary>
        public bool CanBeChanged
        {
            get;
        }

        /// <summary>
        /// Возвращает список параметров, отвечающих за название объекта.
        /// </summary>
        public List<ParameterModel> CaptionParameters
        {
            get;
        } = new List<ParameterModel>();

        /// <summary>
        /// Возвращает дочерние типы.
        /// </summary>
        public List<ObjectType> Children
        {
            get;
        } = new List<ObjectType>();

        /// <summary>
        /// Возвращает цвет объектов данного типа.
        /// </summary>
        public Color Color
        {
            get;
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеет ли тип объекта дочерние типы.
        /// </summary>
        public bool HasChildren
        {
            get
            {
                return this.Children.Count > 0;
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеет ли тип объекта параметры.
        /// </summary>
        public bool HasParameters
        {
            get
            {
                return this.Parameters.Count > 0;
            }
        }

        /// <summary>
        /// Возвращает цвет неактивных объектов данного типа.
        /// </summary>
        public Color InactiveColor
        {
            get;
        }

        /// <summary>
        /// Возвращает название типа объекта.
        /// </summary>
        public string Name
        {
            get;
        }

        /// <summary>
        /// Возвращает список необходимых параметров.
        /// </summary>
        public List<ParameterModel> NecessaryParameters
        {
            get;
        } = new List<ParameterModel>();

        /// <summary>
        /// Возвращает вид объектов данного типа.
        /// </summary>
        public ObjectKind ObjectKind
        {
            get;
        }

        /// <summary>
        /// Возвращает связи между параметрами типа в виде "Параметр - Родитель".
        /// </summary>
        public Dictionary<ParameterModel, ParameterModel> ParameterBonds
        {
            get;
        } = new Dictionary<ParameterModel, ParameterModel>();

        /// <summary>
        /// Возвращает параметры типа объекта.
        /// </summary>
        public List<ParameterModel> Parameters
        {
            get;
        } = new List<ParameterModel>();

        /// <summary>
        /// Возвращает список параметров только для чтения.
        /// </summary>
        public List<ParameterModel> ReadonlyParameters
        {
            get;
        } = new List<ParameterModel>();
        
        /// <summary>
        /// Возвращает название типа объекта в единственном числе.
        /// </summary>
        public string SingularName
        {
            get;
        }

        /// <summary>
        /// Возвращает идентификатор типа объекта.
        /// </summary>
        public int TypeId
        {
            get;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Возвращает набор значений вычисляемых параметров по умолчанию.
        /// </summary>
        /// <returns>Набор значений вычисляемых параметров по умолчанию.</returns>
        public ParameterValueSetModel GetDefaultCalcParameterValues()
        {
            if (!this.HasParameters)
                return null;

            var result = new Dictionary<ParameterModel, object>();

            foreach (var param in this.Parameters.Where(x => x.Format.IsCalculate))
                result.Add(param, param.Format.GetDefaultValue());

            return new ParameterValueSetModel(result);
        }

        /// <summary>
        /// Возвращает набор значений параметров по умолчанию.
        /// </summary>
        /// <returns>Набор значений параметров по умолчанию.</returns>
        public ParameterValueSetModel GetDefaultParameterValues()
        {
            if (!this.HasParameters)
                return null;

            var result = new Dictionary<ParameterModel, object>(new ParameterModelEqualityComparer());

            foreach (var param in this.Parameters.Where(x => !x.Format.IsCalculate))
                result.Add(param, param.Format.GetDefaultValue());

            return new ParameterValueSetModel(result);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что имеет ли тип заданный параметр.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <returns>true, если имеет, иначе - false.</returns>
        public bool HasParameter(ParameterModel parameter)
        {
            return this.Parameters.Contains(parameter);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли заданный параметр необходимым.
        /// </summary>
        /// <param name="param">Параметр.</param>
        /// <returns>true, если является неободимым, иначе - false.</returns>
        public bool IsParameterNecessary(ParameterModel param)
        {
            return this.NecessaryParameters.Contains(param);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли заданный параметр параметром только для чтения.
        /// </summary>
        /// <param name="param">Параметр.</param>
        /// <returns>true, если является параметром только для чтения, иначе - false.</returns>
        public bool IsParameterReadonly(ParameterModel param)
        {
            return this.ReadonlyParameters.Contains(param);
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что является ли заданный параметр видимым.
        /// </summary>
        /// <param name="param">Параметр.</param>
        /// <param name="parameterValueSet">Набор значений параметров.</param>
        /// <returns>true, если является видимым, иначе - false.</returns>
        public bool IsParameterVisible(ParameterModel param, ParameterValueSetModel parameterValueSet)
        {
            if (this.paramVisibilities.ContainsKey(param))
            {
                var visProv = this.paramVisibilities[param].Item1;
                var visValues = this.paramVisibilities[param].Item2;

                // Этот небольшой костыль используется для того, чтобы не происходила ошибка при отсутствии требуемого параметра у типа объекта.
                if (parameterValueSet.ParameterValues.ContainsKey(visProv))
                    return visValues.Contains(Convert.ToInt32(parameterValueSet.ParameterValues[visProv]));
            }

            return param.IsVisible;
        }

        /// <summary>
        /// Задает значения видимости заданного параметра.
        /// </summary>
        /// <param name="param">Параметр.</param>
        /// <param name="visibilityProvider">Параметр, от которого зависит видимость заданного параметра.</param>
        /// <param name="visibilityValues">Значения параметра, при которых будет виден заданный параметр.</param>
        public void SetParameterVisibility(ParameterModel param, ParameterModel visibilityProvider, List<int> visibilityValues)
        {
            this.paramVisibilities.Add(param, new Tuple<ParameterModel, List<int>>(visibilityProvider, visibilityValues));
        }

        #endregion
        
        #region Открытые переопределенные методы

        /// <summary>
        /// Определяет, эквивалентен ли заданный объект текущему.
        /// </summary>
        /// <param name="obj">Сравниваемый объект.</param>
        /// <returns>true, если объекты эквивалентны, иначе - false.</returns>
        public override bool Equals(object obj)
        {
            var compObj = obj as ObjectType;

            if (ReferenceEquals(compObj, null))
                return false;
            else
                return typeIdComparer.Equals(this.TypeId, compObj.TypeId);
        }

        /// <summary>
        /// Возвращает хеш код объекта.
        /// </summary>
        /// <returns>Хеш код.</returns>
        public override int GetHashCode()
        {
            return typeIdComparer.GetHashCode(this.TypeId);
        }

        #endregion
    }
}