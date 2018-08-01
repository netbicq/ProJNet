using ProJ.API.Public;
using ProJ.Model;
using ProJ.Model.Para;
using ProJ.Model.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProJ.API.Controllers
{
    /// <summary>
    /// 陆登建权
    /// </summary>
    [RoutePrefix("api/auth")]
    public class AuthController : ProJAPI
    {

        private IBll.IAuth bll = null;

        public AuthController(IBll.IAuth user)
        {
            bll = user as Bll.AuthService;
            BusinessService = bll;
        }

        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("yamm/{id:Guid}")]
        public ActionResult<bool> yami(Guid ID)
        {
            return bll.Ys(ID);
        }
        /// <summary>
        /// 新建 用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("newuser")]
        public ActionResult<bool> Add(UserNew user)
        {
            return bll.Add(user);
        }

        /// <summary>
        /// 新建角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("addrole")]
        public ActionResult<bool> AddRole(RoleNew role)
        {
            return bll.AddRole(role);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [Route("changepwd")]
        [HttpPost]
        public ActionResult<bool> ChangePwd(UserPwdChange para)
        {
            return bll.ChangePwd(para);
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("deluser/{id:Guid}")]
        [HttpGet]
        public ActionResult<bool> Delete(Guid id)
        {
            return bll.Delete(id);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("delrole/{roleid:Guid}")]
        public ActionResult<bool> DelRole(Guid roleid)
        {
            return bll.DelRole(roleid);
        }
        /// <summary>
        /// 获取用户菜单
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getmenu/{login}")]
        public ActionResult<IEnumerable<AuthModuleMenu>> GetLoginMenu(string login)
        {
            return bll.GetLoginMenu(login);
        }
        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getrole/{login}")]
        public ActionResult<UserRole> GetLoginRoles(string login)
        {
            return bll.GetLoginRoles(login);
        }
        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getauth/{roleid:Guid}")]
        public ActionResult<IEnumerable<AuthModule>> GetRoleAuth(Guid roleid)
        {
            return bll.GetRoleAuth(roleid);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [Route("userlist")]
        [HttpPost]
        public ActionResult<Pager<UserView>> GetUserList(PagerQuery<UserQuery> para)
        {
            return bll.GetUserList(para);
        }
        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("setrole")]
        public ActionResult<bool> LoginSetRole(LoginSetRole para)
        {
            return bll.LoginSetRole(para);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [Route("resetpwd")]
        [HttpPost]
        public ActionResult<bool> ReSetPwd(UserReSetPwd para)
        {
            return bll.ReSetPwd(para);
        }

        /// <summary>
        /// 设置用户Profile
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [Route("setprofile")]
        [HttpPost]
        public ActionResult<bool> SetProfile(UserSetProfile para)
        {
            return bll.SetProfile(para);
        }
        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("setauth")]
        public ActionResult<bool> SetRoleAuth(RoleSet para)
        {
            return bll.SetRoleAuth(para);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [Route("edituser")]
        [HttpPost]
        public ActionResult<bool> Update(UserEdit para)
        {
            return bll.Update(para);
        }
        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [Route("signin")]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult<UserView> UserSignin(UserSignin para)
        {
            return bll.UserSignin(para);
        }
        /// <summary>
        /// 用户注销
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [Route("exituser")]
        [HttpPost]
        public ActionResult<bool> exit()
        {
            return bll.exit();
        }
        /// <summary>
        /// 第一次进入修改密码
        /// </summary>
        /// <returns></returns>
        [Route("checkuser")]
        [HttpPost]
        public ActionResult<bool> check()
        {
            return bll.check();
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("reguser")]
        [AllowAnonymous]
        public ActionResult<bool> Regter(UserReg user)
        {
            return bll.Regter(user);
        }
    }
}
