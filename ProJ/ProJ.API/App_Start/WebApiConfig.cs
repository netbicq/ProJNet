using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ProJ.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            //设置跨域
            config.EnableCors(new System.Web.Http.Cors.EnableCorsAttribute("*", "*", "*"));
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Filters.Add(new Public.FilterExceptionFilter());
            config.Filters.Add(new Public.ProJActionFilter());
            config.Filters.Add(new Public.ProJFilter());
        }
    }
}
