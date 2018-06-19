using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuettoPatron.DataModel
{
    class SqlRepositor
    {
        public class TableSchema
        {
            public string PlayerId { get; set; }
            public string ADT { get; set; }
        }
        public List<TableSchema> tableRows = new List<TableSchema>();

        SqlConnection sqlConnection = null;
        Setting setting = Setting.LoadSetting();
        SqlDataReader sqlReader;
        SqlCommand sqlCommand;

        public SqlRepositor()
        {
            if(sqlConnection == null)
            {
                sqlConnection = new SqlConnection(setting.sqldb.Connection);
            }
        }

        public void ReadData()
        {
            try
            {
                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                    sqlConnection.Open();

                string cmdText = "select p.PlayerID, (sum(pd.Slot_TheoWin) + sum(pd.Table_TheoWin)) / count(pd.PlayerID) as ADT, pd.EndTime as LastUpdate ";
                cmdText += "from PlayerManagement.dbo.PlayerDay pd (nolock) ";
                cmdText +=  "join PlayerManagement.dbo.Player p (nolock) on p.PlayerID = pd.PlayerID ";
                cmdText += "where p.status = 'A'  AND CAST(pd.EndTime AS Datetime) > '" + setting.sqldb.LastUpdate + "' ";
                cmdText += "group by p.playerid, pd.EndTime ";
                cmdText += "order by pd.EndTime ";

                sqlCommand = new SqlCommand(cmdText);
                sqlCommand.Connection = sqlConnection;
                sqlReader = sqlCommand.ExecuteReader();

                DateTime dtLatest = new DateTime();
                while (sqlReader.Read())
                {
                    int iFld = 0;
                    TableSchema tableSchema = new TableSchema();
                    tableSchema.PlayerId = sqlReader.IsDBNull(iFld++)? string.Empty:sqlReader["PlayerId"].ToString();
                    tableSchema.ADT = sqlReader.IsDBNull(iFld++) ? string.Empty:sqlReader["ADT"].ToString();
                    var dtCurrent = sqlReader.IsDBNull(iFld++) ? new DateTime() : Convert.ToDateTime(sqlReader["LastUpdate"]);
                    if(dtLatest < dtCurrent)
                    {
                        dtLatest = dtCurrent;
                    }
                    tableRows.Add(tableSchema);
                }
                if (tableRows.Count > 0)
                {
                    //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Records Sent: " + tableRows.Count.ToString());
                    Loger.Log.Info("Records Sent: " + tableRows.Count.ToString());
                    setting.sqldb.LastUpdate = dtLatest.ToString(setting.sqldb.DateFormat);
                    Setting.UpdateSetting(setting);

                    CsvParser csvParser = new CsvParser();
                    var csvPath = csvParser.write(tableRows);

                    var sftp = new SFTPConn();
                    sftp.TransferFile(csvPath);
                    File.Delete(csvPath);
                    //clear all records
                    tableRows.Clear();

                }

            }
            catch(Exception ex)
            {
                Loger.Log.Error(ex);
            }
            finally
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                    sqlConnection.Close();

            }

        }






    }
}
