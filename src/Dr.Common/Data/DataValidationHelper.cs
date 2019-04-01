
#region DataValidationHelper 声明
/**************************************************************
* 命名空间 ：Dr.Common.Data
* 类 名 称 ：DataValidationHelper
* 创 建 人 ：XIAOXL084520
* 邮    箱 ：Xiaoxl084520@163.com
* 创建时间 ：2019-3-29 11:55:00
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dr.Common.Data
{
    /// <summary>
    /// 数据合法性验证帮助类
    /// </summary>
    public static class DataValidationHelper
    {
        #region 正则表达式验证规则

        //手机号码
        private const string regexPattern_PhoneNum = @"^1(?:3\d|4[4-9]|5[0-35-9]|6[67]|7[013-8]|8\d|9\d)\d{8}$";
        //电话号码
        private const string regexPattern_TelNum = @"^(\d{3}-)?\d{8}|(\d{4}-)?\d{7}$";
        //Ip地址
        private const string regexPattern_IpAddress = @"^((25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))$";
        //邮箱
        private const string regexPattern_Email = @"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$";
        //中文姓名 （2-4个汉字）
        private const string regexPattern_RealName = @"^[\u4e00-\u9fa5]{2,4}$";
        //登录名   （字母开头，允许5-16字节，允许字母数字下划线）
        private const string regexPattern_LoginName = @"^[a-zA-Z][A-Za-z0-9_]{4,15}$";
        //登录密码 （必须包含字母、数字、特殊字符且长度8字节以上）
        private const string regexPattern_Password = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[^A-Za-z0-9])\S{8,}$";
        //int
        private const string regexPattern_Int = @"^[-+]?\d+$";
        //decimal  
        private const string regexPattern_Decimal = @"^[-+]?\d+(\.\d+)?$";
        //Date    （yyyy-MM-dd 或 yyyy/MM/dd）
        private const string regexPattern_Date = @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})[-/](((0[13578]|1[02])[-/](0[1-9]|[12][0-9]|3[01]))|((0[469]|11)[-/](0[1-9]|[12][0-9]|30))|(02[-/](0[1-9]|[1][0-9]|2[0-8])))$";
        //Time (HH:mm:ss)
        private const string regexPattern_Time = @"^([0-1]\d|2[0-3])[:]([0-5]\d)[:]([0-5]\d)$";
        //身份证号
        private const string regexPattern_CardNo = @"^\d{6}(18|19|20)?\d{2}(0[1-9]|1[012])(0[1-9]|[12]\d|3[01])\d{3}(\d|X)$";
        //SQL 防注入
        private const string regexPattern_SqlFilter = @"^\+/v(8|9)|\b(and|or)\b.{1,6}?(=|>|<|\bin\b|\blike\b)|/\*.+?\*/|<\s*script\b|<\s*img\b|\bEXEC\b|UNION.+?SELECT|UPDATE.+?SET|INSERT\s+INTO.+?VALUES|(SELECT|DELETE).+?FROM|(CREATE|ALTER|DROP|TRUNCATE)\s+(TABLE|DATABASE)$";

        #endregion

        /// <summary>
        /// 判断是否是手机号码
        /// </summary>
        /// <param name="str">待验证数据</param>
        /// <returns></returns>
        public static bool IsPhoneNum(this string str)
        {
            return str.IsRegex(regexPattern_PhoneNum, s => 11 == s.Length);
        }

        /// <summary>
        /// 判断是否是电话号码
        /// </summary>
        /// <param name="str">待验证数据</param>
        /// <returns></returns>
        public static bool IsTelNum(this string str)
        {
            return str.IsRegex(regexPattern_TelNum);
        }

        /// <summary>
        /// 判断是否是IP
        /// </summary>
        /// <param name="str">待验证数据</param>
        /// <returns></returns>
        public static bool IsIPAddress(this string str)
        {
            return str.IsRegex(regexPattern_IpAddress, s => !(str.Length < 7 || str.Length > 15));
        }

        /// <summary>
        /// 判断是否是邮箱地址
        /// </summary>
        /// <param name="str">待验证数据</param>
        /// <returns></returns>
        public static bool IsEmail(this string str)
        {
            return str.IsRegex(regexPattern_Email);
        }

        /// <summary>
        /// 判断是否是真实姓名格式（2-4个汉字）
        /// </summary>
        /// <param name="str">待验证数据</param>
        /// <returns></returns>
        public static bool IsRealName(this string str)
        {
            return str.IsRegex(regexPattern_RealName);
        }

        /// <summary>
        /// 判断是否满足登录名规则（字母开头，允许5-16字节，允许字母数字下划线）
        /// </summary>
        /// <param name="str">待验证数据</param>
        /// <returns></returns>
        public static bool IsLoginName(this string str)
        {
            return str.IsRegex(regexPattern_LoginName);
        }

        /// <summary>
        /// 判断是否满足登录密码格式（必须包含字母、数字、特殊字符且长度8字节以上）
        /// </summary>
        /// <param name="str">待验证数据</param>
        /// <returns></returns>
        public static bool IsPassword(this string str)
        {
            return str.IsRegex(regexPattern_Password);
        }

        /// <summary>
        /// 判断是否是int
        /// </summary>
        /// <param name="str">待验证数据</param>
        /// <returns></returns>
        public static bool IsInt(this string str)
        {
            return str.IsRegex(regexPattern_Int);
        }

        /// <summary>
        /// 判断是否是decimal
        /// </summary>
        /// <param name="str">待验证数据</param>
        /// <returns></returns>
        public static bool IsDecimal(this string str)
        {
            return str.IsRegex(regexPattern_Decimal);
        }

        /// <summary>
        /// 判断是否满足日期格式 yyyy-MM-dd 或 yyyy/MM/dd
        /// </summary>
        /// <param name="str">待验证数据</param>
        /// <returns></returns>
        public static bool IsDate(this string str)
        {
            return str.IsRegex(regexPattern_Date);
        }

        /// <summary>
        /// 判断是否满足24制时间格式 HH:mm:ss
        /// </summary>
        /// <param name="str">待验证数据</param>
        /// <returns></returns>
        public static bool IsTime(this string str)
        {
            return str.IsRegex(regexPattern_Time);
        }

        /// <summary>
        /// 判断是否是身份证号
        /// </summary>
        /// <param name="str">待验证数据</param>
        /// <returns></returns>
        public static bool IsCardId(this string str)
        {
            try
            {
                str = str.Trim();

                var city = new int[] { 11, 12, 13, 14, 15, 21, 22, 23, 31, 32, 33, 34, 35, 36, 37, 41, 42, 43, 44, 45, 46, 50, 51, 52, 53, 54, 61, 62, 63, 64, 65, 71, 81, 82, 91 };

                if (str.IsRegex(regexPattern_CardNo))
                {
                    if (city.Contains(str.Substring(0, 2).ToInt(0)))
                    {
                        //获取出生日期
                        if (str.GetBirthdayFromCardId().ToDateTime(DateTime.Now) >= DateTime.Now)
                        {
                            return false;
                        }

                        if (str.Length == 18)
                        {
                            var arr = str.ToCharArray().Select(m => m.ToString().ToInt()).ToArray();
                            var factor = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
                            var parity = new string[] { "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };
                            var sum = 0;
                            var ai = 0;
                            var wi = 0;
                            for (var i = 0; i < 17; i++)
                            {
                                ai = arr[i];
                                wi = factor[i];
                                sum += ai * wi;
                            }
                            var last = parity[sum % 11];
                            if (arr[17].ToString().ToUpper() == last.ToString())
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            catch
            { }
            return false;
        }

        /// <summary>
        /// 验证是否存在注入代码
        /// </summary>
        /// <param name="str">待验证数据</param>
        public static bool IsSqlFilter(this string str)
        {
            return str.IsRegex(regexPattern_SqlFilter);
        }

        /// <summary>
        /// 正则表达式规则验证
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="pattern">正则表达式验证规则</param>
        /// <param name="predicate">验证条件</param>
        /// <returns></returns>
        public static bool IsRegex(this string str, string pattern, Predicate<string> predicate = null)
        {
            try
            {
                str = str.Trim();
                if (string.IsNullOrEmpty(str))
                {
                    return false;
                }

                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

                return (predicate?.Invoke(str) ?? true) && regex.IsMatch(str);
            }
            catch (Exception ex)
            {
                ex.AddLog();
                return false;
            }
        }
    }
}
