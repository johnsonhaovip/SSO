using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Utils.Configuration
{
    /// <summary>
    /// Redis 配置
    /// </summary>
    [Serializable, XmlRoot("redis")]
    public class RedisDbConfig : ConfigLoader<RedisDbConfig>
    {
        #region Ctor
        public RedisDbConfig(): base(PubConstant.REDIS_CONFIG_PATH) { }
        #endregion

        #region Properties

        /// <summary>
        /// db host
        /// </summary>
        private static string host = "127.0.0.1";
        [XmlElement("host")]
        public  string Host
        {
            get { return host; }
            set { host = value; }
        }

        /// <summary>
        /// 端口(默认6379)
        /// </summary>
        private  int port = 6379;
        [XmlElement("port")]
        public  int Port
        {
            get { return port; }
            set { port = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("password")]
        public  string Password { get; set; }

        /// <summary>
        /// db编号(默认0, max:16384)
        /// </summary>
        private  int db = 0;
        [XmlElement("db")]
        public int Db
        {
            get { return db; }
            set { db = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 初始化配置文件
        /// </summary>
        /// <returns></returns>
        protected override RedisDbConfig initConfig()
        {
            var config = new RedisDbConfig()
            {
                Host = "127.0.0.1",
                Port = 6379,
                Password = "aaaa",
                Db = 0
            };
            return this.saveConifg(config);
        }

        #endregion
    }
}
