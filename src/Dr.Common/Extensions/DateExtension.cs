
#region DateExtension 声明

/**************************************************************
* 命名空间 ：Dr.Common.Extensions
* 类 名 称 ：DateExtension
* 创 建 人 ：XIAOXL084520
* 邮    箱 ：Xiaoxl084520@163.com
* 创建时间 ：2019-3-29 12:03:24
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dr.Common.Extensions
{
    /// <summary>
    /// 时间 帮助类
    /// </summary>
    public static class DateExtension
    {

        private const string dateTimeFormatter = "yyyy-MM-dd HH:mm:ss";
        private const string dateFormatter = "yyyy-MM-dd";
        private const string timeFormatter = "HH:mm:ss";

        /// <summary>
        /// 时间转换成字符串 yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToDateTimeStr(this DateTime dateTime)
        {
            return dateTime.ToString(dateTimeFormatter);
        }

        /// <summary>
        /// 时间转换成字符串 yyyy-MM-dd
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToDateStr(this DateTime dateTime)
        {
            return dateTime.ToString(dateFormatter);
        }

        /// <summary>
        /// 时间转换成字符串HH:mm:ss
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToTimeStr(this DateTime dateTime)
        {
            return dateTime.ToString(timeFormatter);
        }

        /// <summary>
        /// 转换为中文星期
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static string ToChWeekDay(this DateTime dateTime)
        {
            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return "星期日";
                case DayOfWeek.Monday:
                    return "星期一";
                case DayOfWeek.Tuesday:
                    return "星期二";
                case DayOfWeek.Wednesday:
                    return "星期三";
                case DayOfWeek.Thursday:
                    return "星期四";
                case DayOfWeek.Friday:
                    return "星期五";
                case DayOfWeek.Saturday:
                    return "星期六";
                default:
                    return "";
            }
        }
    }
}
