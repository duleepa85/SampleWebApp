using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs;
using Sample.Auth;
using Sample.Auth.Models;
using Sample.EmployeeService.Test.Factory;
using System;
using System.Collections.Generic;
using ExecutionContext = Microsoft.Azure.WebJobs.ExecutionContext;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pipelines.Sockets.Unofficial.Arenas;
using Xunit;
using Sample.EmployeeService.Core.DomainObjects;
using Sample.EmployeeService.Test.Mock.Service;
using Sample.EmployeeService.EmployeeFunctions;
using Sample.EmployeeService.Test.Mock.RequestValidators;
using Sample.EmployeeService.Core.Validators;

namespace Sample.EmployeeService.Test
{
    public class EmployeeFunctionTests
    {
        private readonly ILogger logger = Logger.CreateLogger();
        private readonly static TokenUser tokenUser = new TokenUser()
        {
            AzureUserId = new Guid("E40FD12D-EBDB-481C-912A-AAEC7A95EC21"),
            TenantId = new Guid("854C5085-256D-4B7E-BACD-708A465CB7B4"),
            UserDBConnectionString = "Data Source=UNICORNDKAL\\SQLEXPRESS;Initial Catalog=SampleTenantDB;User ID=duleepa;Password=duleepa123;"
        };
        private readonly AuthorizedRequest authorizedRequest = AuthorizedRequest.Success(tokenUser);
        readonly ExecutionContext executionContext = new ExecutionContext() { InvocationId = new Guid("d02746de-fa6f-4a83-bd0b-c3269dc68800") };

        #region Register-Employee
     
        [Fact]
        public async void RegisterEmployee_RegisterEmployeeRequestValid()
        {
            Employee employee = new Employee()
            {
                Name = "John Carter",
                Age = 34
            };

            var mockValidator = new MockEmployeeServiceRequestValidator().RequestValidatorRegisterEmployeeRequestValid(employee);
            var validator = new EmployeeValidator();
            var validatorResult = validator.Validate(employee);

            Assert.Equal(mockValidator.IsValid, validatorResult.IsValid);
        }

        [Fact]
        public async void RegisterEmployee_RegisterEmployeeRequestInvalid()
        {
            Employee employee = new Employee()
            {
                Name = "", //name should not be empty for valid request
                Age = 34
            };

            var mockValidator = new MockEmployeeServiceRequestValidator().RequestValidatorRegisterEmployeeRequestInvalid(employee);
            var validator = new EmployeeValidator();
            var validatorResult = validator.Validate(employee);

            Assert.Equal(mockValidator.IsValid, validatorResult.IsValid);
        }

        [Fact]
        public async void RegisterEmployee_Function_Success()
        {
            int employeeId = 6;
            Employee newEmployee = new Employee()
            {
                Name = "Robin Smith",
                Age = 32
            };

            RegisterEmployeeResponse registerEmployeeResponse = new RegisterEmployeeResponse()
            {
                IsError = false,
                Message = $"Employee is registered successfully",
                EmployeeId = employeeId
            };

            var request = Http.CreateDynamicRequest(JsonConvert.SerializeObject(newEmployee), "POST");
            var moqService = new MockEmployeeService().RegisterEmployee(registerEmployeeResponse);

            var response = (OkObjectResult)await new EmployeeFunctions.EmployeeFunctions(moqService.Object).RegisterEmployee(request, authorizedRequest, executionContext, logger);
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async void RegisterEmployee_BadRequest()
        {
            Employee employee = new Employee()
            {
                Name = "James Falkner",
                Age = -2 //age should be possitive always
            };
            int employeeId = 0;

            RegisterEmployeeResponse registerEmployeeResponse = new RegisterEmployeeResponse()
            {
                IsError = false,
                Message = $"Employee is registered successfully",
                EmployeeId = employeeId
            };

            var request = Http.CreateDynamicRequest(JsonConvert.SerializeObject(employee));
            var moqService = new MockEmployeeService().RegisterEmployee(registerEmployeeResponse);

            var response = (BadRequestObjectResult)await new EmployeeFunctions.EmployeeFunctions(moqService.Object)
                .RegisterEmployee(request, authorizedRequest, executionContext, logger);
            Assert.Equal(400, response.StatusCode);
        }

        #endregion

        #region Get-Employees

        [Fact]
        public async void GetEmployees_Function_Success()
        {
            int employeeId = 4;
            Employee employee = new Employee()
            {
                Name = "Brian Thomas",
                Age = 34
            };
            List<Employee>  employees = new List<Employee>();
            employees.Add(employee);

            GetAllEmployeesResponse getAllEmployeesResponse = new GetAllEmployeesResponse()
            {
                Employees = employees
            };
            var request = Http.CreateGetRequest();
            var moqService = new MockEmployeeService().GetEmployees(getAllEmployeesResponse);

            var response = (OkObjectResult)await new EmployeeFunctions.EmployeeFunctions(moqService.Object)
                .GetEmployees(request, authorizedRequest, executionContext,logger);
            Assert.Equal(200, response.StatusCode);
        }

        #endregion
    }

}
