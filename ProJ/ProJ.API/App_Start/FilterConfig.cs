using System.Web;
using System.Web.Mvc;

namespace ProJ.API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new Public.FilterExceptionFilter());
            filters.Add(new Public.ProJActionFilter());
            filters.Add(new Public.ProJFilter());
        }
    }
}
