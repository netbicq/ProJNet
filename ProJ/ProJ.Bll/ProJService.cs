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
        /// 问题发布
        /// </summary>
        /// <param name="Iss"></param>
        /// <returns></returns>
        public ActionResult<bool> IssueNew(IssueNew Iss)
        {
            var Issue = _work.Repository<Model.DB.Project_Issue>();
            var issu = new Project_Issue();
            Iss.Clone(issu);
            issu.CreateMan = AppUser.CurrentUserInfo.UserProfile.CNName;
            issu.State = 1;
            Issue.Add(issu);
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
            var Log = _work.Repository < Model.DB.Project_Log>().Queryable();
            var retemp = from ac in proj
                         let Schedules = Schedule.Where(q=>q.ProjectID==ac.ID)
                         let logs = Log.Where(q=>q.ProjectID==ac.ID)
                         let Issues = Issue.Where(q=>q.ProjectID==ac.ID)
                         select new ProjectView
                         {
                             Project_Info = ac,
                             Project_Schedule= Schedules.Count()==0?null: Schedules,
                             Project_Issue= Issues.Count() == 0 ? null : Issues,
                             Project_Log= logs.Count() == 0 ? null : logs,
                             StateStr = ac.State == (int)PublicEnum.ProjState.Normal ? "正常" :
                             ac.State == (int)PublicEnum.ProjState.Apply ? "申请" :
                             ac.State == (int)PublicEnum.ProjState.Modified ? "待修改" :
                             ac.State == (int)PublicEnum.ProjState.Start ? "开工" :"未知"
                         };
            var re = new Pager<ProjectView>().GetCurrentPage(retemp, para.PageSize, para.PageIndex);
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
            owner.Schedules.Clone(dbs2);
            dbs2.ProjectID = ID;
            dbs2.ScheduleType =(int)PublicEnum.PlanType.Ement;
            Schedule.Add(dbs2);

            var logs = new Project_Log();
            logs.ProjectID = ID;
            logs.LogContent = "添加工程项目："+ owner.ProjectInfo.ProjectName;
            logs.CreateMan = AppUser.CurrentUserInfo.UserProfile.CNName;
            logs.State = 1;
            Log.Add(logs);
            _work.Commit();
            return new ActionResult<bool>(true);
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
            em.State = (int)state;
            proj.Update(em);
            _work.Commit();
            return new ActionResult<bool>(true);
        }
    }
}
