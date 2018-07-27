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
        /// 状态
        /// </summary>
        public string StateStr { get; set; }
    }
}
