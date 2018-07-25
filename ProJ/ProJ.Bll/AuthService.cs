﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProJ.IBll;
using ProJ.Model;
using ProJ.Model.Para;
using ProJ.Model.View;
using ProJ.ORM;

namespace ProJ.Bll
{
    public class AuthService : ServiceBase, IAuth
    {
        private IUnitwork _work = null;

        private IRepository<Model.DB.Auth_User> _rpsuser = null;
        private IRepository<Model.DB.Auth_UserProfile> _rpsprofiel = null;

        public AuthService(ORM.IUnitwork work)
        {
            _work = work;
            Unitwork = work;

            _rpsprofiel = work.Repository<Model.DB.Auth_UserProfile>();
            _rpsuser = work.Repository<Model.DB.Auth_User>();

        }

        /// <summary>
        /// 新建用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

        public ActionResult<bool> Add(UserNew user)
        {
            var dbuser = new Model.DB.Auth_User()
            {
                Login = user.Login,
                OtherEdit = user.OtherEdit,
                OtherView = user.OtherView,
                Pwd = user.Pwd,
                TokenValidTime = DateTime.Now,
                State = 1,
                Token = ""
            };
            var profile = new Model.DB.Auth_UserProfile()
            {
                CNName = user.CNName,
                Login = user.Login,
                HeadIMG = "",
                Tel = user.Tel
            };

            if (string.IsNullOrEmpty(user.Pwd) || string.IsNullOrEmpty(user.Login))
            {
                throw new Exception("用户名密码均不能为空");
            }
            if (_rpsuser.Any(q => q.Login == user.Login))
            {
                throw new Exception("用户名：" + user.Login + "已经存在");
            }

            _rpsprofiel.Add(profile);
            _rpsuser.Add(dbuser);

            _work.Commit();

            return new ActionResult<bool>(true);

        }
        /// <summary>
        /// 新建角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public ActionResult<bool> AddRole(RoleNew role)
        {
            var rolerps = _work.Repository<Model.DB.Auth_Role>();

            if (rolerps.Any(q => q.RoleName == role.RoleName))
            {
                throw new Exception("角色名称:" + role.RoleName + " 已经存在");
            }
            var dbrole = new Model.DB.Auth_Role
            {
                RoleName = role.RoleName
            };

            rolerps.Add(dbrole);
            _work.Commit();

            return new ActionResult<bool>(true);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public ActionResult<bool> ChangePwd(UserPwdChange para)
        {
            var user = _rpsuser.GetModel(q => q.ID == para.ID);

            if (user == null)

            {
                throw new Exception("用户不存在");
            }

            if (string.Compare(user.Pwd, para.OldPwd, false) != 0)
            {
                throw new Exception("原密码错误");
            }
            user.Pwd = para.Pwd;

            _rpsuser.Update(user);
            _work.Commit();

            return new ActionResult<bool>(true);

        }
        /// <summary>
        /// 第一次进入需要改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult<bool> check()
        {
            var token = AppUser.UserInfo.Token;
            var user = _rpsuser.GetModel(q => q.Token == token).Pwd;
            if (user == "admin")
            {
                return new ActionResult<bool>(true);
            }
            else
            {
                return new ActionResult<bool>(false);
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>  
        public ActionResult<bool> Delete(Guid id)
        {
            var dbuser = _rpsuser.GetModel(q => q.ID == id);
            if (dbuser == null)
            {
                throw new Exception("用户不存在");
            }

            _rpsuser.Delete(dbuser);
            _rpsprofiel.Delete(q => q.Login
            == dbuser.Login);

            _work.Repository<Model.DB.Auth_UserRole>().Delete(q => q.Login == dbuser.Login);

            _work.Commit();

            return new ActionResult<bool>(true);
        }
        /// <summary>
        /// 删除指定ID的角色
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public ActionResult<bool> DelRole(Guid roleid)
        {
            var rolerps = _work.Repository<Model.DB.Auth_Role>();
            var roleauth = _work.Repository<Model.DB.Auth_RoleAuthScope>();
            var urole = _work.Repository<Model.DB.Auth_UserRole>();

            var role = rolerps.GetModel(roleid);
            if (role == null)
            {
                throw new Exception("角色不存在");
            }

            rolerps.Delete(role);
            roleauth.Delete(q => q.RoleID == roleid);
            urole.Delete(q => q.RoleID == roleid);


            _work.Commit();

            return new ActionResult<bool>(true);

        }
        /// <summary>
        /// 用户注销
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult<bool> exit()
        {
            var da = AppUser.UserInfo.ID;
            var dbuser = _rpsuser.GetModel(q => q.ID == da);
            if (dbuser == null)
            {
                throw new Exception("用户不存在");
            }
            dbuser.Token = "";
            _rpsuser.Update(dbuser);

            _work.Commit();

            return new ActionResult<bool>(true);
        }

        /// <summary>
        /// 获取指定的操作员的菜单
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public ActionResult<IEnumerable<AuthModuleMenu>> GetLoginMenu(string login)
        {
            var roles = _work.Repository<Model.DB.Auth_UserRole>().Queryable(q => q.Login == login);
            var roleids = roles.Select(s => s.RoleID);

            var authkeys = _work.Repository<Model.DB.Auth_RoleAuthScope>().Queryable(q => roleids.Contains(q.RoleID));

            var keyss = authkeys.Select(s => s.AuthKey);

            var keybase = _work.Repository<Model.DB.Auth_Key>().Queryable(q => keyss.Contains(q.AuthKey));

            var modulestrs = keybase.Select(s => s.ModuleName);
            var menustrs = keybase.Select(s => s.MenuName);

            var modulebase = _work.Repository<Model.DB.Auth_Key>().Queryable(q => modulestrs.Contains(q.ModuleName) && string.IsNullOrEmpty(q.FunctionName) && string.IsNullOrEmpty(q.MenuName));

            var menubase = _work.Repository<Model.DB.Auth_Key>().Queryable(q => modulestrs.Contains(q.ModuleName) && menustrs.Contains(q.MenuName));

            var keysall = keybase.Union(modulebase).Union(menubase);

            var re = from ak in keysall
                     group ak by ak.ModuleName into modeulg
                     //group ak by new { ak.ModuleName, Index = ak.OrderIndex } into modeulg
                     //orderby modeulg.Key.Index
                     select new AuthModuleMenu
                     {
                         ModuleName = modeulg.Key,
                         ModulInfo = keysall.FirstOrDefault(q => q.ModuleName == modeulg.Key && string.IsNullOrEmpty(q.MenuName) && string.IsNullOrEmpty(q.RoutUrl)),
                         Menu = keysall.Where(q => q.ModuleName == modeulg.Key && !string.IsNullOrEmpty(q.MenuName) && !string.IsNullOrEmpty(q.RoutUrl)).OrderBy(q => q.OrderIndex)

                     };
            var result = from r in re
                         orderby r.ModulInfo.OrderIndex
                         select r;

            return new ActionResult<IEnumerable<AuthModuleMenu>>(result);

        }

        /// <summary>
        /// 获取用户的角色
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public ActionResult<UserRole> GetLoginRoles(string login)
        {
            var templist = _work.Repository<Model.DB.Auth_UserRole>().Queryable(q => q.Login == login);
            var roles = _work.Repository<Model.DB.Auth_Role>().Queryable();
            var lg = _work.Repository<Model.DB.Auth_User>().GetModel(q => q.Login == login);
            if (lg == null)
            {
                throw new Exception("用户" + login + "不存在");
            }
            var re = new UserRole()
            {
                Login = lg.Login
            };

            var lgroles = from role in roles
                          let check = templist.Any(q => q.RoleID == role.ID)
                          select new RoleView
                          {
                              Checked = check,
                              Role = role
                          };
            re.Roles = lgroles;

            return new ActionResult<UserRole>(re);

        }
        /// <summary>
        /// 获取角色的
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public ActionResult<IEnumerable<AuthModule>> GetRoleAuth(Guid roleid)
        {
            var authlist = _work.Repository<Model.DB.Auth_Key>().Queryable();
            var alauthkey = _work.Repository<Model.DB.Auth_RoleAuthScope>().Queryable(
                    q => q.RoleID == roleid);

            var authkeys = _work.Repository<Model.DB.Auth_Key>().Queryable();

            var temp = from auth in authlist
                       group auth by auth.ModuleName into authg
                       select new AuthModule
                       {
                           ModuleName = authg.Key,
                           AuthMenus = from menu in authg.Where(q => !string.IsNullOrEmpty(q.RoutUrl) || !string.IsNullOrEmpty(q.FunctionName))
                                       group menu by menu.MenuName into menug
                                       select new AuthKeyMenu
                                       {
                                           MenuName = menug.Key,
                                           AuthFuncS = from authkey in menug.Where(q => !string.IsNullOrEmpty(q.FunctionName))
                                                       let check = alauthkey.Any(q => q.AuthKey == authkey.AuthKey)
                                                       let aukey = authkeys.FirstOrDefault(q => q.AuthKey == authkey.AuthKey)
                                                       select new AuthKeyFunc
                                                       {
                                                           AuthFunc = aukey,
                                                           Checked = check
                                                       }

                                       }

                       };

            return new ActionResult<IEnumerable<AuthModule>>(temp);


        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public ActionResult<Pager<UserView>> GetUserList(PagerQuery<UserQuery> para)
        {
            var users = _rpsuser.Queryable();
            var profiles = _rpsprofiel.Queryable();

            var retmp = from u in users.Where(q => (q.Login.Contains(para.KeyWord)
                        || string.IsNullOrEmpty(para.KeyWord)
                        ))
                        let pro = profiles.FirstOrDefault(q => q.Login == u.Login)
                        select new UserView
                        {
                            UserInfo = u,
                            UserProfile = pro,
                            StateStr = u.State == (int)PublicEnum.GenericState.Cancel ? "作废" :
                              u.State == (int)PublicEnum.GenericState.Normal ? "正常" : "未知"
                        };

            var re = new Pager<UserView>().GetCurrentPage(retmp, para.PageSize, para.PageIndex);

            return new ActionResult<Pager<UserView>>(re);

        }
        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public ActionResult<bool> LoginSetRole(LoginSetRole para)
        {
            var userrole = _work.Repository<Model.DB.Auth_UserRole>();

            var dbroles = new List<Model.DB.Auth_UserRole>();
            foreach (var role in para.RoleID)
            {
                var urole = new Model.DB.Auth_UserRole
                {
                    Login = para.Login,
                    RoleID = role,

                    ID = Guid.NewGuid()
                };
                dbroles.Add(urole);
            };

            userrole.Delete(q => q.Login == para.Login);

            userrole.Add(dbroles);

            _work.Commit();

            return new ActionResult<bool>(true);

        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public ActionResult<bool> ReSetPwd(UserReSetPwd para)
        {
            var user = _rpsuser.GetModel(q => q.ID == para.ID);
            if (user == null)
            {
                throw new Exception("用户不存在");
            }

            user.Pwd = para.Pwd;
            _rpsuser.Update(user);
            _work.Commit();
            return new ActionResult<bool>(true);

        }

        /// <summary>
        /// 设置用户Profile
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public ActionResult<bool> SetProfile(UserSetProfile para)
        {
            var pro = _rpsprofiel.GetModel(q => q.Login == para.Login)
                ;
            if (pro == null)
            {
                throw new Exception("用户Profile不存在");
            }

            para.Clone(pro);

            _rpsprofiel.Update(pro);
            _work.Commit();

            return new ActionResult<bool>(true);


        }
        /// <summary>
        /// 为角色设置权限
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public ActionResult<bool> SetRoleAuth(RoleSet para)
        {
            var authrole = _work.Repository<Model.DB.Auth_RoleAuthScope>();

            var dbrauth = new List<Model.DB.Auth_RoleAuthScope>();
            foreach (var rauth in para.AuthKeys)
            {
                var irauth = new Model.DB.Auth_RoleAuthScope
                {
                    AuthKey = rauth,

                    RoleID = para.RoleID,
                    ID = Guid.NewGuid()
                };
                dbrauth.Add(irauth);
            }

            authrole.Delete(q => q.RoleID == para.RoleID);

            authrole.Add(dbrauth);

            _work.Commit();

            return new ActionResult<bool>(true);


        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public ActionResult<bool> Update(UserEdit para)
        {
            var user = _rpsuser.GetModel(q => q.ID == para.ID);
            if (user == null)
            {
                throw new Exception("用户不存在");
            }

            para.Clone(user);
            _rpsuser.Update(user);
            _work.Commit();

            return new ActionResult<bool>(true);
        }
        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public virtual ActionResult<UserView> UserSignin(UserSignin para)
        {
            var user = _rpsuser.GetModel(q => q.Login == para.Login);
            if (user == null)
            {
                throw new Exception("用户名或密码错误");
            }
            if (string.Compare(user.Pwd, para.Pwd, false) != 0)
            {
                throw new Exception("用户名或密码错误");
            }
            var profile = _rpsprofiel.GetModel(q => q.Login == para.Login);

            user.Token = Command.CreateToken(64);
            _rpsuser.Update(user);
            _work.Commit();

            return new ActionResult<UserView>(new UserView
            {
                UserInfo = user,
                UserProfile = profile
            });


        }
    }
}
