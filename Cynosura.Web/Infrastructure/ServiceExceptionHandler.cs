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
        public Type ExceptionType => typeof(ServiceException);

        public ServiceExceptionHandler(IWebHostEnvironment env)
        {
            _env = env;
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