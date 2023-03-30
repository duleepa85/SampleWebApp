using FluentValidation.Results;
using Sample.EmployeeService.Core.DomainObjects;
using Sample.EmployeeService.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.EmployeeService.Test.Mock.RequestValidators
{
    public class MockEmployeeServiceRequestValidator
    {
        public RequestValidator<Employee> RequestValidatorRegisterEmployeeRequestInvalid(Employee employee)
        {
            var result = new RequestValidator<Employee>()
            {
                Value = employee,
                IsValid = false,
                Errors = SetErrors()
            };
            return result;
        }
        public RequestValidator<Employee> RequestValidatorRegisterEmployeeRequestValid(Employee employee)
        {
            var result = new RequestValidator<Employee>()
            {
                Value = employee,
                IsValid = true,
            };
            return result;
        }

        private IList<ValidationFailure> SetErrors()
        {
            return new List<ValidationFailure>()
            {
                new ValidationFailure("Name","Name can't be empty")
                {
                    ErrorCode = "400",
                    ErrorMessage = "Name can't be empty"
                }
            };
        }
    }
}
