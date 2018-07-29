using ProJ.Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.View
{
    public class ProjectView
    {
        public Project_Info Project_Info { get; set; }
        /// <summary>
        /// 问题
        /// </summary>
        public IEnumerable<Project_Issue> Project_Issue { get; set; }
        public IEnumerable<Project_Log> Project_Log { get; set; }
        /// <summary>
        /// 计划
        /// </summary>
        public IEnumerable<Project_Schedule> Project_Schedule { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public Project_Contacts Project_Contacts { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string StateStr { get; set; }
        /// <summary>
        /// 项目级别
        /// </summary>
        public string ProJLeveStr { get; set; }
        /// <summary>
        /// 项目行业
        /// </summary>
        public string ProjStr { get; set; }
        /// <summary>
        /// 业主单位
        /// </summary>
        public string OwnerStr { get; set; }
    }
}
