using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Utils
{
    public class ClientHelper
    {
        /// <summary>
        /// 跳过代理获取客户端IP
        /// </summary>
        /// <returns>客户端IP</returns>
        public static string GetClientIP()
        {
            return GetClientIP(false);
        }
        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <param name="skipproxy">是否跳过代理</param>
        /// <returns>客户端IP</returns>
        /// 创建者：蒋浩
        /// 创建时间：2017-11-10
        public static string GetClientIP(bool skipproxy)
        {
            string result = null;
            if (skipproxy)
            {//忽略代理,获取客户端IP或最后一个代理IP
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                #region 有代理,获取客户端IP,有伪造可能
                foreach (var header in new[] { "HTTP_CLIENT_IP", "HTTP_X_FORWARDED_FOR", "HTTP_FROM", "REMOTE_ADDR" })
                {
                    string temp = HttpContext.Current.Request.ServerVariables[header];
                    if (!string.IsNullOrEmpty(temp) && ValidateHelper.IsIPAddress(temp))
                    {
                        result = temp;
                        break;
                    }
                }
                if (string.IsNullOrEmpty(result))
                {
                    result = System.Web.HttpContext.Current.Request.UserHostAddress;
                }
                if (string.IsNullOrEmpty(result))
                {
                    #region get local(LAN) Connected ip address
                    //获取本地的ip地址名
                    string stringHostName = Dns.GetHostName();
                    //根据IP地址名获取主机地址
                    IPHostEntry ipHostEntries = Dns.GetHostEntry(stringHostName);
                    //从主机关联的Ip列表获取ip地址
                    IPAddress[] arrIpAddress = ipHostEntries.AddressList;

                    if (arrIpAddress.Length >= 2)
                    {
                        result = arrIpAddress[arrIpAddress.Length - 2].ToString();
                    }
                    else if (arrIpAddress.Length > 0)
                    {
                        result = arrIpAddress[0].ToString();
                    }
                    else
                    {
                        arrIpAddress = Dns.GetHostAddresses(stringHostName);
                        if (arrIpAddress.Length >= 0)
                        {
                            result = arrIpAddress[0].ToString();
                        }
                        else
                        {
                            result = "127.0.0.1";
                        }
                    }
                    #endregion
                }
                //多个代理情况处理,一般第一个会是真实iP
                if (!string.IsNullOrEmpty(result))
                {
                    result = result.Split(',')[0].Trim();
                }
                #endregion
            }

            if (result == "::1")
            {
                return "127.0.0.1";
            }
            else if (ValidateHelper.IsIPAddress(result))
            {
                return result;
            }
            else
            {
                return "invalid ip";
            }
        }
    }
}
