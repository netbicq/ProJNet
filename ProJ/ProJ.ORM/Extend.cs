using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProJ.ORM
{
    /// <summary>
    /// 系统扩展
    /// </summary>
    public static class Extend
    {
        /// <summary>
        /// 相同属性名称的值进行直接赋值
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Clone(this object source, object target)
        {

            Type st = source.GetType();
            Type tt = target.GetType();

            var sps = st.GetProperties();
            var tps = tt.GetProperties();

            foreach (var sp in sps)
            {
                foreach (var tp in tps)
                {
                    if (sp.Name == tp.Name)
                    {
                        tp.SetValue(target, sp.GetValue(source));
                        break;
                    }
                }
            }

        }
        /// <summary>
        /// 源对象转换为指定类型的新对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(this object source) where T : new()
        {
            Type tt = typeof(T);
            T re = Activator.CreateInstance<T>();
            Type rt = source.GetType();

            foreach (var rp in rt.GetProperties())
            {
                foreach (var tp in tt.GetProperties())
                {
                    if (rp.Name == tp.Name)
                    {
                        rp.SetValue(re, tp.GetValue(source));
                        break;
                    }
                }
            }
            return re;
        }

        /// <summary>
        /// 将Unicode字符转为String
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToUnicode(this string source)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(source);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i += 2)
            {
                stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 将String转为Unicode字符
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Unicode(this string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
               source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

    }
}
