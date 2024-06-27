using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testConsole.Class;


using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

using System.IO.Compression;

namespace testConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            AutorizationSql sqlAutorization = new AutorizationSql();
            sqlAutorization.ReadAutorizationFile();
            ServiceSqlConnection lersSqlService = sqlAutorization.getSqlAccessService();

            //lersSqlService.getIdParameters();
            System.Console.WriteLine("before gettValues" + DateTime.Now);
            List<Dictionary<Guid, List<Dictionary<int, object>>>> values = lersSqlService.gettValues();

            
            System.Console.WriteLine("after gettValues " + DateTime.Now + " result count = "+ values.Count);
            FileStream fs = new FileStream("DataFile2.gz", FileMode.Create);
            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            Console.WriteLine(values.ToArray().Length.ToString());
            BinaryFormatter formatter = new BinaryFormatter();
            int count = 0;
            
            foreach (var value in values)
            {
                count++;
                MemoryStream ms = new MemoryStream();
                formatter.Serialize(ms, value);
                using (GZipStream compressionStream = new GZipStream(ms, CompressionLevel.Optimal))
                {
                    formatter.Serialize(compressionStream, value);
                    //compressionStream.CopyTo(fs);
                    //fs.CopyTo(compressionStream);
                    ms.WriteTo(fs);
                }
                ms.Close();
                //formatter.Serialize(fs, value);
            }
            
            //MemoryStream memstream = new MemoryStream();
            //formatter.Serialize(memstream, values);
            //byte[] bytes = memstream.ToArray();

            //Console.WriteLine(bytes.Length.ToString());
            try
            {
                //formatter.Serialize(fs, values);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                
            }
            finally
            {
                
                fs.Close();
            }
            /*
            foreach (var items in values)
            {
                foreach (var it in items)
                {
                    System.Console.Write (it.Key.ToString() );

                    int kol = 0;
                    foreach (var pp in it.Value)
                    {
                        if (kol > 2)
                            break;

                        foreach (var p in pp)
                        {
                            System.Console.Write(" " + p.Key + " " + p.Value.ToString());
                        }
                        kol++;

                        //System.Console.Write(" " + pp.ke);
                    }
                    System.Console.WriteLine();
                }
            }*/


            List<Dictionary<Guid, List<string>>> dynamic_pivotResult = lersSqlService.Dynamic_Pivot();

            System.Console.WriteLine("after dynamic_pivotResult " + DateTime.Now + " result count = " + dynamic_pivotResult.Count);

            
            
            Console.ReadLine();
        }
    }
}
