using System;

namespace Kts.Utilities
{
    /// <summary>
    /// Представляет базовый сервис доступа к данным базы данных SQL.
    /// </summary>
    [Serializable]
    public abstract class BaseSqlDataAccessService
    {
        

        public static SerializedSqlQuery serrializedSqlQuery
        {
            get;
            set;
        }

        public static bool loadAllObjectFlag
        {
            get;
            set;
        } = false;

        public static bool localModeFlag
        {
            get;
            set;
        } = false;

        #region Конструкторы
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="BaseSqlDataAccessService"/>.
        /// </summary>
        /// <param name="connector">Коннектор с базой данных SQL.</param>
        public BaseSqlDataAccessService(SqlConnector connector)
        {
            //serrializedSqlQuery = new SerializedSqlQuery();
            serrializedSqlQuery = SerializedSqlQuery.getInstance();
            this.Connector = connector;
        }
        /*
        public bool deserializationSqlQuery()
        {
            bool result = false;
            return result;
        }

        public bool deserializationStaticSqlQuery()
        {
            bool result = false;
            if (serrializedSqlQuery != null)
            {
                
            }
            return result;
        }
        */
        public static bool serializeStaticSqlQuery(int userID)
        {
            return SerializedSqlQuery.Serialize(serrializedSqlQuery, userID);
        }

        #endregion

        #region Защищенные свойства
        
        /// <summary>
        /// Возвращает коннектор с базой данных SQL.
        /// </summary>
        protected SqlConnector Connector
        {
            get;
        }

        /// <summary>
        /// Проверка соединения с БД
        /// </summary>

        static public bool testConnectionFlag
        {
            get;
            set;
        } = true;


        public bool testConnection(string operation = "", bool silence = false)
        {
            testConnectionFlag = this.Connector.GetTestConnection(operation, silence) != null;
            return testConnectionFlag;
        }

        #endregion
    }
}