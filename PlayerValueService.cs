using DuettoPatron.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Topshelf;

namespace DuettoPatron
{
    class PlayerValueService: ServiceControl
    {
        public bool Start(HostControl hostControl)
        {
            try
            {
                var setting = Setting.LoadSetting();
                Loger.Log.Info("Service Started");
                Console.WriteLine("");
                var waitInMin = setting.sqldb.PullFrequency * 60 * 1000;

                System.Timers.Timer aTimer = new System.Timers.Timer();
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                aTimer.Interval = waitInMin;
                aTimer.Enabled = true;


                //while (true)
                //{
                //}
                return true;
            }
            catch(Exception ex)
            {
                Loger.Log.Error(ex);
                return false;
            }
        }
        public bool Stop(HostControl hostControl)
        {
            try
            {
                Loger.Log.Info("Service Stopped");
                return true;
            }
            catch(Exception ex)
            {
                Loger.Log.Error(ex);
                return false;
            }
        }
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            SqlRepositor sqlRepo = new SqlRepositor();
            sqlRepo.ReadData();
        }





    }
}
