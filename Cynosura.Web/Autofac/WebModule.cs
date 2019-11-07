using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Cynosura.Web.Infrastructure;
using Cynosura.Web.Infrastructure.Authorization;
using Cynosura.Web.Infrastructure.Menu;

namespace Cynosura.Web.Autofac
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApiExceptionFilterAttribute>();
            builder.RegisterType<ServiceExceptionHandler>().As<IExceptionHandler>();
            builder.RegisterType<MenuProvider>().As<IMenuProvider>().SingleInstance();
            builder.RegisterType<PolicyProvider>().As<IPolicyProvider>().SingleInstance();
        }
    }
}
