
#region FTPHelper 声明

/**************************************************************
* 命名空间 ：Dr.Common.FTP
* 类 名 称 ：FTPHelper
* 创 建 人 ：XIAOXL084520
* 邮    箱 ：Xiaoxl084520@163.com
* 创建时间 ：2019-4-1 15:51:51
* 版 本 号 ：V1.0
* 功能描述 ：N/A
* 
*┌ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┐
*│　 Copyright (c) 2019 XIAOXL084520 DR.All rights reserved.   │
*└ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┘
*
****************************************************************/

#endregion

using Dr.Common.Extensions;
using Dr.Common.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Dr.Common.FTP
{
    /// <summary> 
    /// FTPHelper 的摘要说明 
    /// </summary> 
    public class FTPHelper : IDisposable
    {
        private string _ftpServerIP;
        private string _ftpUserName;
        private string _ftpPassword;

        private Uri ftpUri;
        private string _path;

        #region 属性

        /// <summary>
        /// ftp 路径
        /// </summary>
        public string FtpPath
        {
            get { return _path; }
            set { _path = value; }
        }

        /// <summary>
        /// ftp Ip地址
        /// </summary>
        public string FtpServerIP
        {
            get { return _ftpServerIP; }
            set { _ftpServerIP = value; }
        }

        /// <summary>
        /// ftp 用户名
        /// </summary>
        public string FtpUserName
        {
            get { return _ftpUserName; }
            set { _ftpUserName = value; }
        }

        /// <summary>
        /// ftp 密码
        /// </summary>
        public string FtpPassword
        {
            get { return _ftpPassword; }
            set { _ftpPassword = value; }
        }

        #endregion

        #region 构造函数

        public FTPHelper(string ftpServerIp, string username, string passwd)
        {
            this.FtpServerIP = ftpServerIp;
            this.FtpUserName = username;
            this.FtpPassword = passwd;
            this.ftpUri = new Uri("ftp://" + ftpServerIp);
        }

        public FTPHelper(string ftpServerIp, string username, string passwd, string ftp_path) : this(ftpServerIp, username, passwd)
        {
            this.FtpPath = ftp_path;
            this.ftpUri = new Uri("ftp://" + ftpServerIp + "/" + ftp_path);
        }


        #endregion

        #region 方法
        public string GetFiles()
        {
            FtpWebRequest listRequest = (FtpWebRequest)WebRequest.Create(ftpUri);

            listRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            listRequest.Credentials = new NetworkCredential(FtpUserName, FtpPassword);

            using (FtpWebResponse listResponse = (FtpWebResponse)listRequest.GetResponse())
            {
                using (Stream responseStream = listResponse.GetResponseStream())
                {
                    using (StreamReader readStream = new StreamReader(responseStream, System.Text.Encoding.Default))
                    {
                        string result = string.Empty;

                        if (readStream != null)
                        {
                            result = readStream.ReadToEnd();
                        }
                        return result;
                    }
                }
            }
        }

        /// <summary>
        /// 获取文件及文件夹列表
        /// </summary>
        /// <returns></returns>
        public List<FileStruct> GetList()
        {
            string dataString = GetFiles();
            DirectoryListParser parser = new DirectoryListParser(dataString);
            List<FileStruct> list = parser.DirectoryList;
            list.AddRange(parser.FileList);
            return list;
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="dirName"></param>
        public bool MakeDir(string dirName)
        {
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(ftpUri + "/" + dirName));
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(FtpUserName, FtpPassword);
                using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
                {
                    using (Stream ftpStream = response.GetResponseStream())
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.AddLog();
                return false;
            }
        }
        /// <summary>
        /// 将文件上载到ftp服务器
        /// </summary>
        /// <param name="filename">文件名</param>
        public bool Upload(string filename)
        {
            bool result = false;
            try
            {
                FileInfo fileInf= new FileInfo(filename);
                string uri = ftpUri + "/" + fileInf.Name;
                FtpWebRequest reqFTP= (FtpWebRequest)WebRequest.Create(new Uri(uri));
                reqFTP.Credentials = new NetworkCredential(FtpUserName, FtpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                reqFTP.UseBinary = true;
                reqFTP.ContentLength = fileInf.Length;

                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;
                using (FileStream fs = fileInf.OpenRead())
                {
                    using (Stream strm = reqFTP.GetRequestStream())
                    {
                        contentLen = fs.Read(buff, 0, buffLength);
                        while (contentLen != 0)
                        {
                            strm.Write(buff, 0, contentLen);
                            contentLen = fs.Read(buff, 0, buffLength);
                        }
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                ex.AddLog();
                return false;
            }

            return result;
        }
        /// <summary>
        /// 将文件追加到现有的文件夹内
        /// </summary>
        /// <param name="filename">文件名</param>
        public void AppendFile(string filename)
        {
            try
            {
                FileInfo fileInf = new FileInfo(filename);
                string uri = ftpUri + fileInf.Name;
                FtpWebRequest reqFTP= (FtpWebRequest)WebRequest.Create(new Uri(uri));
                reqFTP.Credentials = new NetworkCredential(FtpUserName, FtpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.AppendFile;
                reqFTP.UseBinary = true;
                reqFTP.ContentLength = fileInf.Length;
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;
                using (FileStream fs = fileInf.OpenRead())
                {
                    using (Stream strm = reqFTP.GetRequestStream())
                    {
                        contentLen = fs.Read(buff, 0, buffLength);
                        while (contentLen != 0)
                        {
                            strm.Write(buff, 0, contentLen);
                            contentLen = fs.Read(buff, 0, buffLength);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.AddLog();
            }
        }
        /// <summary>
        /// 用于删除ftp服务器的文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        public bool Delete(string fileName)
        {
            try
            {
                string uri = ftpUri + "/" + fileName;
                FtpWebRequest reqFTP= (FtpWebRequest)WebRequest.Create(new Uri(uri));
                reqFTP.Credentials = new NetworkCredential(FtpUserName, FtpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                string result = string.Empty;
                using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
                {
                    long size = response.ContentLength;
                    if (size == 0)
                    {
                        return false;
                    }
                    using (Stream datastream = response.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(datastream))
                        {
                            result = sr.ReadToEnd();
                            return true;
                        }  
                    }  
                }
            }
            catch (Exception ex)
            {
                ex.AddLog();
                return false;
            }
        }

        /// <summary>
        /// 重命名目录
        /// </summary>
        /// <param name="currentFilename">原来名称</param>
        /// <param name="newFilename">新名称</param>
        public bool ReName(string currentFilename, string newFilename)
        {
            try
            {
                var reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(ftpUri + currentFilename));
                reqFTP.Method = WebRequestMethods.Ftp.Rename;
                reqFTP.RenameTo = newFilename;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(FtpUserName, FtpPassword);
                using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
                {
                    using(Stream ftpStream = response.GetResponseStream())
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.AddLog();
                return false;
            }
        }

        /// <summary>
        /// 判断当前目录下指定的子目录是否存在
        /// </summary>
        /// <param name="RemoteDirectoryName">指定的目录名</param>
        public bool DirectoryExist(string RemoteDirectoryName)
        {
            string[] dirList = GetDirectoryList();
            foreach (string str in dirList)
            {
                if (str.Trim() == RemoteDirectoryName.Trim())
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取当前目录下所有的文件夹列表(仅文件夹)
        /// </summary> 
        /// <returns></returns>
        public string[] GetDirectoryList()
        {
            string[] drectory = GetFilesDetailList();
            string m = string.Empty;
            if (drectory != null && drectory.Length > 0)
            {
                foreach (string str in drectory)
                {
                    if (str.Trim().Substring(0, 1).ToUpper() == "D")
                    {
                        m += str.Substring(54).Trim() + "\n";
                    }
                }
            }
            char[] n = new char[] { '\n' };
            return m.Split(n);
        }
        /// <summary>
        /// 获取当前目录下明细(包含文件和文件夹)
        /// </summary>
        /// <returns></returns>
        public string[] GetFilesDetailList()
        {
            string[] downloadFiles;
            try
            {
                StringBuilder result = new StringBuilder();
                FtpWebRequest ftp= (FtpWebRequest)WebRequest.Create(ftpUri);
                ftp.Credentials = new NetworkCredential(FtpUserName, FtpPassword);
                ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                using (WebResponse response = ftp.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        while (line != null)
                        {
                            result.Append(line);
                            result.Append("\n");
                            line = reader.ReadLine();
                        }
                        result.Remove(result.ToString().LastIndexOf("\n"), 1);
                        return result.ToString().Split('\n');
                    }
                }  
            }
            catch (Exception ex)
            {
                ex.AddLog();
                downloadFiles = null;
                return downloadFiles;
            }
        }
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="folderName"></param>
        public bool RemoveDirectory(string folderName)
        {
            try
            {
                string uri = ftpUri + "/" + folderName;
                FtpWebRequest reqFTP= (FtpWebRequest)WebRequest.Create(new Uri(uri));
                reqFTP.Credentials = new NetworkCredential(FtpUserName, FtpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;

                string result = string.Empty;
                using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
                {
                    long size = response.ContentLength;
                    if (size == 0)
                    {
                        return false;
                    }
                    using (Stream datastream = response.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(datastream))
                        {
                            
                            result = sr.ReadToEnd();
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.AddLog();
                return false;
            }
        }
        /// <summary>
        /// 获得当前文件夹下的所有目录（仅文件夹）
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllFolder()
        {

            //返回值
            List<string> result = new List<string>(); ;
            //先得到此文件夹下的所有信息
            List<FileStruct> allList = this.GetList();
            //循环选出文件夹
            FileStruct fs = new FileStruct();
            for (int i = 0; i < allList.Count; i++)
            {
                fs = allList[i];
                if (fs.FileType == "文件夹")
                {
                    result.Add(fs.Name);
                }
            }
            return result;
        }

        /// <summary>
        /// 获得当前文件夹下的所有文件（仅文件）
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllFiles()
        {
            //返回值
            List<string> result = new List<string>(); ;
            //先得到此文件夹下的所有信息
            List<FileStruct> allList = this.GetList();
            //循环选出文件夹
            FileStruct fs = new FileStruct();
            for (int i = 0; i < allList.Count; i++)
            {
                fs = allList[i];
                if (fs.FileType != "文件夹")
                {
                    result.Add(fs.Name);
                }
            }
            return result;
        }
        #endregion

        public void Dispose()
        {}
    }

}
