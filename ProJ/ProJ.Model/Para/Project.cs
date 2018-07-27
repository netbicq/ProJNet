using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.Para
{

    public class ProjectAdd
    {
        /// <summary>
        /// 项目信息
        /// </summary>
        public ProjectNew ProjectInfo { get; set; }
        /// <summary>
        /// 项目计划
        /// </summary>
        public ProjectScheduleNew Schedules { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public ProjectContactNew Contacts { get; set; }
    }
    public class ProjectNew
    {


    }

    public class ProjectScheduleNew
    {

    }

    public class ProjectContactNew
    {

    }
}
