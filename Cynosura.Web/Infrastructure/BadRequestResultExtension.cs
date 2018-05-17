using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Cynosura.Web.Infrastructure
{
    public static class BadRequestResultExtension
    {
        public static BadRequestObjectResult GetBadRequestResult(this IHostingEnvironment env, BadRequestModel model)
        {
            dynamic result = new ExpandoObject();
            result.Message = model.Message;
            result.ErrorCode = model.ErrorCode;
            result.Errors = model.Errors;
            if (env.IsDevelopment())
            {
                result.ExceptionMessage = model.ExceptionMessage;
                result.ExceptionType = model.ExceptionType;
            }
            return
                new BadRequestObjectResult(result);
        }
    }
}
