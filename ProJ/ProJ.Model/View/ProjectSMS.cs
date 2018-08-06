using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.View
{
    public class ProjectSMS
    {
        /// <summary>
        /// 项目联系人
        /// </summary>
        public Model.DB.Project_Contacts ProjectContact { get; set; }
        /// <summary>
        /// 项目基本信息
        /// </summary>
        public Model.DB.Project_Info ProjectInfo { get; set; }
        /// <summary>
        /// 业主基本信息
        /// </summary>
        public Model.DB.Basic_Owner OwerInfo { get; set; }

        /// <summary>
        /// 超期的节点集合
        /// </summary>
        public IEnumerable<SMSBase> Timeouts { get; set; }

    }
    /// <summary>
    /// 超期短信基类
    /// </summary>
    public class SMSBase
    {
        public int WeekInt { get; set; }

        public string PointName { get; set; }

    }
}
