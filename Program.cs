using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuettoPatron.DataModel;
using System.Threading;
using Topshelf;

namespace DuettoPatron
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                var setting = Setting.LoadSetting();
                Console.WriteLine("*************************************************************************");
                Console.WriteLine("*                         OCEAN RESORT CASINO                           *");
                Console.WriteLine("*                     (  Palyer Value Service  )                        *");
                Console.WriteLine("*                         (  Version: " + setting.version.ToString("#.##") + "   )                             *");
                Console.WriteLine("*************************************************************************");

                Console.WriteLine("");
                HostFactory.Run(host =>
                {
                    host.SetServiceName("DuettoPatron"); //cannot contain spaces or / or \
                    host.SetDisplayName("Duetto Patron");
                    host.SetDescription("To push player value in csv format.");
                    host.StartAutomatically();

                    //host.RunAs("service account name", "the password");
                    //Don't think you like to expose your password in the code. =P
                    //We can set it manually for one time after installing the windows service in the services.msc

                    host.Service<PlayerValueService>();
                });
            }
            catch(Exception ex)
            {
                Loger.Log.Error(ex);
            }

        }
    }
}
