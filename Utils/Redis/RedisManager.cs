using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;
using Utils.Configuration;

namespace Utils.Redis
{
    /// <summary>
    /// redis管理
    /// </summary>
    public class RedisManager: IRedisManager
    {
        /// <summary>
        /// Redis配置
        /// </summary>
        private readonly RedisDbConfig _rdsCfg;
        private readonly SiteConfig _siteCfg;

        /// <summary>
        /// 连接客户端
        /// </summary>
        protected static RedisClient _redisClient;

        public RedisManager()
        {
            //this._rdsCfg = RedisDbConfig.Instance;
            //this._siteCfg = SiteConfig.Instance;

            //string redisHost = String.IsNullOrEmpty(this._siteCfg.IM.Host) ? this._rdsCfg.Host : this._siteCfg.IM.Host;
            //_redisClient = new RedisClient(redisHost, this._rdsCfg.Port, this._rdsCfg.Password, this._rdsCfg.Db);
            _redisClient = new RedisClient(RedisDbConfig.Host, RedisDbConfig.Port, RedisDbConfig.Password, 0);

            _redisClient.ConnectTimeout = 1000;
        }

            /// <summary>
            /// 根据key获取缓存
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="key"></param>
            /// <returns></returns>
        public T Get<T>(string key)
        {
            return _redisClient.Get<T>(key);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        public void Set<T>(string key, T data, int cacheTime)
        {
            if (data == null)
                return;

            _redisClient.Set<T>(key, data);
        }
        /// <summary>
        /// 缓存是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsSet(string key)
        {
            var hasKey = _redisClient.ContainsKey(key);
            return hasKey;
        }
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            _redisClient.Remove(key);
        }
        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        public void Update(string key, object data, int cacheTime)
        {
            if (IsSet(key))
                Remove(key);
            Set(key, data, cacheTime);
        }
        /// <summary>
        /// 根据模式移除缓存
        /// </summary>
        /// <param name="pattern"></param>
        public void RemoveByPattern(string pattern)
        {
            _redisClient.RemoveByPattern(pattern);
        }
        /// <summary>
        /// 清空缓存
        /// </summary>
        public void Clear()
        {
            _redisClient.RemoveAll(_redisClient.GetAllKeys());
        }

        public void Set(string key, object data, int cacheTime)
        {
            if (data == null) return;
            _redisClient.Set(key, data);
        }

        public void Set(string key, object data, int cacheTime, Guid userGuid)
        {
            if (data == null) return;
            _redisClient.Set(key, data, DateTime.Now.AddMinutes(cacheTime));
        }

        public void Remove(Guid userGuid)
        {
            throw new NotImplementedException();
        }

        public void Update(string key, object data, int cacheTime, Guid userGuid)
        {
            throw new NotImplementedException();
        }
    }
}
