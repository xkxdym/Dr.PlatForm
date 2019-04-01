
#region MemoryCache 声明

/**************************************************************
* 命名空间 ：Dr.Common.Cache.ALL
* 类 名 称 ：MemoryCache
* 创 建 人 ：XIAOXL084520
* 邮    箱 ：Xiaoxl084520@163.com
* 创建时间 ：2019-3-29 14:15:30
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
using System.IO;
using System.Runtime.Caching;
using System.Collections.Concurrent;
using Dr.Common.Helpers;

namespace Dr.Common.Cache.ALL
{
    /// <summary>
    /// 缓存帮助类
    /// </summary>
    public class MemoryCache
    {
        private const string MemoryCachePrefix = "Dr_";
        private static ConcurrentBag<string> _KEYS = new ConcurrentBag<string>();
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

                    CacheItem cacheItem = cache.GetCacheItem(cacheKey);
                    if (cacheItem != null)
                    {
                        result = (T)cacheItem.Value;
                    }
                    else
                    {
                        result = getValue();
                        if (result == null)
                        {
                            return result;
                        }
                        cacheItem = new CacheItem(cacheKey, result);

                        CacheItemPolicy policy = new CacheItemPolicy()
                        {
                            Priority = CacheItemPriority.Default,
                            UpdateCallback = (m) =>
                            {
                                //LogHelper.Instance.AddLog($"[{m.UpdatedCacheItem.Key}]已更新缓存！");
                            },
                            RemovedCallback = (m) =>
                            {
                                var key = m.CacheItem.Key;
                                _KEYS.TryTake(out key);
                                //LogHelper.Instance.AddLog($"[{m.CacheItem.Key}]已移除缓存：{m.RemovedReason}！");
                            }
                        };
                        if (timeOut.HasValue)
                        {
                            policy.AbsoluteExpiration = timeOut.Value;
                        }
                        if (slidingExpiration.HasValue)
                        {
                            policy.SlidingExpiration = slidingExpiration.Value;
                        }
                        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                        {
                            policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string>() { filePath }));
                        }
                        cache.Remove(cacheKey);
                        cache.Add(cacheItem, policy);
                        _KEYS.Add(cacheKey);
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
                    var cache = GetCache();

                    cacheKey = BuildCacheKey(cacheKey);

                    var cacheItem = cache.GetCacheItem(cacheKey);
                    if (cacheItem != null)
                    {
                        result = (T)cacheItem.Value;
                    }
                    else
                    {
                        result = Activator.CreateInstance<T>();
                    } 
                }
            }
            catch (Exception ex)
            {
                ex.AddLog();
                result = default(T);
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

                    _KEYS.TryTake(out cacheKey); 
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
                    var cache = GetCache();
                    var items = _KEYS.GetEnumerator();
                    while (items.MoveNext())
                    {
                        if (items.Current.StartsWith(MemoryCachePrefix))
                        {
                            cache.Remove(items.Current);
                        }
                    } 
                }
            }
            catch
            { }
        }

        private ObjectCache GetCache()
        {
            ObjectCache cache = System.Runtime.Caching.MemoryCache.Default;

            return cache;
        }

        private string BuildCacheKey(string key)
        {
            return $"{MemoryCachePrefix}{key}";
        }
    }
}

