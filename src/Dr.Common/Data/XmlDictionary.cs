
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
    /// </summary>
    [Serializable]
    public class XmlDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
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
            xr.Read();
            while (xr.NodeType != XmlNodeType.EndElement)
            {
                Add((TKey)Convert.ChangeType(xr.Name, typeof(TKey)), (TValue)Convert.ChangeType(xr.ReadElementString(), typeof(TValue)));
                xr.MoveToContent();
            }
            xr.ReadEndElement();
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="xw"></param>
        public void WriteXml(XmlWriter xw)
        {
            foreach (var key in Keys)
            {
                xw.WriteStartElement(key.ToString());
                xw.WriteValue(this[key]);
                xw.WriteEndElement();
            }
        }
    }
}
