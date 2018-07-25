using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.DB
{
    /// <summary>
    /// 工程项目日志
    /// </summary>
    public class Project_Log:ModelBaseEx
    {
        public Guid ProjectID { get; set; }
        public string LogContent { get; set; }
    }
}
