
#region EnumHelper 声明

/**************************************************************
* 命名空间 ：Dr.Common.Helpers
* 类 名 称 ：EnumHelper
* 创 建 人 ：XIAOXL084520
* 邮    箱 ：Xiaoxl084520@163.com
* 创建时间 ：2019-3-29 13:57:00
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
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Dr.Common.Helpers
{
    /// <summary>
    /// 枚举帮助类
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// 返回枚举项的描述信息。
        /// </summary>
        /// <param name="e">要获取描述信息的枚举项。</param>
        /// <returns>枚举项的描述信息。</returns>
        public static string GetDesc(Enum e)
        {
            if (e == null)
            {
                return string.Empty;
            }
            Type enumType = e.GetType();
            FieldInfo fieldInfo = enumType.GetField(e.ToString());
            if (fieldInfo != null)
            {
                var attr = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
                if (attr != null && !string.IsNullOrEmpty(attr.Description))
                {
                    return attr.Description;
                }
                else
                {
                    return e.ToString();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 枚举转化为 List(KeyValuePair(描述或名称,值))
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<KeyValuePair<string, int>> EnumToList<T>()
        {
            List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();

            Type type = typeof(T);

            foreach (int item in Enum.GetValues(type))
            {
                var key = Enum.GetName(type, item);

                FieldInfo fieldInfo = type.GetField(key);

                var attr = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute),false).FirstOrDefault() as DescriptionAttribute;
                if (attr != null && !string.IsNullOrEmpty(attr.Description))
                {
                    key = attr.Description;
                }

                list.Add(new KeyValuePair<string, int>(key, item));
            }
            return list;
        }

        /// <summary>
        /// 枚举转化为 Dictionary(描述或名称,枚举项)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<string, T> EnumToDic<T>()
        {
            Dictionary<string, T> dic = new Dictionary<string, T>();

            Type type = typeof(T);

            foreach (int item in Enum.GetValues(type))
            {
                var key = Enum.GetName(type, item);

                FieldInfo fieldInfo = type.GetField(key);

                var attr = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
                if (attr != null && !string.IsNullOrEmpty(attr.Description))
                {
                    key = attr.Description;
                }

                if (!dic.ContainsKey(key))
                {
                    dic.Add(key, (T)Enum.Parse(type, item.ToString()));
                }
            }

            return dic;
        }
    }
}
