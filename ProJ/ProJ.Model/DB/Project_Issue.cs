using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.DB
{
    /// <summary>
    /// 项目存在的问题
    /// </summary>
    public class Project_Issue:ModelBaseEx
    {
        public Guid ProjectID { get; set; }
        public string IssueContent { get; set; }
    }
}
