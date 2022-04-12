using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Cynosura.Core.Services
{
    public class ServiceException : Exception
    {
        public int? ErrorCode { get; }
        public ICollection? Errors { get; }
        public ErrorSeverity Severity { get; set; } = ErrorSeverity.Error;

        public ServiceException(int errorCode, string message, ICollection errors) : base(message)
        {
            ErrorCode = errorCode;
            Errors = errors;
        }

        public ServiceException(int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
            Errors = null;
        }

        public ServiceException(string message) : base(message)
        {
            ErrorCode = null;
            Errors = null;
        }

        public ServiceException(string message, ICollection errors) : base(message)
        {
            ErrorCode = null;
            Errors = errors;
        }

        public ServiceException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }

    public enum ErrorSeverity
    {
        Error,
        Warning
    }
}
