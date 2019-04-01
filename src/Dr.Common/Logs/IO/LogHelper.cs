
#region LogHelper 声明

/**************************************************************
* 命名空间 ：Dr.Common.Logs.File
* 类 名 称 ：LogHelper
* 创 建 人 ：XIAOXL084520
* 邮    箱 ：Xiaoxl084520@163.com
* 创建时间 ：2019-3-29 13:34:40
* 版 本 号 ：V1.0
* 功能描述 ：N/A
* 
*┌ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┐
*│　 Copyright (c) 2019 XIAOXL084520 DR.All rights reserved.   │
*└ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┘
*
****************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dr.Common.Logs.IO
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public class LogHelper:ILog
    {
        static readonly Lazy<LogHelper> _lazy = new Lazy<LogHelper>();
        static object _lock = new object();

        /// <summary>
        /// 获取实例
        /// </summary>
        public static LogHelper Instance => _lazy.Value;

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="logLevel">日志级别</param>
        public void AddLog(string message, LogLevel logLevel = LogLevel.INFO)
        {
             AddLog(message, logLevel, true);
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="logLevel">日志级别</param>
        /// <param name="withTemplate">是否使用模版</param>
        public void AddLog(string message, LogLevel logLevel, bool withTemplate)
        {
            try
            {
                Task.Run(() =>
                {
                    lock (_lock)
                    {
                        var dirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", logLevel.ToString(), DateTime.Now.ToString("yyyy年MM月"));

                        DirectoryInfo info = new DirectoryInfo(dirPath);
                        if (!info.Exists)
                        {
                            info.Create();
                        }
                        StringBuilder content = new StringBuilder();
                        content.Append("===============");
                        content.Append(DateTime.Now);
                        content.Append("===============");
                        content.Append(Environment.NewLine);
                        content.Append(message);
                        content.Append(Environment.NewLine);

                        var file = Path.Combine(dirPath, DateTime.Now.Day + "日.log");
                        File.AppendAllText(file, content.ToString(), Encoding.Default);
                    }
                });
            }
            catch { }
        }
    }
}
