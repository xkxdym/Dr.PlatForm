
#region FileStruct 声明

/**************************************************************
* 命名空间 ：Dr.Common.FTP
* 类 名 称 ：FileStruct
* 创 建 人 ：XIAOXL084520
* 邮    箱 ：Xiaoxl084520@163.com
* 创建时间 ：2019-4-1 15:50:43
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

namespace Dr.Common.FTP
{
    /// <summary> 
    /// 文件描述信息
    /// </summary> 
    public class FileStruct
    {
        /// <summary>
        /// 属性
        /// </summary>
        public string Flags;
        /// <summary>
        /// 所有者
        /// </summary>
        public string Owner;
        /// <summary>
        /// 是否为目录
        /// </summary>
        public bool IsDirectory;
        /// <summary>
        /// 更新时间
        /// </summary>
        public string CreateTime;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 文件大小
        /// </summary>
        public int FileSize;

        /// <summary>
        /// 类型
        /// </summary>
        public string FileType;
    }
}
