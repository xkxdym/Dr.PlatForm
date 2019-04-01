
#region TransactionHelper 声明

/**************************************************************
* 命名空间 ：Dr.Common.Helpers
* 类 名 称 ：TransactionHelper
* 创 建 人 ：XIAOXL084520
* 邮    箱 ：Xiaoxl084520@163.com
* 创建时间 ：2019-3-29 15:09:11
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Dr.Common.Helpers
{
    /// <summary>
    /// 事务帮助类
    /// </summary>
    public class TransactionHelper
    {
        /// <summary>
        /// 按照事务性执行方法
        /// </summary>
        /// <param name="func">要执行的function</param>
        /// <returns>是否成功</returns>
        public static bool ExecuteByTransaction(Func<bool> func)
        {
            var result = false;
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        result = func?.Invoke() ?? false;
                    }
                    catch (Exception ex)
                    {
                        ex.AddLog();
                        result = false;
                    }
                    finally
                    {
                        if (result)
                        {
                            try
                            {
                                ts.Complete();
                            }
                            catch (InvalidOperationException ex)
                            {
                                ex.AddLog();
                                result = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.AddLog();
                result = false;
                throw ex;
            }
            return result;
        }
    }
}
