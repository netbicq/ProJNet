using ProJ.API.Public;
using ProJ.IBll;
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
    /// 词典管理
    /// </summary>
    [RoutePrefix("api/dict")]
    public class DictController : ProJAPI
    {
        private IBll.IDict bll = null;

        public DictController(IDict dict)
        {
            bll = dict;
            BusinessService = bll;

        }
        /// <summary>
        /// 新建词典
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        [Route("addnew")]
        [HttpPost]
        public ActionResult<bool> AddDict(DictNew dict)
        {
            return bll.AddDict(dict);
        }
        /// <summary>
        /// 删除指定ID的词典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("deldict/{id:Guid}")]
        [HttpGet]
        public ActionResult<bool> DelDict(Guid id)
        {
            return bll.DelDict(id);
        }
        /// <summary>
        /// 修改词典
        /// </summary>
        /// <param name="updater"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("editdict")]
        public ActionResult<bool> EditDict(EidtDict updater)
        {
            return bll.EditDict(updater);
        }
        /// <summary>
        /// 获取词典列表
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getlist")]
        public ActionResult<Pager<DictView>> GetDictList(PagerQuery<DictQuery> para)
        {
            return bll.GetDictList(para);
        }
        /// <summary>
        /// 词典选择器
        /// </summary>
        /// <param name="dicttype"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Selector/{dicttype}")]
        public ActionResult<IEnumerable<Model.DB.Basic_Dict>> DictSelector(PublicEnum.DictType dicttype)
        {
            return bll.DictSelector(dicttype);
        }
    }
}
