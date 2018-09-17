using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.View
{
    /// <summary>
    /// 用户列表
    /// </summary>
    public class UserView
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public Model.DB.Auth_User UserInfo { get; set; }
        /// <summary>
        /// 用户Profile
        /// </summary>
        public Model.DB.Auth_UserProfile UserProfile { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string StateStr { get; set; }
        /// <summary>
        /// 申请通过
        /// </summary>
        public bool Check { get; set; }
        /// <summary>
        /// 开工
        /// </summary>
        public bool Start { get; set; }
        /// <summary>
        /// 群发短信
        /// </summary>
        public bool Messagew { get; set; }
    }

    /// <summary>
    /// 用户角色
    /// </summary>
    public class UserRole
    {
        /// <summary>
        /// 操作员
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// 角色集合
        /// </summary>
        public IEnumerable<RoleView> Roles { get; set; }


    }
    /// <summary>
    /// 角色视图
    /// </summary>
    public class RoleView
    {
        /// <summary>
        /// 角色
        /// </summary>
        public Model.DB.Auth_Role Role { get; set; }

        /// <summary>
        /// checked
        /// </summary>
        public bool Checked { get; set; }

    }


    /// <summary>
    /// 模块
    /// </summary>
    public class AuthModule
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// 菜单列表
        /// </summary>
        public IEnumerable<AuthKeyMenu> AuthMenus { get; set; }
    }
    /// <summary>
    /// 菜单
    /// </summary>
    public class AuthModuleMenu
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// 一菜单菜单信息
        /// </summary>
        public Model.DB.Auth_Key ModulInfo { get; set; }
        /// <summary>
        /// 菜单
        /// </summary>
        public IEnumerable<Model.DB.Auth_Key> Menu { get; set; }
        /// <summary>
        /// 三级菜单
        /// </summary>
        public IEnumerable<Model.DB.Auth_Key> Menu33 { get; set; }
    }

    /// <summary>
    /// 菜单
    /// </summary>
    public class AuthKeyMenu
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 功能列表
        /// </summary>
        public IEnumerable<AuthKeyFunc> AuthFuncS { get; set; }
    }
    /// <summary>
    /// 功能菜单项
    /// </summary>
    public class AuthKeyFunc
    {
        /// <summary>
        /// 功能菜单
        /// </summary>
        public Model.DB.Auth_Key AuthFunc { get; set; }
        /// <summary>
        /// checked
        /// </summary>
        public bool Checked { get; set; }
    }

}
