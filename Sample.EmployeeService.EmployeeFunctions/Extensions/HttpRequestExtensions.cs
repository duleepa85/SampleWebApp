using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using Sample.Auth;
using Sample.EmployeeService.Core.DomainObjects;
using Sample.EmployeeService.Core.Validators;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sample.EmployeeService.EmployeeFunctions.Extensions
{
    public static class HttpRequestExtensions
    {
        static Dictionary<string, string> _errorList;
        public static async Task<RequestValidator<T>> GetJsonBody<T, V>(this HttpRequest request)
            where V : AbstractValidator<T>, new()
        {
            new Dictionary<string, string>();
            _errorList = new Dictionary<string, string>();

            var requestObject = await request.GetJsonBody<T>();

            if (requestObject == null)
            {
                throw new ErrorResponse(HttpStatusCode.BadRequest, "Bad Request", "Request body is required.");
            }

            if (_errorList != null && _errorList.Count > 0)
            {
                return new RequestValidator<T>
                {
                    Value = requestObject,
                    IsValid = false,
                    CustomErrors = _errorList
                };
            }

            var validator = new V();
            var validationResult = validator.Validate(requestObject);


            if (!validationResult.IsValid)
            {
                return new RequestValidator<T>
                {
                    Value = requestObject,
                    IsValid = false,
                    Errors = validationResult.Errors
                };
            }

            return new RequestValidator<T>
            {
                Value = requestObject,
                IsValid = true
            };
        }

        /// <summary>
        /// Returns the deserialized request body.
        /// </summary>
        /// <typeparam name="T">Type used for deserialization of the request body.</typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<T> GetJsonBody<T>(this HttpRequest request)
        {
            var requestBody = await request.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(requestBody, new JsonSerializerSettings
            {
                Error = HandleDeserializationError,
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            });
        }
        public static async Task<string> ReadAsStringAsync(this HttpRequest request)
        {
            request.EnableBuffering();
            string result = null;
            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, 1024, leaveOpen: true))
            {
                result = await reader.ReadToEndAsync();
            }

            request.Body.Seek(0L, SeekOrigin.Begin);
            return result;
        }
        public static void HandleDeserializationError(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs errorArgs)
        {
            if (!_errorList.ContainsKey(errorArgs.ErrorContext.Path))
            {
                _errorList.Add(errorArgs.ErrorContext.Path, $"{errorArgs.ErrorContext.Path} is not valid");
            }
            errorArgs.ErrorContext.Handled = true;
        }
        public static void SetupLoggingContext(this AuthorizedRequest authorizedRequest, ExecutionContext executionContext)
        {
            LogContext.PushProperty("UserID", authorizedRequest.AuthorizedUser.AzureUserId);
            LogContext.PushProperty("UserName", authorizedRequest.AuthorizedUser.TenantUserLogin);
            LogContext.PushProperty("TenantID", authorizedRequest.AuthorizedUser.TenantId);
            LogContext.PushProperty("InvocationId", executionContext.InvocationId);
            LogContext.PushProperty("ServiceName", "BMDService");
        }
    }
}
