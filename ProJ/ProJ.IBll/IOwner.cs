using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProJ.Model;
using ProJ.Model.DB;

namespace ProJ.IBll
{
    public interface IOwner
    {
        /// <summary>
        /// 群发短信
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        ActionResult<bool> AllSend(DateTime dt);
        ActionResult<bool> ApplyOwner(Model.Para.OwnerNew owner);

        ActionResult<bool> AddOwner(Model.Para.OwnerEdit owner);

        ActionResult<bool> DelOwner(Guid id);

        ActionResult<IEnumerable<Model.DB.Basic_Owner>> OwnerSelector();

        ActionResult<Pager<Model.View.OwnerView>> GetOwnerList(PagerQuery<string> para);


    }
}
