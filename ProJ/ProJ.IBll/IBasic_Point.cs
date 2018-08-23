using ProJ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.IBll
{
    public interface IBasic_Point
    {
        ActionResult<bool> AddBap(Model.Para.BapNew Bap);

        ActionResult<bool> EditBap(Model.Para.EidtBap updater);

        ActionResult<bool> DelBap(Guid id);

        ActionResult<Pager<Model.Para.BapView>> GetBapList(PagerQuery<Model.Para.BapQuery> para);
    }
}
