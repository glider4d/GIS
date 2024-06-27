using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFClientWithoutFactory
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //DispatchRef.IWcfDispatchServer clientDisp 

            var client = new DispatchRef.WcfDispatchServerClient("BasicHttpBinding_IWcfDispatchServer");


            bool resultInitialize = client.Initialize("Якутск");


            Console.WriteLine("resultInitialize = " + resultInitialize);


            var serverList = client.getServerList();
            foreach (var item in serverList)
            {

                Console.Write(" " + item.Key.Serverk__BackingField + " ");
                Console.Write(" " + item.Key.Namek__BackingField + " ");
                Console.WriteLine(item.Value);
                
            }



            

            Console.ReadLine();
        }
    }
}
