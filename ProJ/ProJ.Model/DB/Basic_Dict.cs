using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.DB
{
    /// <summary>
    /// 词典
    /// </summary>
   public class Basic_Dict:ModelBaseEx
    {
        /// <summary>
        /// 词典类型
        /// </summary>
        public int DictType { get; set; }
        /// <summary>
        /// 词典名称
        /// </summary>
        public string DictName { get; set; }
        /// <summary>
        /// 词典值
        /// </summary>
        public string DictValue { get; set; }
    }
}
