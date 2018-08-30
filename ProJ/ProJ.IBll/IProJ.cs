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

        /// <summary>
        /// 删除工程项目
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        ActionResult<bool> DeletePro(Guid ID);

        ActionResult<bool> ProJNew(Model.Para.ProjectAdd owner);

        ActionResult<Pager<Model.View.ProjectView>> ProJList(PagerQuery<string> para);
        /// <summary>
        /// 获取超期工程项目列表
        /// </summary>
        /// <returns></returns>
        ActionResult<IEnumerable<Model.View.ProjectSMS>> GetProjectSMS();
        /// <summary>
        /// 项目行业和项目级别
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        ActionResult<IEnumerable<Basic_Dict>> Statedict(PublicEnum.DictType state);
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
        /// <summary>
        /// 修改联系人
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        ActionResult<bool> ProJCon(Model.Para.ProJCon Con);
        /// <summary>
        /// 修改进度计划
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        ActionResult<bool> ProJSch(Model.Para.ProJSch Sch);
        /// <summary>
        /// 修改基本信息
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        ActionResult<bool> Projedit(Model.Para.ProjEdit pro);
        /// <summary>
        /// 修改问题
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        ActionResult<bool> IssEdit(Model.Para.IssueEdit Iss);
        /// <summary>
        /// 删除问题
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        ActionResult<bool> IssDel(Guid id);
        /// <summary>
        /// 后续计划修改
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        ActionResult<bool> AfterEdit(AfterEdit After);
        /// <summary>
        /// 执行计划修改
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        ActionResult<bool> ScheduleEdit(ScheduleEdit Sch);



        #region "调整工程项目的节点自定义"

        /// <summary>
        /// 获取节点
        /// </summary>
        /// <returns></returns>
        ActionResult<IEnumerable<Model.View.ProjectPoint>> GetPoints();
        /// <summary>
        /// 新增节点计划
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        ActionResult<bool> AddPoints(Model.Para.ProjectPointScheduleNew para);
        /// <summary>
        /// 修改节点计划
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        ActionResult<bool> EditPoint(Model.Para.ProjetPointScheduleEdit para);
        /// <summary>
        /// 新增节点执行
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        ActionResult<bool> AddPointExec(Model.Para.ProjectPointExecNew para);
        /// <summary>
        /// 修改节点执行
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        ActionResult<bool> EditPointExec(Model.Para.ProjectPointExecEdit para);
        /// <summary>
        /// 新增工程项目
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        ActionResult<bool> AddProjectByPoint(Model.Para.ProjectAdd para);

        #endregion
    }
}
