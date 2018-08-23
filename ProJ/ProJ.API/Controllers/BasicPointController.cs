using ProJ.API.Public;
using ProJ.IBll;
using ProJ.Model;
using ProJ.Model.Para;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProJ.API.Controllers
{
    /// <summary>
    /// 标准管理
    /// </summary>
    [RoutePrefix("api/bap")]
    public class BasicPointController : ProJAPI
    {
        private IBll.IBasic_Point bll = null;

        public BasicPointController(IBasic_Point bap)
        {
            bll = bap;
            BusinessService = bll;

        }
        /// <summary>
        /// 新建标准
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        [Route("addnew")]
        [HttpPost]
        public ActionResult<bool> AddBap(BapNew bap)
        {
            return bll.AddBap(bap);
        }
        /// <summary>
        /// 删除指定ID的标准
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("delbap/{id:Guid}")]
        [HttpGet]
        public ActionResult<bool> Delbap(Guid id)
        {
            return bll.DelBap(id);
        }
        /// <summary>
        /// 修改标准
        /// </summary>
        /// <param name="updater"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("editbap")]
        public ActionResult<bool> Editbap(EidtBap updater)
        {
            return bll.EditBap(updater);
        }
        /// <summary>
        /// 获取标准列表
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getlist")]
        public ActionResult<Pager<BapView>> GetDictList(PagerQuery<BapQuery> para)
        {
            return bll.GetBapList(para);
        }
    }
}
