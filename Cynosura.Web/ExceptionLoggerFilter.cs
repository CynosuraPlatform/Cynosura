using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Cynosura.Core.Services;

namespace Cynosura.Web
{
    public class ExceptionLoggerFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionLoggerFilter> _logger;

        public ExceptionLoggerFilter(ILogger<ExceptionLoggerFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            if (exception != null)
            {
                if (exception is ServiceException serviceException && serviceException.Severity == ErrorSeverity.Warning)
                {
                    _logger.LogWarning(0, exception, "Exception occurred: {exceptionMessage}", exception.Message);
                }
                else
                {
                    _logger.LogError(0, exception, "Exception occurred: {exceptionMessage}", exception.Message);
                }
            }
        }
    }
}
