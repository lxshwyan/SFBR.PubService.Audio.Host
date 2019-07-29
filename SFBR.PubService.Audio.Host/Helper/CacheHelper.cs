using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SFBR.PubService.Audio.Host.Helper
{
    public  class CacheHelper
    {
        public readonly static ObjectCache cacheVoices = MemoryCache.Default;

        private readonly static object lockobj = new object();
        public static object GetValue(string key)
        {
            lock (lockobj)
            {
                if (cacheVoices.Contains(key))
                {
                    return cacheVoices.Get(key);
                }
                return null;
            }

        }








        /// <summary>
        /// 默认缓存
        /// </summary>
        private static CacheHelper Default { get { return new CacheHelper(); } }

        /// <summary>
        /// 缓存初始化
        /// </summary>
        private MemoryCache cache = MemoryCache.Default;

        /// <summary>
        /// 锁
        /// </summary>
        private object locker = new object();

        /// <summary>
        /// 构造器
        /// </summary>
        private CacheHelper()
        {
            //CacheItemPolicy policy = new CacheItemPolicy();  //创建缓存项策略
            ////过期时间设置,以下两种只能设置一种
            //policy.AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddMinutes(5)); //设定某个时间过后将逐出缓存
            //policy.SlidingExpiration = new TimeSpan(0, 0, 10);    //设定某个时间段内未被访问将逐出缓存
            ////逐出通知,以下两种只能设置一种
            //policy.UpdateCallback = arguments => { Console.WriteLine("即将逐出缓存" + arguments.Key); };
        }

        /// <summary>
        /// 从缓存中获取对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>缓存对象</returns>
        public static object Get(string key)
        {
            return Default.GetFromCache(key);
        }

        /// <summary>
        /// 从缓存中获取对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>缓存对象</returns>
        private object GetFromCache(string key)
        {
            lock (locker)
            {
                if (cache.Contains(key))
                {
                    return cache[key];
                }
                return null;
            }
        }

        /// <summary>
        /// 设置缓存指定时间未访问过期
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">数据对象</param>
        /// <param name="expire">过期时间</param>
        public static bool Set(string key, Object value, TimeSpan expiresIn)
        {
            var policy = new CacheItemPolicy()
            {
                SlidingExpiration = expiresIn
            };
            return Default.SetToCache(key, value, policy);
        }

        /// <summary>
        /// 设置缓存绝对时间过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresIn"></param>
        /// <returns></returns>
        public static bool Set(string key, Object value, DateTimeOffset expiresIn)
        {
            var policy = new CacheItemPolicy()
            {
                AbsoluteExpiration = expiresIn
            };
            return Default.SetToCache(key, value, policy);
        }

        /// <summary>
        /// 添加到缓存
        /// </summary>
        /// <typeparam name="T">缓存对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>结果状态</returns>
        public static bool Set(string key, object value)
        {
            CacheItemPolicy policy = new CacheItemPolicy()
            {
                Priority = CacheItemPriority.NotRemovable,
            };
            return Default.SetToCache(key, value, policy);
        }

        /// <summary>
        /// 数据对象装箱缓存
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">数据对象</param>
        /// <param name="expire">过期时间</param>
        private bool SetToCache(string key, object value, CacheItemPolicy policy)
        {
            lock (locker)
            {
                cache.Set(key, value, policy);
                return true;
            }
        }

        /// <summary>
        /// 获取键的集合
        /// </summary>
        /// <returns>键的集合</returns>
        public static ICollection<string> Keys()
        {
            return Default.GetCacheKeys();
        }

        /// <summary>
        /// 获取键的集合
        /// </summary>
        /// <returns>键的集合</returns>
        private ICollection<string> GetCacheKeys()
        {
            lock (locker)
            {
                IEnumerable<KeyValuePair<string, object>> items = cache.AsEnumerable();
                return items.Select(m => m.Key).ToList();
            }
        }

        /// <summary>
        /// 判断缓存中是否有此对象
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>是否存在</returns>
        public static bool Contain(string key)
        {
            return Default.ContainKey(key);
        }

        /// <summary>
        /// 判断缓存中是否有此对象
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>是否存在</returns>
        private bool ContainKey(string key)
        {
            lock (locker)
            {
                return cache.Contains(key);
            }
        }

        /// <summary>
        /// 数据对象从缓存对象中移除
        /// </summary>
        /// <param name="key">键</param>
        public static bool Remove(string key)
        {
            return Default.RemoveFromCache(key);
        }

        /// <summary>
        /// 数据对象从缓存对象中移除
        /// </summary>
        /// <param name="key">键</param>
        private bool RemoveFromCache(string key)
        {
            lock (locker)
            {
                if (cache.Contains(key))
                {
                    cache.Remove(key);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 清除实例
        /// </summary>
        public static void Clear()
        {
            Default.ClearCache();
        }

        /// <summary>
        /// 清除实例
        /// </summary>
        private void ClearCache()
        {
            lock (locker)
            {
                cache.ToList().ForEach(m => cache.Remove(m.Key));
            }
        }

        /// <summary>
        /// 获取缓存对象集合
        /// </summary>
        /// <typeparam name="T">缓存对象类型</typeparam>
        /// <returns>缓存对象集合</returns>
        public static ICollection<T> Values<T>()
        {
            return Default.GetValues<T>();
        }

        /// <summary>
        /// 获取缓存对象集合
        /// </summary>
        /// <typeparam name="T">缓存对象类型</typeparam>
        /// <returns>缓存对象集合</returns>
        private ICollection<T> GetValues<T>()
        {
            lock (locker)
            {
                IEnumerable<KeyValuePair<string, object>> items = cache.AsEnumerable();
                return items.Select(m => (T)m.Value).ToList();
            }
        }

        /// <summary>
        /// 获取缓存尺寸
        /// </summary>
        /// <returns>缓存尺寸</returns>
        public static long Size()
        {
            return Default.GetCacheSize();
        }

        /// <summary>
        /// 获取缓存尺寸
        /// </summary>
        /// <returns>缓存尺寸</returns>
        private long GetCacheSize()
        {
            lock (locker)
            {
                return cache.GetCount();
            }
        }
    }
}
