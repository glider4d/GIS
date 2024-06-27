using System;
using System.Collections.Generic;

namespace Kts.Gis.Models
{
    /// <summary>
    /// Представляет модель ограничения доступа к функции приложения.
    /// </summary>
    [Serializable]
    public sealed class AccessModel
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AccessModel"/>.
        /// </summary>
        /// <param name="accessKind">Текстовое представления вида ограничения доступа к функциям приложения.</param>
        /// <param name="value">Значение ограничения.</param>
        public AccessModel(string accessKind, object value)
        {
            switch (accessKind)
            {
                case "CanDraw":
                    this.Kind = AccessKind.CanDraw;

                    break;

                case "CanDrawIS":
                    this.Kind = AccessKind.CanDrawIS;

                    break;

                case "IsAdmin":
                    this.Kind = AccessKind.IsAdmin;

                    break;

                case "PermittedTypes":
                    this.Kind = AccessKind.PermittedTypes;

                    break;

                case "PermittedRegions":
                    this.Kind = AccessKind.PermittedRegions;

                    break;

                default:
                    throw new NotImplementedException("Не реализована работа со следующим видом ограничения доступа к функциям приложения: " + accessKind.ToString());
            }

            switch (this.Kind)
            {
                case AccessKind.CanDraw:
                case AccessKind.CanDrawIS:
                case AccessKind.IsAdmin:
                    this.Value = Convert.ToBoolean(Convert.ToInt32(value));

                    break;

                case AccessKind.PermittedRegions:
                case AccessKind.PermittedTypes:
                    var temp = new List<int>();

                    var div = new char[1]
                    {
                        ','
                    };

                    foreach (var entry in value.ToString().Trim().Split(div, StringSplitOptions.RemoveEmptyEntries))
                        temp.Add(Convert.ToInt32(entry));

                    this.Value = temp;

                    break;

                default:
                    throw new NotImplementedException("Не реализована работа со следующим видом ограничения доступа к функциям приложения: " + accessKind.ToString());
            }
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает вид ограничения доступа к функциям приложения.
        /// </summary>
        public AccessKind Kind
        {
            get;
        }

        /// <summary>
        /// Возвращает значение ограничения.
        /// </summary>
        public object Value
        {
            get;
        }

        #endregion
    }
}