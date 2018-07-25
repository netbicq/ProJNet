using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ProJ.Bll;
using ProJ.Model.DB;

namespace ProJ.API.Public
{
    public class ProJAPI: ApiController
    {
        /// <summary>
        /// 当前操作员
        /// </summary>
        public Model.DB.Auth_User AppUser { get; set; }
        /// <summary>
        /// 业务类
        /// </summary>
        public object BusinessService { get; set; }

        [NonAction]
        public void SetService()
        {
            ServiceBase obj = BusinessService as ServiceBase;
            if(AppUser !=null)
            {
                obj.AppUser = new AppServiceUser {
                     OutPutPaht =OutPutPath,
                      UploadPath =uploadPath,
                       UserInfo =AppUser
                };
                
            }
        }
        /// <summary>
        /// 上传文件地址
        /// </summary>
        protected string uploadPath = HttpContext.Current.Server.MapPath("~/uploads/");
        /// <summary>
        /// 导出临时文件夹
        /// </summary>
        protected string OutPutPath = HttpContext.Current.Server.MapPath("~/OutPutTemp/");

    }
}