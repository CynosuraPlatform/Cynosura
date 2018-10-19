using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cynosura.Web.Infrastructure
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private const string DefaultErrorMessage = "Error occurred";
        private readonly IHostingEnvironment _env;
        private readonly IEnumerable<IExceptionHandler> _handlers;

        public ApiExceptionFilterAttribute(IHostingEnvironment env, IEnumerable<IExceptionHandler> handlers)
        {
            _env = env;
            _handlers = handlers;
        }

        public override void OnException(ExceptionContext context)
        {
            var handler = _handlers.FirstOrDefault(f => f.ExceptionType == context.Exception.GetType());
            context.Result = handler != null
                ? handler.HandleException(context.Exception)
                : _env.GetBadRequestResult(new BadRequestModel(DefaultErrorMessage, context.Exception));
            context.ExceptionHandled = true;
        }
    }
}
