using Kts.Gis.ViewModels;
using Kts.History;
using System;
using System.Text;

namespace Kts.Gis.RevertibleActions
{
    /// <summary>
    /// Представляет действие задания свойства объекта в обход его сеттера.
    /// </summary>
    internal sealed partial class SetPropertyAction : IRevertibleAction
    {
        #region Закрытые неизменяемые поля

        /// <summary>
        /// Предыдущее значение свойства.
        /// </summary>
        private readonly object oldValue;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SetPropertyAction"/>.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <param name="propertyName">Название свойства.</param>
        /// <param name="oldValue">Предыдущее значение свойства.</param>
        /// <param name="newValue">Новое значение свойства.</param>
        public SetPropertyAction(ISetterIgnorer obj, string propertyName, object oldValue, object newValue)
        {
            this.Object = obj;
            this.PropertyName = propertyName;
            this.oldValue = oldValue;
            this.NewValue = newValue;
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает новое значение свойства.
        /// </summary>
        public object NewValue
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает объект.
        /// </summary>
        public ISetterIgnorer Object
        {
            get;
        }

        /// <summary>
        /// Возвращает название свойства.
        /// </summary>
        public string PropertyName
        {
            get;
        }

        #endregion
    }

    // Реализация IRevertibleAction.
    internal sealed partial class SetPropertyAction
    {
        #region Открытые методы

        /// <summary>
        /// Выполняет действие.
        /// </summary>
        public void Do()
        {
            this.Object.SetValue(this.PropertyName, this.NewValue);
        }

        /// <summary>
        /// Возвращает строковое представление действия.
        /// </summary>
        /// <returns>Строковое представление действия.</returns>
        public string GetString()
        {
            var sb = new StringBuilder();

            sb.Append("Action: ");
            sb.Append(nameof(ReplaceNodeAction));
            sb.Append(Environment.NewLine);
            sb.Append("Object: ");
            if (this.Object is IObjectViewModel)
                sb.Append((this.Object as IObjectViewModel).Id);
            else
                sb.Append(this.Object.GetType());
            sb.Append(Environment.NewLine);
            sb.Append("Property: ");
            sb.Append(this.PropertyName);
            sb.Append(Environment.NewLine);
            sb.Append("Value: ");
            sb.Append(this.oldValue);
            sb.Append(" -> ");
            sb.Append(this.NewValue);

            return sb.ToString();
        }

        /// <summary>
        /// Выполняет действие, обратное прямому действию.
        /// </summary>
        public void Revert()
        {
            this.Object.SetValue(this.PropertyName, this.oldValue);
        }

        #endregion
    }
}