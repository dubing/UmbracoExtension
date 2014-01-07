using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;

namespace AKQA.Common.Caching
{
    /// <summary>
    /// Cache
    /// </summary>
    public static class Cache
    {
        private static string _extensionname = "_extension";
        private static int? _queryCacheTimeOut;
        private static int? _commonCacheTimeOut;

        /// <summary>
        /// Cache lock
        /// </summary>
        private static readonly object _cacheLocker = new object();


        /// <summary>
        /// CommonCacheTimeOut
        /// </summary>
        /// <value>The common cache time out.</value>
        public static int CommonCacheTimeOut
        {
            get
            {
                if (_commonCacheTimeOut == null)
                {
                    string settings = ConfigurationManager.AppSettings["CommonCacheTimeOut"];

                    int timeOut;
                    if (!int.TryParse(settings, out timeOut))
                    {
                        timeOut = 3600;
                    }

                    _commonCacheTimeOut = timeOut;
                }

                return _commonCacheTimeOut.Value;
            }
        }





        /// <summary>
        /// Get CachedObject by key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            var cache = MemoryCache.Default;
            return (T)cache.Get(key);
        }

        /// <summary>
        /// GetCachedObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="timeOut">timeOut</param>
        /// <param name="onCreateInstance">The on create instance.</param>
        /// <returns></returns>
        public static T GetCachedObject<T>(string key, int timeOut, Func<T> onCreateInstance)
        {
            if (timeOut > 0)
            {
                var cache = MemoryCache.Default;

                var cachedObject = (T)cache.Get(key);

                if (cachedObject == null)
                {
                    lock (_cacheLocker)
                    {
                        cachedObject = (T)cache.Get(key);
                        if (cachedObject == null)
                        {
                            cachedObject = onCreateInstance();
                            if (cachedObject != null)
                                cache.Add(key, cachedObject, DateTime.Now.AddSeconds(timeOut));
                        }
                    }

                }
                return cachedObject;
            }
            return onCreateInstance();
        }

        /// <summary>
        /// GetCachedObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="timeOut">timeOut</param>
        /// <param name="onCreateInstance">The on create instance.</param>
        /// <param name="extensionobject"></param>
        /// <returns></returns>
        public static T GetCachedObject<T>(string key, int timeOut, Func<T> onCreateInstance, object extensionobject)
        {
            if (extensionobject == null)
            {
                return GetCachedObject(key, timeOut, onCreateInstance);
            }
            if (timeOut > 0)
            {
                var cache = MemoryCache.Default;

                var cachedObject = (T)cache.Get(key);
                extensionobject = cache.Get(key + _extensionname);

                if (cachedObject == null || extensionobject == null)
                {
                    lock (_cacheLocker)
                    {
                        cachedObject = (T)cache.Get(key);
                        extensionobject = cache.Get(key + _extensionname);
                        if (cachedObject == null || extensionobject == null)
                        {
                            cachedObject = onCreateInstance();
                            if (cachedObject != null)
                            {
                                cache.Add(key, cachedObject, DateTime.Now.AddSeconds(timeOut));
                            }
                            if (extensionobject != null)
                            {
                                cache.Add(key + _extensionname, extensionobject, DateTime.Now.AddSeconds(timeOut));
                            }
                        }
                    }

                }
                return cachedObject;
            }
            return onCreateInstance();
        }

        /// <summary>
        /// T GetCachedObject<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="onCreateInstance">The on create instance.</param>
        /// <returns></returns>
        public static T GetCachedObject<T>(string key, Func<T> onCreateInstance)
        {
            return GetCachedObject(key, CommonCacheTimeOut, onCreateInstance);
        }



        /// <summary>
        /// Remove Cache
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static object Remove(string key)
        {
            var cache = MemoryCache.Default;

            lock (_cacheLocker)
            {
                return cache.Remove(key);
            }
        }

        /// <summary>
        /// RemoveAll Cache
        /// </summary>
        public static void RemoveAll()
        {
            var cache = MemoryCache.Default;
            foreach (var key in cache.Cast<DictionaryEntry>().Select(de => de.Key).OfType<string>())
            {
                cache.Remove(key);
            }
        }
    }

    /// <summary>
    /// DataListCacheItem
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataListCacheItem<T>
    {
        public T[] DataSource { get; set; }
        public int RecordCount { get; set; }
    }
}
