using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;

namespace DuettoPatron.DataModel
{
    class SFTPConn
    {
        static ConnectionInfo ConnInfo = null;
        Setting setting = Setting.LoadSetting();
        int port = 22;
        public SFTPConn()
        {
            if (ConnInfo == null)
            {
                try
                {
                    ConnInfo = new ConnectionInfo(setting.sftp.Host, port, setting.sftp.User,
                    new AuthenticationMethod[]{

                    // Pasword based Authentication
                    new PasswordAuthenticationMethod(setting.sftp.User,setting.sftp.Password)
                    });
                }
                catch(Exception ex)
                {
                    Loger.Log.Error(ex);
                }
            }
        }
        public void TransferFile(string filePath)
        {
            try
            {
                using (var sftp = new SftpClient(ConnInfo))
                {
                    FileInfo fi = new FileInfo(filePath);
                    string uploadfn = setting.sftp.Path + fi.Name;
                    sftp.Connect();
                    using (var uplfileStream = System.IO.File.OpenRead(filePath))
                    {
                        sftp.UploadFile(uplfileStream, uploadfn, true);
                    }
                    sftp.Disconnect();
                }
            }
            catch(Exception ex)
            {
                Loger.Log.Error(ex);
            }


        }



    }
}
