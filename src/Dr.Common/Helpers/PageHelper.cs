#region PageHelper 声明

/**************************************************************
* 命名空间 ：Dr.Common.Helpers
* 类 名 称 ：PageHelper
* 创 建 人 ：XIAOXL084520
* 邮    箱 ：Xiaoxl084520@163.com
* 创建时间 ：2019-3-29 13:57:10
* 版 本 号 ：V1.0
* 功能描述 ：N/A
* 
*┌ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┐
*│　 Copyright (c) 2019 XIAOXL084520 DR.All rights reserved.   │
*└ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┘
*
****************************************************************/

#endregion


namespace Dr.Common.Helpers
{
    /// <summary>
    /// 分页帮助类
    /// </summary>
    public class PageHelper
    {
        //页码
        private int _PageIndex = 1;
        //页容
        private int _PageSize = 20;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容</param>
        public PageHelper(int pageIndex, int pageSize)
        {
            if (pageIndex > 1)
            {
                this._PageIndex = pageIndex;
            }
            if (pageSize > 1)
            {
                this._PageSize = pageSize;
            }
        }

        /// <summary>
        ///开始项
        /// </summary>
        public int StartIndex => (_PageIndex - 1) * _PageSize + 1;

        /// <summary>
        /// 结束项
        /// </summary>
        public int EndIndex => this._PageIndex * this._PageSize;
    }
}
