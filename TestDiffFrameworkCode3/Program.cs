using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace TestDiffFrameworkCode3
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController service in services)
            {
                string serviceName = service.ServiceName;
                string serviceStatus = service.Status.ToString();
                string displayName = service.DisplayName;
                string serviceStartMode = "";

                if (displayName.Contains("Microsoft"))
                {
                    displayName = displayName.Substring(displayName.IndexOf("$") + 1);

                    // This is the check of witch .Net Framework this code is running in.
                    if (CheckDotNetVersionMin461())
                    {
                        // Only if the code is running on .Net 4.6.1 the ServiceStartMode is fetched.
                        serviceStartMode = GetServiceStartMode(service);
                    }
                    else
                    {
                        serviceStartMode = "Unknown";
                    }
                    Console.WriteLine($"ServiceName : {serviceName}");
                    Console.WriteLine($"DisplayName : {displayName}");
                    Console.WriteLine($"Status      : {serviceStatus}");
                    Console.WriteLine($"StartMode   : {serviceStartMode}");
                    Console.WriteLine($"----------------------------------------------------------------------");
                }
            }

            Console.WriteLine("Press key to continue.");
            Console.ReadKey();
        }

        public static bool CheckDotNetVersionMin461()
        {
            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
            {
                int releaseKey = Convert.ToInt32(ndpKey.GetValue("Release"));
                if (releaseKey >= 394254)
                {
                    return true;
                }
            }
            return false;
        }
        private static String GetServiceStartMode(ServiceController _service)
        {
            return _service.StartType.ToString();
        }
    }
}
