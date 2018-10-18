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
        /// Target Exception type
        /// </summary>
        Type ExceptionType { get; }

        /// <summary>
        /// Exception handling method
        /// </summary>
        /// <param name="exception">Exception of target type</param>
        /// <returns></returns>
        ObjectResult HandleException(Exception exception);
    }
}