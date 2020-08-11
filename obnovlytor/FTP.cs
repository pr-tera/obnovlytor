using System.IO;
using System.Net;

namespace obnovlytor
{
    class FTP
    {
        public static void Download(string File)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(getURI(File));
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(Request.Login, Request.Password);
            request.EnableSsl = false;
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();

            FileStream fs = new FileStream(File, FileMode.Create);

            byte[] buffer = new byte[64];
            int size = 0;
            while ((size = responseStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                fs.Write(buffer, 0, size);
            }
            fs.Close();
            response.Close();
        }
        public static void GetFolder()
        {
            FtpWebRequest listRequest = (FtpWebRequest)WebRequest.Create(getURI(null));
            listRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            listRequest.Credentials = new NetworkCredential(Request.Login, Request.Password);
            using (FtpWebResponse listResponse = (FtpWebResponse)listRequest.GetResponse())
            using (Stream listStream = listResponse.GetResponseStream())
            using (StreamReader listReader = new StreamReader(listStream))
            {
                while (!listReader.EndOfStream)
                {
                    Request.FileList.Add(listReader.ReadLine());
                }
            }

        }
        public static string getURI(string File)
        {
            string uri = string.Empty;
            if (string.IsNullOrEmpty(File))
            {
                uri = Request.Head + Request.ServerDDNS + Request.Port + Request.Folder;
                if (CheckFTP(uri) == false)
                {
                    uri = Request.Head + Request.ServerRezerv + Request.Port + Request.Folder;
                    if (CheckFTP(uri) == false)
                    {
                        uri = Request.Head + Request.ServetRezerv2 + Request.Port + Request.Folder;
                    }
                }
            }
            else
            {
                uri = Request.Head + Request.ServerDDNS + Request.Port + Request.Folder + File;
                if (CheckFTP(uri) == false)
                {
                    uri = Request.Head + Request.ServerDDNS + Request.Port + Request.Folder + File;
                    if (CheckFTP(uri) == false)
                    {
                        uri = Request.Head + Request.ServerDDNS + Request.Port + Request.Folder + File;
                        if (CheckFTP(uri) == false)
                        {
                            Function.LocalInstall();
                        }
                    }
                }
            }
            return uri;
        }
        static bool CheckFTP(string uri)
        {
            bool check = false;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential(Request.Login, Request.Password);
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            switch (response.StatusCode)
            {
                case FtpStatusCode.AccountNeeded:
                    check = false;
                    break;
                case FtpStatusCode.OpeningData:
                    check = true;
                    break;
                case FtpStatusCode.CommandOK:
                    check = true;
                    break;
            }
            response.Close();
            return check;
        }
    }
}
