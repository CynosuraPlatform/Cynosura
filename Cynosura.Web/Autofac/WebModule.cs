using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Cynosura.Web.Infrastructure;

namespace Cynosura.Web.Autofac
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApiExceptionFilterAttribute>();
        }
    }
}
