using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace DuettoPatron.DataModel
{
    class CsvParser
    {
        string cvsPath = @"./CsvFiles/";
        Setting setting = Setting.LoadSetting();
        public string write(List<SqlRepositor.TableSchema> records)
        {
            if (records == null || records.Count <= 0)
                return null;

            if (!Directory.Exists(cvsPath))
                Directory.CreateDirectory(cvsPath);

            DateTime dt = DateTime.Now;
            string csvFile = setting.hotelName + "_scores_" + dt.ToString("yyyy-MM-dd-HH-mm") + ".csv";
            string csvpath = Path.Combine(cvsPath, csvFile);
            using (var writer = new StreamWriter(csvpath))
            {
                var csv = new CsvWriter(writer);
                csv.WriteRecords(records);
                csv.Flush();
                return csvpath;
            }
        }
    }
}
