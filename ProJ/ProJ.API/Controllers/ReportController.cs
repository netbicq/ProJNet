﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProJ.Model;

namespace ProJ.API.Controllers
{
    [RoutePrefix("api/report")]
    public class ReportController : Public.ProJAPI
    {
        private IBll.IReport bll = null;

        public ReportController(IBll.IReport report)
        {
            bll = report;
            BusinessService = bll;
        }

        /// <summary>
        /// 获取报表数据
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>[
        [Route("getdata")]
        [HttpPost]
        public Model.ActionResult<Pager<Model.View.Report>>GetReport(PagerQuery<Model.Para.ReportQuery> para)
        {
            return bll.GetReport(para);
        }

    }
}
