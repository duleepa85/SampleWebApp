using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sample.EmployeeService.EmployeeManager;
using Microsoft.Extensions.Options;
using Sample.EmployeeService.Core.DomainObjects;
using Sample.Auth.Enum;
using Sample.Auth;
using System.Collections.Generic;
using System.Net;
using Sample.EmployeeService.EmployeeFunctions.Extensions;
using Pipelines.Sockets.Unofficial.Arenas;
using Sample.EmployeeService.Core.Validators;

namespace Sample.EmployeeService.EmployeeFunctions
{
    public class EmployeeFunctions : BaseFunction
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeFunctions(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Function for register an employee
        /// </summary>
        /// <param name="req"></param>
        /// <param name="request"></param>
        /// <param name="executionContext"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("Employee-RegisterEmployee")]
        public async Task<IActionResult> RegisterEmployee(
           [HttpTrigger(AuthorizationLevel.Function, "post", Route = "employees")] HttpRequest req, [Authorization] AuthorizedRequest request,
           ExecutionContext executionContext, ILogger log)
        {
            if (request.Status != AuthorizationStatus.Valid) return AuthorizeUser(request);
            request.SetupLoggingContext(executionContext);
            try
            {
                    log.LogInformation("RegisterEmployee function is triggered");
                    var registerEmployeeReq = await req.GetJsonBody<Employee, EmployeeValidator>();
                    if (registerEmployeeReq.IsValid)
                    {
                        log.LogInformation("RegisterEmployee validation is succeeded: " + JsonConvert.SerializeObject(registerEmployeeReq.Value));
                        return new OkObjectResult(_employeeService.RegisterEmployee(request, registerEmployeeReq.Value));
                    }
                    else
                    {
                        log.LogInformation("RegisterEmployee validation failed: " + JsonConvert.SerializeObject(registerEmployeeReq));
                        return RequestValidatorExtension.ToBadRequest(registerEmployeeReq);
                    }
            }
            catch (ErrorResponse er)
            {
                log.LogInformation("Bussiness validation error : Code : " + er.Code + "Subject : " + er.Subject + "Message : " + er.Message);
                return new ObjectResult(
                    new ErrorsResponse()
                    {
                        Errors = new List<ErrorResponseClient>()
                    {
                        new ErrorResponseClient(er.Code,er.Subject,er.Message)
                    }
                    })
                { StatusCode = (int)er.Code };
            }
            catch (Exception ex)
            {
                log.LogError("RegisterEmployee Exception", ex);
                return new ObjectResult(
                    new ErrorsResponse()
                    {
                        Errors = new List<ErrorResponseClient>()
                        {
                            new ErrorResponseClient(HttpStatusCode.InternalServerError,"Internal Server Error.","Please contact admin.")
                        }
                    }
                    )
                { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }


        /// <summary>
        /// function for return all the employees
        /// </summary>
        /// <param name="req"></param>
        /// <param name="request"></param>
        /// <param name="executionContext"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("Employee-GetEmployees")]
        public async Task<IActionResult> GetEmployees(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "employees")] HttpRequest req, [Authorization] AuthorizedRequest request,
            ExecutionContext executionContext, ILogger log)
        {
            if (request.Status != AuthorizationStatus.Valid) return AuthorizeUser(request);
            request.SetupLoggingContext(executionContext);

            try
            {
                log.LogInformation("EmployeeService-GetAllEmployees processing started");

                var response = _employeeService.GetAllEmployees(request);

                if (response.Employees.Count > 0)
                {
                    log.LogInformation("EmployeeService-GetAllEmployees Success : " + JsonConvert.SerializeObject(response));
                    return new OkObjectResult(response);
                }
                else
                {
                    log.LogInformation("EmployeeService-GetAllEmployees : No Employees");
                    return new NoContentResult();
                }
            }
            catch (Exception ex)
            {
                log.LogError("GetAllEmployees - Exception", ex);
                return new ObjectResult(
                    new ErrorsResponse()
                    {
                        Errors = new List<ErrorResponseClient>()
                       {
                           new ErrorResponseClient(HttpStatusCode.InternalServerError,"Internal Server Error.","Please contact admin.")
                       }
                    }
                    )
                { StatusCode = (int)HttpStatusCode.InternalServerError };
            }

        }

    }
}
