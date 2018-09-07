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
    /// 协调工作
    /// </summary>
    [RoutePrefix("api/team")]
    public class TeamController : ProJAPI
    {
        private IBll.ITeam bll = null;

        public TeamController(ITeam team)
        {
            bll = team;
            BusinessService = bll;

        }
        /// <summary>
        /// 新建协调工作
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [Route("addnew")]
        [HttpPost]
        public ActionResult<bool> Addteam(TeamNew para)
        {
            return bll.Addteam(para);
        }
        /// <summary>
        /// 删除指定ID的
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("delteam/{id:Guid}")]
        [HttpGet]
        public ActionResult<bool> Delteam(Guid id)
        {
            return bll.Delteam(id);
        }
        /// <summary>
        /// 获取协调工作列表
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getlist")]
        public ActionResult<Pager<TemaView>> GetteamList(PagerQuery<TimeQuery> para)
        {
            return bll.GetteamList(para);
        }
    }
}
