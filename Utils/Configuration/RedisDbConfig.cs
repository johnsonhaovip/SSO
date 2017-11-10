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
    public class RedisDbConfig
    {
        #region Properties

        /// <summary>
        /// db host
        /// </summary>
        private static string host = "127.0.0.1";
        [XmlElement("host")]
        public static string Host
        {
            get { return host; }
            set { host = value; }
        }

        /// <summary>
        /// 端口(默认6379)
        /// </summary>
        private static int port = 6379;
        [XmlElement("port")]
        public static int Port
        {
            get { return port; }
            set { port = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("password")]
        public static string Password { get; set; }

        /// <summary>
        /// db编号(默认0, max:16384)
        /// </summary>
        private static int db = 0;
        [XmlElement("db")]
        public int Db
        {
            get { return db; }
            set { db = value; }
        }

        #endregion
    }
}
