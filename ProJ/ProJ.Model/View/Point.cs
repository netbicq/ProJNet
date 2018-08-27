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

        public Guid ID { get; set; }

        public string PointName { get; set; }

        public int OrderIndex { get; set; }

        public DateTime? Schedule { get; set; }

        public DateTime? Exec { get; set; }

        public string ExecMemo { get; set; }
    }
}
