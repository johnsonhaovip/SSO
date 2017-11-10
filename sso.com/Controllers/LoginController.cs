using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Utils.Configuration;
using Utils.Redis;

namespace sso.com.Controllers
{
    public class LoginController : Controller
    {
        #region Fields
        private readonly RedisClient _redisClient;
        #endregion

        #region Ctor
        /// <summary>
        /// 构造函数
        /// </summary>
        public LoginController()
        {
            var redisDbConfig = RedisDbConfig.Instance;
            this._redisClient = new RedisClient(redisDbConfig.Host, redisDbConfig.Port, redisDbConfig.Password, redisDbConfig.Db);
        }
        #endregion
        // 登录
        public ActionResult Index(string redirect_url, string client_id = null)
        {
            //验证redirect_url是否可靠 TODO
            //验证client_id是否可信  TODO
            ViewBag.redirect_url = redirect_url;
            return View();
        }
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <param name="redirect_url"></param>
        /// <param name="client_id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Post(string name, string pwd, string redirect_url, string client_id = null)
        {
            //限制暴力登录-设置IP登录频率
            //验证登录名密码是否正确 TODO
            //生成token--应该以更复杂的形式生成
            string ip = Utils.ClientHelper.GetClientIP();
            string token = name + "_" + Guid.NewGuid().ToString().Substring(4, 5) + ip + DateTime.Now.Millisecond;
            token = EncryptMD5(token);
            //将用户登录信息保存在cache中，有效时间一分钟
            Utils.CacheHelper.Insert(token, name, 1);

            //需要支持单点登录的分站--统统需要写cookie 
            //TODO :可以放在配置文件中
            string acom = "http://localhost:2260/home/jump?token=" + token;
            string bcom = "http://localhost:2177/home/jump?token=" + token;
            string substation = acom + "," + bcom;//用逗号分隔，依次序写cookie

            return Redirect(acom + "&others=" + substation + "&main=" + redirect_url);

        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="sSource">需加密的字符串</param>
        /// <returns></returns>
        public string EncryptMD5(string sSource)
        {
            try
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(sSource);
                MD5CryptoServiceProvider md5csp = new MD5CryptoServiceProvider();
                byte[] resultData = md5csp.ComputeHash(inputByteArray);

                // Create a new Stringbuilder to collect the bytes and create a string.
                StringBuilder sBuilder = new StringBuilder();
                // Loop through each byte of the hashed data and format each one as a hexadecimal string.
                for (int i = 0; i < resultData.Length; i++)
                    sBuilder.Append(resultData[i].ToString("x2"));
                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
            catch
            {
                return null;
            }
        }

        //验证是否登录--每个需要登录的页面都要调用
        [HttpPost]
        public string validateLogin(string token)
        {
            //验证IP是否可靠

            var v = Utils.CacheHelper.Get(token);
            if (v == null)
            {
                return "error";
            }
            //更新缓存过期时间
            Utils.CacheHelper.Remove(token);
            Utils.CacheHelper.Insert(token, v, 1);
            //this._redisClient.Add(token, v, TimeSpan.FromMinutes(5));//将信息插入到redis中（TODO:只是测试，因为redis服务器并未有开启）
            //TODO:将用户相关的信息返回
            return v.ToString();
        }

        public ActionResult LoginOut(string redirect_url, string token)
        {
            Utils.CacheHelper.Remove(token);
            return Redirect(redirect_url);
        }
    }
}