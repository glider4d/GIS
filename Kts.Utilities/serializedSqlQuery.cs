using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Kts.Utilities
{
    [Serializable]
    public class SerializedSqlQuery
    {
        private int m_index = -1;
        private List<Dictionary<string, object>> updateNewObjectParamValues;// = new List<Dictionary<string, object>>();
        private List<string> sufixList;// = new List<string>();
        private List<string> procedureList;// = new List<string>();
        private List<int> parrentIndexList;// = new List<int>();
        private static string m_fileName = "file.s";

        private static readonly SerializedSqlQuery instance = new SerializedSqlQuery();
        private SerializedSqlQuery()
        {
            m_index = -1;
            updateNewObjectParamValues = new List<Dictionary<string, object>>();
            sufixList = new List<string>();
            procedureList = new List<string>();
            parrentIndexList = new List<int>();
        }

        public void clearSerializedSqlQuery()
        {
            sufixList.Clear();
            procedureList.Clear();
            parrentIndexList.Clear();
            foreach (var item in updateNewObjectParamValues)
            {
                item.Clear();
            }
            updateNewObjectParamValues.Clear();

            if (File.Exists(m_fileName))
            {
                File.Delete(m_fileName);
            }
            m_index = -1;

        }

        public int getCountProcedure()
        {
            int result = 0;
            if (procedureList != null)
                result = procedureList.Count;
            return result;
        }
        public string getProcedureName(int index)
        {
            string result = string.Empty;
            if (procedureList != null && index < procedureList.Count)
                result = procedureList[index];
            return result;

        }
        public string getSufix(int index)
        {
            string result = string.Empty;
            if (sufixList != null && index < sufixList.Count)
                result = sufixList[index];
            return sufixList[index];
        }
        public Dictionary<string, object> getParametersFromIndex(int index)
        {
            Dictionary<string, object> result = null;
            if (updateNewObjectParamValues != null &&  index < updateNewObjectParamValues.Count)
            {
                result = updateNewObjectParamValues[index];
            }
            return result;
        }

        public static SerializedSqlQuery getInstance()
        {
            return instance;
        }
        public void addNewObjectParamValues(string procedure, string sufix, bool parrentLayer = false)
        {
            m_index++;
            if (parrentIndexList == null)
                parrentIndexList = new List<int>();

            if (parrentLayer)
                parrentIndexList.Add(0);
            else
                parrentIndexList.Add(-1);

            sufixList.Add(sufix);
            procedureList.Add(procedure);
            updateNewObjectParamValues.Add(new Dictionary<string, object>());
        }



        public object addObjectParamValue(string paramName, object param, bool uniqueIdCreate = false)
        {
            object id = null;
            if (uniqueIdCreate)
            {

                id = Guid.NewGuid();
                updateNewObjectParamValues[m_index].Add(paramName, id);
            }
            else
            {
                updateNewObjectParamValues[m_index].Add(paramName, param);
            }
            return id;
            
        }


        public void setParrent()
        {
            parrentIndexList[parrentIndexList.Count - 1] = findParrent();
        }
        private int findParrent()
        {
            int result = -1;
            if (parrentIndexList[parrentIndexList.Count - 1] == 0)
                return result;
            for (int i = m_index; i > 0; i--)
            {
                if (parrentIndexList != null && parrentIndexList.Count > i)
                    if (parrentIndexList[i] == 0)// ищем ближайший элемент справа с parrentLayer 0
                        break;
            }
            return result;
        }

        public static bool Serialize(object obj, int userID)
        {
            bool result = true;
            try
            {

                m_userID = userID;

                m_fileName = "data_" + userID + ".srl";
                FileStream fs = new FileStream(m_fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                BinaryFormatter bf = new BinaryFormatter();

                bf.Serialize(fs, obj);
                fs.Close();
            }
            catch 
            {
                result = false;
            }

            if (obj == null)
                result = false;
            return result;

            
        }

        public static object Deserialize()
        {
            FileStream fs = new FileStream(m_fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryFormatter bf = new BinaryFormatter();

            Object obj = bf.Deserialize(fs);
            fs.Close();
            return obj;
        }

        private static int m_userID = 0;


        public static SerializedSqlQuery getSerializedObject()
        {
            return BaseSqlDataAccessService.serrializedSqlQuery;
        }
        public static bool deserializationLocalData(int userID)
        {
            bool result = false;
            m_userID = userID;
            m_fileName = "data_" + userID + ".srl";
            if (File.Exists(m_fileName))
            {
                BaseSqlDataAccessService.serrializedSqlQuery = Deserialize() as SerializedSqlQuery;
                if (BaseSqlDataAccessService.serrializedSqlQuery != null)
                    result = true;
            }

            
            return result;
        }
    }
}
