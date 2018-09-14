using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
                State = (int)PublicEnum.GenericState.Normal,
                RegDate=DateTime.Now
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
        /// <summary>
        /// 群发短信
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public ActionResult<bool> AllSend(DateTime dt)
        {
            var proj = _work.Repository<Model.DB.Project_Info>().Queryable(q => q.CreateDate.Year == dt.Year && q.CreateDate.Month == dt.Month);
            var Point = _work.Repository<Model.DB.Project_Point>().Queryable();
            var Contacts = _work.Repository<Model.DB.Project_Contacts>().Queryable();
            var sms = _work.Repository<Model.DB.Project_SMS>().Queryable();
            var retemp = from ac in proj
                         let pon = Point.Where(q => q.ProjectID == ac.ID)
                         let con = Contacts.FirstOrDefault(q => q.ProjectID == ac.ID)
                         select new ProjectSMS
                         {
                             ProjectContact=con,
                             Timeouts = from bc in pon.Where(q => q.PointExecMemo == null && q.PointSchedule != null && q.IsSend == true)
                                        let a = (int)(Math.Floor((decimal)DbFunctions.DiffDays((DateTime)bc.PointSchedule, (DateTime)DateTime.Now)))
                                        let b = (int)(Math.Floor((decimal)DbFunctions.DiffDays((DateTime)bc.PointSchedule, (DateTime)bc.PointExec)))
                                        select new SMSBase
                                        {
                                            WeekInt = bc.PointExec == null ? a : b
                                        }
                         };
            var moth = dt.ToString("yyyy.MM");
            //前期项目 未按序时推进 滞后１个月 滞后２个月 滞后３个月
            var Prophase = proj.Count(); int z = 0;int x = 0;int c = 0;int v = 0;
            foreach (var item in retemp)
            {
                if (item.Timeouts.Count() > 0)
                {
                    var bae = item.Timeouts.Max(s => s.WeekInt);
                    if (bae > 0) z += 1; if (bae>= 30 && bae < 60) x += 1; if (bae >=60 && bae < 90) c += 1; if (bae > 90) v += 1;
                }
            }
            //正常推进
            var baea = Prophase-z;
            foreach (var item in retemp)
            {
                SMSPacket jj = new SMSPacket();
                List<string> ss = new List<string>();
                ss.Add(moth); ss.Add(Prophase.ToString()); ss.Add(baea.ToString());
                ss.Add(z.ToString()); ss.Add(x.ToString()); ss.Add(c.ToString()); ss.Add(v.ToString());
                jj.mobile = item.ProjectContact.SitePrincipalTEL + "," + item.ProjectContact.PrincipalTEL + "," + item.ProjectContact.SiteLinkTEL + "," + item.ProjectContact.LeaderTEL
                + "," + item.ProjectContact.DeptPrincipalTEL + "," + item.ProjectContact.ComPrincipalTEL + "," + item.ProjectContact.ComLeadTEL + "," + item.ProjectContact.HandlerTEL;
                jj.templateno = System.Configuration.ConfigurationManager.AppSettings["smsmu3"];
                jj.variables = ss;
                SendTelMSG(jj);
            }
            return new ActionResult<bool>(true);
        }
        public static void SendTelMSG(SMSPacket pack)
        {
                string smsur = System.Configuration.ConfigurationManager.AppSettings["smsmulturl"];//短信地址
                string smsui = System.Configuration.ConfigurationManager.AppSettings["smsuid"];//短信平台用户名
                string smspwd = System.Configuration.ConfigurationManager.AppSettings["smspwd"];//短信平台密码

                pack.uid = smsui;
                pack.userpwd = smspwd;

            var parastr = Newtonsoft.Json.JsonConvert.SerializeObject(pack);
            System.Net.WebRequest request = (System.Net.WebRequest)System.Net.HttpWebRequest.Create(smsur);
            request.Method = "POST";
            Byte[] postbytes = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(parastr);
            request.ContentType = "application/json";
            request.ContentLength = postbytes.Length;
            using (System.IO.Stream stream = request.GetRequestStream())
            {
                stream.Write(postbytes, 0, postbytes.Length);
                stream.Close();
            }
            string re = "";
            using (System.Net.WebResponse response = request.GetResponse())
            {
                if (response == null)
                {
                    throw new Exception("Response is not Created");
                }
                using (System.IO.Stream restream = response.GetResponseStream())
                {
                    using (System.IO.StreamReader getrd = new System.IO.StreamReader(restream, System.Text.Encoding.UTF8))
                    {
                        re = getrd.ReadToEnd();
                        getrd.Close();
                    }
                    restream.Close();
                }
            }

        }
    public class SMSPacket
    {
        /// <summary>
        /// 用户名，不赋值
        /// </summary>
        public string uid { get; set; }
        /// <summary>
        /// 密码，不赋值
        /// </summary>
        public string userpwd { get; set; }
        /// <summary>
        /// 多号码用英文逗号
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 签名可以不传
        /// </summary>
        public string signature { get; set; }
        /// <summary>
        /// 变理值集合，传入的值顺序与变理顺序对应
        /// </summary>
        public IEnumerable<string> variables { get; set; }
        /// <summary>
        /// 模板ID
        /// </summary>
        public string templateno { get; set; }
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
                            StateStr = ac.State == (int)PublicEnum.GenericState.Normal ? "正常" :
                             ac.State == (int)PublicEnum.GenericState.Cancel ? "未审核" : "未知"
                        };
            var re = new Pager<OwnerView>().GetCurrentPage(retmp, para.PageSize, para.PageIndex);
            return new ActionResult<Pager<OwnerView>>(re);
        }

        public ActionResult<IEnumerable<Basic_Owner>> OwnerSelector()
        {
            var ownerdb = _work.Repository<Model.DB.Basic_Owner>();
            var re = ownerdb.Queryable(q=>q.State==(int)PublicEnum.GenericState.Normal&&(AppUser.CurrentUserInfo.UserInfo.OwnerID==q.ID|| AppUser.CurrentUserInfo.UserInfo.OwnerID==Guid.Empty));
            return new ActionResult<IEnumerable<Basic_Owner>>(re);
        }
    }
}
