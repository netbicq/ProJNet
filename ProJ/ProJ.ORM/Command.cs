using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.ORM
{
    public class Command
    {
        /// <summary>
        /// 创建登陆Token
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CreateToken(int length)
        {
            //定义  
            string basestr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            string sb = "";
            for (int i = 0; i < length; i++)
            {
                int number = random.Next(basestr.Length);
                sb += basestr.Substring(number, 1);
            }
            return sb;
        }
    }
}
