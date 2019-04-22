
#region XmlDictionary 声明

/**************************************************************
* 命名空间 ：Dr.Common.Data
* 类 名 称 ：XmlDictionary
* 创 建 人 ：XIAOXL084520
* 邮    箱 ：Xiaoxl084520@163.com
* 创建时间 ：2019-4-1 14:44:33
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
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Dr.Common.Data
{
    /// <summary>
    /// 支持XML序列化的字典
    /// <para>
    ///     默认是Element，如果是Attribute 需要执行TKey+"@attr",如
    ///     {"id@attr":"1"}
    /// </para>
    /// </summary>
    [Serializable]
    public class XmlDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        /// <summary>
        /// 以 attributeTag 为结尾的则需要序列化为Attribute
        /// </summary>
        public const string attributeTag = "@attr";

        #region 构造方法
        public XmlDictionary() { }

        public XmlDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }

        public XmlDictionary(IEqualityComparer<TKey> comparer) : base(comparer) { }

        public XmlDictionary(int capacity) : base(capacity) { }

        public XmlDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer) { }

        protected XmlDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

        public XmlSchema GetSchema() => null;

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="xr"></param>
        public void ReadXml(XmlReader xr)
        {
            if (xr.IsEmptyElement)
            {
                return;
            }
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));

            xr.Read();
            while (xr.NodeType != XmlNodeType.EndElement)
            {
                try
                {
                    Add((TKey)Convert.ChangeType(xr.LocalName, typeof(TKey)), (TValue)Convert.ChangeType(xr.ReadElementString(), typeof(TValue)));
                }
                finally
                {
                    xr.MoveToContent();
                }
            }
            xr.ReadEndElement();
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="xw"></param>
        public void WriteXml(XmlWriter xw)
        {
            try
            {
                //attribute
                foreach (var key in Keys)
                {
                    try
                    {
                        var _key = key.ToString();
                        if (_key.EndsWith(attributeTag))
                        {
                            xw.WriteStartAttribute(_key.Replace(attributeTag, string.Empty));
                            xw.WriteValue(this[key]);
                            xw.WriteEndAttribute();
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.AddLog();
                        continue;
                    }
                }
                //element
                foreach (var key in Keys)
                {
                    try
                    {
                        var _key = key.ToString();
                        if (!_key.EndsWith(attributeTag))
                        {
                            xw.WriteStartElement(key.ToString());
                            xw.WriteValue(this[key]);
                            xw.WriteEndElement();
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.AddLog();
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.AddLog();
            }
        }
    }
}
