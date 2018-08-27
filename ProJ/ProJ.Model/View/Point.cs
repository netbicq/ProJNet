using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.View
{
    public class PointInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        public string PointName { get; set; }
        /// <summary>
        /// 节点排序值 
        /// </summary>
        public int OrderIndex { get; set; }
    }

    public class ProjectPoint
    {
        /// <summary>
        /// 计划id
        /// </summary>
        public Guid? ID { get; set; }
        /// <summary>
        /// 执行id
        /// </summary>
        public Guid? UID { get; set; }
        /// <summary>
        /// 执行是否可修改
        /// </summary>
        public bool Check { get; set; }

        public Guid PointID { get; set; }

        public string PointName { get; set; }

        public int OrderIndex { get; set; }

        public DateTime? Schedule { get; set; }

        public DateTime? Exec { get; set; }

        public string ExecMemo { get; set; }
    }
}
