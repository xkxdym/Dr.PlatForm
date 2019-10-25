
#region HttpHelper 声明

/**************************************************************
* 命名空间 ：Dr.Common.Http
* 类 名 称 ：HttpHelper
* 创 建 人 ：XIAOXL084520
* 邮    箱 ：Xiaoxl084520@163.com
* 创建时间 ：2019-3-29 13:47:58
* 版 本 号 ：V1.0
* 功能描述 ：N/A
* 
*┌ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┐
*│　 Copyright (c) 2019 XIAOXL084520 DR.All rights reserved.   │
*└ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┘
*
****************************************************************/

#endregion

using Dr.Common.Helpers;
using Dr.Common.Http.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace Dr.Common.Http
{
    /// <summary>
    /// http请求帮助类
    /// </summary>
    public class HttpHelper
    {
        private HttpWebRequest request;

        private HttpWebResponse response;

        /// <summary>
        /// 获取实例
        /// </summary>
        public static HttpHelper Instance => Singleton<HttpHelper>.Instance;

        /// <summary>
        /// 获取请求结果
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public HttpResult GetResult(HttpItem item)
        {

            HttpResult result = null;

            try
            {
                InitRequest(item);
            }
            catch (Exception ex)
            {
                result = new HttpResult()
                {
                    Html = ex.Message,
                    StatusDescription = "配置参数时出错：" + ex.Message
                };
                return result;
            }

            try
            {
                using (response = (HttpWebResponse)request.GetResponse())
                {
                    result = new HttpResult()
                    {
                        StatusCode = response.StatusCode,
                        StatusDescription = response.StatusDescription,
                        Header = response.Headers,
                        CookieCollection = response.Cookies,
                        Cookie = response.Headers["set-cookie"]
                    };

                    MemoryStream _stream = new MemoryStream();
                    //GZIIP处理
                    if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                    {
                        _stream = GetMemoryStream(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress));

                    }
                    else
                    {
                        _stream = GetMemoryStream(response.GetResponseStream());
                    }

                    //获取Byte
                    byte[] ResponseByte = _stream.ToArray();
                    _stream.Close();
                    if (ResponseByte != null & ResponseByte.Length > 0)
                    {
                        //是否返回Byte类型数据
                        if (item.ResultType == HttpResultType.StringAndByte)
                        {
                            result.ResultByte = ResponseByte;
                        }

                        #region 当编码是空的时候从mete中获取编码类型
                        if (item.Encoding == null)
                        {
                            try
                            {
                                string pattern = @"(?i)\bcharset=(?<charset>[-a-zA-Z_0-9]+)";
                                string charset = Regex.Match(Encoding.Default.GetString(ResponseByte), pattern).Groups["charset"].Value;

                                if (!string.IsNullOrEmpty(charset))
                                {
                                    item.Encoding = Encoding.GetEncoding(charset.Trim());
                                }

                                if (item.Encoding == null)
                                {
                                    if (string.IsNullOrEmpty(response.CharacterSet))
                                    {
                                        item.Encoding = Encoding.UTF8;
                                    }
                                    else
                                    {
                                        item.Encoding = Encoding.GetEncoding(response.CharacterSet);
                                    }
                                }
                            }
                            catch
                            {
                                item.Encoding = Encoding.Default;
                            }
                        }
                        #endregion

                        //得到返回的HTML
                        result.Html = item.Encoding.GetString(ResponseByte);
                    }
                    else
                    {
                        result.Html = string.Empty;
                    }
                }
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
                result = new HttpResult()
                {
                    Html = ex.Message
                };
                if (response != null)
                {
                    result.StatusCode = response.StatusCode;
                    result.StatusDescription = response.StatusDescription;
                }
            }
            catch (Exception ex)
            {
                result = new HttpResult()
                {
                    Html = ex.Message
                };
            }
            finally
            {
                if (item.IsToLower)
                {
                    result.Html = result.Html.ToLower();
                }
                try
                {
                    item.Finaly?.Invoke(result);
                }
                catch { }
            }
            return result;
        }

        /// <summary>
        /// 初始化请求
        /// </summary>
        private void InitRequest(HttpItem item)
        {
            //证书
            SetCer(item);

            //设置Header
            if (item.Header != null && item.Header.Count > 0)
            {
                foreach (string key in item.Header.AllKeys)
                {
                    request.Headers.Add(key, item.Header[key]);
                }
            }
            request.Proxy = item.WebProxy;
            request.Method = item.Method.ToString();
            request.Timeout = item.TimeOut;
            request.KeepAlive = item.KeepAlive;
            request.ReadWriteTimeout = item.ReadWriteTimeOut;
            request.Accept = item.Accept;
            request.ContentType = item.ContentType;
            request.UserAgent = item.UserAgent;
            request.Referer = item.Referer;
            request.AllowAutoRedirect = item.AllowAutoRedirect;
            request.Credentials = item.ICredentials;
            if (item.ConnectionLimit > 0)
            {
                request.ServicePoint.ConnectionLimit = item.ConnectionLimit;
            };
            //设置Cookie
            if (!string.IsNullOrEmpty(item.Cookie))
            {
                request.Headers[HttpRequestHeader.Cookie] = item.Cookie;
            }
            //设置CookieCollection
            if (item.CookieCollection != null && item.CookieCollection.Count > 0)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(item.CookieCollection);
            }

            //设置Post数据
            SetPostData(item);

        }

        /// <summary>
        /// 设置Post数据
        /// </summary>
        /// <param name="item">Http参数</param>
        private void SetPostData(HttpItem item)
        {
            //验证在得到结果时是否有传入数据
            if (item.Method == HttpMethod.GET)
            {
                byte[] buffer = null;
                switch (item.PostDataType)
                {
                    case HttpPostDataType.String:
                        buffer = item.PostEncoding.GetBytes(item.PostData);
                        break;
                    case HttpPostDataType.Dictionary:
                        Dictionary<string, string> dic = item.PostData;
                        if (dic != null && dic.Count > 0)
                        {
                            var str = string.Join("&", dic.Select(m => $"{m.Key}={m.Value}"));
                            buffer = item.PostEncoding.GetBytes(str);
                        }
                        break;
                    case HttpPostDataType.Byte:
                        byte[] bytes = item.PostData;
                        if (bytes != null && bytes.Length > 0)
                        {
                            buffer = bytes;
                        }
                        break;
                    case HttpPostDataType.FilePath:
                        using (StreamReader r = new StreamReader(item.PostData, item.PostEncoding))
                        {
                            buffer = item.PostEncoding.GetBytes(r.ReadToEnd());
                        }
                        break;
                }
                if (buffer != null)
                {
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }
            }
        }

        /// <summary>
        /// 设置证书
        /// </summary>
        /// <param name="item"></param>
        private void SetCer(HttpItem item)
        {
            if (!string.IsNullOrEmpty(item.CerPath))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                //初始化对像，并设置请求的URL地址
                request = (HttpWebRequest)WebRequest.Create(item.URL);
                SetCerList(item);
                //将证书添加到请求里
                request.ClientCertificates.Add(new X509Certificate(item.CerPath));
            }
            else
            {
                //初始化对像，并设置请求的URL地址
                request = (HttpWebRequest)WebRequest.Create(item.URL);
                SetCerList(item);
            }
        }

        /// <summary>
        /// 设置多个证书
        /// </summary>
        /// <param name="item"></param>
        private void SetCerList(HttpItem item)
        {
            if (item.ClentCertificates != null && item.ClentCertificates.Count > 0)
            {
                foreach (X509Certificate c in item.ClentCertificates)
                {
                    request.ClientCertificates.Add(c);
                }
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="streamResponse">流</param>
        private MemoryStream GetMemoryStream(Stream streamResponse)
        {
            MemoryStream _stream = new MemoryStream();
            int Length = 256;
            byte[] buffer = new byte[Length];
            int bytesRead = streamResponse.Read(buffer, 0, Length);
            while (bytesRead > 0)
            {
                _stream.Write(buffer, 0, bytesRead);
                bytesRead = streamResponse.Read(buffer, 0, Length);
            }
            return _stream;
        }

        /// <summary>
        /// 回调验证证书问题
        /// </summary>
        /// <param name="sender">流对象</param>
        /// <param name="certificate">证书</param>
        /// <param name="chain">X509Chain</param>
        /// <param name="errors">SslPolicyErrors</param>
        /// <returns>bool</returns>
        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) => true;
    }
}
