using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Kts.Importer.Data
{
    public class Serializer
    {
        public static void Serialize(object obj)
        {
            FileStream fs = new FileStream("file.s", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(fs, obj);
            fs.Close();
        }

        public static object Deserialize()
        {
            FileStream fs = new FileStream("file.s", FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryFormatter bf = new BinaryFormatter();

            Object obj = bf.Deserialize(fs);
            fs.Close();
            return obj;
        }
    }
}
