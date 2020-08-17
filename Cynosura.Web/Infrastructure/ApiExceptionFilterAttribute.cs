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
        private readonly IWebHostEnvironment _env;
        private readonly IList<IExceptionHandler> _handlers;

        public ApiExceptionFilterAttribute(IWebHostEnvironment env, IEnumerable<IExceptionHandler> handlers)
        {
            _env = env;
            _handlers = handlers.OrderBy(h => h.Priority).ToList();
        }

        public override void OnException(ExceptionContext context)
        {
            var handler = _handlers.FirstOrDefault(f => f.CanHandleException(context.Exception));
            context.Result = handler != null
                ? handler.HandleException(context.Exception)
                : _env.GetServerErrorResult(new BadRequestModel(DefaultErrorMessage, context.Exception));
            context.ExceptionHandled = true;
        }
    }
}
