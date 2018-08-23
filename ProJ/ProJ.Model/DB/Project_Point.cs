using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.DB
{
    public class Project_Point:ModelBase
    {
        /// <summary>
        /// 工程ID
        /// </summary>
        public Guid ProjectID { get; set; }
        public Guid PointID { get; set; }
        /// <summary>
        /// 计划
        /// </summary>
        public DateTime? PointSchedule { get; set; }
        /// <summary>
        /// 执行
        /// </summary>
        public DateTime? PointExec { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string PointExecMemo { get; set; }
    }
}
