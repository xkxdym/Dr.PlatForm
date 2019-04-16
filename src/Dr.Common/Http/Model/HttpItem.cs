﻿
#region HttpItem 声明

/**************************************************************
* 命名空间 ：Dr.Common.Http.Model
* 类 名 称 ：HttpItem
* 创 建 人 ：XIAOXL084520
* 邮    箱 ：Xiaoxl084520@163.com
* 创建时间 ：2019-3-29 13:49:02
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
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Dr.Common.Http.Model
{
    /// <summary>
    /// http 请求参数模型
    /// </summary>
    public class HttpItem
    {
        /// <summary>
        /// 请求的URL地址
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// 请求方法 默认是GET
        /// </summary>
        public string Method { get; set; } ="GET";

        /// <summary>
        /// 请求的Header
        /// </summary>
        public WebHeaderCollection Header { get; set; }

        /// <summary>
        /// 请求超时时间 默认10000
        /// </summary>
        public int TimeOut { get; set; } = 1000 * 10;

        /// <summary>
        /// 写入Post数据超时时间 默认30000
        /// </summary>
        public int ReadWriteTimeOut { get; set; } = 1000 * 30;

        /// <summary>
        /// 请求是否与 Internet 资源建立持久性连接 默认为true
        /// </summary>
        public bool KeepAlive { get; set; } = true;

        /// <summary>
        /// 请求标头值 默认为text/html, application/xhtml+xml,application/json, */*
        /// </summary>
        public string Accept { get; set; } = "text/html, application/xhtml+xml,application/json, */*";

        /// <summary>
        /// 请求参数类型 默认application/x-www-form-urlencoded
        /// </summary>
        public string ContentType { get; set; } = "application/x-www-form-urlencoded";

        /// <summary>
        /// 客户端访问信息 默认 Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3554.0 Safari/537.36
        /// </summary>
        public string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3554.0 Safari/537.36";

        /// <summary>
        /// 返回数据编码
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Post 的数据类型 默认 String
        /// </summary>
        public HttpPostDataType PostDataType { get; set; } = HttpPostDataType.String;

        /// <summary>
        /// Post参数编码 默认为Default
        /// </summary>
        public Encoding PostEncoding { get; set; } = Encoding.Default;

        /// <summary>
        /// Post请求时要发送的字符串Post数据
        /// </summary>
        public dynamic PostData { get; set; }

        /// <summary>
        /// 代理 默认Null
        /// </summary>
        public WebProxy WebProxy { get; set; }

        /// <summary>
        /// 请求时的Cookie
        /// </summary>
        public string Cookie { get; set; }

        /// <summary>
        /// Cookie对象集合
        /// </summary>
        public CookieCollection CookieCollection { get; set; }

        /// <summary>
        /// 请求的来源地址
        /// </summary>
        public string Referer { get; set; }

        /// <summary>
        /// 是否设置为全文小写，默认false
        /// </summary>
        public bool IsToLower { get; set; }

        /// <summary>
        /// 支持跳转页面，查询结果将是跳转后的页面，默认是false
        /// </summary>
        public bool AllowAutoRedirect { get; set; }

        /// <summary>
        /// 最大连接数 默认 1024
        /// </summary>
        public int ConnectionLimit { get; set; } = 1024;

        /// <summary>
        /// 返回类型 默认String
        /// </summary>
        public HttpResultType ResultType { get; set; } = HttpResultType.String;

        /// <summary>
        /// 证书绝对路径
        /// </summary>
        public string CerPath { get; set; }

        /// <summary>
        /// 设置509证书集合
        /// </summary>
        public X509CertificateCollection ClentCertificates { get; set; }

        /// <summary>
        /// 请求的身份验证信息。
        /// </summary>
        public ICredentials ICredentials { get; set; } = CredentialCache.DefaultCredentials;

        /// <summary>
        /// 
        /// </summary>
        public Action<HttpResult> Finaly { get; set; }
    }
}
