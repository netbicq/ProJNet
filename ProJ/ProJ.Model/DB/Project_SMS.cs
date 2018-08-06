using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.DB
{
    /// <summary>
    /// 工程项目超期短信发送日志
    /// </summary>
    public class Project_SMS:ModelBase
    {
        ///// <summary>
        ///// ID
        ///// </summary>
        //public Guid ID { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        public string PointName { get; set; }
        /// <summary>
        /// 超期周数
        /// </summary>
        public int WeekInt { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendDate { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public Guid ProjectID { get; set; }
    }
}
