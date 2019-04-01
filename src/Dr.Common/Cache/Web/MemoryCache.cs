
#region MemoryCache 声明

/**************************************************************
* 命名空间 ：Dr.Common.Cache.Web
* 类 名 称 ：MemoryCache
* 创 建 人 ：XIAOXL084520
* 邮    箱 ：Xiaoxl084520@163.com
* 创建时间 ：2019-3-29 14:11:43
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
using Dr.Common.Helpers;
using System;
using System.IO;
using System.Web;
using System.Web.Caching;

namespace Dr.Common.Cache.Web
{
    /// <summary>
    /// 缓存帮助类
    /// </summary>
    public class MemoryCache
    {
        private const string MemoryCachePrefix = "Dr_";
        private static object _lock = new object();

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns></returns>
        public static MemoryCache Instance => Singleton<MemoryCache>.Instance;

        /// <summary>
        /// 从缓存中获取数据
        /// </summary>
        /// <typeparam name="T">返回的类型</typeparam>
        /// <param name="cacheKey">缓存的key</param>
        /// <param name="getValue">获取数据</param>
        /// <param name="timeOut">绝对过期时间 (null 则 表示从不过期 )</param>
        /// <param name="filePath">文件依赖项</param>
        /// <returns></returns>
        public T GetOrCreate<T>(string cacheKey, Func<T> getValue, DateTime? timeOut = null, string filePath = null)
        {
            return GetOrCreate(cacheKey, getValue, timeOut, null, filePath);
        }

        /// <summary>
        /// 从缓存中获取数据
        /// </summary>
        /// <typeparam name="T">返回的类型</typeparam>
        /// <param name="cacheKey">缓存的key</param>
        /// <param name="getValue">获取数据</param>
        /// <param name="slidingExpiration">滑动过期时间间隔</param>
        /// <param name="filePath">文件依赖项</param>
        /// <returns></returns>
        public T GetOrCreate<T>(string cacheKey, Func<T> getValue, TimeSpan slidingExpiration, string filePath = null)
        {
            return GetOrCreate(cacheKey, getValue, null, slidingExpiration, filePath);
        }

        /// <summary>
        /// 从缓存中获取数据
        /// </summary>
        /// <typeparam name="T">返回的类型</typeparam>
        /// <param name="cacheKey">缓存的key</param>
        /// <param name="getValue">获取数据</param>
        /// <param name="timeOut">过期时间</param>
        /// <param name="slidingExpiration">滑动过期时间间隔</param>
        /// <param name="filePath">文件依赖项</param>
        /// <returns></returns>
        private T GetOrCreate<T>(string cacheKey, Func<T> getValue, DateTime? timeOut = null, TimeSpan? slidingExpiration = null, string filePath = null)
        {
            T result;
            try
            {
                lock (_lock)
                {
                    var cache = GetCache();

                    cacheKey = BuildCacheKey(cacheKey);

                    var value = cache.Get(cacheKey);
                    if (value != null)
                    {
                        result = (T)value;
                    }
                    else
                    {
                        CacheDependency dependency = null;
                        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                        {
                            dependency = new CacheDependency(filePath);
                        }

                        result = getValue();
                        if (result == null)
                        {
                            return result;
                        }
                        cache.Remove(cacheKey);
                        cache.Insert(cacheKey, result, dependency, timeOut ?? System.Web.Caching.Cache.NoAbsoluteExpiration, slidingExpiration ?? System.Web.Caching.Cache.NoSlidingExpiration);
                    } 
                }
            }
            catch (Exception ex)
            {
                ex.AddLog();
                result = Activator.CreateInstance<T>();
            }
            return result;
        }

        /// <summary>
        /// 获取缓存的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>(string cacheKey)
        {
            T result;

            try
            {
                lock (_lock)
                {
                    System.Web.Caching.Cache cache = GetCache();

                    cacheKey = BuildCacheKey(cacheKey);

                    var value = cache.Get(cacheKey);
                    if (value != null)
                    {
                        result = (T)value;
                    }
                    else
                    {
                        result = default(T);
                    } 
                }
            }
            catch (Exception ex)
            {
                ex.AddLog();
                result = Activator.CreateInstance<T>();
            }

            return result;
        }


        /// <summary>
        /// 清除指定缓存数据
        /// </summary>
        /// <param name="cacheKey"></param>
        public void Clear(string cacheKey)
        {
            try
            {
                lock (_lock)
                {
                    GetCache().Remove(BuildCacheKey(cacheKey)); 
                }
            }
            catch
            { }
        }

        /// <summary>
        /// 清除所有缓存数据
        /// </summary>
        public void ClearAll()
        {
            try
            {
                lock (_lock)
                {
                    System.Web.Caching.Cache cache = GetCache();

                    var items = cache.GetEnumerator();
                    while (items.MoveNext())
                    {
                        if (items.Key.ToString().StartsWith(MemoryCachePrefix))
                        {
                            cache.Remove(items.Key.ToString());
                        }
                    } 
                }
            }
            catch
            { }
        }

        private System.Web.Caching.Cache GetCache()
        {
            System.Web.Caching.Cache cache = null;
            try
            {
                cache = HttpContext.Current.Cache;
            }
            catch { }
            if (cache == null)
            {
                cache = HttpRuntime.Cache;
            }

            return cache;
        }

        private string BuildCacheKey(string key)
        {
            return $"{MemoryCachePrefix}{key}";
        }
    }
}
