using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProJ.Model;

namespace ProJ.IBll
{
    public interface IAuth
    {

        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

        ActionResult<bool> Ys(Guid id);
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

        ActionResult<bool> Regter(Model.Para.UserReg user);
        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

        ActionResult<bool> Add(Model.Para.UserNew user);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ActionResult<bool> Delete(Guid id);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        ActionResult<bool> Update(Model.Para.UserEdit para);
        /// <summary>
        /// 设置用户Profile
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        ActionResult<bool> SetProfile(Model.Para.UserSetProfile para);
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        ActionResult<bool> ReSetPwd(Model.Para.UserReSetPwd para);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        ActionResult<bool> ChangePwd(Model.Para.UserPwdChange para);
        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>

        ActionResult<Model.View.UserView> UserSignin(Model.Para.UserSignin para);
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        ActionResult<Pager<Model.View.UserView>> GetUserList(PagerQuery<Model.Para.UserQuery> para);

        /// <summary>
        /// 新建角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        ActionResult<bool> AddRole(Model.Para.RoleNew role);
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        ActionResult<bool> DelRole(Guid roleid);
        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        ActionResult<bool> SetRoleAuth(Model.Para.RoleSet para);

        /// <summary>
        /// 获取操作员角色列表
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        ActionResult<Model.View.UserRole> GetLoginRoles(string login);

        /// <summary>
        /// 操作员设置角色
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        ActionResult<bool> LoginSetRole(Model.Para.LoginSetRole para);

        /// <summary>
        /// 获取角色权限列表
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        ActionResult<IEnumerable<Model.View.AuthModule>> GetRoleAuth(Guid roleid);

        /// <summary>
        /// 获取操作员的菜单
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        ActionResult<IEnumerable<Model.View.AuthModuleMenu>> GetLoginMenu(string login);
        /// <summary>
        /// 用户注销
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        ActionResult<bool> exit();
        /// <summary>
        /// 第一次修改密码
        /// </summary>
        /// <returns></returns>
        ActionResult<bool> check();

    }
}
