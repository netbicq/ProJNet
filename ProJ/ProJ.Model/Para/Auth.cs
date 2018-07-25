using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.Para
{
    /// <summary>
    /// 新建用户参数模型
    /// </summary>
    public class UserNew
    {
        /// <summary>
        /// Login
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }
        /// <summary>
        /// 查看他们数据
        /// </summary>
        public bool OtherView { get; set; }
        /// <summary>
        /// 修改他人数据
        /// </summary>
        public bool OtherEdit { get; set; }

    }
    /// <summary>
    /// 修改用户
    /// </summary>
    public class UserEdit
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 查看他人数据
        /// </summary>
        public bool OtherView { get; set; }
        /// <summary>
        /// 修改他人数据
        /// </summary>
        public bool OtherEdit { get; set; }
       
    }
    /// <summary>
    /// 用户设置Profile
    /// </summary>
    public class UserSetProfile
    {
        /// <summary>
        /// Login
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string CNName { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadIMG { get; set; }
    }

    
}
