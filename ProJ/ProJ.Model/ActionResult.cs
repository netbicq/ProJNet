using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model
{
    /// <summary>
    /// 操作返回类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ActionResult<T>
    {

        public ActionResult()
        {

        }

        public ActionResult(Exception err)
        {
            string errstr = err.Message;
            while(err.InnerException!=null)
            {                
                err = err.InnerException;
                errstr += err.Message;
            }
            data = default(T);
            state = 1000;
            errmsg = errstr;
        } 

        public ActionResult(T sdata)
        {
            data = sdata;
            errmsg = "";
            state = 200;
        }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string errmsg { get; set; }
        /// <summary>
        /// 返回状态，正确为 200，错误返回错误码，或者 1000
        /// </summary>
        public int state { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public T data { get; set; }
    }
}
