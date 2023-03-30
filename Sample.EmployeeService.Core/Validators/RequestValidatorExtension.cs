using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sample.EmployeeService.Core.Validators
{
    public static class RequestValidatorExtension
    {
        public static BadRequestObjectResult ToBadRequest<T>(this RequestValidator<T> request)
        {
            if (request.Errors != null && request.Errors.Any())
            {
                return new BadRequestObjectResult(
                new
                {
                    Errors = request.Errors.Select(e => new
                    {
                        Field = e.PropertyName,
                        Error = e.ErrorMessage
                    }),
                    ErrorType = "Request Validation Error",
                    Code = HttpStatusCode.BadRequest
                }
                );
            }
            else
            {
                return new BadRequestObjectResult(
                new
                {
                    Errors = request.CustomErrors.Select(e => new
                    {
                        Field = e.Key,
                        Error = e.Value
                    }),
                    ErrorType = "Request Validation Error",
                    Code = HttpStatusCode.BadRequest
                }
                );
            }
        }

        public static IActionResult ToBadRequest(Exception ex, string message, ILogger logger)
        {
            object errors = null;
            logger.LogError(message, ex);

            HttpStatusCode code = HttpStatusCode.BadRequest;
            errors = message;
            string errortype = "Request Validation Error";

            var result = new ObjectResult(new
            {
                Errors = new Object[] { errors },
                ErrorType = errortype,
                Code = code
            });
            result.StatusCode = (int)code;
            return result;
        }
    }
}
