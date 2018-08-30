using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.Para
{
    /// <summary>
    /// 新建标准
    /// </summary>
    public class BapNew
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string PointName { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        public string ColName { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public int PointOrderIndex { get; set; }

    }
    /// <summary>
    /// 修改信息
    /// </summary>
    public class EidtBap : BapNew
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid ID { get; set; }
    }

    /// <summary>
    /// 查询
    /// </summary>
    public class BapQuery
    {
        /// <summary>
        /// 名臣
        /// </summary>
        public string Name { get; set; }
    }
    /// <summary>
    /// 列表
    /// </summary>
    public class BapView
    {
        /// <summary>
        /// 标准信息
        /// </summary>
        public Model.DB.Basic_Point Basic_Point { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string StateStr { get; set; }
    }
}
