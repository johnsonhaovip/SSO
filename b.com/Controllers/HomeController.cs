using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Utils;

namespace b.com.Controllers
{
    public class HomeController : Controller
    {
        //需要登录的页面-TODO
        public ActionResult Index()
        {
            HttpCookie cookie = new HttpCookie("currentUser");
            cookie.HttpOnly = true;
            cookie.Expires = DateTime.Now.AddYears(100);//永不过期
            ViewBag.token = Request.Cookies["currentUser"].Value;
            cookie.Value = ViewBag.token;
            Response.Cookies.Add(cookie);
            //验证是否登录--每个需要登录验证的地方都应该调用
            string v = HttpHelper.OpenReadWithHttps("http://localhost:2076/login/validateLogin", "token=" + ViewBag.token);
            ViewBag.v = v;
            return View();
        }


        /// <summary>
        /// 用跳转的方式写cookie
        /// </summary>
        /// <param name="token">授权验证的令牌</param>
        /// <param name="others">一系列分站</param>
        /// <param name="main">登录成功返回的站点</param>
        /// <returns></returns>
        public ActionResult Jump(string token, string others, string main)
        {
            HttpCookie cookie = new HttpCookie("currentUser");
            cookie.HttpOnly = true;
            cookie.Expires = DateTime.Now.AddYears(100);//永不过期
            cookie.Value = token;
            Response.Cookies.Add(cookie);

            //依次减掉已经写过cookie的分站
            if (!string.IsNullOrEmpty(others))
            {
                //获取分站集合
                var substationList = others.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (substationList.Count == 1)
                {
                    return Redirect(others + "&main=" + main);
                }
                else
                {
                    string currentRedirect = substationList[0];
                    substationList.RemoveAt(0);
                    string otherss = string.Join(",", substationList);
                    return Redirect(currentRedirect + "&others=" + otherss + "&main=" + main);
                }

            }
            else
            {
                return Redirect(main);//跳转到登录来源页面
            }

        }

    }
}