using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.DB
{
    /// <summary>
    /// 权限控制明细
    /// </summary>
    public class Auth_KeyDetail : ModelBase
    {
        /// <summary>
        /// 权限Key
        /// </summary>
        public string AuthKey { get; set; }
        /// <summary>
        /// 权限控制的具体方法全名称路径
        /// </summary>
        public string ActionFullName { get; set; } 
       
    }
}

