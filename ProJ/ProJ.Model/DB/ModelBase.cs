using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.DB
{
    /// <summary>
    /// 模型基类
    /// </summary>
    public class ModelBase
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid ID { get; set; }
    }
    /// <summary>
    /// 模型扩展基类
    /// </summary>
    public class ModelBaseEx:ModelBase
    {
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateMan { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }
    }
}
