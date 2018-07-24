using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace ProJ.Model.DB
{
    /// <summary>
    /// 用户
    /// </summary>
    public class Auth_User:ModelBaseEx
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }
        /// <summary>
        /// token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 业主单位
        /// </summary>
        public Guid OwnerID { get; set; }
        /// <summary>
        /// 查看他人数据
        /// </summary>
        public bool OtherView { get; set; }
        /// <summary>
        /// 编辑他人数据
        /// </summary>
        public bool OtherEdit { get; set; } 
        /// <summary>
        /// token过期时间
        /// </summary>
        public DateTime TokenValidTime { get; set; }
    }
}
