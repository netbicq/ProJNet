using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProJ.Model;
using ProJ.Model.DB;
using ProJ.Model.Para;
using ProJ.Model.View;
using ProJ.ORM;

namespace ProJ.Bll
{
    /// <summary>
    /// 业主单位
    /// </summary>
    public class OwnerService : ServiceBase, IBll.IOwner
    {

        private ORM.IUnitwork _work = null;

        public OwnerService(ORM.IUnitwork work)
        {
            _work = work;
            Unitwork = work;

        }
        public ActionResult<bool> AddOwner(OwnerNew owner)
        {


            if (string.IsNullOrEmpty(owner.Handler)
                || string.IsNullOrEmpty(owner.HandlerTEL)
                || string.IsNullOrEmpty(owner.Principal)
                || string.IsNullOrEmpty(owner.PrincipalTEL)
                )
            {
                throw new Exception("联系人 联系人电话 负责人 负责人电话均不能为空");
            }
            if (string.IsNullOrEmpty(owner.Login)
                || string.IsNullOrEmpty(owner.Pwd)
                )
            {
                throw new Exception("操作员和密码不能为空");
            }



            Model.DB.Basic_Owner ownermodel = new Basic_Owner
            {
                ID = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                CreateMan = AppUser.CurrentUserInfo.UserProfile.CNName,
                State = (int)PublicEnum.GenericState.Normal
            };

            owner.Clone(ownermodel);

            //操作员
            Model.DB.Auth_User usermodel = new Auth_User()
            {
                CreateDate = DateTime.Now,
                CreateMan = AppUser.CurrentUserInfo.UserProfile.CNName,
                ID = Guid.NewGuid(),
                Login = owner.Login,
                OtherEdit = false,
                OtherView = false,
                OwnerID = ownermodel.ID,
                Pwd = owner.Pwd,
                State = (int)PublicEnum.GenericState.Normal,
                Token = "",
                TokenValidTime = DateTime.Now
            };

            //Profile
            Model.DB.Auth_UserProfile profilemodel = new Auth_UserProfile()
            {
                CNName = owner.OwnerName,
                HeadIMG = "",
                ID = Guid.NewGuid(),
                Login = owner.Login,
                Tel = ""
            };

            var ownerdb = _work.Repository<Model.DB.Basic_Owner>();
            var userdb = _work.Repository<Model.DB.Auth_User>();
            var profiledb = _work.Repository<Model.DB.Auth_UserProfile>();

            if (ownerdb.Any(q => q.OwnerName == ownermodel.OwnerName))
            {
                throw new Exception("业主名称已经存在");
            }
            if (userdb.Any(q => q.Login == usermodel.Login))
            {
                throw new Exception("登陆名已经存在");
            }
            ownerdb.Add(ownermodel);
            userdb.Add(usermodel);
            profiledb.Add(profilemodel);


            _work.Commit();

            return new ActionResult<bool>(true);

        }

        public ActionResult<bool> AddOwner(OwnerEdit owner)
        {

            var db = _work.Repository<Model.DB.Basic_Owner>();
            var dbmodel = db.GetModel(q => q.ID == owner.ID);
            if (db.Any(q => q.ID != owner.ID && q.OwnerName == owner.OwnerName))
            {
                throw new Exception("单位名称：" + owner.OwnerName + "已经存在");
            }

            owner.Clone(dbmodel);

            db.Update(dbmodel);

            _work.Commit();


            return new ActionResult<bool>(true);
        }

        public ActionResult<bool> ApplyOwner(OwnerNew owner)
        {
            if (string.IsNullOrEmpty(owner.Handler)
    || string.IsNullOrEmpty(owner.HandlerTEL)
    || string.IsNullOrEmpty(owner.Principal)
    || string.IsNullOrEmpty(owner.PrincipalTEL)
    )
            {
                throw new Exception("联系人 联系人电话 负责人 负责人电话均不能为空");
            }
            if (string.IsNullOrEmpty(owner.Login)
                || string.IsNullOrEmpty(owner.Pwd)
                )
            {
                throw new Exception("操作员和密码不能为空");
            }



            Model.DB.Basic_Owner ownermodel = new Basic_Owner
            {
                ID = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                RegDate=DateTime.Now,
                CreateMan = AppUser.CurrentUserInfo.UserProfile.CNName,
                State = (int)PublicEnum.GenericState.Normal
            };

            owner.Clone(ownermodel);

            //操作员
            Model.DB.Auth_User usermodel = new Auth_User()
            {
                CreateDate = DateTime.Now,
                CreateMan = "自主申请",
                ID = Guid.NewGuid(),
                Login = owner.Login,
                OtherEdit = false,
                OtherView = false,
                OwnerID = ownermodel.ID,
                Pwd = owner.Pwd,
                State = (int)PublicEnum.GenericState.Normal,
                Token = "",
                TokenValidTime = DateTime.Now
            };

            //Profile
            Model.DB.Auth_UserProfile profilemodel = new Auth_UserProfile()
            {
                CNName = owner.OwnerName,
                HeadIMG = "",
                ID = Guid.NewGuid(),
                Login = owner.Login,
                Tel = ""
            };

            var ownerdb = _work.Repository<Model.DB.Basic_Owner>();
            var userdb = _work.Repository<Model.DB.Auth_User>();
            var profiledb = _work.Repository<Model.DB.Auth_UserProfile>();

            if (ownerdb.Any(q => q.OwnerName == ownermodel.OwnerName))
            {
                throw new Exception("业主名称已经存在");
            }
            if (userdb.Any(q => q.Login == usermodel.Login))
            {
                throw new Exception("登陆名已经存在");
            }
            ownerdb.Add(ownermodel);
            userdb.Add(usermodel);
            profiledb.Add(profilemodel);


            _work.Commit();

            return new ActionResult<bool>(true);
        }

        public ActionResult<bool> DelOwner(Guid id)
        {
            var ownerdb = _work.Repository<Model.DB.Basic_Owner>();
            var projdb = _work.Repository<Model.DB.Project_Info>();

            var model = ownerdb.GetModel(q => q.ID == id);
            if(model==null)
            {
                throw new Exception("业主不存在");
            }
            if(projdb.Any(q=>q.OwnerID == model.ID))
            {
                throw new Exception("业主单位已经存在项目，不允许被删除");
            }

            ownerdb.Delete(model);

            _work.Commit();
            return new ActionResult<bool>(true);

        }

        public ActionResult<Pager<OwnerView>> GetOwnerList(PagerQuery<string> para)
        {
            var ownerdb = _work.Repository<Model.DB.Basic_Owner>();
            var retmp = from ac in ownerdb.GetList(q =>
                      (q.OwnerName.Contains(para.KeyWord)
                       || string.IsNullOrEmpty(para.KeyWord)
                       ))
                        select new Model.View.OwnerView
                        {
                            OwnerInfo = ac,
                            StateStr = ac.State == (int)PublicEnum.GenericState.Normal ? "正常" : "未知"
                        };
            var re = new Pager<OwnerView>().GetCurrentPage(retmp, para.PageSize, para.PageIndex);
            return new ActionResult<Pager<OwnerView>>(re);
        }

        public ActionResult<IEnumerable<Basic_Owner>> OwnerSelector()
        {
            var ownerdb = _work.Repository<Model.DB.Basic_Owner>();
            var re = ownerdb.Queryable(q=>q.State==(int)PublicEnum.GenericState.Normal);
            return new ActionResult<IEnumerable<Basic_Owner>>(re);
        }
    }
}
