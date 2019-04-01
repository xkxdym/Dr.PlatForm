#region ILog 声明

/******************************
* 命名空间 ：Dr.Common.Logs
* 类 名 称 ：ILog
* 创 建 人 ：XXL
* 创建时间 ：2019-3-29 13:35:13
* 版 本 号 ：V1.0
* 功能描述 ：N/A
******************************/

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dr.Common.Logs
{
    public interface ILog
    {
        void AddLog(string message, LogLevel logLevel = LogLevel.INFO);

        void AddLog(string message, LogLevel logLevel,bool withTemplate);
    }
}
