
#region DirectoryListParser 声明

/**************************************************************
* 命名空间 ：Dr.Common.FTP
* 类 名 称 ：DirectoryListParser
* 创 建 人 ：XIAOXL084520
* 邮    箱 ：Xiaoxl084520@163.com
* 创建时间 ：2019-4-1 15:52:03
* 版 本 号 ：V1.0
* 功能描述 ：N/A
* 
*┌ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┐
*│　 Copyright (c) 2019 XIAOXL084520 DR.All rights reserved.   │
*└ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┘
*
****************************************************************/

#endregion

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Dr.Common.FTP
{

    /// <summary>
    /// 文件夹转换类
    /// </summary>
    public class DirectoryListParser
    {
        private List<FileStruct> _myListArray;

        public List<FileStruct> FullListing
        {
            get
            {
                return _myListArray;
            }
        }

        /// <summary>
        /// 文件列表
        /// </summary>
        public List<FileStruct> FileList
        {
            get
            {
                List<FileStruct> _fileList = new List<FileStruct>();
                foreach (FileStruct thisstruct in _myListArray)
                {
                    if (!thisstruct.IsDirectory)
                    {
                        _fileList.Add(thisstruct);
                    }
                }
                return _fileList;
            }
        }

        /// <summary>
        /// 目录列表
        /// </summary>
        public List<FileStruct> DirectoryList
        {
            get
            {
                List<FileStruct> _dirList = new List<FileStruct>();
                foreach (FileStruct thisstruct in _myListArray)
                {
                    if (thisstruct.IsDirectory)
                    {
                        _dirList.Add(thisstruct);
                    }
                }
                return _dirList;
            }
        }

        public DirectoryListParser(string responseString)
        {
            _myListArray = GetList(responseString);
        }

        private List<FileStruct> GetList(string datastring)
        {
            List<FileStruct> myListArray = new List<FileStruct>();
            string[] dataRecords = datastring.Split('\n');
            FileListStyle _directoryListStyle = GuessFileListStyle(dataRecords);
            foreach (string s in dataRecords)
            {
                if (_directoryListStyle != FileListStyle.Unknown && s != "")
                {
                    FileStruct f = new FileStruct();
                    f.Name = "..";
                    switch (_directoryListStyle)
                    {
                        case FileListStyle.UnixStyle:
                            f = ParseFileStructFromUnixStyleRecord(s);
                            break;
                        case FileListStyle.WindowsStyle:
                            f = ParseFileStructFromWindowsStyleRecord(s);
                            break;
                    }
                    if (f.Name != "" && f.Name != "." && f.Name != "..")
                    {
                        myListArray.Add(f);
                    }
                }
            }
            return myListArray;
        }

        private FileStruct ParseFileStructFromWindowsStyleRecord(string Record)
        {
            FileStruct f = new FileStruct();
            
            string[] arr = Regex.Split(Record.Trim(), "\\s+");

            f.Name = arr[3];
            f.CreateTime = arr[0] + "  " + arr[1];

            if (arr[2] == "<DIR>")
            {
                f.FileSize = 0;
                f.IsDirectory = true;
                f.FileType = "文件夹";
            }
            else
            {
                f.FileSize = int.Parse(arr[2]);
                f.IsDirectory = false;
                if (arr[3].IndexOf(".") > -1)
                    f.FileType = arr[3].Split('.')[1];
                else
                    f.FileType = "未知";
            }

            return f;
        }

        public FileListStyle GuessFileListStyle(string[] recordList)
        {
            foreach (string s in recordList)
            {
                if (s.Length > 10
                    && Regex.IsMatch(s.Substring(0, 10), "(-|d)((-|r)(-|w)(-|x)){3}"))
                {
                    return FileListStyle.UnixStyle;
                }
                else if (s.Length > 8
                    && Regex.IsMatch(s.Substring(0, 8), "[0-9]{2}-[0-9]{2}-[0-9]{2}"))
                {
                    return FileListStyle.WindowsStyle;
                }
            }
            return FileListStyle.Unknown;
        }

        private FileStruct ParseFileStructFromUnixStyleRecord(string record)
        {
            FileStruct f = new FileStruct();
            if (record[0] == '-' || record[0] == 'd')
            {
                string processstr = record.Trim();
                f.Flags = processstr.Substring(0, 9);
                f.IsDirectory = (f.Flags[0] == 'd');
                processstr = (processstr.Substring(11)).Trim();
                _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);  
                f.Owner = _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);
                f.CreateTime = getCreateTimeString(record);
                int fileNameIndex = record.IndexOf(f.CreateTime) + f.CreateTime.Length;
                f.Name = record.Substring(fileNameIndex).Trim();            

            }
            else
            {
                f.Name = "";
            }
            return f;
        }

        private string getCreateTimeString(string record)
        {
            string month = "(jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec)";
            string space = @"(\040)+";
            string day = "([0-9]|[1-3][0-9])";
            string year = "[1-2][0-9]{3}";
            string time = "[0-9]{1,2}:[0-9]{2}";
            Regex dateTimeRegex = new Regex(month + space + day + space + "(" + year + "|" + time + ")", RegexOptions.IgnoreCase);
            Match match = dateTimeRegex.Match(record);
            return match.Value;
        }

        private string _cutSubstringFromStringWithTrim(ref string s, char c, int startIndex)
        {
            int pos1 = s.IndexOf(c, startIndex);
            string retString = s.Substring(0, pos1);
            s = (s.Substring(pos1)).Trim();
            return retString;
        }
    }

}
