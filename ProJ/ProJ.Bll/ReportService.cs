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
            var re = new Pager<Report>();

            return new ActionResult<Pager<Report>>(re);
        }
    }
}
