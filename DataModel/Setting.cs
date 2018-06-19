using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Threading;

namespace DuettoPatron.DataModel
{
    class Setting
    {
        static string settingFile = "Setting.json";
        public double version { get; set; }
        public SqlDb sqldb { get; set; } = new SqlDb();
        public Sftp sftp { get; set; } = new Sftp();
        public string hotelName { get; set; }
        private Setting(){}
        static private Setting _Instance = null;
        static public Setting LoadSetting()
        {
            var syncObject = new object();
            if (_Instance == null)
            {
                try
                {
                    Monitor.Enter(syncObject);
                    _Instance = JsonConvert.DeserializeObject<Setting>(File.ReadAllText(settingFile));
                    if (_Instance.sqldb.PullFrequency < 1)
                        _Instance.sqldb.PullFrequency = 1; // pull frequency can't be less then 1
                    if (string.IsNullOrEmpty(_Instance.sqldb.LastUpdate)) // is last update is empty it will go 1 year back
                    {
                        _Instance.sqldb.LastUpdate = DateTime.Now.AddYears(-1).ToString(_Instance.sqldb.DateFormat);
                        UpdateSetting(_Instance);
                    }
                }
                catch(Exception ex)
                {
                    Loger.Log.Error(ex);
                }
                finally
                {
                    Monitor.Exit(syncObject);
                }
            }
            return _Instance;
        }
        static public void UpdateSetting(Setting setting)
        {
            try
            {
                string json = JsonConvert.SerializeObject(setting, Formatting.Indented);
                System.IO.File.WriteAllText(settingFile, json);
            }
            catch(Exception ex)
            {
                Loger.Log.Error(ex);
            }
        }
    };

    class SqlDb
    {
        public string Connection { get; set; }
        public int PullFrequency { get; set; } = 5; // How feequently data will be pulled from database
        public string LastUpdate { get; set; }
        public string DateFormat { get; set; }
    }

    class Sftp
    {
        public string Host { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Path { get; set; }
    }
}
