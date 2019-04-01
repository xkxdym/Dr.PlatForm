
#region StringExtension 声明

/**************************************************************
* 命名空间 ：Dr.Common.Extensions
* 类 名 称 ：StringExtension
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

using Dr.Common.Data;
using Dr.Common.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Dr.Common.Extensions
{
    /// <summary>
    /// string 扩展
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// IsNullOrEmpty
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// IsNullOrWhiteSpace
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// left截取字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Left(this string str, int length)
        {
            return str.Substring(0, length > str.Length ? str.Length : length);
        }

        /// <summary>
        /// Right截取字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Right(this string str, int length)
        {
            return str.Substring(length > str.Length ? 0 : str.Length - length);
        }

        /// <summary>
        /// mid截取字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Mid(this string str, int start, int length)
        {
            int iStartPoint = start > str.Length ? str.Length : start;
            return str.Substring(iStartPoint, iStartPoint + length > str.Length ? str.Length - iStartPoint : length);
        }

        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToMD5(this string str)
        {
            return EncryptsHelper.Md5(str);
        }

        /// <summary>
        /// SHA1 加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToSHA1(this string str)
        {
            return EncryptsHelper.SHA1(str);
        }

        /// <summary>
        /// HMAC-SHA256 加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToHMAC_SHA256(this string str)
        {
            return EncryptsHelper.HMAC_SHA256(str);
        }

        /// <summary>
        /// SHA256 加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToSHA256(this string str)
        {
            return EncryptsHelper.SHA256(str);
        }
        
        /// <summary>
        /// SHA512 加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToSHA512(this string str)
        {
            return EncryptsHelper.SHA512(str);
        }

        /// <summary>
        /// 获取GUID
        /// </summary>
        /// <returns></returns>
        public static string GetGuid(this string str)
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 字符串用 , 分割成int List
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<int> ToIntList(this string str)
        {
            return str.ToList(',').Select(m => m.ToInt()).ToList();
        }

        /// <summary>
        /// 字符串用 , 分割成List
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> ToList(this string str)
        {
            return str.ToList(',');
        }

        /// <summary>
        /// 字符串List
        /// </summary>
        /// <param name="str"></param>
        /// <param name="split">分割字符</param>
        /// <returns></returns>
        public static List<string> ToList(this string str, params char[] split)
        {
            List<string> result = new List<string>();

            try
            {
                if (!string.IsNullOrWhiteSpace(str.Trim()))
                {
                    result = str.Split(split ?? new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
            }
            catch
            { }
            return result;
        }

        /// <summary>
        /// 通过身份证号获取用户性别
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns>男/女</returns>
        public static string GetSexFromCardId(this string cardId)
        {
            string sex = string.Empty;
            try
            {
                if (cardId.IsCardId())
                {
                    var num = 0;
                    if (cardId.Length == 18)
                    {
                        num = cardId.Substring(14, 3).ToInt();
                    }
                    if (cardId.Length == 15)
                    {
                        num = cardId.Substring(12, 3).ToInt();
                    }
                    if (num % 2 == 0)//性别代码为偶数是女性奇数为男性
                    {
                        sex = "女";
                    }
                    else
                    {
                        sex = "男";
                    }
                }
            }
            catch
            { }

            return sex;
        }

        /// <summary>
        /// 通过身份证号获取用户生日
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns>yyyy-MM-dd</returns>
        public static string GetBirthdayFromCardId(this string cardId)
        {
            string birthday = string.Empty;
            try
            {
                if (cardId.Length == 18)
                {
                    birthday = cardId.Substring(6, 4) + "-" + cardId.Substring(10, 2) + "-" + cardId.Substring(12, 2);
                }
                if (cardId.Length == 15)
                {
                    birthday = "19" + cardId.Substring(6, 2) + "-" + cardId.Substring(8, 2) + "-" + cardId.Substring(10, 2);
                }
            }
            catch
            { }
            return birthday;
        }

        /// <summary>
        /// 过滤Sql查询关键词中的敏感词汇
        /// </summary>
        public static string SqlFilter(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            value = value.Replace("'", "''").Trim();
            //捕获的字符转换为""
            return Regex.Replace(value,
                @"0x([0-9a-fA-F]{4})+|(%[0-9a-fA-F]{2})+|--|@@|count|asc|mid|char|chr|sysobjects|sys.|select|insert|delete|update|drop|truncate|xp_cmdshell|netlocalgroup|administrator|net user|exec|master|declare|localgroup|remove|create|extended_properties|objects|columns|types|extended|comments|table|cast",
                "", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// HTML转纯文本
        /// </summary>
        public static string HtmlToText(this string str)
        {
            string regexstr = @"(&(#)?.+;)|(<[^>]*>)";
            return Regex.Replace(str, regexstr, string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 过滤HTML中的不安全标签
        /// </summary>
        public static string HtmlFilter(this string str)
        {
            str = Regex.Replace(str, @"(\<|\s+)on([a-z]+\s?=)", "$1_$2", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"(DOCTYPE|html|base|head|meta|form|button|optgroup|canvas|command|noframes|output|keygen|datalist|option|select|textarea|input|link|iframe|frameset|frame|form|applet|embed|layer|param|meta|object|script|noscript)([\s|:|>])+", "$1_$2", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"javascript|eval", "", RegexOptions.IgnoreCase);
            return str;
        }

        /// <summary>
        /// XML 格式化
        /// </summary>
        /// <param name="xml">要格式化的xml</param>
        /// <returns></returns>
        public static string XmlFormat(this string xml)
        {
            try
            {
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(xml);
                StringBuilder sb = new StringBuilder();
                using (StringWriter sw = new StringWriter(sb))
                {
                    using (XmlTextWriter xtw = new XmlTextWriter(sw))
                    {
                        xtw.Formatting = Formatting.Indented;
                        xtw.Indentation = 1;
                        xtw.IndentChar = '\t';

                        xd.WriteTo(xtw);
                    }
                }

                xml = sb.ToString();
            }
            catch
            {}

            return xml;
        }

        /// <summary>
        /// 压缩字符串
        /// </summary>
        /// <param name="unCompressedString">要压缩的字符串</param>
        /// <returns></returns>
        public static string ZipString(this string unCompressedString)
        {
            byte[] bytData = Encoding.UTF8.GetBytes(unCompressedString);
            MemoryStream ms = new MemoryStream();
            Stream s = new GZipStream(ms, CompressionMode.Compress);
            s.Write(bytData, 0, bytData.Length);
            s.Close();
            byte[] compressedData = (byte[])ms.ToArray();
            return Convert.ToBase64String(compressedData, 0, compressedData.Length);
        }

        /// <summary>
        ///  解压字符串
        /// </summary>
        /// <param name="unCompressedString">要解压的字符串</param>
        /// <returns></returns>
        public static string UnzipString(this string unCompressedString)
        {
           StringBuilder uncompressedString = new StringBuilder();
            byte[] writeData = new byte[4096];

            byte[] bytData = Convert.FromBase64String(unCompressedString);
            int totalLength = 0;
            int size = 0;

            Stream s = new GZipStream(new MemoryStream(bytData), CompressionMode.Decompress);
            while (true)
            {
                size = s.Read(writeData, 0, writeData.Length);
                if (size > 0)
                {
                    totalLength += size;
                    uncompressedString.Append(System.Text.Encoding.UTF8.GetString(writeData, 0, size));
                }
                else
                {
                    break;
                }
            }
            s.Close();
            return uncompressedString.ToString();
        }

        /// <summary>
        ///字符串编码转换
        /// </summary>
        public static string ConvertEncode(this string str, Encoding fromEncoding,Encoding toEncoding)
        {
            if (str.IsNullOrWhiteSpace() || fromEncoding == null || toEncoding == null)
            {
                return str;
            }
            return toEncoding.GetString(Encoding.Convert(fromEncoding, toEncoding, fromEncoding.GetBytes(str)));
        }
    }
}