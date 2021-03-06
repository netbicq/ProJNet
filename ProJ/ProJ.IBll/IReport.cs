using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProJ.Model;

namespace ProJ.IBll
{
    public interface IReport
    {
        //修改表头配置文件
        ActionResult<bool> EditWeb(string para);
        ActionResult<Pager<Model.View.Report>> GetReport(PagerQuery<Model.Para.ReportQuery> para);

        ActionResult<Model.View.ReportDyn> GetReportDyn(PagerQuery<Model.Para.ReportQuery> para);
    }
}
