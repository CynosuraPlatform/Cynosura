using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Cynosura.Core.Services;

namespace Cynosura.Web.Infrastructure
{
    /// <summary>
    /// ExceptionHandler for <see cref="ServiceException"/>
    /// </summary>
    public class ServiceExceptionHandler: IExceptionHandler
    {
        private readonly IWebHostEnvironment _env;

        public ServiceExceptionHandler(IWebHostEnvironment env)
        {
            _env = env;
        }

        public int Priority => 0;

        public bool CanHandleException(Exception exception)
        {
            return exception is ServiceException;
        }

        public ObjectResult HandleException(Exception exception)
        {
            var serviceException = (ServiceException) exception;
            return _env.GetBadRequestResult(new BadRequestModel(serviceException.Message, serviceException,
                serviceException.Errors,
                serviceException.ErrorCode));
         }
    }
}