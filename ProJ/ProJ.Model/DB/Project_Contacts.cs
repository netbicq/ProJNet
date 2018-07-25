using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.DB
{
    /// <summary>
    /// 项目联系人
    /// </summary>
    public class Project_Contacts:ModelBase
    {
        public Guid ProjectID { get; set; }
        /// <summary>
        /// 片区负责人
        /// </summary>
        public string SitePrincipal { get; set; }
        /// <summary>
        /// 片区负责人电话
        /// </summary>
        public string SitePrincipalTEL { get; set; }
        /// <summary>
        /// 片区联系人
        /// </summary>
        public string SiteLink { get; set; }
        /// <summary>
        /// 片区联系人电话
        /// </summary>
        public string SiteLinkTEL { get; set; }
        /// <summary>
        /// 经办人
        /// </summary>
        public string Handler { get; set; }
        /// <summary>
        /// 经办人电话
        /// </summary>
        public string HandlerTEL { get; set; }
        /// <summary>
        /// 分管领导
        /// </summary>
        public string Principal { get; set; }
        /// <summary>
        /// 分管领导电话
        /// </summary>
        public string PrincipalTEL { get; set; }
        /// <summary>
        /// 主要领导
        /// </summary>
        public string Leader { get; set; }
        /// <summary>
        /// 主要领导电话
        /// </summary>
        public string LeaderTEL { get; set; }
    }
}
