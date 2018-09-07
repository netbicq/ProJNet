using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.DB
{
    /// <summary>
    /// 协调工作
    /// </summary>
    public class Proiect_Teamwork: ModelBase
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 人员
        /// </summary>
        public string Personnel { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Contents { get; set; }
    }
}
