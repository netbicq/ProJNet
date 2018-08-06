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
    /// 项目管理
    /// </summary>
    public class ProJService : ServiceBase, IBll.IProJ
    {
        private ORM.IUnitwork _work = null;

        public ProJService(ORM.IUnitwork work)
        {
            _work = work;
            Unitwork = work;
        }
        /// <summary>
        /// 后续计划修改
        /// </summary>
        /// <param name="After"></param>
        /// <returns></returns>
        public ActionResult<bool> AfterEdit(AfterEdit After)
        {
            var info1 = _work.Repository<Model.DB.Project_Info>();
            var to = info1.GetModel(q => q.ID == After.ID);
            if (to.State != (int)PublicEnum.ProjState.Modified)
            {
                throw new Exception("此项目状态不允许操作");
            }
            if (to.State == (int)PublicEnum.ProjState.Start)
            {
                throw new Exception("开工不允许任何操作");
            }
            to.State = (int)PublicEnum.ProjState.Normal;
            info1.Update(to);

            var Afterinfo = _work.Repository<Model.DB.Project_Info>();
            var info = Afterinfo.GetModel(q => q.ID == After.ID);
            After.Clone(info);
            Afterinfo.Update(info);
            //switch (After.Quarter)
            //{
            //    case PublicEnum.QuarterState.One:
            //        info.Q1Invest = After.QInvest;
            //        info.Q1Memo = After.QMemo;
            //        Afterinfo.Update(info);
            //        break;
            //    case PublicEnum.QuarterState.Two:
            //        info.Q2Invest = After.QInvest;
            //        info.Q2Memo = After.QMemo;
            //        Afterinfo.Update(info);
            //        break;
            //    case PublicEnum.QuarterState.Three:
            //        info.Q3Invest = After.QInvest;
            //        info.Q3Memo = After.QMemo;
            //        Afterinfo.Update(info);
            //        break;
            //    case PublicEnum.QuarterState.Four:
            //        info.Q4Invest = After.QInvest;
            //        info.Q4Memo = After.QMemo;
            //        Afterinfo.Update(info);
            //        break;
            //    default:
            //        break;
            //}
            var Log = _work.Repository<Project_Log>();
            var logs = new Project_Log();
            logs.ProjectID = After.ID;
            logs.LogContent = "修改了后续计划：";
            logs.CreateMan = AppUser.CurrentUserInfo.UserProfile.CNName;
            logs.State = 1;
            Log.Add(logs);
            _work.Commit();
            return new ActionResult<bool>(true);
        }

        /// <summary>
        /// 获取当前时间存在超期节点的工程项目列表
        /// 用于手机短信提醒
        /// </summary>
        /// <returns></returns>
        public ActionResult<IEnumerable<ProjectSMS>> GetProjectSMS()
        {
            ///查出已经超期未执行的节点
            ///算出超期了几周
            ///查询 Model.DB.Project_SMS表里面是否存在已经发送，
            ///如果存在则不再返回，如果不存在则返回用于手机短信发送
            var proj = _work.Repository<Model.DB.Project_Info>().Queryable();
            var Contacts = _work.Repository<Model.DB.Project_Contacts>().Queryable();
            var Schedule = _work.Repository<Model.DB.Project_Schedule>().Queryable();
            var owner = _work.Repository<Model.DB.Basic_Owner>().Queryable();
            var sms = _work.Repository<Model.DB.Project_SMS>().Queryable();
            var retemp = from ac in proj.Where(q=>q.State!=(int)PublicEnum.ProjState.Start)
                         let owners = owner.FirstOrDefault(q => q.ID == ac.OwnerID)
                         let con = Contacts.FirstOrDefault(q => q.ProjectID == ac.ID)
                         select new ProjectSMS
                         {
                             OwerInfo = owners,
                             ProjectContact = con,
                             ProjectInfo = ac
                         };
            var probin = retemp.ToList();
            List<ProjectSMS> proview = new List<ProjectSMS>();
            foreach (var item in probin)
            {
                List<SMSBase> baseword = new List<SMSBase>();
                var sch1 = Schedule.FirstOrDefault(q => q.ProjectID == item.ProjectInfo.ID && q.ScheduleType == (int)PublicEnum.PlanType.Plan);
                var sch2 = Schedule.FirstOrDefault(q => q.ProjectID == item.ProjectInfo.ID && q.ScheduleType == (int)PublicEnum.PlanType.Ement);
                if (sch2.Point_CBSJJGSPF != null && sch1.Point_CBSJJGSPF != null&&((DateTime)sch2.Point_CBSJJGSPF - (DateTime)sch1.Point_CBSJJGSPF).TotalDays>0)
                {
                    var me = new SMSBase {Exec=PublicEnum.PlanEnd.Point_CBSJJGSPF,PointName= "初步设计及概算批复",WeekInt= Convert.ToInt32(Math.Floor(((DateTime)sch2.Point_CBSJJGSPF - (DateTime)sch1.Point_CBSJJGSPF).TotalDays / 7))};
                    if (!sms.Any(q=>q.PointName==me.PointName&&q.WeekInt==me.WeekInt))
                    {
                        baseword.Add(me);
                    }
                }
                if (sch2.Point_CSKZJPF != null && sch1.Point_CSKZJPF != null&&((DateTime)sch2.Point_CSKZJPF - (DateTime)sch1.Point_CSKZJPF).TotalDays > 0)
                {
                    var me = new SMSBase { Exec = PublicEnum.PlanEnd.Point_CSKZJPF, PointName = "财审控制价批复", WeekInt = Convert.ToInt32(Math.Floor(((DateTime)sch2.Point_CSKZJPF - (DateTime)sch1.Point_CSKZJPF).TotalDays / 7)) };
                    if (!sms.Any(q => q.PointName == me.PointName && q.WeekInt == me.WeekInt))
                    {
                        baseword.Add(me);
                    }
                }
                if (sch2.Point_DKBGWC != null && sch1.Point_DKBGWC != null&&((DateTime)sch2.Point_DKBGWC - (DateTime)sch1.Point_DKBGWC).TotalDays > 0)
                {
                    var me = new SMSBase { Exec = PublicEnum.PlanEnd.Point_DKBGWC, PointName = "地勘报告完成", WeekInt = Convert.ToInt32(Math.Floor(((DateTime)sch2.Point_DKBGWC - (DateTime)sch1.Point_DKBGWC).TotalDays / 7)) };
                    if (!sms.Any(q => q.PointName == me.PointName && q.WeekInt == me.WeekInt))
                    {
                        baseword.Add(me);
                    }
                }
                if (sch2.Point_GCKXXYJBGPF != null && sch1.Point_GCKXXYJBGPF != null&&((DateTime)sch2.Point_GCKXXYJBGPF - (DateTime)sch1.Point_GCKXXYJBGPF).TotalDays > 0)
                {
                    var me = new SMSBase { Exec = PublicEnum.PlanEnd.Point_GCKXXYJBGPF, PointName = "工程可行性研究报告批复", WeekInt = Convert.ToInt32(Math.Floor(((DateTime)sch2.Point_GCKXXYJBGPF - (DateTime)sch1.Point_GCKXXYJBGPF).TotalDays / 7)) };
                    if (!sms.Any(q => q.PointName == me.PointName && q.WeekInt == me.WeekInt))
                    {
                        baseword.Add(me);
                    }
                }
                if (sch2.Point_GHXZYDJYJSPF != null && sch1.Point_GHXZYDJYJSPF != null&&((DateTime)sch2.Point_GHXZYDJYJSPF - (DateTime)sch1.Point_GHXZYDJYJSPF).TotalDays > 0)
                {
                    var me = new SMSBase { Exec = PublicEnum.PlanEnd.Point_GHXZYDJYJSPF, PointName = "规划选址及用地意见书批复", WeekInt = Convert.ToInt32(Math.Floor(((DateTime)sch2.Point_GHXZYDJYJSPF - (DateTime)sch1.Point_GHXZYDJYJSPF).TotalDays / 7)) };
                    if (!sms.Any(q => q.PointName == me.PointName && q.WeekInt == me.WeekInt))
                    {
                        baseword.Add(me);
                    }
                }
                if (sch2.Point_JSGCGHXKZPF != null && sch1.Point_JSGCGHXKZPF != null&&((DateTime)sch2.Point_JSGCGHXKZPF - (DateTime)sch1.Point_JSGCGHXKZPF).TotalDays > 0)
                {
                    var me = new SMSBase { Exec = PublicEnum.PlanEnd.Point_JSGCGHXKZPF, PointName = "建设工程规划许可证批复", WeekInt = Convert.ToInt32(Math.Floor(((DateTime)sch2.Point_JSGCGHXKZPF - (DateTime)sch1.Point_JSGCGHXKZPF).TotalDays / 7)) };
                    if (!sms.Any(q => q.PointName == me.PointName && q.WeekInt == me.WeekInt))
                    {
                        baseword.Add(me);
                    }
                }
                if (sch2.Point_JSYDGHXKZPF != null && sch1.Point_JSYDGHXKZPF != null&&((DateTime)sch2.Point_JSYDGHXKZPF - (DateTime)sch1.Point_JSYDGHXKZPF).TotalDays > 0)
                {
                    var me = new SMSBase { Exec = PublicEnum.PlanEnd.Point_JSYDGHXKZPF, PointName = "建设用地规划许可证批复", WeekInt = Convert.ToInt32(Math.Floor(((DateTime)sch2.Point_JSYDGHXKZPF - (DateTime)sch1.Point_JSYDGHXKZPF).TotalDays / 7)) };
                    if (!sms.Any(q => q.PointName == me.PointName && q.WeekInt == me.WeekInt))
                    {
                        baseword.Add(me);
                    }
                }
                if (sch2.Point_LZYSXJGDPF != null && sch1.Point_LZYSXJGDPF != null&&((DateTime)sch2.Point_LZYSXJGDPF - (DateTime)sch1.Point_LZYSXJGDPF).TotalDays > 0)
                {
                    var me = new SMSBase { Exec = PublicEnum.PlanEnd.Point_LZYSXJGDPF, PointName = "农转用手续及供地批复", WeekInt = Convert.ToInt32(Math.Floor(((DateTime)sch2.Point_LZYSXJGDPF - (DateTime)sch1.Point_LZYSXJGDPF).TotalDays / 7)) };
                    if (!sms.Any(q => q.PointName == me.PointName && q.WeekInt == me.WeekInt))
                    {
                        baseword.Add(me);
                    }
                }
                if (sch2.Point_SGJLRYBA != null && sch1.Point_SGJLRYBA != null&&((DateTime)sch2.Point_SGJLRYBA - (DateTime)sch1.Point_SGJLRYBA).TotalDays > 0)
                {
                    var me = new SMSBase { Exec = PublicEnum.PlanEnd.Point_SGJLRYBA, PointName = "施工监理人员备案", WeekInt = Convert.ToInt32(Math.Floor(((DateTime)sch2.Point_SGJLRYBA - (DateTime)sch1.Point_SGJLRYBA).TotalDays / 7)) };
                    if (!sms.Any(q => q.PointName == me.PointName && q.WeekInt == me.WeekInt))
                    {
                        baseword.Add(me);
                    }
                }
                if (sch2.Point_SGJLZTP != null && sch1.Point_SGJLZTP != null&&((DateTime)sch2.Point_SGJLZTP - (DateTime)sch1.Point_SGJLZTP).TotalDays > 0)
                {
                    var me = new SMSBase { Exec = PublicEnum.PlanEnd.Point_SGJLZTP, PointName = "施工监理招投标", WeekInt = Convert.ToInt32(Math.Floor(((DateTime)sch2.Point_SGJLZTP - (DateTime)sch1.Point_SGJLZTP).TotalDays / 7)) };
                    if (!sms.Any(q => q.PointName == me.PointName && q.WeekInt == me.WeekInt))
                    {
                        baseword.Add(me);
                    }
                }
                if (sch2.Point_SGTBZHSC != null && sch1.Point_SGTBZHSC != null&&((DateTime)sch2.Point_SGTBZHSC - (DateTime)sch1.Point_SGTBZHSC).TotalDays > 0)
                {
                    var me = new SMSBase { Exec = PublicEnum.PlanEnd.Point_SGTBZHSC, PointName = "施工图编制和审查", WeekInt = Convert.ToInt32(Math.Floor(((DateTime)sch2.Point_SGTBZHSC - (DateTime)sch1.Point_SGTBZHSC).TotalDays / 7)) };
                    if (!sms.Any(q => q.PointName == me.PointName && q.WeekInt == me.WeekInt))
                    {
                        baseword.Add(me);
                    }
                }
                if (sch2.Point_SGXKZPF != null && sch1.Point_SGXKZPF != null&&((DateTime)sch2.Point_SGXKZPF - (DateTime)sch1.Point_SGXKZPF).TotalDays > 0)
                {
                    var me = new SMSBase { Exec = PublicEnum.PlanEnd.Point_SGXKZPF, PointName = "施工许可证批复", WeekInt = Convert.ToInt32(Math.Floor(((DateTime)sch2.Point_SGXKZPF - (DateTime)sch1.Point_SGXKZPF).TotalDays / 7)) };
                    if (!sms.Any(q => q.PointName == me.PointName && q.WeekInt == me.WeekInt))
                    {
                        baseword.Add(me);
                    }
                }
                if (sch2.Point_TDCRHT != null && sch1.Point_TDCRHT != null&&((DateTime)sch2.Point_TDCRHT - (DateTime)sch1.Point_TDCRHT).TotalDays > 0)
                {
                    var me = new SMSBase { Exec = PublicEnum.PlanEnd.Point_TDCRHT, PointName = "土地出让合同", WeekInt = Convert.ToInt32(Math.Floor(((DateTime)sch2.Point_TDCRHT - (DateTime)sch1.Point_TDCRHT).TotalDays / 7)) };
                    if (!sms.Any(q => q.PointName == me.PointName && q.WeekInt == me.WeekInt))
                    {
                        baseword.Add(me);
                    }
                }
                if (sch2.Point_TDSYQZ != null && sch1.Point_TDSYQZ != null&&((DateTime)sch2.Point_TDSYQZ - (DateTime)sch1.Point_TDSYQZ).TotalDays > 0)
                {
                    var me = new SMSBase { Exec = PublicEnum.PlanEnd.Point_TDSYQZ, PointName = "土地使用权证", WeekInt = Convert.ToInt32(Math.Floor(((DateTime)sch2.Point_TDSYQZ - (DateTime)sch1.Point_TDSYQZ).TotalDays / 7)) };
                    if (!sms.Any(q => q.PointName == me.PointName && q.WeekInt == me.WeekInt))
                    {
                        baseword.Add(me);
                    }
                }
                if (sch2.Point_XMKG != null && sch1.Point_XMKG != null&&((DateTime)sch2.Point_XMKG - (DateTime)sch1.Point_XMKG).TotalDays > 0)
                {
                    var me = new SMSBase { Exec = PublicEnum.PlanEnd.Point_XMKG, PointName = "项目开工", WeekInt = Convert.ToInt32(Math.Floor(((DateTime)sch2.Point_XMKG - (DateTime)sch1.Point_XMKG).TotalDays / 7)) };
                    if (!sms.Any(q => q.PointName == me.PointName && q.WeekInt == me.WeekInt))
                    {
                        baseword.Add(me);
                    }
                }
                if (sch2.Point_XMZPSJFAPF != null && sch1.Point_XMZPSJFAPF != null&&((DateTime)sch2.Point_XMZPSJFAPF - (DateTime)sch1.Point_XMZPSJFAPF).TotalDays > 0)
                {
                    var me = new SMSBase { Exec = PublicEnum.PlanEnd.Point_XMZPSJFAPF, PointName = "项目总平设计方案批复", WeekInt = Convert.ToInt32(Math.Floor(((DateTime)sch2.Point_CBSJJGSPF - (DateTime)sch1.Point_CBSJJGSPF).TotalDays / 7)) };
                    if (!sms.Any(q => q.PointName == me.PointName && q.WeekInt == me.WeekInt))
                    {
                        baseword.Add(me);
                    }
                }
                if (sch2.Point_YSBZWC != null && sch1.Point_YSBZWC != null&&((DateTime)sch2.Point_YSBZWC - (DateTime)sch1.Point_YSBZWC).TotalDays > 0)
                {
                    var me = new SMSBase { Exec = PublicEnum.PlanEnd.Point_YSBZWC, PointName = "预算编制完成", WeekInt = Convert.ToInt32(Math.Floor(((DateTime)sch2.Point_YSBZWC - (DateTime)sch1.Point_YSBZWC).TotalDays / 7)) };
                    if (!sms.Any(q => q.PointName == me.PointName && q.WeekInt == me.WeekInt))
                    {
                        baseword.Add(me);
                    }
                }
                item.Timeouts = baseword;
                if (baseword.Count()>0)
                {
                    proview.Add(item);
                }
            };
            

            return new ActionResult<IEnumerable<ProjectSMS>>(proview);
        }

        /// <summary>
        /// 删除问题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult<bool> IssDel(Guid id)
        {
            var Issue = _work.Repository<Model.DB.Project_Issue>();
            var re = Issue.Delete(q => q.ID == id);
            return new ActionResult<bool>(re > 0);
        }
        /// <summary>
        /// 修改问题
        /// </summary>
        /// <param name="Iss"></param>
        /// <returns></returns>
        public ActionResult<bool> IssEdit(IssueEdit Iss)
        {
            var info1 = _work.Repository<Model.DB.Project_Info>();
            var to = info1.GetModel(q => q.ID == Iss.ProjectID);
            if (to.State != (int)PublicEnum.ProjState.Modified)
            {
                throw new Exception("此项目状态不允许操作");
            }
            if (to.State == (int)PublicEnum.ProjState.Start)
            {
                throw new Exception("开工不允许任何操作");
            }
            to.State = (int)PublicEnum.ProjState.Normal;
            info1.Update(to);
            var Issue = _work.Repository<Model.DB.Project_Issue>();
            var re = Issue.GetModel(q => q.ID == Iss.ID);
            re.IssueContent = Iss.IssueContent;
            Issue.Update(re);

            var Log = _work.Repository<Project_Log>();
            var logs = new Project_Log();
            logs.ProjectID = Iss.ProjectID;
            logs.LogContent = "修改了问题";
            logs.CreateMan = AppUser.CurrentUserInfo.UserProfile.CNName;
            logs.State = 1;
            Log.Add(logs);
            _work.Commit();
            return new ActionResult<bool>(true);
        }

        /// <summary>
        /// 问题发布
        /// </summary>
        /// <param name="Iss"></param>
        /// <returns></returns>
        public ActionResult<bool> IssueNew(IssueNew Iss)
        {
            var pro = _work.Repository<Model.DB.Project_Info>();
            var to = pro.GetModel(q => q.ID == Iss.ProjectID);
            if (to.State == (int)PublicEnum.ProjState.Start)
            {
                throw new Exception("开工不允许任何操作");
            }
            var Issue = _work.Repository<Model.DB.Project_Issue>();
            var issu = new Project_Issue();
            Iss.Clone(issu);
            issu.CreateMan = AppUser.CurrentUserInfo.UserProfile.CNName;
            issu.State = 1;
            Issue.Add(issu);

            var Log = _work.Repository<Project_Log>();
            var logs = new Project_Log();
            logs.ProjectID = Iss.ProjectID;
            logs.LogContent = "发布了问题";
            logs.CreateMan = AppUser.CurrentUserInfo.UserProfile.CNName;
            logs.State = 1;
            Log.Add(logs);
            _work.Commit();
            return new ActionResult<bool>(true);
        }
        /// <summary>
        /// 修改联系人
        /// </summary>
        /// <param name="Con"></param>
        /// <returns></returns>
        public ActionResult<bool> ProJCon(ProJCon Con)
        {
            var info1 = _work.Repository<Model.DB.Project_Info>();
            var to = info1.GetModel(q => q.ID == Con.ProjectID);
            if (to.State != (int)PublicEnum.ProjState.Modified)
            {
                throw new Exception("此项目状态不允许操作");
            }
            if (to.State == (int)PublicEnum.ProjState.Start)
            {
                throw new Exception("开工不允许任何操作");
            }
            to.State = (int)PublicEnum.ProjState.Normal;
            info1.Update(to);
            var Contact = _work.Repository<Model.DB.Project_Contacts>();
            var re = Contact.GetModel(q => q.ID == Con.ID);
            Con.Clone(re);
            Contact.Update(re);

            var Log = _work.Repository<Project_Log>();
            var logs = new Project_Log();
            logs.ProjectID = Con.ProjectID;
            logs.LogContent = "修改了联系人";
            logs.CreateMan = AppUser.CurrentUserInfo.UserProfile.CNName;
            logs.State = 1;
            Log.Add(logs);
            _work.Commit();
            return new ActionResult<bool>(true);
        }
        /// <summary>
        /// 修改基本信息
        /// </summary>
        /// <param name="pro"></param>
        /// <returns></returns>
        public ActionResult<bool> Projedit(ProjEdit pro)
        {
            var info1 = _work.Repository<Model.DB.Project_Info>();
            var to = info1.GetModel(q => q.ID == pro.ID);
            if (to.State != (int)PublicEnum.ProjState.Modified)
            {
                throw new Exception("此项目状态不允许操作");
            }
            if (to.State == (int)PublicEnum.ProjState.Start)
            {
                throw new Exception("开工不允许任何操作");
            }
            pro.Clone(to);
            to.State = (int)PublicEnum.ProjState.Normal;
            info1.Update(to);

            var Log = _work.Repository<Project_Log>();
            var logs = new Project_Log();
            logs.ProjectID = pro.ID;
            logs.LogContent = "修改了基本信息";
            logs.CreateMan = AppUser.CurrentUserInfo.UserProfile.CNName;
            logs.State = 1;
            Log.Add(logs);
            _work.Commit();
            return new ActionResult<bool>(true);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public ActionResult<Pager<ProjectView>> ProJList(PagerQuery<string> para)
        {
            var proj = _work.Repository<Model.DB.Project_Info>().Queryable();
            var Contacts = _work.Repository<Model.DB.Project_Contacts>().Queryable();
            var Schedule = _work.Repository<Model.DB.Project_Schedule>().Queryable();
            var Issue = _work.Repository<Model.DB.Project_Issue>().Queryable();
            var Log = _work.Repository<Model.DB.Project_Log>().Queryable();
            var dict = _work.Repository<Model.DB.Basic_Dict>().Queryable();
            var owner = _work.Repository<Model.DB.Basic_Owner>().Queryable();
            var retemp = from ac in proj.Where(q =>
                        (q.OwnerID == AppUser.CurrentUserInfo.UserInfo.OwnerID || AppUser.CurrentUserInfo.UserInfo.OwnerID == Guid.Empty)
                        && (para.KeyWord.Contains(q.ProjectName) || string.IsNullOrEmpty(para.KeyWord))
                         )
                         let owners = owner.FirstOrDefault(q => q.ID == ac.OwnerID)
                         let dicts = dict.FirstOrDefault(q => q.ID == ac.IndustryID)
                         let dicts2 = dict.FirstOrDefault(q => q.ID == ac.LevelID)
                         let con = Contacts.FirstOrDefault(q => q.ProjectID == ac.ID)
                         select new ProjectView
                         {
                             Project_Info = ac,
                             Project_Schedule = from a in Schedule.Where(q => q.ProjectID == ac.ID) select a,
                             Project_Issue = from s in Issue.Where(q => q.ProjectID == ac.ID) select s,
                             Project_Log = from d in Log.Where(q => q.ProjectID == ac.ID) orderby d.CreateDate descending select d,
                             StateStr = ac.State == (int)PublicEnum.ProjState.Normal ? "正常" :
                             ac.State == (int)PublicEnum.ProjState.Apply ? "申请" :
                             ac.State == (int)PublicEnum.ProjState.Modified ? "待修改" :
                             ac.State == (int)PublicEnum.ProjState.Start ? "开工" : "未知",
                             OwnerStr = owners == null ? null : owners.OwnerName,
                             ProjStr = dicts == null ? null : dicts.DictName,
                             ProJLeveStr = dicts2 == null ? null : dicts2.DictName,
                             Project_Contacts = con,
                             EditTable = new Schss()
                         };
            var re = new Pager<ProjectView>().GetCurrentPage(retemp, para.PageSize, para.PageIndex);
            var relist = re.Data.ToList();
            foreach (var item in relist)
            {
                item.Project_Schedule = item.Project_Schedule.OrderBy(q => q.ScheduleType);
                var ye = item.Project_Schedule.FirstOrDefault(q => q.ScheduleType == 2);
                if ((ye.Point_CBSJJGSMemo == null && ye.Point_CBSJJGSPF == null) || item.Project_Info.State == (int)PublicEnum.ProjState.Modified)
                    item.EditTable.Point_CBSJJGSPF = true;
                if ((ye.Point_CSKZJMemo == null && ye.Point_CSKZJPF == null) || item.Project_Info.State == (int)PublicEnum.ProjState.Modified)
                    item.EditTable.Point_CSKZJPF = true;
                if ((ye.Point_DKBGWC == null && ye.Point_DKBGWCMemo == null) || item.Project_Info.State == (int)PublicEnum.ProjState.Modified)
                    item.EditTable.Point_DKBGWC = true;
                if ((ye.Point_GCKXXYJBGMemo == null && ye.Point_GCKXXYJBGPF == null) || item.Project_Info.State == (int)PublicEnum.ProjState.Modified)
                    item.EditTable.Point_GCKXXYJBGPF = true;
                if ((ye.Point_GHXZJYDYJSMemo == null && ye.Point_GHXZYDJYJSPF == null) || item.Project_Info.State == (int)PublicEnum.ProjState.Modified)
                    item.EditTable.Point_GHXZYDJYJSPF = true;
                if ((ye.Point_JSGCGHXKZPF == null && ye.Point_JSGSGHXKZMemo == null) || item.Project_Info.State == (int)PublicEnum.ProjState.Modified)
                    item.EditTable.Point_JSGCGHXKZPF = true;
                if ((ye.Point_JSYDGHXKZMemo == null && ye.Point_JSYDGHXKZPF == null) || item.Project_Info.State == (int)PublicEnum.ProjState.Modified)
                    item.EditTable.Point_JSYDGHXKZPF = true;
                if ((ye.Point_LZYSXJGDMemo == null && ye.Point_LZYSXJGDPF == null) || item.Project_Info.State == (int)PublicEnum.ProjState.Modified)
                    item.EditTable.Point_LZYSXJGDPF = true;
                if ((ye.Point_SGJLRYBA == null && ye.Point_SGJLRYBAMemo == null) || item.Project_Info.State == (int)PublicEnum.ProjState.Modified)
                    item.EditTable.Point_SGJLRYBA = true;
                if ((ye.Point_SGJLZTP == null && ye.Point_SGJLZTPMemo == null) || item.Project_Info.State == (int)PublicEnum.ProjState.Modified)
                    item.EditTable.Point_SGJLZTP = true;
                if ((ye.Point_SGTBZHSC == null && ye.Point_SGTBZHSCMemo == null) || item.Project_Info.State == (int)PublicEnum.ProjState.Modified)
                    item.EditTable.Point_SGTBZHSC = true;
                if ((ye.Point_SGXKZMemo == null && ye.Point_SGXKZPF == null) || item.Project_Info.State == (int)PublicEnum.ProjState.Modified)
                    item.EditTable.Point_SGXKZPF = true;
                if ((ye.Point_TDCRHT == null && ye.Point_TDCRHTMemo == null) || item.Project_Info.State == (int)PublicEnum.ProjState.Modified)
                    item.EditTable.Point_TDCRHT = true;
                if ((ye.Point_TDSYQZ == null && ye.Point_TDSYQZMemo == null) || item.Project_Info.State == (int)PublicEnum.ProjState.Modified)
                    item.EditTable.Point_TDSYQZ = true;
                if ((ye.Point_XMKG == null && ye.Point_XMKGMemo == null) || item.Project_Info.State == (int)PublicEnum.ProjState.Modified)
                    item.EditTable.Point_XMKG = true;
                if ((ye.Point_XMZPSJFAMemo == null && ye.Point_XMZPSJFAPF == null) || item.Project_Info.State == (int)PublicEnum.ProjState.Modified)
                    item.EditTable.Point_XMZPSJFAPF = true;
                if ((ye.Point_YSBZWC == null && ye.Point_YSBZWCMemo == null) || item.Project_Info.State == (int)PublicEnum.ProjState.Modified)
                    item.EditTable.Point_YSBZWC = true;
            }
            re.Data = relist;
            return new ActionResult<Pager<ProjectView>>(re);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public ActionResult<bool> ProJNew(ProjectAdd owner)
        {
            var ID = Guid.NewGuid();
            var proj = _work.Repository<Model.DB.Project_Info>();
            var Contacts = _work.Repository<Model.DB.Project_Contacts>();
            var Schedule = _work.Repository<Model.DB.Project_Schedule>();
            var Log = _work.Repository<Model.DB.Project_Log>();

            var dbe = new Project_Info();
            if (proj.Any(q => q.ProjectName == owner.ProjectInfo.ProjectName))
            {
                throw new Exception("不能有相同工程名称");
            }
            owner.ProjectInfo.Clone(dbe);
            dbe.ID = ID;
            dbe.State = (int)PublicEnum.GenericState.Normal;
            dbe.CreateMan = AppUser.CurrentUserInfo.UserProfile.CNName;
            proj.Add(dbe);

            var dbc = new Project_Contacts();
            owner.Contacts.Clone(dbc);
            dbc.ProjectID = ID;
            Contacts.Add(dbc);

            var dbs = new Project_Schedule();
            owner.Schedules.Clone(dbs);
            dbs.ProjectID = ID;
            dbs.ScheduleType = (int)PublicEnum.PlanType.Plan;
            Schedule.Add(dbs);
            var dbs2 = new Project_Schedule();
            dbs2.ProjectID = ID;
            dbs2.ScheduleType = (int)PublicEnum.PlanType.Ement;
            Schedule.Add(dbs2);

            var logs = new Project_Log();
            logs.ProjectID = ID;
            logs.LogContent = "添加工程项目：" + owner.ProjectInfo.ProjectName;
            logs.CreateMan = AppUser.CurrentUserInfo.UserProfile.CNName;
            logs.State = 1;
            Log.Add(logs);
            _work.Commit();
            return new ActionResult<bool>(true);
        }
        /// <summary>
        /// 修改进度计划
        /// </summary>
        /// <param name="Sch"></param>
        /// <returns></returns>
        public ActionResult<bool> ProJSch(ProJSch Sch)
        {
            var info = _work.Repository<Model.DB.Project_Info>();
            var to = info.GetModel(q => q.ID == Sch.ProjectID);
            if (to.State != (int)PublicEnum.ProjState.Modified)
            {
                throw new Exception("此项目状态不允许操作");
            }
            if (to.State == (int)PublicEnum.ProjState.Start)
            {
                throw new Exception("开工不允许任何操作");
            }
            to.State = (int)PublicEnum.ProjState.Normal;
            info.Update(to);
            var Schedule = _work.Repository<Model.DB.Project_Schedule>();
            var re = Schedule.GetModel(q => q.ID == Sch.ID);
            Sch.Clone(re);
            Schedule.Update(re);

            var Log = _work.Repository<Project_Log>();
            var logs = new Project_Log();
            logs.ProjectID = Sch.ProjectID;
            logs.LogContent = "修改了进度计划";
            logs.CreateMan = AppUser.CurrentUserInfo.UserProfile.CNName;
            logs.State = 1;
            Log.Add(logs);
            _work.Commit();
            return new ActionResult<bool>(true);
        }
        /// <summary>
        /// 执行计划修改
        /// </summary>
        /// <param name="Sch"></param>
        /// <returns></returns>
        public ActionResult<bool> ScheduleEdit(ScheduleEdit Sch)
        {
            if (string.IsNullOrEmpty(Sch.Memo) && Sch.Value == DateTime.MinValue)
            {
                throw new Exception("参数不能为空");
            }
            var info1 = _work.Repository<Model.DB.Project_Info>();
            var to = info1.GetModel(q => q.ID == Sch.ProjectID);
            if (to.State == (int)PublicEnum.ProjState.Start)
            {
                throw new Exception("开工不允许任何操作");
            }
            var Schedule = _work.Repository<Model.DB.Project_Schedule>();
            var info = Schedule.GetModel(q => q.ID == Sch.ID);
            switch (Sch.Quarter)
            {
                case PublicEnum.PlanEnd.Point_GCKXXYJBGPF:
                    if (info.Point_GCKXXYJBGPF != null || info.Point_GCKXXYJBGMemo != null)
                    {
                        if (to.State != (int)PublicEnum.ProjState.Modified)
                        {
                            throw new Exception("此项目状态不允许操作");
                        }
                        to.State = (int)PublicEnum.ProjState.Normal;
                        info1.Update(to);
                    }
                    info.Point_GCKXXYJBGPF = Sch.Value;
                    info.Point_GCKXXYJBGMemo = Sch.Memo;
                    Schedule.Update(info);
                    break;
                case PublicEnum.PlanEnd.Point_JSYDGHXKZPF:
                    if (info.Point_JSYDGHXKZPF != null || info.Point_JSYDGHXKZMemo != null)
                    {
                        if (to.State != (int)PublicEnum.ProjState.Modified)
                        {
                            throw new Exception("此项目状态不允许操作");
                        }
                        to.State = (int)PublicEnum.ProjState.Normal;
                        info1.Update(to);
                    }
                    info.Point_JSYDGHXKZPF = Sch.Value;
                    info.Point_JSYDGHXKZMemo = Sch.Memo;
                    Schedule.Update(info);
                    break;
                case PublicEnum.PlanEnd.Point_DKBGWC:
                    if (info.Point_DKBGWC != null || info.Point_DKBGWCMemo != null)
                    {
                        if (to.State != (int)PublicEnum.ProjState.Modified)
                        {
                            throw new Exception("此项目状态不允许操作");
                        }
                        to.State = (int)PublicEnum.ProjState.Normal;
                        info1.Update(to);
                    }
                    info.Point_DKBGWC = Sch.Value;
                    info.Point_DKBGWCMemo = Sch.Memo;
                    Schedule.Update(info);
                    break;
                case PublicEnum.PlanEnd.Point_CBSJJGSPF:
                    if (info.Point_CBSJJGSPF != null || info.Point_CBSJJGSMemo != null)
                    {
                        if (to.State != (int)PublicEnum.ProjState.Modified)
                        {
                            throw new Exception("此项目状态不允许操作");
                        }
                        to.State = (int)PublicEnum.ProjState.Normal;
                        info1.Update(to);
                    }
                    info.Point_CBSJJGSPF = Sch.Value;
                    info.Point_CBSJJGSMemo = Sch.Memo;
                    Schedule.Update(info);
                    break;
                case PublicEnum.PlanEnd.Point_SGTBZHSC:
                    if (info.Point_SGTBZHSC != null || info.Point_SGTBZHSCMemo != null)
                    {
                        if (to.State != (int)PublicEnum.ProjState.Modified)
                        {
                            throw new Exception("此项目状态不允许操作");
                        }
                        to.State = (int)PublicEnum.ProjState.Normal;
                        info1.Update(to);
                    }
                    info.Point_SGTBZHSC = Sch.Value;
                    info.Point_SGTBZHSCMemo = Sch.Memo;
                    Schedule.Update(info);
                    break;
                case PublicEnum.PlanEnd.Point_YSBZWC:
                    if (info.Point_YSBZWC != null || info.Point_YSBZWCMemo != null)
                    {
                        if (to.State != (int)PublicEnum.ProjState.Modified)
                        {
                            throw new Exception("此项目状态不允许操作");
                        }
                        to.State = (int)PublicEnum.ProjState.Normal;
                        info1.Update(to);
                    }
                    info.Point_YSBZWC = Sch.Value;
                    info.Point_YSBZWCMemo = Sch.Memo;
                    Schedule.Update(info);
                    break;
                case PublicEnum.PlanEnd.Point_CSKZJPF:
                    if (info.Point_CSKZJPF != null || info.Point_CSKZJMemo != null)
                    {
                        if (to.State != (int)PublicEnum.ProjState.Modified)
                        {
                            throw new Exception("此项目状态不允许操作");
                        }
                        to.State = (int)PublicEnum.ProjState.Normal;
                        info1.Update(to);
                    }
                    info.Point_CSKZJPF = Sch.Value;
                    info.Point_CSKZJMemo = Sch.Memo;
                    Schedule.Update(info);
                    break;
                case PublicEnum.PlanEnd.Point_SGJLZTP:
                    if (info.Point_SGJLZTP != null || info.Point_SGJLZTPMemo != null)
                    {
                        if (to.State != (int)PublicEnum.ProjState.Modified)
                        {
                            throw new Exception("此项目状态不允许操作");
                        }
                        to.State = (int)PublicEnum.ProjState.Normal;
                        info1.Update(to);
                    }
                    info.Point_SGJLZTP = Sch.Value;
                    info.Point_SGJLZTPMemo = Sch.Memo;
                    Schedule.Update(info);
                    break;
                case PublicEnum.PlanEnd.Point_XMKG:
                    if (info.Point_XMKG != null || info.Point_XMKGMemo != null)
                    {
                        if (to.State != (int)PublicEnum.ProjState.Modified)
                        {
                            throw new Exception("此项目状态不允许操作");
                        }
                        to.State = (int)PublicEnum.ProjState.Normal;
                        info1.Update(to);
                    }
                    info.Point_XMKG = Sch.Value;
                    info.Point_XMKGMemo = Sch.Memo;
                    Schedule.Update(info);
                    break;
                case PublicEnum.PlanEnd.Point_JSGCGHXKZPF:
                    if (info.Point_JSGCGHXKZPF != null || info.Point_JSGSGHXKZMemo != null)
                    {
                        if (to.State != (int)PublicEnum.ProjState.Modified)
                        {
                            throw new Exception("此项目状态不允许操作");
                        }
                        to.State = (int)PublicEnum.ProjState.Normal;
                        info1.Update(to);
                    }
                    info.Point_JSGCGHXKZPF = Sch.Value;
                    info.Point_JSGSGHXKZMemo = Sch.Memo;
                    Schedule.Update(info);
                    break;
                case PublicEnum.PlanEnd.Point_SGJLRYBA:
                    if (info.Point_SGJLRYBA != null || info.Point_SGJLRYBAMemo != null)
                    {
                        if (to.State != (int)PublicEnum.ProjState.Modified)
                        {
                            throw new Exception("此项目状态不允许操作");
                        }
                        to.State = (int)PublicEnum.ProjState.Normal;
                        info1.Update(to);
                    }
                    info.Point_SGJLRYBA = Sch.Value;
                    info.Point_SGJLRYBAMemo = Sch.Memo;
                    Schedule.Update(info);
                    break;
                case PublicEnum.PlanEnd.Point_SGXKZPF:
                    if (info.Point_SGXKZPF != null || info.Point_SGXKZMemo != null)
                    {
                        if (to.State != (int)PublicEnum.ProjState.Modified)
                        {
                            throw new Exception("此项目状态不允许操作");
                        }
                        to.State = (int)PublicEnum.ProjState.Normal;
                        info1.Update(to);
                    }
                    info.Point_SGXKZPF = Sch.Value;
                    info.Point_SGXKZMemo = Sch.Memo;
                    Schedule.Update(info);
                    break;
                case PublicEnum.PlanEnd.Point_GHXZYDJYJSPF:
                    if (info.Point_GHXZYDJYJSPF != null || info.Point_GHXZJYDYJSMemo != null)
                    {
                        if (to.State != (int)PublicEnum.ProjState.Modified)
                        {
                            throw new Exception("此项目状态不允许操作");
                        }
                        to.State = (int)PublicEnum.ProjState.Normal;
                        info1.Update(to);
                    }
                    info.Point_GHXZYDJYJSPF = Sch.Value;
                    info.Point_GHXZJYDYJSMemo = Sch.Memo;
                    Schedule.Update(info);
                    break;
                case PublicEnum.PlanEnd.Point_LZYSXJGDPF:
                    if (info.Point_LZYSXJGDPF != null || info.Point_LZYSXJGDMemo != null)
                    {
                        if (to.State != (int)PublicEnum.ProjState.Modified)
                        {
                            throw new Exception("此项目状态不允许操作");
                        }
                        to.State = (int)PublicEnum.ProjState.Normal;
                        info1.Update(to);
                    }
                    info.Point_LZYSXJGDPF = Sch.Value;
                    info.Point_LZYSXJGDMemo = Sch.Memo;
                    Schedule.Update(info);
                    break;
                case PublicEnum.PlanEnd.Point_TDCRHT:
                    if (info.Point_TDCRHT != null || info.Point_TDCRHTMemo != null)
                    {
                        if (to.State != (int)PublicEnum.ProjState.Modified)
                        {
                            throw new Exception("此项目状态不允许操作");
                        }
                        to.State = (int)PublicEnum.ProjState.Normal;
                        info1.Update(to);
                    }
                    info.Point_TDCRHT = Sch.Value;
                    info.Point_TDCRHTMemo = Sch.Memo;
                    Schedule.Update(info);
                    break;
                case PublicEnum.PlanEnd.Point_TDSYQZ:
                    if (info.Point_TDSYQZ != null || info.Point_TDSYQZMemo != null)
                    {
                        if (to.State != (int)PublicEnum.ProjState.Modified)
                        {
                            throw new Exception("此项目状态不允许操作");
                        }
                        to.State = (int)PublicEnum.ProjState.Normal;
                        info1.Update(to);
                    }
                    info.Point_TDSYQZ = Sch.Value;
                    info.Point_TDSYQZMemo = Sch.Memo;
                    Schedule.Update(info);
                    break;
                case PublicEnum.PlanEnd.Point_XMZPSJFAPF:
                    if (info.Point_XMZPSJFAPF != null || info.Point_XMZPSJFAMemo != null)
                    {
                        if (to.State != (int)PublicEnum.ProjState.Modified)
                        {
                            throw new Exception("此项目状态不允许操作");
                        }
                        to.State = (int)PublicEnum.ProjState.Normal;
                        info1.Update(to);
                    }
                    info.Point_XMZPSJFAPF = Sch.Value;
                    info.Point_XMZPSJFAMemo = Sch.Memo;
                    Schedule.Update(info);
                    break;
                default:
                    break;
            }
            var Log = _work.Repository<Project_Log>();
            var logs = new Project_Log();
            logs.ProjectID = Sch.ProjectID;
            logs.LogContent = "修改了执行计划：";
            logs.CreateMan = AppUser.CurrentUserInfo.UserProfile.CNName;
            logs.State = 1;
            Log.Add(logs);
            _work.Commit();
            return new ActionResult<bool>(true);
        }
        /// <summary>
        /// 词典选择器
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult<IEnumerable<Basic_Dict>> Statedict(PublicEnum.DictType state)
        {
            var dict = _work.Repository<Model.DB.Basic_Dict>();
            var re = dict.Queryable(q => q.DictType == (int)state);
            return new ActionResult<IEnumerable<Basic_Dict>>(re);
        }

        /// <summary>
        /// 状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult<bool> StateSet(PublicEnum.ProjState state, Guid ID)
        {
            var proj = _work.Repository<Model.DB.Project_Info>();
            var em = proj.GetModel(q => q.ID == ID);
            if (em == null)
            {
                throw new Exception("项目不存在");
            }
            if (em.State == (int)PublicEnum.ProjState.Start)
            {
                throw new Exception("开工不允许任何操作");
            }
            em.State = (int)state;
            proj.Update(em);

            var Log = _work.Repository<Project_Log>();
            var logs = new Project_Log();
            logs.ProjectID = em.ID;
            logs.CreateMan = AppUser.CurrentUserInfo.UserProfile.CNName;
            logs.State = 1;
            if (state == PublicEnum.ProjState.Apply)
            {
                logs.LogContent = "申请修改";
                Log.Add(logs);
            }
            else if (state == PublicEnum.ProjState.Modified)
            {
                AuthService hen = new AuthService(_work);
                var be = hen.GetLoginMenu(AppUser.CurrentUserInfo.UserInfo.Login).data;
                var me = be.Select(s => s.Menu33.FirstOrDefault(q => q.AuthKey == "project.project.check"));
                var fe = me.FirstOrDefault(q => q != null);
                if (fe == null)
                {
                    throw new Exception("没有此功能的权限");
                }
                logs.LogContent = "同意申请修改";
                Log.Add(logs);
            }
            else if (state == PublicEnum.ProjState.Start)
            {
                logs.LogContent = "项目开工";
                Log.Add(logs);
            }
            _work.Commit();
            return new ActionResult<bool>(true);
        }
    }
}
