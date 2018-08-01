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

        ActionResult<Pager<Model.View.Report>> GetReport(PagerQuery<Model.Para.ReportQuery> para);
        
    }
}
