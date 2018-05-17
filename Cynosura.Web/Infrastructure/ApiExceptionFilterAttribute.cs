using System;
using System.Collections.Generic;
using System.Text;
using Cynosura.Core.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cynosura.Web.Infrastructure
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private const string DefaultErrorMessage = "Error occurred";
        private readonly IHostingEnvironment _env;

        public ApiExceptionFilterAttribute(IHostingEnvironment env)
        {
            _env = env;
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is ServiceException serviceException)
            {
                context.Result = _env.GetBadRequestResult(new BadRequestModel(serviceException.Message, serviceException, serviceException.Errors));
            }
            else
            {
                context.Result = _env.GetBadRequestResult(new BadRequestModel(DefaultErrorMessage, context.Exception));
            }
            context.ExceptionHandled = true;
        }
    }
}
