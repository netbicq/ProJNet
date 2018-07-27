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
    /// 工程项目
    /// </summary>
    [RoutePrefix("api/proj")]
    public class ProJController : ProJAPI
    {
        private IBll.IProJ bll = null;

        public ProJController(IProJ proj)
        {
            bll = proj;
            BusinessService = bll;

        }
        /// <summary>
        /// 新建项目
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        [Route("addnew")]
        [HttpPost]
        public ActionResult<bool> Add(ProjectAdd pro)
        {
            return bll.ProJNew(pro);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [Route("getlist")]
        [HttpPost]
        public ActionResult<Pager<Model.View.ProjectView>> GetListlog(PagerQuery<string> para)
        {
            return bll.ProJList(para);
        }
        /// <summary>
        /// 状态管理
        /// </summary>
        /// <param name="state"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [Route("stateset/{state}/{ID:Guid}")]
        [HttpGet]
        public ActionResult<bool> StateSet(PublicEnum.ProjState state, Guid ID)
        {
            return bll.StateSet(state, ID);
        }
        /// <summary>
        /// 问题发布
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        [Route("addiss")]
        [HttpPost]
        public ActionResult<bool> Iss(IssueNew para)
        {
            return bll.IssueNew(para);
        }
    }
}
