
#region SingletonHelper 声明

/**************************************************************
* 命名空间 ：Dr.Common.Helpers
* 类 名 称 ：SingletonHelper
* 创 建 人 ：XIAOXL084520
* 邮    箱 ：Xiaoxl084520@163.com
* 创建时间 ：2019-3-29 14:00:27
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

namespace Dr.Common.Helpers
{
    /// <summary> 
    /// 单例帮助类 
    /// </summary> 
    public class Singleton<T>
        where T:class,new ()
    {
        private static Lazy<T> _instance = new Lazy<T>();
        private static T t = null;
        private static object _lock = new object();
        public static T Instance
        {
            get
            {
                if (t == null)
                {
                    lock (_lock)
                    {
                        t = _instance.Value;
                    }
                }
                return t;
            }
        }
    }
}
