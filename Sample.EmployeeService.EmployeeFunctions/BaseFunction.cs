using Microsoft.AspNetCore.Mvc;
using Sample.Auth;
using Sample.Auth.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sample.EmployeeService.EmployeeFunctions
{
    public class BaseFunction
    {
        public IActionResult AuthorizeUser(AuthorizedRequest result)
        {
            object errors = null;
            HttpStatusCode code = HttpStatusCode.BadRequest;
            switch (result.Status)
            {
                case AuthorizationStatus.Error:
                    errors = "Token Error";
                    code = HttpStatusCode.BadRequest;
                    break;
                case AuthorizationStatus.NoToken:
                    errors = "No Token";
                    code = HttpStatusCode.BadRequest;
                    break;
                case AuthorizationStatus.Expired:
                    errors = "Expired Token";
                    code = HttpStatusCode.Unauthorized;
                    break;
                case AuthorizationStatus.Unauthorized:
                    errors = "Unauthorized Access";
                    code = HttpStatusCode.Unauthorized;
                    break;
            }

            var tokenResult = new ObjectResult(new { Errors = new Object[] { errors }, ErrorType = "TokenError", Code = code });
            tokenResult.StatusCode = (int)code;
            return tokenResult;
        }
    }
}
