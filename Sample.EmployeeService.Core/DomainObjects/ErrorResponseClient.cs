using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sample.EmployeeService.Core.DomainObjects
{
    public class ErrorResponseClient
    {
        public HttpStatusCode Code { get; }
        public string Message { get; }
        public string Subject { get; }
        public ErrorResponseClient(HttpStatusCode code, string subject = null, string message = null)
        {
            Code = code;
            Message = message;
            Subject = subject;
        }
    }
}
