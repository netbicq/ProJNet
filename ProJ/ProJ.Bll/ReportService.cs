using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using ProJ.Model;
using ProJ.Model.Para;
using ProJ.Model.View;
using ProJ.ORM;

namespace ProJ.Bll
{
    public class ReportService : ServiceBase, IBll.IReport
    {

        private ORM.IUnitwork _work = null;

        public ReportService(ORM.IUnitwork work)
        {
            _work = work;
            Unitwork = work;
        }
        //修改配置文件表头
        public ActionResult<bool> EditWeb(string para)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");

            AppSettingsSection app = config.AppSettings;
            //app.Settings.Add("x", "this is X");

            app.Settings["report1"].Value = para;

            config.Save(ConfigurationSaveMode.Modified);
            return new ActionResult<bool>(true);
        }

        public ActionResult<Pager<Report>> GetReport(PagerQuery<ReportQuery> para)
        {
            //根据参数选出项目

            var projects = _work.Repository<Model.DB.Project_Info>().Queryable(
                q =>
                //行业
                (para.Query.ProjectIndustry.Contains(q.IndustryID) || para.Query.ProjectIndustry.Count() == 0)
                &&
                //级别
                (para.Query.ProjectLevel.Contains(q.LevelID) || para.Query.ProjectLevel.Count() == 0)
                //
                &&
                (para.Query.ProjectOwner.Contains(q.OwnerID) || para.Query.ProjectOwner.Count() == 0)
                &&
                (para.KeyWord == "" || q.ProjectName.Contains(para.KeyWord))
                &&
                (q.OwnerID == AppUser.CurrentUserInfo.UserInfo.OwnerID || AppUser.CurrentUserInfo.UserInfo.OwnerID == Guid.Empty)
                );
            var proids = projects.Select(s => s.ID);
            var proidsid = projects.Select(s => s.OwnerID);

            var sch = _work.Repository<Model.DB.Project_Schedule>().Queryable(
                q => proids.Contains(q.ProjectID)
                );
            var issu = _work.Repository<Model.DB.Project_Issue>().Queryable(
                q => proids.Contains(q.ProjectID));
            var own = _work.Repository<Model.DB.Basic_Owner>().Queryable(
               q => proidsid.Contains(q.ID));
            var Cons = _work.Repository<Model.DB.Project_Contacts>().Queryable(
               q => proids.Contains(q.ProjectID));

            var reme = from proj in projects
                       let issues = issu.Where(q => q.ProjectID == proj.ID).OrderByDescending(q => q.CreateDate).FirstOrDefault()
                       let owner = own.FirstOrDefault(q => q.ID == proj.OwnerID)
                       let con = Cons.FirstOrDefault(q => q.ProjectID == proj.ID)
                       select new Model.View.Report
                       {
                           Issues = issues,
                           ProjectOwner = owner,
                           ProjectInfo = proj,
                           Project_Contacts = con
                       };
            var retemp = reme.ToList();
            foreach (var item in retemp)
            {
                var plan = sch.FirstOrDefault(q => q.ProjectID == item.ProjectInfo.ID && q.ScheduleType == (int)PublicEnum.PlanType.Plan);
                var exec = sch.FirstOrDefault(q => q.ProjectID == item.ProjectInfo.ID && q.ScheduleType == (int)PublicEnum.PlanType.Ement);
                //集合装换字符串
                //item.IssuesStr = string.Join(".", item.Issues.Select(s => s.IssueContent));
                item.Point_CBSJJGSPF = new PointBase
                {
                    Exec = exec.Point_CBSJJGSPF == null ? exec.Point_CBSJJGSMemo == null ? "" : exec.Point_CBSJJGSMemo : ((DateTime)exec.Point_CBSJJGSPF).ToString("yyyy.MM.dd"),
                    Plan = plan.Point_CBSJJGSPF == null ? plan.Point_CBSJJGSMemo == null ? "" : plan.Point_CBSJJGSMemo : ((DateTime)plan.Point_CBSJJGSPF).ToString("yyyy.MM.dd"),
                    Execeed = ((DateTime?)((DateTime?)exec.Point_CBSJJGSPF == null && string.IsNullOrEmpty(exec.Point_CBSJJGSMemo) ? DateTime.Now : (DateTime?)exec.Point_CBSJJGSPF) - (DateTime?)plan.Point_CBSJJGSPF) == null ? false : ((DateTime)((DateTime?)exec.Point_CBSJJGSPF == null && string.IsNullOrEmpty(exec.Point_CBSJJGSMemo) ? DateTime.Now : (DateTime?)exec.Point_CBSJJGSPF) - (DateTime)plan.Point_CBSJJGSPF).TotalDays > 0
                };
                item.Point_CSKZJPF = new PointBase
                {
                    Exec = exec.Point_CSKZJPF == null ? exec.Point_CSKZJMemo == null ? "" : exec.Point_CSKZJMemo : ((DateTime)exec.Point_CSKZJPF).ToString("yyyy.MM.dd"),
                    Plan = plan.Point_CSKZJPF == null ? plan.Point_CSKZJMemo == null ? "" : plan.Point_CSKZJMemo : ((DateTime)plan.Point_CSKZJPF).ToString("yyyy.MM.dd"),
                    Execeed = ((DateTime?)((DateTime?)exec.Point_CSKZJPF == null && string.IsNullOrEmpty(exec.Point_CSKZJMemo) ? DateTime.Now : (DateTime?)exec.Point_CSKZJPF) - (DateTime?)plan.Point_CSKZJPF) == null ? false : ((DateTime)((DateTime?)exec.Point_CSKZJPF == null && string.IsNullOrEmpty(exec.Point_CSKZJMemo) ? DateTime.Now : (DateTime?)exec.Point_CSKZJPF) - (DateTime)plan.Point_CSKZJPF).TotalDays > 0
                };
                item.Point_DKBGWC = new PointBase
                {
                    Exec = exec.Point_DKBGWC == null ? exec.Point_DKBGWCMemo == null ? "" : exec.Point_DKBGWCMemo : ((DateTime)exec.Point_DKBGWC).ToString("yyyy.MM.dd"),
                    Plan = plan.Point_DKBGWC == null ? plan.Point_DKBGWCMemo == null ? "" : plan.Point_DKBGWCMemo : ((DateTime)plan.Point_DKBGWC).ToString("yyyy.MM.dd"),
                    Execeed = ((DateTime?)((DateTime?)exec.Point_DKBGWC == null && string.IsNullOrEmpty(exec.Point_DKBGWCMemo) ? DateTime.Now : (DateTime?)exec.Point_DKBGWC) - (DateTime?)plan.Point_DKBGWC) == null ? false : ((DateTime)((DateTime?)exec.Point_DKBGWC == null && string.IsNullOrEmpty(exec.Point_DKBGWCMemo) ? DateTime.Now : (DateTime?)exec.Point_DKBGWC) - (DateTime)plan.Point_DKBGWC).TotalDays > 0
                };
                item.Point_GCKXXYJBGPF = new PointBase
                {
                    Exec = exec.Point_GCKXXYJBGPF == null ? exec.Point_GCKXXYJBGMemo == null ? "" : exec.Point_GCKXXYJBGMemo : ((DateTime)exec.Point_GCKXXYJBGPF).ToString("yyyy.MM.dd"),
                    Plan = plan.Point_GCKXXYJBGPF == null ? plan.Point_GCKXXYJBGMemo == null ? "" : plan.Point_GCKXXYJBGMemo : ((DateTime)plan.Point_GCKXXYJBGPF).ToString("yyyy.MM.dd"),
                    Execeed = ((DateTime?)((DateTime?)exec.Point_GCKXXYJBGPF == null && string.IsNullOrEmpty(exec.Point_GCKXXYJBGMemo) ? DateTime.Now : (DateTime?)exec.Point_GCKXXYJBGPF) - (DateTime?)plan.Point_GCKXXYJBGPF) == null ? false : ((DateTime)((DateTime?)exec.Point_GCKXXYJBGPF == null && string.IsNullOrEmpty(exec.Point_GCKXXYJBGMemo) ? DateTime.Now : (DateTime?)exec.Point_GCKXXYJBGPF) - (DateTime)plan.Point_GCKXXYJBGPF).TotalDays > 0
                };
                item.Point_GHXZYDJYJSPF = new PointBase
                {
                    Exec = exec.Point_GHXZYDJYJSPF == null ? exec.Point_GHXZJYDYJSMemo == null ? "" : exec.Point_GHXZJYDYJSMemo : ((DateTime)exec.Point_GHXZYDJYJSPF).ToString("yyyy.MM.dd"),
                    Plan = plan.Point_GHXZYDJYJSPF == null ? plan.Point_GHXZJYDYJSMemo == null ? "" : plan.Point_GHXZJYDYJSMemo : ((DateTime)plan.Point_GHXZYDJYJSPF).ToString("yyyy.MM.dd"),
                    Execeed = ((DateTime?)((DateTime?)exec.Point_GHXZYDJYJSPF == null && string.IsNullOrEmpty(exec.Point_GHXZJYDYJSMemo) ? DateTime.Now : (DateTime?)exec.Point_GHXZYDJYJSPF) - (DateTime?)plan.Point_GHXZYDJYJSPF) == null ? false : ((DateTime)((DateTime?)exec.Point_GHXZYDJYJSPF == null && string.IsNullOrEmpty(exec.Point_GHXZJYDYJSMemo) ? DateTime.Now : (DateTime?)exec.Point_GHXZYDJYJSPF) - (DateTime)plan.Point_GHXZYDJYJSPF).TotalDays > 0
                };
                item.Point_JSGCGHXKZPF = new PointBase
                {
                    Exec = exec.Point_JSGCGHXKZPF == null ? exec.Point_JSGSGHXKZMemo == null ? "" : exec.Point_JSGSGHXKZMemo : ((DateTime)exec.Point_JSGCGHXKZPF).ToString("yyyy.MM.dd"),
                    Plan = plan.Point_JSGCGHXKZPF == null ? plan.Point_JSGSGHXKZMemo == null ? "" : plan.Point_JSGSGHXKZMemo : ((DateTime)plan.Point_JSGCGHXKZPF).ToString("yyyy.MM.dd"),
                    Execeed = ((DateTime?)((DateTime?)exec.Point_JSGCGHXKZPF == null && string.IsNullOrEmpty(exec.Point_JSGSGHXKZMemo) ? DateTime.Now : (DateTime?)exec.Point_JSGCGHXKZPF) - (DateTime?)plan.Point_JSGCGHXKZPF) == null ? false : ((DateTime)((DateTime?)exec.Point_JSGCGHXKZPF == null && string.IsNullOrEmpty(exec.Point_JSGSGHXKZMemo) ? DateTime.Now : (DateTime?)exec.Point_JSGCGHXKZPF) - (DateTime)plan.Point_JSGCGHXKZPF).TotalDays > 0
                };
                item.Point_JSYDGHXKZPF = new PointBase
                {
                    Exec = exec.Point_JSYDGHXKZPF == null ? exec.Point_JSGSGHXKZMemo == null ? "" : exec.Point_JSGSGHXKZMemo : ((DateTime)exec.Point_JSYDGHXKZPF).ToString("yyyy.MM.dd"),
                    Plan = plan.Point_JSYDGHXKZPF == null ? plan.Point_JSGSGHXKZMemo == null ? "" : plan.Point_JSGSGHXKZMemo : ((DateTime)plan.Point_JSYDGHXKZPF).ToString("yyyy.MM.dd"),
                    Execeed = ((DateTime?)((DateTime?)exec.Point_JSYDGHXKZPF == null && string.IsNullOrEmpty(exec.Point_JSYDGHXKZMemo) ? DateTime.Now : (DateTime?)exec.Point_JSYDGHXKZPF) - (DateTime?)plan.Point_JSYDGHXKZPF) == null ? false : ((DateTime)((DateTime?)exec.Point_JSYDGHXKZPF == null && string.IsNullOrEmpty(exec.Point_JSYDGHXKZMemo) ? DateTime.Now : (DateTime?)exec.Point_JSYDGHXKZPF) - (DateTime)plan.Point_JSYDGHXKZPF).TotalDays > 0
                };
                item.Point_LZYSXJGDPF = new PointBase
                {
                    Exec = exec.Point_LZYSXJGDPF == null ? exec.Point_LZYSXJGDMemo == null ? "" : exec.Point_LZYSXJGDMemo : ((DateTime)exec.Point_LZYSXJGDPF).ToString("yyyy.MM.dd"),
                    Plan = plan.Point_LZYSXJGDPF == null ? plan.Point_LZYSXJGDMemo == null ? "" : plan.Point_LZYSXJGDMemo : ((DateTime)plan.Point_LZYSXJGDPF).ToString("yyyy.MM.dd"),
                    Execeed = ((DateTime?)((DateTime?)exec.Point_LZYSXJGDPF == null && string.IsNullOrEmpty(exec.Point_LZYSXJGDMemo) ? DateTime.Now : (DateTime?)exec.Point_LZYSXJGDPF) - (DateTime?)plan.Point_LZYSXJGDPF) == null ? false : ((DateTime)((DateTime?)exec.Point_LZYSXJGDPF == null && string.IsNullOrEmpty(exec.Point_LZYSXJGDMemo) ? DateTime.Now : (DateTime?)exec.Point_LZYSXJGDPF) - (DateTime)plan.Point_LZYSXJGDPF).TotalDays > 0
                };
                item.Point_SGJLRYBA = new PointBase
                {
                    Exec = exec.Point_SGJLRYBA == null ? exec.Point_SGJLRYBAMemo == null ? "" : exec.Point_SGJLRYBAMemo : ((DateTime)exec.Point_SGJLRYBA).ToString("yyyy.MM.dd"),
                    Plan = plan.Point_SGJLRYBA == null ? plan.Point_SGJLRYBAMemo == null ? "" : plan.Point_SGJLRYBAMemo : ((DateTime)plan.Point_SGJLRYBA).ToString("yyyy.MM.dd"),
                    Execeed = ((DateTime?)((DateTime?)exec.Point_SGJLRYBA == null && string.IsNullOrEmpty(exec.Point_SGJLRYBAMemo) ? DateTime.Now : (DateTime?)exec.Point_SGJLRYBA) - (DateTime?)plan.Point_SGJLRYBA) == null ? false : ((DateTime)((DateTime?)exec.Point_SGJLRYBA == null && string.IsNullOrEmpty(exec.Point_SGJLRYBAMemo) ? DateTime.Now : (DateTime?)exec.Point_SGJLRYBA) - (DateTime)plan.Point_SGJLRYBA).TotalDays > 0
                };
                item.Point_SGJLZTP = new PointBase
                {
                    Exec = exec.Point_SGJLZTP == null ? exec.Point_SGJLZTPMemo == null ? "" : exec.Point_SGJLZTPMemo : ((DateTime)exec.Point_SGJLZTP).ToString("yyyy.MM.dd"),
                    Plan = plan.Point_SGJLZTP == null ? plan.Point_SGJLZTPMemo == null ? "" : plan.Point_SGJLZTPMemo : ((DateTime)plan.Point_SGJLZTP).ToString("yyyy.MM.dd"),
                    Execeed = ((DateTime?)((DateTime?)exec.Point_SGJLZTP == null && string.IsNullOrEmpty(exec.Point_SGJLZTPMemo) ? DateTime.Now : (DateTime?)exec.Point_SGJLZTP) - (DateTime?)plan.Point_SGJLZTP) == null ? false : ((DateTime)((DateTime?)exec.Point_SGJLZTP == null && string.IsNullOrEmpty(exec.Point_SGJLZTPMemo) ? DateTime.Now : (DateTime?)exec.Point_SGJLZTP) - (DateTime)plan.Point_SGJLZTP).TotalDays > 0
                };
                item.Point_SGTBZHSC = new PointBase
                {
                    Exec = exec.Point_SGTBZHSC == null ? exec.Point_SGTBZHSCMemo == null ? "" : exec.Point_SGTBZHSCMemo : ((DateTime)exec.Point_SGTBZHSC).ToString("yyyy.MM.dd"),
                    Plan = plan.Point_SGTBZHSC == null ? plan.Point_SGTBZHSCMemo == null ? "" : plan.Point_SGTBZHSCMemo : ((DateTime)plan.Point_SGTBZHSC).ToString("yyyy.MM.dd"),
                    Execeed = ((DateTime?)((DateTime?)exec.Point_SGTBZHSC == null && string.IsNullOrEmpty(exec.Point_SGTBZHSCMemo) ? DateTime.Now : (DateTime?)exec.Point_SGTBZHSC) - (DateTime?)plan.Point_SGTBZHSC) == null ? false : ((DateTime)((DateTime?)exec.Point_SGTBZHSC == null && string.IsNullOrEmpty(exec.Point_SGTBZHSCMemo) ? DateTime.Now : (DateTime?)exec.Point_SGTBZHSC) - (DateTime)plan.Point_SGTBZHSC).TotalDays > 0
                };
                item.Point_SGXKZPF = new PointBase
                {
                    Exec = exec.Point_SGXKZPF == null ? exec.Point_SGXKZMemo == null ? "" : exec.Point_SGXKZMemo : ((DateTime)exec.Point_SGXKZPF).ToString("yyyy.MM.dd"),
                    Plan = plan.Point_SGXKZPF == null ? plan.Point_SGXKZMemo == null ? "" : plan.Point_SGXKZMemo : ((DateTime)plan.Point_SGXKZPF).ToString("yyyy.MM.dd"),
                    Execeed = ((DateTime?)((DateTime?)exec.Point_SGXKZPF == null && string.IsNullOrEmpty(exec.Point_SGXKZMemo) ? DateTime.Now : (DateTime?)exec.Point_SGXKZPF) - (DateTime?)plan.Point_SGXKZPF) == null ? false : ((DateTime)((DateTime?)exec.Point_SGXKZPF == null && string.IsNullOrEmpty(exec.Point_SGXKZMemo) ? DateTime.Now : (DateTime?)exec.Point_SGXKZPF) - (DateTime)plan.Point_SGXKZPF).TotalDays > 0
                };
                item.Point_TDCRHT = new PointBase
                {
                    Exec = exec.Point_TDCRHT == null ? exec.Point_TDCRHTMemo == null ? "" : exec.Point_TDCRHTMemo : ((DateTime)exec.Point_TDCRHT).ToString("yyyy.MM.dd"),
                    Plan = plan.Point_TDCRHT == null ? plan.Point_TDCRHTMemo == null ? "" : plan.Point_TDCRHTMemo : ((DateTime)plan.Point_TDCRHT).ToString("yyyy.MM.dd"),
                    Execeed = ((DateTime?)((DateTime?)exec.Point_TDCRHT == null && string.IsNullOrEmpty(exec.Point_TDCRHTMemo) ? DateTime.Now : (DateTime?)exec.Point_TDCRHT) - (DateTime?)plan.Point_TDCRHT) == null ? false : ((DateTime)((DateTime?)exec.Point_TDCRHT == null && string.IsNullOrEmpty(exec.Point_TDCRHTMemo) ? DateTime.Now : (DateTime?)exec.Point_TDCRHT) - (DateTime)plan.Point_TDCRHT).TotalDays > 0
                };
                item.Point_TDSYQZ = new PointBase
                {
                    Exec = exec.Point_TDSYQZ == null ? exec.Point_TDSYQZMemo == null ? "" : exec.Point_TDSYQZMemo : ((DateTime)exec.Point_TDSYQZ).ToString("yyyy.MM.dd"),
                    Plan = plan.Point_TDSYQZ == null ? plan.Point_TDSYQZMemo == null ? "" : plan.Point_TDSYQZMemo : ((DateTime)plan.Point_TDSYQZ).ToString("yyyy.MM.dd"),
                    Execeed = ((DateTime?)((DateTime?)exec.Point_TDSYQZ == null && string.IsNullOrEmpty(exec.Point_TDSYQZMemo) ? DateTime.Now : (DateTime?)exec.Point_TDSYQZ) - (DateTime?)plan.Point_TDSYQZ) == null ? false : ((DateTime)((DateTime?)exec.Point_TDSYQZ == null && string.IsNullOrEmpty(exec.Point_TDSYQZMemo) ? DateTime.Now : (DateTime?)exec.Point_TDSYQZ) - (DateTime)plan.Point_TDSYQZ).TotalDays > 0
                };
                item.Point_XMKG = new PointBase
                {
                    Exec = exec.Point_XMKG == null ? exec.Point_XMKGMemo == null ? "" : exec.Point_XMKGMemo : ((DateTime)exec.Point_XMKG).ToString("yyyy.MM.dd"),
                    Plan = plan.Point_XMKG == null ? plan.Point_XMKGMemo == null ? "" : plan.Point_XMKGMemo : ((DateTime)plan.Point_XMKG).ToString("yyyy.MM.dd"),
                    Execeed = ((DateTime?)((DateTime?)exec.Point_XMKG == null && string.IsNullOrEmpty(exec.Point_XMKGMemo) ? DateTime.Now : (DateTime?)exec.Point_XMKG) - (DateTime?)plan.Point_XMKG) == null ? false : ((DateTime)((DateTime?)exec.Point_XMKG == null && string.IsNullOrEmpty(exec.Point_XMKGMemo) ? DateTime.Now : (DateTime?)exec.Point_XMKG) - (DateTime)plan.Point_XMKG).TotalDays > 0
                };
                item.Point_XMZPSJFAPF = new PointBase
                {
                    Exec = exec.Point_XMZPSJFAPF == null ? exec.Point_XMZPSJFAMemo == null ? "" : exec.Point_XMZPSJFAMemo : ((DateTime)exec.Point_XMZPSJFAPF).ToString("yyyy.MM.dd"),
                    Plan = plan.Point_XMZPSJFAPF == null ? plan.Point_XMZPSJFAMemo == null ? "" : plan.Point_XMZPSJFAMemo : ((DateTime)plan.Point_XMZPSJFAPF).ToString("yyyy.MM.dd"),
                    Execeed = ((DateTime?)((DateTime?)exec.Point_XMZPSJFAPF == null && string.IsNullOrEmpty(exec.Point_XMZPSJFAMemo) ? DateTime.Now : (DateTime?)exec.Point_XMZPSJFAPF) - (DateTime?)plan.Point_XMZPSJFAPF) == null ? false : ((DateTime)((DateTime?)exec.Point_XMZPSJFAPF == null && string.IsNullOrEmpty(exec.Point_XMZPSJFAMemo) ? DateTime.Now : (DateTime?)exec.Point_XMZPSJFAPF) - (DateTime)plan.Point_XMZPSJFAPF).TotalDays > 0
                };
                item.Point_YSBZWC = new PointBase
                {
                    Exec = exec.Point_YSBZWC == null ? exec.Point_YSBZWCMemo == null ? "" : exec.Point_YSBZWCMemo : ((DateTime)exec.Point_YSBZWC).ToString("yyyy.MM.dd"),
                    Plan = plan.Point_YSBZWC == null ? plan.Point_YSBZWCMemo == null ? "" : plan.Point_YSBZWCMemo : ((DateTime)plan.Point_YSBZWC).ToString("yyyy.MM.dd"),
                    Execeed = ((DateTime?)((DateTime?)exec.Point_YSBZWC == null && string.IsNullOrEmpty(exec.Point_YSBZWCMemo) ? DateTime.Now : (DateTime?)exec.Point_YSBZWC) - (DateTime?)plan.Point_YSBZWC) == null ? false : ((DateTime)((DateTime?)exec.Point_YSBZWC == null && string.IsNullOrEmpty(exec.Point_YSBZWCMemo) ? DateTime.Now : (DateTime?)exec.Point_YSBZWC) - (DateTime)plan.Point_YSBZWC).TotalDays > 0
                };
                if (
                    item.Point_CBSJJGSPF.Execeed || item.Point_CSKZJPF.Execeed || item.Point_DKBGWC.Execeed
                         || item.Point_GCKXXYJBGPF.Execeed || item.Point_GHXZYDJYJSPF.Execeed || item.Point_JSGCGHXKZPF.Execeed
                         || item.Point_JSYDGHXKZPF.Execeed || item.Point_LZYSXJGDPF.Execeed || item.Point_SGJLRYBA.Execeed
                         || item.Point_SGJLZTP.Execeed || item.Point_SGTBZHSC.Execeed || item.Point_SGXKZPF.Execeed
                         || item.Point_TDCRHT.Execeed || item.Point_TDSYQZ.Execeed || item.Point_XMKG.Execeed
                         || item.Point_XMZPSJFAPF.Execeed || item.Point_YSBZWC.Execeed
                    )
                {
                    item.ProJBool = true;
                }
            }



            var relist = from r in retemp
                         where r.Point_CBSJJGSPF.Execeed || r.Point_CSKZJPF.Execeed || r.Point_DKBGWC.Execeed
                         || r.Point_GCKXXYJBGPF.Execeed || r.Point_GHXZYDJYJSPF.Execeed || r.Point_JSGCGHXKZPF.Execeed
                         || r.Point_JSYDGHXKZPF.Execeed || r.Point_LZYSXJGDPF.Execeed || r.Point_SGJLRYBA.Execeed
                         || r.Point_SGJLZTP.Execeed || r.Point_SGTBZHSC.Execeed || r.Point_SGXKZPF.Execeed
                         || r.Point_TDCRHT.Execeed || r.Point_TDSYQZ.Execeed || r.Point_XMKG.Execeed
                         || r.Point_XMZPSJFAPF.Execeed || r.Point_YSBZWC.Execeed
                         select r;

            var exp = para.Query.ExeceedType == PublicEnum.ExeceedType.Normal ? retemp : relist;

            string excel = "";
            if (para.ToExcel)
            {
                excel = Command.CreateExcel(exp, AppUser.OutPutPaht);
            }
            var re = new Pager<Model.View.Report>().GetCurrentPage(exp, para.PageSize, para.PageIndex);
            re.ExcelResult = excel;
            return new ActionResult<Pager<Report>>(re);
        }

        public ActionResult<ReportDyn> GetReportDyn(PagerQuery<ReportQuery> para)
        {
            string report1 = System.Configuration.ConfigurationManager.AppSettings["report1"];//报表名字
            //根据参数选出项目
            ReportDyn getlistpro = new ReportDyn();
            var projects = _work.Repository<Model.DB.Project_Info>().Queryable(
                q =>
                //行业
                (para.Query.ProjectIndustry.Contains(q.IndustryID) || para.Query.ProjectIndustry.Count() == 0)
                &&
                //级别
                (para.Query.ProjectLevel.Contains(q.LevelID) || para.Query.ProjectLevel.Count() == 0)
                //
                &&
                (para.Query.ProjectOwner.Contains(q.OwnerID) || para.Query.ProjectOwner.Count() == 0)
                &&
                (para.KeyWord == "" || q.ProjectName.Contains(para.KeyWord))
                &&
                (q.OwnerID == AppUser.CurrentUserInfo.UserInfo.OwnerID || AppUser.CurrentUserInfo.UserInfo.OwnerID == Guid.Empty)
                );
            var proids = projects.Select(s => s.ID);
            var proidsid = projects.Select(s => s.OwnerID);

            //var sch = _work.Repository<Model.DB.Project_Point>().Queryable(
            //    q => proids.Contains(q.ProjectID)
            //    );
            var issu = _work.Repository<Model.DB.Project_Issue>().Queryable(
                q => proids.Contains(q.ProjectID));
            var own = _work.Repository<Model.DB.Basic_Owner>().Queryable(
               q => proidsid.Contains(q.ID));
            var Cons = _work.Repository<Model.DB.Project_Contacts>().Queryable(
               q => proids.Contains(q.ProjectID));
            var point = _work.Repository<Model.DB.Basic_Point>().Queryable();
            //存储过程
            var exc = _work.ExecProcedre("Exec " + "ProjectReport");
            var data = exc.Read();
            //报表列
            var LeftColumns = new List<ReportColumn>();
            int leftorder = 100;

            var RightColumns = new List<ReportColumn>();
            int rightorder = 1000;
            //增加节点左边按钮

            LeftColumns.Add(new ReportColumn
            {
                Caption = "项目名称",
                ColumnFixed = true,
                IsClass = true,
                IsPoint = false,
                MultiColumn = false,
                OrderIndex = leftorder + 1,
                ShowModal = false,
                ColName = "ProjectInfo.ProjectName"
            });
            LeftColumns.Add(new ReportColumn
            {
                Caption = "计划/实际",
                Isbool = true,
                ColumnFixed = true,
                IsClass = false,
                IsPoint = false,
                MultiColumn = false,
                OrderIndex = leftorder + 2,
                ShowModal = false,
                ColName = "SchExe"
            });
            LeftColumns.Add(new ReportColumn
            {
                Caption = "计划开工月份",
                ColumnFixed = true,
                IsClass = false,
                IsPoint = false,
                MultiColumn = false,
                OrderIndex = leftorder + 4,
                ShowModal = false,
                ColName = "ProjectInfo.ComemenceDate"
            });
            LeftColumns.Add(new ReportColumn
            {
                Caption = "年度计划投资",
                ColumnFixed = true,
                IsClass = false,
                IsPoint = false,
                MultiColumn = false,
                OrderIndex = leftorder + 3,
                ShowModal = false,
                ColName = "ProjectInfo.InvestMoney"
            });
            //增加节点右边
            RightColumns.Add(new ReportColumn
            {
                Caption = "进展情况及存在问题",
                IsClass = false,
                ColumnFixed = false,
                IsPoint = false,
                MultiColumn = false,
                OrderIndex = rightorder + 1,
                ShowModal = true,
                ColName = "Issues.IssueContent"
            });
            RightColumns.Add(new ReportColumn
            {
                Caption = "下一周工作计划",
                IsClass = false,
                ColumnFixed = false,
                IsPoint = false,
                MultiColumn = false,
                OrderIndex = rightorder + 2,
                ShowModal = true,
                ColName = "ProjectInfo.NextPlan"
            });
            List<ReportColumn> h1 = new List<ReportColumn>();
            RightColumns.Add(new ReportColumn
            {
                Caption = "后续在建工作计划",
                IsClass = false,
                ColumnFixed = false,
                IsPoint = false,
                MultiColumn = true,
                OrderIndex = rightorder + 3,
                ShowModal = false,
                ColName = "",
                Children = h1
            });
            h1.Add(new ReportColumn
            {
                Caption = "第一季度完成投资",
                IsClass = false,
                ColumnFixed = false,
                IsPoint = false,
                MultiColumn = true,
                OrderIndex = rightorder + 3,
                ShowModal = false,
                ColName = "ProjectInfo.Q1Invest"
            });
            h1.Add(new ReportColumn
            {
                Caption = "第一季度期末形象进度",
                IsClass = false,
                ColumnFixed = false,
                IsPoint = false,
                MultiColumn = true,
                OrderIndex = rightorder + 4,
                ShowModal = false,
                ColName = "ProjectInfo.Q1Memo"
            });
            h1.Add(new ReportColumn
            {
                Caption = "第二季度完成投资",
                IsClass = false,
                ColumnFixed = false,
                IsPoint = false,
                MultiColumn = true,
                OrderIndex = rightorder + 5,
                ShowModal = false,
                ColName = "ProjectInfo.Q2Invest"
            });
            h1.Add(new ReportColumn
            {
                Caption = "第二季度期末形象进度",
                IsClass = false,
                ColumnFixed = false,
                IsPoint = false,
                MultiColumn = true,
                OrderIndex = rightorder + 6,
                ShowModal = false,
                ColName = "ProjectInfo.Q2Memo"
            });
            h1.Add(new ReportColumn
            {
                Caption = "第三季度完成投资",
                IsClass = false,
                ColumnFixed = false,
                IsPoint = false,
                MultiColumn = false,
                OrderIndex = rightorder + 7,
                ShowModal = false,
                ColName = "ProjectInfo.Q3Invest"
            });
            h1.Add(new ReportColumn
            {
                Caption = "第三季度期末形象进度",
                IsClass = false,
                ColumnFixed = false,
                IsPoint = false,
                MultiColumn = true,
                OrderIndex = rightorder + 8,
                ShowModal = false,
                ColName = "ProjectInfo.Q3Memo"
            });
            h1.Add(new ReportColumn
            {
                Caption = "第四季度完成投资",
                IsClass = false,
                ColumnFixed = false,
                IsPoint = false,
                MultiColumn = true,
                OrderIndex = rightorder + 9,
                ShowModal = false,
                ColName = "ProjectInfo.Q4Invest"
            });
            h1.Add(new ReportColumn
            {
                Caption = "第四季度期末形象进度",
                IsClass = false,
                ColumnFixed = false,
                IsPoint = false,
                MultiColumn = true,
                OrderIndex = rightorder + 10,
                ShowModal = false,
                ColName = "ProjectInfo.Q4Memo"
            });
            List<ReportColumn> h2 = new List<ReportColumn>();
            RightColumns.Add(new ReportColumn
            {
                Caption = "责任管理部门",
                IsClass = false,
                ColumnFixed = false,
                IsPoint = false,
                MultiColumn = true,
                OrderIndex = rightorder + 11,
                ShowModal = false,
                ColName = "",
                Children = h2
            });
            h2.Add(new ReportColumn
            {
                Caption = "单位名称",
                IsClass = false,
                ColumnFixed = false,
                IsPoint = false,
                MultiColumn = true,
                OrderIndex = rightorder + 11,
                ShowModal = false,
                ColName = "ProjectInfo.Department"
            });
            h2.Add(new ReportColumn
            {
                Caption = "项目负责人",
                IsClass = false,
                ColumnFixed = false,
                IsPoint = false,
                MultiColumn = true,
                OrderIndex = rightorder + 12,
                ShowModal = false,
                ColName = "Project_Contacts.SitePrincipal"
            });
            h2.Add(new ReportColumn
            {
                Caption = "具体责任人",
                IsClass = false,
                ColumnFixed = false,
                IsPoint = false,
                MultiColumn = false,
                OrderIndex = rightorder + 13,
                ShowModal = false,
                ColName = "Project_Contacts.SiteLink"
            });
            List<ReportColumn> h3 = new List<ReportColumn>();
            RightColumns.Add(new ReportColumn
            {
                Caption = "业主单位",
                IsClass = false,
                ColumnFixed = false,
                IsPoint = false,
                MultiColumn = true,
                OrderIndex = rightorder + 14,
                ShowModal = false,
                ColName = "",
                Children = h3
            });
            h3.Add(new ReportColumn
            {
                Caption = "单位名称",
                IsClass = false,
                ColumnFixed = false,
                IsPoint = false,
                MultiColumn = true,
                OrderIndex = rightorder + 14,
                ShowModal = false,
                ColName = "ProjectOwner.OwnerName"
            });
            h3.Add(new ReportColumn
            {
                Caption = "项目负责人",
                IsClass = false,
                ColumnFixed = false,
                IsPoint = false,
                MultiColumn = true,
                OrderIndex = rightorder + 15,
                ShowModal = false,
                ColName = "Project_Contacts.Handler"
            });
            h3.Add(new ReportColumn
            {
                Caption = "具体责任人",
                IsClass = false,
                ColumnFixed = false,
                IsPoint = false,
                MultiColumn = true,
                OrderIndex = rightorder + 16,
                ShowModal = false,
                ColName = "Project_Contacts.OwnerPrinci"
            });
            // getlistpro.ReportCols
            var PointColums = from bc in point
                              orderby bc.PointOrderIndex
                              select new ReportColumn
                              {
                                  Caption = bc.PointName,
                                  ColName = bc.ColName,
                                  OrderIndex = 200 + bc.PointOrderIndex
                              };

            LeftColumns.AddRange(PointColums.ToList());
            LeftColumns.AddRange(RightColumns);

            getlistpro.ReportCols = LeftColumns;


            var reme = from proj in projects
                       let issues = issu.Where(q => q.ProjectID == proj.ID).OrderByDescending(q => q.CreateDate).FirstOrDefault()
                       let owner = own.FirstOrDefault(q => q.ID == proj.OwnerID)
                       let con = Cons.FirstOrDefault(q => q.ProjectID == proj.ID)
                       orderby proj.ComemenceDate
                       select new Model.View.ReporDynlist
                       {
                           Issues = issues,
                           ProjectOwner = owner,
                           ProjectInfo = new Projinfo
                           {
                               test = proj.ComemenceDate,
                               ComemenceDate = "",
                               IndustryID = proj.IndustryID,
                               InvestMoney = proj.InvestMoney,
                               LevelID = proj.LevelID,
                               NextPlan = proj.NextPlan,
                               OwnerID = proj.OwnerID,
                               ProjectName = proj.ProjectName,
                               Q1Invest = proj.Q1Invest,
                               Q1Memo = proj.Q1Memo,
                               Q2Invest = proj.Q2Invest,
                               Q2Memo = proj.Q2Memo,
                               Q3Invest = proj.Q3Invest,
                               Q3Memo = proj.Q3Memo,
                               Q4Invest = proj.Q4Invest,
                               Q4Memo = proj.Q4Memo,
                               ID = proj.ID,
                               State = proj.State,
                               CreateDate = proj.CreateDate,
                               CreateMan = proj.CreateMan,
                               Department = proj.Department
                           },
                           Project_Contacts = new ProjContacts
                           {
                               ComLead = con.ComLead,
                               SiteLink = con.SiteLink + "\n" + con.SiteLinkTEL,
                               LeaderTEL = con.LeaderTEL,
                               OwnerPrinci = con.OwnerPrinci + "\n" + con.OwnerTEL,
                               Leader = con.Leader,
                               OwnerTEL = con.OwnerTEL,
                               Handler = con.Handler + "\n" + con.HandlerTEL,
                               SiteLinkTEL = con.SiteLinkTEL,
                               SitePrincipal = con.SitePrincipal + "\n" + con.SitePrincipalTEL,
                               ID = con.ID,
                               Principal = con.Principal,
                               DeptPrincipal = con.DeptPrincipal,
                               ComPrincipal = con.ComPrincipal,
                               ComLeadTEL = con.ComLeadTEL,
                               ComPrincipalTEL = con.ComPrincipalTEL,
                               HandlerTEL = con.HandlerTEL,
                               DeptPrincipalTEL = con.DeptPrincipalTEL,
                               SitePrincipalTEL = con.SitePrincipalTEL,
                               PrincipalTEL = con.PrincipalTEL,
                               ProjectID = con.ProjectID,
                           },
                       };
            var getdata = reme.ToList();
            //逾期
            int z = 0; int x = 0; int c = 0;
            List<ReporDynlist> bin = new List<ReporDynlist>();
            foreach (var item in getdata)
            {
                item.ProjectInfo.ComemenceDate = ((DateTime)item.ProjectInfo.test).ToString("yyyy.MM");
                item.isssum = true;
                //item.PointData = data;
                foreach (var item1 in data)
                {
                    foreach (var item2 in item1)
                    {
                        if (item2.Key == "ProjectID" && item.ProjectInfo.ID == item2.Value)
                        {
                            item.PointData = item1;
                        }
                    }
                }
                //逾期
                int i = 0; int g = 0;
                foreach (var item5 in item.PointData)
                {
                    
                    if (item5.Key.Contains("tot") && item5.Value > 0)
                    {
                        i = 1;
                        if (item5.Value > g)
                        {
                            g = item5.Value;
                        };
                    }
                }
                if (i == 1)
                {
                    item.ProJBool = true;
                    bin.Add(item);
                }
                if (g >= 30 && g < 60 && item.ProjectInfo.CreateDate.Year == para.Query.Month.Year && item.ProjectInfo.CreateDate.Month <= para.Query.Month.Month)
                    z += 1;
                if (g >= 60 && g < 90 && item.ProjectInfo.CreateDate.Year == para.Query.Month.Year && item.ProjectInfo.CreateDate.Month <= para.Query.Month.Month)
                    x += 1;
                if (g >= 90 && item.ProjectInfo.CreateDate.Year == para.Query.Month.Year && item.ProjectInfo.CreateDate.Month <= para.Query.Month.Month)
                    c += 1;
            }
            var dal = para.Query.ExeceedType == PublicEnum.ExeceedType.Normal ? getdata : bin.OrderBy(q => q.ProjectInfo.ComemenceDate).ToList();
            string excel = "";
            if (para.ToExcel)
            {
                excel = Command.CreateExcel(dal, PointColums, AppUser.OutPutPaht, report1);
            }
            //新增内容
            var moth = para.Query.Month.ToString("yyyy-MM");
            var prokl = _work.Repository<Model.DB.Project_Info>().Queryable(q => q.CreateDate.Year == para.Query.Month.Year && q.CreateDate.Month <= para.Query.Month.Month);
            int o = 0;
            foreach (var item in bin)
            {
                if (item.ProjectInfo.CreateDate.Year == para.Query.Month.Year && item.ProjectInfo.CreateDate.Month <= para.Query.Month.Month)
                {
                    o += 1;
                }

            }

            getlistpro.Pank = new Pank
            {
                IsOv = AppUser.CurrentUserInfo.UserInfo.OwnerID == Guid.Empty ? true : false,
                Moth = Convert.ToDateTime(moth),
                Prophase = prokl.Count().ToString(),
                Normal = (prokl.Count() - o).ToString(),
                Exec = o.ToString(),
                POne = z.ToString(),
                PThree = c.ToString(),
                PTwo = x.ToString(),
                Report1 = report1
            };
            var re = new Pager<Model.View.ReporDynlist>().GetCurrentPage(dal, para.PageSize, para.PageIndex);
            re.ExcelResult = excel;
            getlistpro.ReporDynlist = re; 

            //情况通报数据
            var datainfo = new DataInfo
            {
                
                YearCount = dal.Where(q => !string.IsNullOrWhiteSpace(q.ProjectInfo.ComemenceDate) && DateTime.Parse(q.ProjectInfo.ComemenceDate).Year == para.Query.Month.Year).Count(),
                DelayCount = dal.Where(q => !string.IsNullOrWhiteSpace(q.ProjectInfo.ComemenceDate) && DateTime.Parse(q.ProjectInfo.ComemenceDate).Year == para.Query.Month.Year && q.ProJBool).Count(),
                NormalCount = dal.Where(q => !string.IsNullOrWhiteSpace(q.ProjectInfo.ComemenceDate) && DateTime.Parse(q.ProjectInfo.ComemenceDate).Year == para.Query.Month.Year && !q.ProJBool  && q.ProjectInfo.State !=(int)(PublicEnum.ProjState.Start)).Count(),
                DeylayProje = dal.Where(q => !string.IsNullOrWhiteSpace(q.ProjectInfo.ComemenceDate) && DateTime.Parse(q.ProjectInfo.ComemenceDate).Year == para.Query.Month.Year && q.ProJBool).Select(s => s.ProjectInfo.ProjectName).ToList(),
                StartCount = dal.Where(q => q.ProjectInfo.State == (int)PublicEnum.ProjState.Start).Count(),
                 DelayInfos=(from delay in dal.Where(q=>!string.IsNullOrWhiteSpace(q.ProjectInfo.ComemenceDate) && DateTime.Parse(q.ProjectInfo.ComemenceDate).Year == para.Query.Month.Year && q.ProJBool)
                            select new DelayInfo
                            {
                                 ProjectID =delay.ProjectInfo.ID, ProjectName =delay.ProjectInfo.ProjectName  , DataPoints=delay.PointData
                            }).ToList()
                            
            };
            foreach(var inf in datainfo.DelayInfos.ToList())
            {
               var dpts  = new List<DelayPointInfo>();

                foreach(var pt in inf.DataPoints)
                {
                    if(pt.Key.Contains("tot") && pt.Value > 0)
                    {
                        dpts.Add(new DelayPointInfo
                        {
                            DelayDays = pt.Value,
                            PointName = ((string)pt.Key).Substring(4)
                        });
                    }
                }
                inf.DelayPoints = dpts;
            }

            getlistpro.DataInfo = datainfo;

            return new ActionResult<ReportDyn>(getlistpro);
        }
    }
}
