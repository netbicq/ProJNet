using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.DB
{
    /// <summary>
    /// 角色权限作用域
    /// </summary>
    public class Auth_RoleAuthScope : ModelBase
    {
        /// <summary>
        /// 权限KEY
        /// </summary>
        public string AuthKey { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleID { get; set; }
    }
}
