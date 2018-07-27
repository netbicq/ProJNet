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

namespace ProJ.IBll
{
    public interface IProJ
    {
        ActionResult<bool> ProJNew(Model.Para.ProjectAdd owner);
        ActionResult<Pager<Model.View.ProjectView>> ProJList(PagerQuery<string> para);
        /// <summary>
        /// 状态
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        ActionResult<bool> StateSet(PublicEnum.ProjState state, Guid ID);
        /// <summary>
        /// 问题发布
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        ActionResult<bool> IssueNew(Model.Para.IssueNew Iss);
    }
}
