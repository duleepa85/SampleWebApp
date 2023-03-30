using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Auth.Models
{
    public class AuthorizationError : Exception
    {
        public HttpStatusCode Code { get; }
        public string Errors { get; }
        public AuthorizationError(HttpStatusCode code, string message) : base(message)
        {
            Code = code;
            Errors = message;
        }
    }
}
