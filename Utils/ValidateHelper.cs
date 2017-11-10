using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utils
{
    /// <summary>
    /// 验证帮助
    /// </summary>
    public static class ValidateHelper
    {
        /// <summary>
        /// 验证电子邮箱格式
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmail(string value)
        {
            return QuickValidate(@"^([a-zA-Z0-9]+[_|\-|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\-|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$", value);
            //return QuickValidate(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+))([a-zA-Z]{2,4}|[0-9]{1,3})(\)?]$", value);
        }

        /// <summary>
        /// 验证中国手机号码格式
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPhoneNumber(string value)
        {
            //return QuickValidate("^(13|14|15|18)[0-9]{9}$", value); 
            //TODO：2017-11-10修改为最新手机正则表达式
            return QuickValidate(@"^1(3[0-9]|4[57]|5[0-35-9]|7[01678]|8[0-9])\\d{8}$", value);
        }

        /// <summary>
        /// 检查一个字符串是否是纯数字构成的，一般用于查询字符串参数的有效性验证。(0除外)
        /// </summary>
        /// <param name="value">需验证的字符串。。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool IsNumber(string value)
        {
            return QuickValidate("^[1-9]*[0-9]*$", value);
        }

        /// <summary>
        /// 判断对象是否为空，为空返回true
        /// </summary>
        /// <typeparam name="T">要验证的对象的类型</typeparam>
        /// <param name="value">要验证的对象</param>
        public static bool IsNullOrEmpty<T>(T value)
        {
            //如果为null
            if (value == null)
            {
                return true;
            }

            //如果为""
            if (value.GetType() == typeof(String))
            {
                if (string.IsNullOrEmpty(value.ToString().Trim()))
                {
                    return true;
                }
            }

            //如果为DBNull
            if (value.GetType() == typeof(DBNull))
            {
                return true;
            }

            //不为空
            return false;
        }

        /// <summary>
        /// 判断对象是否为空，为空返回true
        /// </summary>
        /// <param name="value">要验证的对象</param>
        public static bool IsNullOrEmpty(object value)
        {
            //如果为null
            if (value == null)
            {
                return true;
            }

            //如果为""
            if (value.GetType() == typeof(String))
            {
                if (string.IsNullOrEmpty(value.ToString().Trim()))
                {
                    return true;
                }
            }

            //如果为DBNull
            if (value.GetType() == typeof(DBNull))
            {
                return true;
            }

            //不为空
            return false;
        }

        /// <summary>
        /// 校验ip地址
        /// </summary>
        /// <param name="ipstr">IP</param>
        public static bool IsIPAddress(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 7 || value.Length > 15) return false;

            return QuickValidate(@"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$", value);
        }

        /// <summary>
        /// 判断是否为中文汉字
        /// </summary>
        /// <param name="value">要验证的对象</param>
        public static bool IsChinese(string value)
        {
            return QuickValidate(@"^[\u4e00-\u9fa5]+$", value);
        }

        /// <summary>
        /// 快速验证一个字符串是否符合指定的正则表达式。
        /// </summary>
        /// <param name="express">正则表达式的内容。</param>
        /// <param name="value">需验证的字符串。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool QuickValidate(string express, string value)
        {
            if (value == null) return false;
            Regex myRegex = new Regex(express);
            if (value.Length == 0)
            {
                return false;
            }
            return myRegex.IsMatch(value);
        }
    }
}
