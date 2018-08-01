using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.Para
{
    public class ReportQuery
    {
        /// <summary>
        /// 项目级别
        /// </summary>
        public IEnumerable<Guid> ProjectLevel { get; set; }
        /// <summary>
        /// 项目行业
        /// </summary>
        public IEnumerable<Guid> ProjectIndustry { get; set; }
        /// <summary>
        /// 项目业主
        /// </summary>
        public IEnumerable<Guid> ProjectOwner { get; set; }
        /// <summary>
        /// 执行状态
        /// </summary>
        public PublicEnum.ExeceedType ExeceedType { get; set; }
        
    }
}
