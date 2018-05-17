using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Cynosura.Web.Infrastructure
{
    public class BadRequestModel
    {
        public BadRequestModel(string message, Exception exception = null, ICollection errors = null, int? errorCode = null)
        {
            ErrorCode = errorCode;
            Message = message;
            if (exception != null)
            {
                ExceptionMessage = exception.Message;
                ExceptionType = exception.GetType().Name;
            }
            Errors = errors;
        }

        public int? ErrorCode { get; set; }
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }
        public ICollection Errors { get; set; }
    }
}
