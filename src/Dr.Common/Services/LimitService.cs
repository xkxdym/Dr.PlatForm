
#region LimitService 声明

/**************************************************************
* 命名空间 ：Dr.Common.Services
* 类 名 称 ：LimitService
* 创 建 人 ：XIAOXL084520
* 邮    箱 ：Xiaoxl084520@163.com
* 创建时间 ：2019-3-29 14:03:00
* 版 本 号 ：V1.0
* 功能描述 ：N/A
* 
*┌ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┐
*│　 Copyright (c) 2019 XIAOXL084520 DR.All rights reserved.   │
*└ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┘
*
****************************************************************/

#endregion

using Dr.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dr.Common.Services
{
    /// <summary>
    /// 限流服务
    /// <code>
    /// <![CDATA[
    ///     private static LimitService limitService = new Lazy<LimitService>(() => new LimitService(100, 1));
    ///     if(limitService.value.IsContinue)
    ///     {
    ///         // to do 
    ///     }
    /// ]]>  
    /// </code>
    /// </summary>
    public class LimitService
    {
        //限流数组索引
        private int currentIndex = 0;
        //限流数组
        private DateTime?[] requestDateArr = null;

        private int _totalCount;

        /// <summary>
        /// 获取默认实例
        /// </summary>
        public static LimitService Instance = Singleton<LimitService>.Instance;

        /// <summary>
        ///限制的请求数量（默认1000）
        /// </summary>
        public int TotalCount
        {
            get
            {
                return _totalCount;
            }
            set
            {
                requestDateArr = new DateTime?[value];
                _totalCount = value;
            }
        }

        /// <summary>
        /// 限制的时间的秒数(默认1秒)，即：x秒允许多少请求
        /// </summary>
        public int LimitSecond { get; set; }

        private static object _lock = new object();


        public LimitService() : this(1000, 1) { }

        public LimitService(int totalCount, int limitSecond)
        {
            this.TotalCount = totalCount;
            this.LimitSecond = limitSecond;
            requestDateArr = new DateTime?[totalCount];
        }
        
        /// <summary>
        /// 是否允许继续执行
        /// </summary>
        public bool IsContinue
        {
            get
            {
                lock (_lock)
                {
                    var requestTime = DateTime.Now;
                    var currentNode = requestDateArr[currentIndex];
                    if (currentNode != null
                        && currentNode.Value.AddSeconds(LimitSecond) > requestTime)
                    {
                        return false;
                    }

                    requestDateArr[currentIndex] = requestTime;
                    currentIndex = (currentIndex + 1) >= requestDateArr.Length ? 0 : (currentIndex + 1);
                    return true;
                }
            }
        }
    }
}