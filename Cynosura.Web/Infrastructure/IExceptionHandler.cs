using System;
using Microsoft.AspNetCore.Mvc;

namespace Cynosura.Web.Infrastructure
{
    /// <summary>
    /// Exception handler interface for <see cref="ApiExceptionFilterAttribute"/>
    /// </summary>
    public interface IExceptionHandler
    {
        /// <summary>
        /// Handler priority
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Return if handler can handle the exception
        /// </summary>
        bool CanHandleException(Exception exception);    

        /// <summary>
        /// Exception handling method
        /// </summary>
        /// <param name="exception">Exception of target type</param>
        /// <returns></returns>
        ObjectResult HandleException(Exception exception);
    }
}