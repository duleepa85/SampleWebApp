using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sample.EmployeeService.Core.DomainObjects
{
    public class ErrorResponse : Exception
    {
        public HttpStatusCode Code { get; }
        public Object Errors { get; }
        public string Subject { get; }
        public override string Message { get; }
        public ErrorResponse(HttpStatusCode code, Object errors = null)
        {
            Code = code;
            Errors = errors;
        }
        public ErrorResponse(HttpStatusCode code, string subject = null, string message = null)
        {
            Code = code;
            Message = message;
            Subject = subject;
        }
    }
}
