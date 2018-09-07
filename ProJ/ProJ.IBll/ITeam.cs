using ProJ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.IBll
{
    /// <summary>
    /// 协调工作
    /// </summary>
    public interface ITeam
    {
        ActionResult<bool> Addteam(Model.Para.TeamNew para);

        ActionResult<bool> Delteam(Guid id);

        ActionResult<Pager<Model.View.TemaView>> GetteamList(PagerQuery<Model.Para.TimeQuery> para);
    }
}
