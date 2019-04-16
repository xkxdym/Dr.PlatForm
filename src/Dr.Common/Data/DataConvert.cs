
#region DataConvert 声明

/**************************************************************
* 命名空间 ：Dr.Common.Data
* 类 名 称 ：DataConvert
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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Dr.Common.Data
{
    /// <summary>
    /// 数据转化帮助类
    /// </summary>
    public static class DataConvert
    {
        /// <summary>
        /// string To int32
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defaultValue">转换失败时使用的默认值</param>
        /// <returns></returns>
        public static int ToInt(this string str, int defaultValue = 0)
        {
            int result;
            try
            {
                if (!int.TryParse(str, out result))
                {
                    result = defaultValue;
                };
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// string To int64
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defaultValue">转换失败时使用的默认值</param>
        /// <returns></returns>
        public static Int64 ToInt64(this string str, Int64 defaultValue = 0)
        {
            Int64 result;
            try
            {
                if (!Int64.TryParse(str, out result))
                {
                    result = defaultValue;
                };
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// string To double
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defaultValue">转换失败时使用的默认值</param>
        /// <returns></returns>
        public static double ToDouble(this string str, double defaultValue = 0)
        {
            double result;

            try
            {
                if (!double.TryParse(str, out result))
                {
                    result = defaultValue;
                };
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// string To decimal
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defaultValue">转换失败时使用的默认值</param>
        /// <returns></returns>
        public static decimal ToDecimal(this string str, decimal defaultValue = 0)
        {
            decimal result;
            try
            {
                if (!decimal.TryParse(str, out result))
                {
                    result = defaultValue;
                };
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// string To long
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defaultValue">转换失败时使用的默认值</param>
        /// <returns></returns>
        public static long ToLong(this string str, long defaultValue = 0)
        {
            long result;
            try
            {
                if (!long.TryParse(str, out result))
                {
                    result = defaultValue;
                };
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// IP地址转换为整数
        /// </summary>
        /// <param name="ip">ip地址（***.***.***.***）</param>
        /// <param name="defaultValue">转换失败时使用的默认值</param>
        /// <returns></returns>
        public static long ToLongForIp(this string ip, long defaultValue = 0)
        {
            long result;
            try
            {
                IPAddress ipaddress;

                if (IPAddress.TryParse(ip, out ipaddress))
                {
                    byte[] addbuffer = ipaddress.GetAddressBytes();
                    Array.Reverse(addbuffer);
                    result = BitConverter.ToUInt32(addbuffer, 0);
                }
                else
                {
                    result = defaultValue;
                }
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// string To float
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defaultValue">转换失败时使用的默认值</param>
        /// <returns></returns>
        public static float ToFloat(this string str, float defaultValue = 0)
        {
            float result;
            try
            {
                if (!float.TryParse(str, out result))
                {
                    result = defaultValue;
                };
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// string To boolean
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defaultValue">转换失败时使用的默认值</param>
        /// <returns></returns>
        public static bool ToBoolean(this string str, bool defaultValue = false)
        {
            bool result;
            try
            {
                List<string> trueList = new List<string>()
                {
                    "1","是","对","正确","OK","YES","Y"
                };

                if (trueList.Any(a => a.Equals(str.ToUpper())))
                {
                    return true;
                }

                if (!bool.TryParse(str, out result))
                {
                    result = defaultValue;
                };
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// string To DateTime
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defaultValue">转换失败时使用的默认值</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str, DateTime? defaultValue = null)
        {
            DateTime result = DateTime.MinValue;
            try
            {
                if (!DateTime.TryParse(str, out result))
                {
                    if (defaultValue.HasValue)
                    {
                        result = defaultValue.Value;
                    }
                };
            }
            catch
            {
                if (defaultValue.HasValue)
                {
                    result = defaultValue.Value;
                }
            }
            return result;
        }

        /// <summary>
        /// 时间戳转为时间
        /// </summary>
        /// <returns></returns>
        public static DateTime ToDateTimeFromTimestamp(this string timestamp, DateTime? defaultValue = null)
        {
            try
            {
                DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                var lTime = (timestamp + "0000000").ToLong();
                TimeSpan toNow = new TimeSpan(lTime);
                return dtStart.Add(toNow);
            }
            catch
            {
                if (defaultValue.HasValue)
                {
                    return defaultValue.Value;
                }
            }
            return DateTime.MinValue;
        }

        /// <summary>
        /// RMB金额转大写
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToChineseUpper(this decimal number)
        {
            var s = number.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            var d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            var r = Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空整分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());

            return r;
        }

        /// <summary> 
        /// 序列化对象为XML
        /// </summary> 
        /// <returns></returns> 
        public static string ToXml<T>(this T t)
            where T : class, new()
        {
            try
            {
                using (StringWriter sw = new StringWriter())
                {
                    XmlSerializer xz = new XmlSerializer(typeof(T));
                    xz.Serialize(sw, t);
                    return sw.ToString();
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 序列化对象为XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static string ToXml<T>(this T t, Encoding encode)
        {
            if (encode == null)
            {
                encode = Encoding.UTF8;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = false;
                settings.Indent = true;
                settings.Encoding = encode;

                using (XmlWriter writer = XmlWriter.Create(ms, settings))
                {
                    //去除默认命名空间xmlns:xsd和xmlns:xsi
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(writer, t, ns);
                    return encode.GetString(ms.GetBuffer());
                }
            }
        }

        /// <summary> 
        /// 反序列化XML为指定类型对象 
        /// </summary> 
        /// <returns></returns> 
        public static T XmlDeserialize<T>(this string xml)
            where T : class, new()
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xz = new XmlSerializer(typeof(T));
                    return xz.Deserialize(sr) as T;
                }
            }
            catch
            {
                return Activator.CreateInstance<T>();
            }
        }

        /// <summary>
        /// 序列化对象为Json
        /// </summary>
        /// <returns></returns>
        public static string ToJson<T>(this T t)
        {
            try
            {
                return JsonConvert.SerializeObject(t);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary> 
        /// 反序列化JSON为指定类型对象 
        /// </summary> 
        /// <returns></returns> 
        public static T JsonDeserialize<T>(this string jsonStr)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonStr);
            }
            catch
            {
                return Activator.CreateInstance<T>();
            }
        }

        /// <summary>
        /// 字典转成对应的模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static T ToModel<T>(this Dictionary<string, string> dic)
        {
            return ToModel<T>(dic.ToDictionary(m => m.Key, v =>v.Value as object));
        }

        /// <summary>
        /// 字典转成对应的模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static T ToModel<T>(this Dictionary<string, object> dic)
        {
            T model;
            try
            {
                model = Activator.CreateInstance<T>();

                if (dic == null)
                {
                    return model;
                }

                #region 属性
                var props = typeof(T).GetProperties();
                if (props != null)
                {
                    foreach (var item in props)
                    {
                        try
                        {
                            var name = item.Name.ToLower();
                            if (dic.Keys.Any(a => a.ToLower() == name))
                            {
                                var dic_Value = dic.FirstOrDefault(f => f.Key.ToLower() == name).Value;
                                try
                                {
                                    item.SetValue(model, ChangeType(dic_Value, item.PropertyType), null);
                                }
                                catch
                                {
                                    item.SetValue(model, dic_Value,null);
                                }
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                #endregion

                #region 字段

                var fields = typeof(T).GetFields(BindingFlags.Public);
                if (fields != null)
                {
                    foreach (var item in fields)
                    {
                        try
                        {
                            var name = item.Name.ToLower();

                            if (dic.Keys.Any(a => a.ToLower() == name))
                            {
                                var dic_Value = dic.FirstOrDefault(f => f.Key.ToLower() == name).Value;

                                try
                                {
                                    item.SetValue(model, ChangeType(dic_Value, item.FieldType));
                                }
                                catch
                                {
                                    item.SetValue(model, dic_Value);
                                }
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }

                #endregion
            }
            catch
            {
                model = default(T);
            }
            return model;
        }

        /// <summary>
        /// 根据 name 或 JsonPropertyAttribute 转成对应的模型
        /// </summary>
        /// <typeparam name="T">转换后的模型类型</typeparam>
        /// <typeparam name="O">要转换的模型类型</typeparam>
        /// <param name="orgModel">要转换的模型数据</param>
        /// <returns>转换后的模型数据</returns>
        public static T ToModel<T, O>(this O orgModel)
            where T : class
        {
            T model;
            try
            {
                model = Activator.CreateInstance<T>();

                if (orgModel == null)
                {
                    return model;
                }

                Type orgType = typeof(O);

                var orgProperties = orgType.GetProperties();
                var orgFileds = orgType.GetFields();

                if (orgProperties == null && orgFileds == null)
                {
                    return model;
                }

                #region 属性
                var props = typeof(T).GetProperties();
                if (props != null)
                {
                    foreach (var item in props)
                    {
                        try
                        {
                            if (!item.CanWrite)
                            {
                                continue;
                            }

                            object value = null;

                            var mapList = GetMapList(item);

                            var o_p = orgProperties.FirstOrDefault(a => a.CanRead && mapList.Exists(m => m.ToLower() == a.Name.ToLower()));
                            if (o_p != null)
                            {
                                value = o_p.GetValue(orgModel, null);
                            }
                            else
                            {
                                var o_f = orgFileds.FirstOrDefault(a => mapList.Exists(m => m.ToLower() == a.Name.ToLower()));
                                if (o_f != null)
                                {
                                    value = o_f.GetValue(orgModel);
                                }
                            }
                            try
                            {
                                item.SetValue(model, ChangeType(value, item.PropertyType), null);
                            }
                            catch
                            {
                                item.SetValue(model, value, null);
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                #endregion

                #region 字段
                var fields = typeof(T).GetFields(BindingFlags.Public);
                if (fields != null)
                {
                    foreach (var item in fields)
                    {
                        try
                        {
                            var mapList = GetMapList(item);

                            object value = null;
                            var o_p = orgProperties.FirstOrDefault(a => a.CanRead && mapList.Exists(m => m.ToLower() == a.Name.ToLower()));
                            if (o_p != null)
                            {
                                value = o_p.GetValue(orgModel, null);
                            }
                            else
                            {
                                var o_f = orgFileds.FirstOrDefault(a => mapList.Exists(m => m.ToLower() == a.Name.ToLower()));
                                if (o_f != null)
                                {
                                    value = o_f.GetValue(orgModel);
                                }
                            }

                            try
                            {
                                item.SetValue(model, ChangeType(value, item.FieldType));
                            }
                            catch
                            {
                                item.SetValue(model, value);
                            }

                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                #endregion

            }
            catch
            {
                model = default(T);
            }
            return model;
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="value">原数据</param>
        /// <param name="type">目标数据类型</param>
        /// <returns></returns>
        public static object ChangeType(object value, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            try
            {
                if (value.GetType().Equals(type))
                {
                    return value;
                }
                if (type.IsGenericType)
                {
                    if (value == null)
                    {
                        return Activator.CreateInstance(type);
                    }
                    if (type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        Convert.ChangeType(value, Nullable.GetUnderlyingType(type));
                    }
                }
                if (value!=null&&type.IsEnum)
                {
                    if (value is string)
                    {
                        return Enum.Parse(type, value as string,true);
                    }
                    else
                    {
                        return Enum.ToObject(type, value);
                    }
                }
                if(value is string)
                {
                    if (type.Equals(typeof(Guid)))
                    {
                        return Guid.Parse(value as string);
                    }
                    if (type.Equals(typeof(Version)))
                    {
                        return Version.Parse(value as string);
                    }
                }
                if (!(value is IConvertible))
                {
                    return value;
                }
                return Convert.ChangeType(value, type);
            }
            catch
            {
                try
                {
                    return Convert.ChangeType(value, Nullable.GetUnderlyingType(type) ?? type);
                }
                catch
                { }
            }

            return value;
        }

        private static List<string> GetMapList(MemberInfo item)
        {
            List<string> mapList = new List<string>();
            try
            {
                if (item == null)
                {
                    return mapList;
                }

                mapList.Add(item.Name);

                var jsonprop = item.GetCustomAttributes(typeof(JsonPropertyAttribute), false);
                if (jsonprop != null)
                {
                    foreach (var map in jsonprop)
                    {
                        var m = map as JsonPropertyAttribute;
                        if (m != null && !string.IsNullOrWhiteSpace(m.PropertyName))
                        {
                            if (!mapList.Contains(m.PropertyName))
                            {
                                mapList.Add(m.PropertyName);
                            }
                        }
                    }
                }
            }
            catch
            { }
            return mapList;
        }


        /// <summary>
        /// DataTable 转 List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt)
            where T : class, new()
        {
            return dt.AsEnumerable().ToList<T>();

        }

        /// <summary>
        /// DataRow[] 转 List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rows"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this IEnumerable<DataRow> rows)
            where T : class, new()
        {

            List<T> list = new List<T>();

            if (rows == null || rows.Count() == 0)
            {
                return list;
            }
            var columns = rows.FirstOrDefault().Table.Columns;
            Type type = typeof(T);
            foreach (var dr in rows)
            {
                T model = new T();
                foreach (var pi in type.GetProperties())
                {
                    try
                    {
                        if (columns.Contains(pi.Name))
                        {
                            if (!pi.CanWrite)
                            {
                                continue;
                            }

                            object value = dr[pi.Name];
                            if (value != DBNull.Value)
                            {
                                try
                                {
                                    pi.SetValue(model, value, null);
                                }
                                catch
                                {
                                    pi.SetValue(model, value.ToString(), null);
                                }
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// IEnumerable<T> 转 DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="models"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> models)
        {
            try
            {
                Type elementType = typeof(T);
                var dt = new DataTable();
                elementType.GetProperties().ToList().ForEach(propInfo => dt.Columns.Add(propInfo.Name, Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType));
                foreach (T item in models)
                {
                    var row = dt.NewRow();
                    elementType.GetProperties().ToList().ForEach(propInfo => row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value);
                    dt.Rows.Add(row);
                }
                return dt;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// IEnumerable<T> 转 DataSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="models"></param>
        /// <returns></returns>
        public static DataSet ToDataSet<T>(this IEnumerable<T> models)
        {
            try
            {
                DataSet ds = new DataSet();
                var dt = models.ToDataTable();
                ds.Tables.Add(dt.Copy());

                return ds;
            }
            catch
            {
                return null;
            }
        }
    }
}
