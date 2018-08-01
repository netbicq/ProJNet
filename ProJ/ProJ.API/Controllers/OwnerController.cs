using ProJ.API.Public;
using ProJ.Model;
using ProJ.Model.Para;
using ProJ.Model.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProJ.API.Controllers
{
    /// <summary>
    /// 业主单位
    /// </summary>
    [RoutePrefix("api/owner")]
    public class OwnerController : ProJAPI
    {
        private IBll.IOwner bll = null;

        public OwnerController(IBll.IOwner owner)
        {
            bll = owner;
            BusinessService = bll;
        }

        /// <summary>
        /// 新建 业主
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("new")]
        public ActionResult<bool> Add(OwnerNew user)
        {
            return bll.ApplyOwner(user);
        }
        /// <summary>
        /// 删除业主单位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("del/{id:Guid}")]
        [HttpGet]
        public ActionResult<bool> Delete(Guid id)
        {
            return bll.DelOwner(id);
        }
        /// <summary>
        /// 修改业主单位
        /// </summary>
        /// <param name="updater"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("edit")]
        public ActionResult<bool> Edit(OwnerEdit updater)
        {
            return bll.AddOwner(updater);
        }
        /// <summary>
        /// 获取业主单位列表
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getlist")]
        public ActionResult<Pager<OwnerView>> GetDictList(PagerQuery<string> para)
        {
            return bll.GetOwnerList(para);
        }
        /// <summary>
        /// 业主单位选择器
        /// </summary>
        /// <param name="dicttype"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Selector")]
        public ActionResult<IEnumerable<Model.DB.Basic_Owner>> DictSelector()
        {
            return bll.OwnerSelector();
        }
    }
}
