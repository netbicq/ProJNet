using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model
{
    /// <summary>
    /// 服务基类
    /// </summary>
    public class ServiceBase
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        public DB.Auth_User AppUser { get; set; }

    }
}
