using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Cynosura.Web.Infrastructure
{
    public static class BadRequestResultExtension
    {
        public static ObjectResult GetBadRequestResult(this IWebHostEnvironment env, BadRequestModel model)
        {
            return new BadRequestObjectResult(CopyModel(model, env));
        }

        public static ObjectResult GetServerErrorResult(this IWebHostEnvironment env, BadRequestModel model)
        {
            return new ObjectResult(CopyModel(model, env))
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        private static BadRequestModel CopyModel(BadRequestModel model, IWebHostEnvironment env)
        {
            var result = new BadRequestModel();
            result.Message = model.Message;
            result.ErrorCode = model.ErrorCode;
            result.Errors = model.Errors;
            if (env.IsDevelopment())
            {
                result.ExceptionMessage = model.ExceptionMessage;
                result.ExceptionType = model.ExceptionType;
            }
            return result;
        }

    }
}
