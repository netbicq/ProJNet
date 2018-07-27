using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using ProJ.Model;
using ProJ.Bll;

namespace ProJ.API.Public
{
    public class FilterExceptionFilter: ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
           
            var dmsg = new System.Net.Http.HttpResponseMessage();
            var dobj = new ActionResult<bool>(actionExecutedContext.Exception);
            var dmsgstr = Newtonsoft.Json.JsonConvert.SerializeObject(dobj);
            var drcontent = new System.Net.Http.StringContent(dmsgstr);
            dmsg.Content = drcontent;

            actionExecutedContext.Response = dmsg;

            base.OnException(actionExecutedContext);
        }
    }

    public class ProJFilter: AuthorizeAttribute
    {

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            ProJAPI api = (ProJAPI)actionContext.ControllerContext.Controller;
            IEnumerable<string> Token;
            if (!(actionContext.Request.Headers.TryGetValues("token", out Token)))
                throw new Exception("非法请求");


            ServiceBase obj = api.BusinessService as ServiceBase;
             

            var authuser = obj.Unitwork.Repository<Model.DB.Auth_User>();
            var user = authuser.GetModel(q => q.Token == Token.FirstOrDefault());
            if (user == null)
                throw new Exception("非法请求");

            var profiledb = obj.Unitwork.Repository<Model.DB.Auth_UserProfile>();
            var profile = profiledb.GetModel(q => q.Login == user.Login);

            api.CurrentUser = new CurrentUser
            {
                UserInfo = user,
                UserProfile = profile
            };
           
            return true;
            return base.IsAuthorized(actionContext);
        }
    }

    public class ProJActionFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            ProJAPI api = (ProJAPI)actionContext.ControllerContext.Controller;
            api.SetService();

            base.OnActionExecuting(actionContext);
        }
    }
}