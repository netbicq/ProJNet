using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using ProJ.ORM;
using System.Data.Entity;
using ProJ.Bll;

namespace ProJ.API.Public
{
    public class AOP
    {

        public static void Reg()
        {
            var builder = new ContainerBuilder();


            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();



            builder.RegisterType<Model.DB.ModelBase>()
                .As<Model.DB.ModelBase>()
                .InstancePerRequest();


            builder.RegisterType<dbcontext>()
                .As<DbContext>()
                .InstancePerRequest();


            builder.RegisterType<Unitwork>()
                .As<IUnitwork>()
                .InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(AuthService).Assembly)
              .Where(t => t.Name.EndsWith("Service"))
              .AsImplementedInterfaces()
              .InstancePerRequest();
             
            var container = builder.Build();

            var resolver = new AutofacWebApiDependencyResolver(container);
            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }

    }
}