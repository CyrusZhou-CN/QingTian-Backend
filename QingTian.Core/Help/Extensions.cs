using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QingTian.Core
{
    public static class Extensions
    {
        public static dynamic GetValueForNetType(this object value, string NetType = "string")
        {
            switch (NetType)
            {
                case "string": return Convert.ToString(value);
                case "int":
                case "Int32": return Convert.ToInt32(value);
                case "long":
                case "Int64": return Convert.ToInt64(value);
                case "bool": return Convert.ToBoolean(value);
                case "decimal": return Convert.ToDecimal(value);
                case "Single": return Convert.ToSingle(value);
                case "DateTime": return Convert.ToDateTime(value);
                case "double":  return Convert.ToDouble(value);
                case "byte[]": return (byte[])value;
                case "Guid": return Guid.Parse(value.ToString());
                default:
                    break;
            }
            return value;
        }
        /// <summary>
        /// 驼峰转下划线
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToUnderscore(this string value)
        {
            string str = "";
            for (int j = 0; j < value.Length; j++)
            {
                string temp = value[j].ToString();
                if (Regex.IsMatch(temp, "[A-Z]"))
                {
                    if (j == 0)
                    {
                        temp = temp.ToLower();
                    }
                    else
                    {
                        temp = "_" + temp.ToLower();
                    }
                }
                str += temp;
            }
            return str;
        }
        /// <summary>
        /// 转驼峰
        /// </summary>
        /// <param name="value"></param>
        /// <param name="FirstCharUpper">首字母大写</param>
        /// <returns></returns>
        public static string ToHump(this string value, bool FirstCharUpper = true)
        {
            string[] strs = value.Split('_');
            string str = strs[0];
            for (int j = 1; j < strs.Length; j++)
            {
                string temp = strs[j].ToString();
                string temp1 = temp[0].ToString().ToUpper();
                string temp2 = "";
                temp2 = temp1 + temp.Remove(0, 1);
                str += temp2;
            }
            if (FirstCharUpper)
            {
                return str.FirstCharToUpper();
            }
            return str;
        }
        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FirstCharToLower(this string value)
        {
            if (String.IsNullOrEmpty(value))
                return value;
            string str = value.Substring(0, 1).ToLower() + value[1..];
            return str;
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FirstCharToUpper(this string value)
        {
            if (String.IsNullOrEmpty(value))
                return value;
            string str = value.Substring(0, 1).ToUpper() + value[1..];
            return str;
        }
        /// <summary>
        /// 检查 Object 是否为 NULL
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmpty(this object value)
        {
            return value == null || string.IsNullOrEmpty(value.ParseToString());
        }
        /// <summary>
        /// 将object转换为string，若转换失败，则返回""。不抛出异常。  
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ParseToString(this object obj)
        {
            try
            {
                return obj == null ? string.Empty : obj.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 检查 Object 是否为 NULL 或者 0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrZero(this object value)
        {
            return value == null || value.ParseToString().Trim() == "0";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBool(this object value)
        {
            return (value != null && (value.ParseToString().Trim() == "Yes" || value.ParseToString().Trim() == "1"));
        }
    }
}
