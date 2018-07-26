using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.Para
{
    /// <summary>
    /// 新建词典
    /// </summary>
    public class DictNew
    {

        /// <summary>
        /// 词典类型
        /// </summary>
        public PublicEnum.DictType DictType { get; set; }
        /// <summary>
        /// 词典名称
        /// </summary>
        public string DictName { get; set; }
        /// <summary>
        /// 词典值
        /// </summary>
        public string DictValue { get; set; }

    }
    /// <summary>
    /// 修改词典信息
    /// </summary>
    public class EidtDict : DictNew
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid ID { get; set; }
    }

    /// <summary>
    /// 词典查询
    /// </summary>
    public class DictQuery
    {
        /// <summary>
        /// 词典类型
        /// </summary>
        public PublicEnum.DictType DictType { get; set; }
    }
}
