using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceModel;
using System.ServiceProcess;

namespace Kts.GisHost
{
    class Program : ServiceBase
    {
        public ServiceHost serviceHost = null;
        public Program()
        {
            ServiceName = "KtsWcfHostService";
        }
        /*
        static void Main(string[] args)
        {



            //using (var host = new ServiceHost(typeof(Kts.Gis.Data.WcfDispatchServer)))


            //using (var host = new ServiceHost(typeof(Gis.Data.WcfDispatchServer))) { }
            //using ( var host = new ServiceHost(typeof(Gis.TestData.Class1)))
            using (var host = new ServiceHost(typeof(Gis.Data.WcfDispatchServer)))
            {
                /*
                var basicHttpBinding = new BasicHttpBinding();
                basicHttpBinding.MaxReceivedMessageSize = 235;
                host.AddServiceEndpoint(typeof(Kts.Gis.Data.IWcfDispatchServer), basicHttpBinding, new Uri("http://localhost:8080/"));* /
                host.Open();
                Console.WriteLine("Host started ...");
                Console.ReadLine();
            }
        }
        */

        public static void Log(string message)
        {
            try
            {
                System.IO.File.AppendAllText("c:\\1\\log.txt", message + "\r\n");
            }
            catch
            {

            }
        }

        public static void Main()
        {
#if !DEBUG
            ServiceBase.Run(new Program());
#else
            using (var host = new ServiceHost(typeof(Gis.Data.WcfDispatchServer)))

            //var test = new Kts.Gis.Data.WcfDispatchServer();
            //using (var host = new ServiceHost(typeof(Gis.Data.WcfDispatchServer))) { }
            //using ( var host = new ServiceHost(typeof(Gis.TestData.Class1)))
     
            //using (var host = new ServiceHost(typeof(Gis.Data.WcfDispatchServer)))
            {
                /*
                var basicHttpBinding = new BasicHttpBinding();
                basicHttpBinding.MaxReceivedMessageSize = 235;
                host.AddServiceEndpoint(typeof(Kts.Gis.Data.IWcfDispatchServer), basicHttpBinding, new Uri("http://localhost:8080/")); 

                */
                host.Open();
                Console.WriteLine("Host started ...");
                Console.ReadLine();
            }
#endif

        }

        // Start the Windows service.
        protected override void OnStart(string[] args)
        {
            Log("OnStart in\n");
            if (serviceHost != null)
            {
                serviceHost.Close();
            }

            // Create a ServiceHost for the CalculatorService type and
            // provide the base address.
            serviceHost = new ServiceHost(typeof(Gis.Data.WcfDispatchServer));

            // Open the ServiceHostBase to create listeners and start
            // listening for messages.
            serviceHost.Open();
            Log("OnStart out\n");
        }

        protected override void OnStop()
        {
            Log("OnStop in\n");
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
            Log("OnStop out\n");
        }
    }

    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public ProjectInstaller()
        {
            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            service = new ServiceInstaller();
            service.ServiceName = "KtsWcfHostService";
            Installers.Add(process);
            Installers.Add(service);
        }
    }
}
