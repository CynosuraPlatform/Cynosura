using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Cynosura.Web.Infrastructure
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Result = new BadRequestObjectResult(new
                {
                    Message = "Model validation failed",
                    ModelState = Simplify(actionContext.ModelState)
                });
            }
        }
        public override void OnActionExecuted(ActionExecutedContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Result = new BadRequestObjectResult(new
                {
                    Message = "Model validation failed",
                    ModelState = Simplify(actionContext.ModelState)
                });
            }
        }

        public static Dictionary<string, SimpleModelState> Simplify(ModelStateDictionary modelState)
        {
            return modelState.ToDictionary(
                entry => entry.Key,
                entry => new SimpleModelState()
                {
                    Errors = entry.Value.Errors
                    .Select(error => new ModelStateError()
                    {
                        ErrorMessage = error.ErrorMessage,
                        ExceptionMessage = (error.Exception != null) ? error.Exception.Message : "",
                        ExceptionSource = (error.Exception != null) ? error.Exception.Source : ""
                    }).ToArray()
                }
            );
        }
    }

    public class SimpleModelState
    {
        public IList<ModelStateError> Errors { get; set; }
    }

    public class ModelStateError
    {
        public string ErrorMessage { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionSource { get; set; }
    }
}
