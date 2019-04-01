#region HttpEnum 声明

/**************************************************************
* 命名空间 ：Dr.Common.Http.Model
* 类 名 称 ：HttpEnum
* 创 建 人 ：XIAOXL084520
* 邮    箱 ：Xiaoxl084520@163.com
* 创建时间 ：2019-3-29 13:48:50
* 版 本 号 ：V1.0
* 功能描述 ：N/A
* 
*┌ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┐
*│　 Copyright (c) 2019 XIAOXL084520 DR.All rights reserved.   │
*└ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┘
*
****************************************************************/

#endregion


namespace Dr.Common.Http.Model
{
    /// <summary>
    /// Post 的数据类型
    /// </summary>
    public enum HttpPostDataType
    {
        /// <summary>
        /// 字符串
        /// </summary>
        String,

        /// <summary>
        /// 数据字典
        /// </summary>
        Dictionary,

        /// <summary>
        /// Byte
        /// </summary>
        Byte,

        /// <summary>
        /// 文件地址
        /// </summary>
        FilePath
    }

    /// <summary>
    /// 请求返回类型
    /// </summary>
    public enum HttpResultType
    {
        /// <summary>
        /// 字符串 只有Html有数据
        /// </summary>
        String,

        /// <summary>
        /// 返回字符串和字节流
        /// </summary>
        StringAndByte
    }
}
