using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProJ.Model;
using ProJ.Model.Para;
using ProJ.Model.View;

namespace ProJ.Bll
{
    public class ReportService : ServiceBase, IBll.IReport
    {

        private ORM.IUnitwork _work = null;

        public ReportService(ORM.IUnitwork work)
        {
            _work = work; 
        }
        public ActionResult<Pager<Report>> GetReport(PagerQuery<ReportQuery> para)
        {
            //根据参数选出项目

            var projects = _work.Repository<Model.DB.Project_Info>().Queryable(
                q=>
                //行业
                (para.Query.ProjectIndustry.Contains(q.IndustryID) || para.Query.ProjectIndustry==null || para.Query.ProjectIndustry.Count()<=0)
                &&
                //级别
                (para.Query.ProjectLevel.Contains(q.LevelID) || para.Query.ProjectLevel ==null || para.Query.ProjectLevel.Count()<=0)
                //
                &&
                (para.Query.ProjectOwner.Contains(q.OwnerID) || para.Query.ProjectLevel==null || para.Query.ProjectLevel.Count()<=0)
                &&
                (para.KeyWord =="" || q.ProjectName.Contains(para.KeyWord) )
                );
            var proids = projects.Select(s => s.ID);

            var sch = _work.Repository<Model.DB.Project_Schedule>().Queryable(
                q => proids.Contains(q.ProjectID)
                );
            var issu = _work.Repository<Model.DB.Project_Issue>().Queryable(
                q => proids.Contains(q.ProjectID));

            var retemp = from proj in projects
                     let plan = sch.FirstOrDefault(q => q.ProjectID == proj.ID && q.ScheduleType == (int)PublicEnum.PlanType.Plan)
                     let exec = sch.FirstOrDefault(q => q.ProjectID == proj.ID && q.ScheduleType == (int)PublicEnum.PlanType.Ement)
                     let issues = issu.Where(q => q.ProjectID == proj.ID)
                     select new Model.View.Report
                     {
                         Point_CBSJJGSPF = new PointBase
                         {
                             Exec = exec.Point_CBSJJGSPF == null ? "" : ((DateTime)exec.Point_CBSJJGSPF).ToString("yyyy-MM-dd"),
                             Plan = plan.Point_CBSJJGSPF == null ? "" : ((DateTime)exec.Point_CBSJJGSPF).ToString("yyyy-MM-dd"),
                             Execeed = ((DateTime)exec.Point_CBSJJGSPF - (DateTime)plan.Point_CBSJJGSPF).TotalDays > 0
                         },
                         ProjectInfo = proj
                     };

            var relist = from r in retemp
                         where r.Point_CBSJJGSPF.Execeed || r.Point_CSKZJPF.Execeed
                         select r;


            var re = new Pager<Model.View.Report>().GetCurrentPage(relist, para.PageSize, para.PageIndex);

            return new ActionResult<Pager<Report>>(re);
        }
    }
}
