using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.DB
{
    /// <summary>
    /// 用户角色表
    /// </summary>
    public class Auth_UserRole : ModelBase
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Login { get; set; }
    }
}
