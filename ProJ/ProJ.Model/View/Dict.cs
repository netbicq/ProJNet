using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.View
{
    /// <summary>
    /// 词典视图
    /// </summary>
    public class DictView
    {
        /// <summary>
        /// 词典信息
        /// </summary>
        public Model.DB.Basic_Dict DictInfo { get; set; }
        /// <summary>
        /// 词典状态
        /// </summary>
        public string StateStr { get; set; }
    }
}
