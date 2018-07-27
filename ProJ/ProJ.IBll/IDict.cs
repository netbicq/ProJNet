using ProJ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.IBll
{
    /// <summary>
    /// 词典
    /// </summary>
    public interface IDict
    {
        ActionResult<bool> AddDict(Model.Para.DictNew dict);

        ActionResult<bool> EditDict(Model.Para.EidtDict updater);

        ActionResult<bool> DelDict(Guid id);

        ActionResult<Pager<Model.View.DictView>> GetDictList(PagerQuery<Model.Para.DictQuery> para);

        ActionResult<IEnumerable<Model.DB.Basic_Dict>> DictSelector(PublicEnum.DictType dicttype);

    }
}
