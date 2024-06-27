using System.Data;

namespace Kts.Gis.Reports.Models
{
    /// <summary>
    /// Модель акта раздела границ.
    /// </summary>
    public sealed class PartitionActModel
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает имя абонента.
        /// </summary>
        public string Abonent
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает код котельной.
        /// </summary>
        public string BoilerCode
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает название котельной.
        /// </summary>
        public string BoilerName
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает название населенного пункта.
        /// </summary>
        public string City
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает диаметры трубопровода первого порядка.
        /// </summary>
        public int[] Diameters1
        {
            get;
        } = new int[6];

        /// <summary>
        /// Возвращает диаметры трубопровода второго порядка.
        /// </summary>
        public int[] Diameters2
        {
            get;
        } = new int[6];

        /// <summary>
        /// Возвращает утверждающего документа.
        /// </summary>
        public string DocApprover
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает должность утверждающего документа.
        /// </summary>
        public string DocApproverPost
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает номер квартиры.
        /// </summary>
        public string Flat
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает номер дома.
        /// </summary>
        public string House
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает массив байтов, представляющий изображение схемы.
        /// </summary>
        public byte[] Image
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает длины трубопровода первого порядка.
        /// </summary>
        public double[] Lengths1
        {
            get;
        } = new double[6];

        /// <summary>
        /// Возвращает длины трубопровода второго порядка.
        /// </summary>
        public double[] Lengths2
        {
            get;
        } = new double[6];

        /// <summary>
        /// Возвращает название объекта.
        /// </summary>
        public string ObjName
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает количество трубопроводной арматуры первого порядка.
        /// </summary>
        public int[] Pipe1
        {
            get;
        } = new int[6];

        /// <summary>
        /// Возвращает количество трубопроводной арматуры второго порядка.
        /// </summary>
        public int[] Pipe2
        {
            get;
        } = new int[6];

        /// <summary>
        /// Возвращает улицу.
        /// </summary>
        public string Street
        {
            get;
            set;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Возвращает таблицу, представляющую модель.
        /// </summary>
        /// <returns>Таблица, представляющая модель.</returns>
        public DataTable GetDataTable()
        {
            var result = new DataTable();
            result.Columns.Add(new DataColumn(nameof(this.Abonent), this.Abonent.GetType()));
            result.Columns.Add(new DataColumn(nameof(this.BoilerCode), this.BoilerCode.GetType()));
            result.Columns.Add(new DataColumn(nameof(this.BoilerName), this.BoilerName.GetType()));
            result.Columns.Add(new DataColumn(nameof(this.City), this.City.GetType()));
            result.Columns.Add(new DataColumn(nameof(this.Diameters1), this.Diameters1.GetType()));
            result.Columns.Add(new DataColumn(nameof(this.Diameters2), this.Diameters2.GetType()));
            result.Columns.Add(new DataColumn(nameof(this.DocApprover), this.DocApprover.GetType()));
            result.Columns.Add(new DataColumn(nameof(this.DocApproverPost), this.DocApproverPost.GetType()));
            result.Columns.Add(new DataColumn(nameof(this.Flat), this.Flat.GetType()));
            result.Columns.Add(new DataColumn(nameof(this.House), this.House.GetType()));
            result.Columns.Add(new DataColumn(nameof(this.Image), this.Image.GetType()));
            result.Columns.Add(new DataColumn(nameof(this.Lengths1), this.Lengths1.GetType()));
            result.Columns.Add(new DataColumn(nameof(this.Lengths2), this.Lengths2.GetType()));
            result.Columns.Add(new DataColumn(nameof(this.ObjName), this.ObjName.GetType()));
            result.Columns.Add(new DataColumn(nameof(this.Pipe1), this.Pipe1.GetType()));
            result.Columns.Add(new DataColumn(nameof(this.Pipe2), this.Pipe2.GetType()));
            result.Columns.Add(new DataColumn(nameof(this.Street), this.Street.GetType()));

            var row = result.NewRow();
            row[nameof(this.Abonent)] = this.Abonent;
            row[nameof(this.BoilerCode)] = this.BoilerCode;
            row[nameof(this.BoilerName)] = this.BoilerName;
            row[nameof(this.City)] = this.City;
            row[nameof(this.Diameters1)] = this.Diameters1;
            row[nameof(this.Diameters2)] = this.Diameters2;
            row[nameof(this.DocApprover)] = this.DocApprover;
            row[nameof(this.DocApproverPost)] = this.DocApproverPost;
            row[nameof(this.Flat)] = this.Flat;
            row[nameof(this.House)] = this.House;
            row[nameof(this.Image)] = this.Image;
            row[nameof(this.Lengths1)] = this.Lengths1;
            row[nameof(this.Lengths2)] = this.Lengths2;
            row[nameof(this.ObjName)] = this.ObjName;
            row[nameof(this.Pipe1)] = this.Pipe1;
            row[nameof(this.Pipe2)] = this.Pipe2;
            row[nameof(this.Street)] = this.Street;
            result.Rows.Add(row);

            return result;
        }

        #endregion
    }
}