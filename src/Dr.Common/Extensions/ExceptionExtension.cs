
#region ExceptionExtension 声明

/**************************************************************
* 命名空间 ：Dr.Common.Extensions
* 类 名 称 ：ExceptionExtension
* 创 建 人 ：XIAOXL084520
* 邮    箱 ：Xiaoxl084520@163.com
* 创建时间 ：2019-3-29 11:59:25
* 版 本 号 ：V1.0
* 功能描述 ：N/A
* 
*┌ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┐
*│　 Copyright (c) 2019 XIAOXL084520 DR.All rights reserved.   │
*└ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┘
*
****************************************************************/

#endregion

using Dr.Common.Logs;
using Dr.Common.Logs.IO;
using System;
using System.Text;

namespace Dr.Common.Extensions
{
    /// <summary>
    /// 系统异常扩展
    /// </summary>
    public static class ExceptionExtension
    {
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="ex">系统异常</param>
        public static void AddLog(this Exception ex)
        {
            try
            {
                if (ex == null)
                {
                    return;
                }
                StringBuilder errLog = new StringBuilder();
                errLog.Append("Message:" + ex.Message);
                errLog.Append(Environment.NewLine);
                errLog.Append("Source:" + ex.Source);
                errLog.Append(Environment.NewLine);
                errLog.Append("StackTrace:" + ex.StackTrace);
                errLog.Append(Environment.NewLine);
                errLog.Append("TargetSite:" + ex.TargetSite);

                LogHelper.Instance.AddLog(errLog.ToString(), LogLevel.EXCEPTION);
            }
            catch
            { }
        }
    }
}
