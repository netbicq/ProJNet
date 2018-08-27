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
        /// 词典选择器
        /// </summary>
        /// <param name="state"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [Route("stateset/{state}")]
        [HttpGet]
        public ActionResult<IEnumerable<Model.DB.Basic_Dict>> Statedict(PublicEnum.DictType state)
        {
            return bll.Statedict(state);
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
        /// <summary>
        /// 修改联系人
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        [Route("conedit")]
        [HttpPost]
        public ActionResult<bool> ProCon(ProJCon para)
        {
            return bll.ProJCon(para);
        }
        /// <summary>
        /// 修改进度计划
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        [Route("editsch")]
        [HttpPost]
        public ActionResult<bool> ProJSch(ProJSch para)
        {
            return bll.ProJSch(para);
        }
        /// <summary>
        /// 修改问题
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        [Route("editiss")]
        [HttpPost]
        public ActionResult<bool> IssEdit(IssueEdit para)
        {
            return bll.IssEdit(para);
        }
        /// <summary>
        /// 删除问题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("deliss/{id:Guid}")]
        [HttpGet]
        public ActionResult<bool> IssDel(Guid id)
        {
            return bll.IssDel(id);
        }
        /// <summary>
        /// 后续计划修改
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        [Route("editafter")]
        [HttpPost]
        public ActionResult<bool> AfterEdit(AfterEdit para)
        {
            return bll.AfterEdit(para);
        }
        /// <summary>
        /// 执行计划修改
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        [Route("editschedule")]
        [HttpPost]
        public ActionResult<bool> ScheduleEdit(ScheduleEdit para)
        {
            return bll.ScheduleEdit(para);
        }
        /// <summary>
        /// 修改基本信息
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        [Route("editprojs")]
        [HttpPost]
        public ActionResult<bool> ProjEdit(ProjEdit para)
        {
            return bll.Projedit(para);
        }
        #region "调整工程项目的节点自定义"
        /// <summary>
        /// 获取节点
        /// </summary>
        /// <returns></returns>
        [Route("getlistq")]
        [HttpGet]
        public ActionResult<IEnumerable<Model.View.ProjectPoint>> GetPoints()
        {
            return bll.GetPoints();
        }
        /// <summary>
        /// 新增节点计划
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [Route("editschq")]
        [HttpPost]
        public ActionResult<bool> AddPoints(ProjectPointScheduleNew para)
        {
            return bll.AddPoints(para);
        }
        /// <summary>
        /// 修改节点计划
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        [Route("editissq")]
        [HttpPost]
        public ActionResult<bool> EditPoint(ProjetPointScheduleEdit para)
        {
            return bll.EditPoint(para);
        }
        /// <summary>
        /// 新增节点执行
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [Route("addschqe")]
        [HttpPost]
        public ActionResult<bool> AddPointExec(ProjectPointExecNew para)
        {
            return bll.AddPointExec(para);
        }
        /// <summary>
        /// 修改节点执行
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [Route("editissqb")]
        [HttpPost]
        public ActionResult<bool> EditPointExec(ProjectPointExecEdit para)
        {
            return bll.EditPointExec(para);
        }
        /// <summary>
        /// 新增工程项目
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [Route("addprob")]
        [HttpPost]
        public ActionResult<bool> AddProjectByPoint(ProjectAdd para)
        {
            return bll.AddProjectByPoint(para);
        }
        #endregion
    }
}
